<template>
  <div
    class="container bg-body-tertiary mb-4"
    style="min-height: 70vh; font-size: 14px"
    id="test"
  >
    <div class="row">
      <div class="col-md-9">
        <h2 class="mx-2 my-2">Thông tin giỏ hàng</h2>
        <hr />
        <div v-if="this.listRecord.length > 0" class="content-main">
          <div class="header-table">
            <div class="header-table-left"></div>
            <div style="width: 35%" class="header-table-right">
              <div class="input-icon">
                <input
                  placeholder="Tìm kiếm theo tên sản phẩm"
                  type="text"
                  class="m-input m-input-icon"
                  v-model="InfoSearch"
                  @keydown.enter="searchRecord"
                />
                <div @click="searchRecord" class="icon-search">
                  <i class="bi bi-search"></i>
                </div>
              </div>
            </div>
          </div>
          <div class="table">
            <table style="width: 1200px" class="m-table tbl">
              <thead>
                <tr>
                  <th style="width: 140px; background: #f5f5f5">Mã sản phẩm</th>
                  <th>Hình ảnh</th>
                  <th>Tên sản phẩm</th>
                  <th>Số lượng</th>
                  <th>Đơn giá</th>
                  <th>Thành tiền</th>
                  <th>Xoá</th>
                </tr>
              </thead>
              <tbody>
                <tr
                  v-for="record in listRecord"
                  :key="record.CartId"
                  @mouseover="getRecordIdWhenMove(record)"
                  @mouseleave="handleMouseLeave"
                >
                  <td style="background: white">{{ record.ProductCode }}</td>
                  <td
                    style="padding: 0; display: flex; justify-content: center"
                  >
                    <img
                      style="height: 80px; width: 70px"
                      class="card-img-top"
                      :src="require('@/assets/img/product/' + record.Avatar)"
                    />
                  </td>
                  <td>{{ record.ProductName }}</td>
                  <td>{{ record.QuantityPurchased }}</td>
                  <td>
                    {{ this.common.changeDisplayDebitAmount(record.PriceBuy) }}
                  </td>
                  <td>
                    {{
                      this.common.changeDisplayDebitAmount(
                        record.QuantityPurchased * record.PriceBuy
                      )
                    }}
                  </td>
                  <td>
                    <button
                      @click="deleteRecord(record.CartId)"
                      class="btn btn-danger px-4"
                    >
                      Xoá
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
        <div
          class="d-flex justify-content-center align-items-center"
          v-if="
            this.listRecord.length <= 0 || this.listRecord.length === undefined
          "
        >
          <a href="/user/list-product"
            ><img
              src="../../../assets/img/empty-cart.webp"
              alt="Không có sản phẩm nào"
              style="height: 300px"
          /></a>
        </div>
        <div v-if="this.listRecord.length > 0" class="my-2">
          <a
            href="/user/list-product"
            style="width: 220px; margin-bottom: 20px"
            class="d-flex align-items-center justify-content-center text-white btn bg-primary"
            >Tiếp tục mua hàng</a
          >
        </div>
      </div>
      <div class="col-md-3">
        <div v-if="isLogin">
          <a
            href="/user/invoice"
            class="d-flex align-items-center justify-content-center btn bg-primary text-white my-4"
            >Danh sách đơn hàng</a
          >
        </div>
        <div
          v-else
          class="w-100 btn btn-light bg-body-secondary"
          style="cursor: default; pointer-events: none"
        >
          Danh sách đơn hàng
        </div>
        <div v-if="total !== 0" class="w-100 mb-2">
          <label>Nhập mã giảm giá nếu có</label>
          <div
            class="d-flex justify-content-between mt-2"
            style="margin-left: 4px"
          >
            <input
              class="m-input"
              style="width: 72%"
              placeholder="Mã giảm giá"
              type="text"
              v-model="discountCode"
            />
            <button @click="findDiscountCode" class="btn btn-primary">
              Áp dụng
            </button>
          </div>
        </div>
        <div class="mb-2">
          <label>Hình thức thanh toán</label>
          <div class="mt-2 pay" style="margin-left: 4px">
            <input checked @click="payOff" type="radio" name="pay" />
            Thanh toán khi nhận hàng
          </div>
          <div class="mt-2 pay" style="margin-left: 4px">
            <input @click="payOn" type="radio" name="pay" />
            Thanh toán qua PayPal
          </div>
        </div>
        <p>
          Thanh toán online để được free ship
          <i class="bi bi-heart-fill text-danger mx-2"></i>
          <i class="bi bi-heart-fill text-danger"></i>
        </p>
        <div
          v-if="total !== 0"
          class="d-flex mx-1 justify-content-end"
          style="height: 50px"
        >
          <h3 class="text-danger mx-2">
            {{ this.common.changeDisplayDebitAmount(total) }}
          </h3>
        </div>
        <div
          v-if="total !== 0"
          class="d-flex mx-1 justify-content-end"
          style="height: 40px"
        >
          <h3 title="Phí ship">
            + {{ this.common.changeDisplayDebitAmount(this.priceShip) }}
          </h3>
        </div>
        <div
          v-if="total !== 0"
          class="d-flex mx-1 justify-content-end"
          style="height: 40px"
        >
          <h3 title="Giảm giá">
            - {{ this.common.changeDisplayDebitAmount(this.priceReduced) }}
          </h3>
        </div>
        <hr />
        <div
          v-if="total !== 0"
          class="d-flex justify-content-end"
          style="height: 50px"
        >
          <h4 class="my-auto">Tổng tiền:</h4>
          <h3 class="my-auto text-danger mx-2">
            {{
              this.common.changeDisplayDebitAmount(
                total + this.priceShip - this.priceReduced
              )
            }}
          </h3>
        </div>
        <button
          v-if="total !== 0"
          class="w-100 btn btn-primary"
          @click="confirmInvoice"
        >
          Mua hàng
        </button>
        <button
          v-else
          class="w-100 btn btn-light bg-body-secondary"
          style="cursor: default; pointer-events: none"
        >
          Mua hàng
        </button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "UserCart",
  data() {
    return {
      InfoSearch: "",
      listRecord: {},
      recordId: "",
      recordCode: "",
      filterObject: {
        pageNumber: 1,
      },
      total: 0,
      isLogin: false,
      discountCode: "",
      priceReduced: 0,
      priceShip: 25000,
      pay: 1,
      totalInvoice: 0,
    };
  },
  methods: {
    payOff() {
      this.pay = 1;
      this.priceShip = 25000;
      sessionStorage.setItem("pay", 1);
    },
    payOn() {
      this.pay = 2;
      this.priceShip = 0;
      sessionStorage.setItem("pay", 2);
    },
    async findDiscountCode() {
      if (this.discountCode.length > 0) {
        let resultDiscount = await this.apiService.getByInfo(
          "Discount/discountCode",
          this.discountCode
        );
        if (resultDiscount) {
          sessionStorage.setItem("DiscountId", resultDiscount.DiscountId);
          this.priceReduced = resultDiscount.ReducedAmount;
          this.common.showToast("Áp dụng mã giảm giá thành công");
        } else {
          this.common.showToastError("Mã giảm giá không thể áp dụng");
          sessionStorage.setItem("DiscountId", null);
        }
      } else {
        this.common.showToastError("Nhập mã giảm giá để có thể áp dụng");
        sessionStorage.setItem("DiscountId", null);
      }
    },
    /**
     * Xác nhận hoá đơn
     */
    confirmInvoice() {
      sessionStorage.setItem(
        "total",
        this.total + this.priceShip - this.priceReduced
      );
      this.$router.push("/confirm-info-user");
    },
    /**
     * Làm mới lại recordId khi con trỏ chuột chỉ ra ngoài
     * @author: Nguyễn Văn Trúc(3/3/2024)
     */
    handleMouseLeave() {
      this.recordCode = "";
    },

    /**
     * Lấy id nhân viên khi con trỏ chuột ở dòng nhân viên ấy
     * @param {nhân viên} record
     * @author: Nguyễn Văn Trúc(3/3/2024)
     */
    getRecordIdWhenMove(record) {
      this.recordId = record.CartId;
      this.recordCode = record.ProductCode;
      this.recordSelected = record;
    },

    /**
     * Hàm Thực hiện khi nhấn tìm kiếm
     * @author: Nguyễn Văn Trúc (21/1/2024)
     */
    searchRecord: function () {
      try {
        if (localStorage.getItem("AccountId")) {
          this.getProductByUserId();
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Hàm Thực hiện khi nhấn enter
     * @author: Nguyễn Văn Trúc (21/1/2024)
     */
    enterSearchInput: function () {
      try {
        if (localStorage.getItem("AccountId")) {
          this.getProductByUserId();
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm Thực hiện khi chọn xoá khách hàng
     * @author: Nguyễn Văn Trúc (9/12/2023)
     */
    deleteRecord(recordId) {
      try {
        this.common.showDialog(
          `${this.resource.ConfirmDeleteRecord.ContentDelete} [${this.recordCode}] ${this.resource.Question.Content}`,
          this.resource.ConfirmDeleteRecord.Title,
          this.helper.Status.Delete,
          "Cart/delete",
          recordId + "/" + localStorage.getItem("AccountId")
        );
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Bôi đen ô input search khi click vào
     * @author: Nguyễn Văn Trúc (22/1/2024)
     */
    blackenInputSearch() {
      try {
        this.common.blackenInput(this.helper.Ref.InputSearch);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Lấy sản phẩm mua
     */
    async getProductByUserId() {
      this.common.showLoading();
      this.listRecord = await this.apiService.getByInfo(
        "Cart",
        localStorage.getItem("AccountId") + "/" + this.InfoSearch
      );
      this.total = 0;
      // Duyệt qua danh sách bản ghi và tính tổng số tiền
      this.listRecord.forEach((record) => {
        // Kiểm tra nếu record có tồn tại Price và Quantity
        if (record.PriceBuy && record.QuantityPurchased) {
          // Tính thành tiền của mỗi bản ghi và cộng vào tổng số tiền
          this.total += record.PriceBuy * record.QuantityPurchased;
        }
      });

      if (this.listRecord.length > 0) {
        sessionStorage.setItem("invoiceId", this.listRecord[0].InvoiceId);
        sessionStorage.setItem("invoiceCode", this.listRecord[0].InvoiceCode);
      }

      this.common.showLoading(false);
    },
  },
  created() {
    if (localStorage.getItem("AccountId")) {
      this.getProductByUserId();
      this.isLogin = true;
    }
    sessionStorage.setItem("pay", 1);
    sessionStorage.setItem("DiscountId", null);
  },
  mounted() {
    this.emitter.on("Status", (value) => {
      if (
        value === this.helper.Status.Delete ||
        value === this.helper.Status.DeleteMultiple
      ) {
        this.getProductByUserId();
      }
    });
  },
};
</script>

<style scoped>
label {
  font-size: 16px;
}
</style>
