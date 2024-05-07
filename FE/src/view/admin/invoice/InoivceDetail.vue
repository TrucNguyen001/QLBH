<template>
  <div class="container">
    <h1 class="mt-4">Chi tiết đơn hàng</h1>
    <div class="row mx-4 my-4 info-user">
      <div class="col-md-2">
        <div>Mã đơn hàng:</div>
        <div>Tên khách hàng:</div>
        <div>Số điện thoại:</div>
        <div>Địa chỉ nhận hàng:</div>
      </div>
      <div v-if="invoice[0] !== undefined" class="col-md-8">
        <div>{{ invoice[0].InvoiceCode }}</div>
        <div>{{ invoice[0].UserName }}</div>
        <div>{{ invoice[0].PhoneNumber }}</div>
        <div>{{ invoice[0].Address }}</div>
      </div>
    </div>
    <div class="row mx-4 my-5">
      <table class="m-table tbl">
        <tr style="padding-left: 4px; height: 48px">
          <td>Mã sản phẩm</td>
          <td>Hình ảnh</td>
          <td>Tên sản phẩm</td>
          <td>Số lượng</td>
          <td>Đơn giá</td>
          <td>Thành tiền</td>
        </tr>

        <tr v-for="(record, index) in invoice" :key="index">
          <td>{{ record.ProductCode }}</td>
          <td>
            <img
              style="height: 80px"
              :src="require('@/assets/img/product/' + record.Avatar)"
            />
          </td>
          <td>{{ record.ProductName }}</td>
          <td>{{ record.QuantityPurchased }}</td>
          <td>{{ this.common.changeDisplayDebitAmount(record.PriceBuy) }}</td>
          <td>
            {{
              this.common.changeDisplayDebitAmount(
                record.QuantityPurchased * record.PriceBuy
              )
            }}
          </td>
        </tr>
      </table>
    </div>
    <div
      v-if="invoice[0] !== undefined"
      class="row info-user"
      style="font-size: 18px"
    >
      <div class="col-md-8"></div>
      <div class="col-md-2">
        <div>Tổng cộng:</div>
        <div>Voucher giảm giá:</div>
        <div style="font-size: 16px">Phí vận chuyển:</div>
        <div>Thành tiền:</div>
      </div>
      <div class="col-md-2">
        <div>{{ this.common.changeDisplayDebitAmount(total) }}</div>
        <div>
          {{ this.common.changeDisplayDebitAmount(ReducedAmount) }}
        </div>
        <div style="font-size: 16px">
          {{ this.common.changeDisplayDebitAmount(priceShip) }}
        </div>
        <div>{{ this.common.changeDisplayDebitAmount(invoice[0].Total) }}</div>
        <button class="btn text-white btn-primary px-4 my-4" @click="goBack">
          Trở lại
        </button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "InvoiceDetail",
  data() {
    return {
      invoice: {},
      ReducedAmount: 0,
      total: 0,
      role: "",
      priceShip: 25000,
    };
  },
  methods: {
    goBack() {
      this.role === "User"
        ? this.$router.push("/user/invoice")
        : this.$router.push("/admin/invoice");
    },
    /**
     * Lấy hoá đơn
     */
    async getInvoice() {
      let invoiceId = await this.$route.params.id;
      let result = await this.apiService.getByInfo(
        "Invoice/invoice",
        invoiceId
      );
      this.invoice = result;

      // Lấy thông tin mã giảm giá
      if (result[0].DiscountId !== null) {
        let discountCode = await this.apiService.getByInfo(
          "Discount",
          result[0].DiscountId
        );

        this.ReducedAmount =
          discountCode.ReducedAmount === undefined
            ? 0
            : discountCode.ReducedAmount;
      }

      // Kiểm tra xem thanh toán on hay off
      if (result[0].pay === 2) {
        this.priceShip = 0;
      }
      this.total = 0;
      // Duyệt qua danh sách bản ghi và tính tổng số tiền
      result.forEach((record) => {
        // Kiểm tra nếu record có tồn tại Price và Quantity
        if (record.PriceBuy && record.QuantityPurchased) {
          // Tính thành tiền của mỗi bản ghi và cộng vào tổng số tiền
          this.total += record.PriceBuy * record.QuantityPurchased;
        }
      });
    },
  },
  created() {
    this.getInvoice();
    if (localStorage.getItem("Role")) {
      this.role = localStorage.getItem("Role");
    }
  },
};
</script>

<style scoped>
.m-table tr:first-child td {
  border-top: 1px solid #e0e0e0;
}
.m-table tr td:first-child {
  border-left: 1px solid #e0e0e0;
}
.info-user div {
  font-size: 16px;
}
table tr td {
  padding-left: 10px;
}
</style>
