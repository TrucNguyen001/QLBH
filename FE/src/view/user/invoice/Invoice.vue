<template>
  <div
    class="container bg-body-tertiary mb-4"
    style="min-height: 70vh; font-size: 14px"
    id="test"
  >
    <div class="row">
      <div class="col-md-12">
        <h2 class="mx-2 my-2">Thông tin đơn hàng</h2>
        <hr />
        <div class="content-main">
          <div class="header-table d-flex justify-content-end">
            <div>
              <button
                :class="{
                  'btn-info': status === 1,
                  'btn-outline-info': status !== 1,
                }"
                v-on:click="confirmInvoiceWait"
                class="mx-2 btn"
              >
                Đơn chờ xử lý
              </button>
              <button
                :class="{
                  'btn-primary': status === 2,
                  'btn-outline-primary': status !== 2,
                }"
                v-on:click="delivering"
                class="btn"
              >
                Đơn hàng đang giao
              </button>
              <button
                :class="{
                  'btn-danger': status === 3,
                  'btn-outline-danger': status !== 3,
                }"
                v-on:click="invoiceFalse"
                class="btn mx-2"
              >
                Đơn giao thất bại
              </button>
              <button
                :class="{
                  'btn-success': status === 4,
                  'btn-outline-success': status !== 4,
                }"
                v-on:click="invoiceSuccess"
                class="btn"
              >
                Đơn giao thành công
              </button>
            </div>
          </div>
          <div class="table">
            <table class="m-table tbl">
              <thead>
                <tr>
                  <th style="width: 140px; background: #f5f5f5">Mã đơn hàng</th>
                  <th>Tên khách hàng</th>
                  <th>Email</th>
                  <th>Số điện thoại</th>
                  <th>Ngày đặt</th>
                  <th>Trạng thái</th>
                  <th>Xem</th>
                  <th v-if="this.status === 2">Xác nhận</th>
                  <th>Xoá</th>
                </tr>
              </thead>
              <!-- <div
                class="d-flex justify-content-center align-items-center"
                v-if="
                  this.listRecord.length <= 0 ||
                  this.listRecord.length === undefined
                "
              >
                <a href="/user/list-product"
                  ><img
                    src="../../../assets/img/empty_invoice.webp"
                    alt="Không có đơn hàng nào"
                    style="height: 300px; width: 800px"
                /></a>
              </div> -->
              <tbody>
                <tr
                  v-for="record in listRecord"
                  :key="record.CartId"
                  @mouseover="getRecordIdWhenMove(record)"
                  @mouseleave="handleMouseLeave"
                >
                  <td style="background: white">{{ record.InvoiceCode }}</td>
                  <td>{{ record.UserName }}</td>
                  <td>{{ record.Email }}</td>
                  <td>{{ record.PhoneNumber }}</td>
                  <td>
                    {{ this.common.changeDisplayDate(record.CreatedDate) }}
                  </td>
                  <td>
                    {{ this.getStatus(record.StatusInvoice) }}
                  </td>
                  <td @click="detailInvoice(record.InvoiceId)">
                    <button class="btn btn-primary">Xem</button>
                  </td>
                  <td v-if="this.status === 2">
                    <button
                      @click="receiveInvoiceSuccess(record)"
                      class="btn btn-success"
                    >
                      Nhận hàng
                    </button>
                  </td>
                  <td>
                    <button
                      @click="deleteRecord(record)"
                      class="btn btn-danger px-4"
                    >
                      {{ getValueButton(record.StatusInvoice) }}
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
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
    </div>
  </div>
</template>

<script>
export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: "Invoice",
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
      status: 1,
      invoiceId: "",
    };
  },
  methods: {
    confirmInvoiceWait() {
      this.status = 1;
      this.getInvoices();
    },
    /**
     * Đơn hàng đang giao
     */
    delivering() {
      this.status = 2;
      this.getInvoices();
    },
    invoiceSuccess() {
      this.status = 4;
      this.getInvoices();
    },
    invoiceFalse() {
      this.status = 3;
      this.getInvoices();
    },
    /**
     * Hiển thị trạng thái dạng chữ
     * @param {trạng thái đơn hàng} status
     */
    getStatus(status) {
      return status === 1
        ? "Chờ xác nhận"
        : status === 2
        ? "Đang giao hàng"
        : status === 3
        ? "Giao hàng thất bại"
        : "Giao hàng thành công";
    },

    getValueButton(status) {
      return status === 1
        ? "Huỷ đơn hàng"
        : status === 2
        ? "Huỷ đơn hàng"
        : status === 3
        ? "Xoá đơn  hàng"
        : status === 4
        ? "Xoá đơn hàng"
        : "";
    },
    changeButtonDisplayStatus(status) {
      return status === 1 ? "Xác nhận đơn" : status === 2 ? "Huỷ đơn hàng" : "";
    },

    /**
     * Chi tiết hoá đơn
     */
    detailInvoice(id) {
      this.$router.push(`/invoice-detail/${id}`);
    },
    /**
     * Hàm lấy những hoá đơn thành công
     */
    async getInvoiceBuySuccess() {
      this.listRecord = await this.apiService.getByInfo(
        "Invoice/user-success",
        localStorage.getItem("AccountId")
      );
      this.listRecord = this.filterUniqueInvoice(this.listRecord);
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
     * Hàm Thực hiện khi chọn xoá khách hàng
     * @author: Nguyễn Văn Trúc (9/12/2023)
     */
    deleteRecord(record) {
      try {
        this.invoiceId = "";
        if (this.status === 1 || this.status === 2) {
          record.StatusInvoice = 3;
          record.IsSuccess = 3;
          this.invoiceId = record.InvoiceId;
        } else if (this.status === 3) {
          record.IsSuccess = 3;
          record.StatusInvoice = 5;
        } else if (this.status === 4) {
          record.IsSuccess = 4;
          record.StatusInvoice = 5;
        }
        let str =
          this.status === 1 || this.status === 2
            ? "huỷ đơn hàng"
            : "xoá đơn hàng";
        this.common.showDialog(
          `Bạn có muốn ${str} [${record.InvoiceCode}] không?`,
          this.resource.ConfirmDeleteRecord.Title,
          this.helper.Status.Update,
          "Invoice/put",
          record.InvoiceId,
          record
        );
      } catch (error) {
        console.log(error);
      }
    },
    async receiveInvoiceSuccess(record) {
      record.StatusInvoice = 4;
      record.IsSuccess = 4;
      await this.apiService.update(
        "Invoice/put",
        record.InvoiceId + "/" + localStorage.getItem("AccountId"),
        record
      );
      this.getInvoices();
    },

    /**
     * Lấy sản phẩm mua
     */
    async getInvoices() {
      this.common.showLoading();
      this.listRecord = await this.apiService.getByInfo(
        "Invoice/user",
        localStorage.getItem("AccountId") + "/" + this.status
      );
      this.listRecord = this.filterUniqueInvoice(this.listRecord);
      this.common.showLoading(false);
    },

    filterUniqueInvoice(data) {
      // Tạo một đối tượng dùng để lưu trữ các bản ghi không trùng CommentId
      let uniqueComments = {};

      // Lặp qua dữ liệu và thêm các bản ghi không trùng CommentId vào mảng kết quả
      let uniqueRecords = [];
      data.forEach((item) => {
        if (!uniqueComments[item.InvoiceId]) {
          uniqueComments[item.InvoiceId] = true;
          uniqueRecords.push(item);
        }
      });
      return uniqueRecords; // Trả về mảng chứa các bản ghi không trùng CommentId
    },
  },
  async created() {
    if (localStorage.getItem("AccountId")) {
      await this.getInvoices();
      this.isLogin = true;
    }
  },
  mounted() {
    this.emitter.on("Status", (value) => {
      if (value === this.helper.Status.Update) {
        this.getInvoices();
        if (this.invoiceId !== "") {
          this.apiService.getByInfo(
            "Invoice/update-product-false",
            this.invoiceId
          );
        }
      }
    });
  },
};
</script>

<style scoped></style>
