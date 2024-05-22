<template>
  <div class="frame">
    <div
      class="p-4 container"
      style="min-height: 400px; border-radius: 10px; width: 60%"
    >
      <div class="d-flex justify-content-between">
        <h2>Xác nhận thông tin khách hàng</h2>
        <i @click="goBack" class="bi bi-x-lg"></i>
      </div>
      <div class="col-lg-12 mb-3">
        <label>Tên khách hàng<i style="color: red">*</i></label>
        <input class="m-input mt-2" type="text" v-model="record.FullName" />
        <span style="color: red; font-size: 12px">{{ errors.FullName }}</span>
      </div>
      <div class="col-lg-12 mb-3">
        <label>Số điện thoại<i style="color: red">*</i></label>
        <input class="m-input mt-2" type="text" v-model="record.PhoneNumber" />
        <span style="color: red; font-size: 12px">{{
          errors.PhoneNumber
        }}</span>
      </div>
      <div class="col-lg-12 mb-3">
        <label>Địa chỉ nhận hàng<i style="color: red">*</i></label>
        <input class="m-input mt-2" type="text" v-model="record.Address" />
        <span style="color: red; font-size: 12px">{{ errors.Address }}</span>
      </div>
      <div class="col-lg-12 w-100 d-flex justify-content-center mt-4">
        <button
          v-show="this.pay == 1"
          style="
            padding: 4px 12px;
            border: none;
            border-radius: 4px;
            height: 36px;
          "
          class="bg-primary pull-right text-white w-100"
          @click="addInvoice"
        >
          Xác nhận
        </button>
        <div style="width: 750px" v-show="this.pay == 2" ref="paypal"></div>
      </div>
    </div>
  </div>
</template>

<script>
const axios = require("axios");
export default {
  name: "ConfirmInfoUser",
  data() {
    return {
      record: {},
      invoice: {},
      errors: {
        FullName: "",
        PhoneNumber: "",
        Address: "",
      },
      pay: 1,
      totalPay: "",
    };
  },
  methods: {
    goBack() {
      this.$router.push("/user/cart");
    },
    validateData: function () {
      try {
        let me = this;
        let checkError = true;
        me.errors = {};
        // Kiểm tra FullName
        if (me.common.validateInput(me.record.FullName)) {
          me.errors.FullName = "Họ và tên không được phép bỏ trống";
          checkError = false;
        }

        // Kiểm tra PhoneNumber
        if (me.common.validateInput(me.record.PhoneNumber)) {
          me.errors.PhoneNumber = "Số điện thoại không được phép bỏ trống";
          checkError = false;
        }

        // Kiểm tra Address
        if (me.common.validateInput(me.record.Address)) {
          me.errors.Address = "Địa chỉ không được phép bỏ trống";
          checkError = false;
        }

        return checkError;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Thêm hoá đơn
     */
    async addInvoice() {
      if (this.validateData()) {
        this.invoice.UserName = this.record.FullName;
        this.invoice.PhoneNumber = this.record.PhoneNumber;
        this.invoice.Address = this.record.Address;
        this.invoice.Email = this.record.Email;
        this.invoice.Total = sessionStorage.getItem("total");
        if (sessionStorage.getItem("DiscountId") !== "null") {
          this.invoice.DiscountId = sessionStorage.getItem("DiscountId");
        }
        this.invoice.InvoiceId = sessionStorage.getItem("invoiceId");
        this.invoice.InvoiceCode = sessionStorage.getItem("invoiceCode");
        this.invoice.StatusInvoice = 1;
        this.invoice.CreatedDate = new Date();
        this.invoice.pay = sessionStorage.getItem("pay");

        let result = await this.apiService.update(
          "Invoice/put",
          this.invoice.InvoiceId + "/" + localStorage.getItem("AccountId"),
          this.invoice
        );

        if (result === 1) {
          this.$router.push("/");
          this.common.showToast("Bạn đã đặt hàng thành công");
        }
      }
    },
    setLoaded: function () {
      window.paypal
        .Buttons({
          createOrder: (data, actions) => {
            if (this.validateData()) {
              return actions.order.create({
                purchase_units: [
                  {
                    description: "Thanh toán hoá đơn",
                    amount: {
                      currency_code: "USD",
                      value: this.totalPay,
                    },
                  },
                ],
              });
            }
          },
          onApprove: async (data, actions) => {
            await actions.order.capture();
            //  Thực hiện khi thanh toán thành công
            this.paymentSuccess();
          },
          onError: (err) => {
            console.log(err);
          },
        })
        .render(this.$refs.paypal);
    },
    paymentSuccess: function () {
      this.addInvoice();
    },

    async getUSDtoVNDExchangeRate() {
      try {
        let response = await axios.get(
          "https://api.exchangerate-api.com/v4/latest/USD"
        );

        if (response.status === 200 && response.data && response.data.rates) {
          let usdToVndRate = response.data.rates.VND;
          return usdToVndRate;
        } else {
          throw new Error("Không thể lấy dữ liệu tỷ giá hối đoái.");
        }
      } catch (error) {
        console.error("Lỗi khi lấy tỷ giá hối đoái:", error.message);
        return null;
      }
    },

    // Hàm để chuyển đổi số tiền từ VND sang USD
    async convertVNDtoUSD(amountVND) {
      try {
        // Lấy tỷ giá hối đoái từ API
        const exchangeRate = await this.getUSDtoVNDExchangeRate();

        // Kiểm tra nếu tỷ giá hối đoái hợp lệ
        if (exchangeRate !== null) {
          // Chuyển đổi số tiền từ VND sang USD
          const amountUSD = amountVND / exchangeRate;

          // Làm tròn ở con số thứ 2 sau dấu phẩy
          const roundedAmountUSD = amountUSD.toFixed(2);

          return roundedAmountUSD;
        } else {
          throw new Error("Không thể chuyển đổi.");
        }
      } catch (error) {
        console.error("Lỗi khi chuyển đổi:", error.message);
        return null;
      }
    },
  },
  async created() {
    if (localStorage.getItem("AccountId")) {
      this.record = await this.apiService.getByInfo(
        "Account/getById",
        localStorage.getItem("AccountId")
      );
    }
    if (sessionStorage.getItem("pay")) {
      this.pay = sessionStorage.getItem("pay");
    }
  },
  mounted: function () {
    if (sessionStorage.getItem("pay") == 2) {
      let script = document.createElement("script");
      script.src =
        "https://www.paypal.com/sdk/js?client-id=AQjMioGAvaPO7xVSyvdC6bzeHAPd0XQEn1oZYH5YWtIxNG9efQcxBX6IzivjBtNHBBxzzvRgWfqKiIxd";
      script.addEventListener("load", this.setLoaded);
      document.body.appendChild(script);
      this.convertVNDtoUSD(sessionStorage.getItem("total")).then(
        (amountInUSD) => {
          if (amountInUSD !== null) {
            this.totalPay = amountInUSD;
          } else {
            console.log("Không thể chuyển đổi.");
          }
        }
      );
    }
  },
};
</script>

<style scoped>
.frame {
  position: absolute;
  top: 0;
  right: 0;
  left: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.2);
  z-index: 10000;
  position: fixed;
}
.container {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  background: white;
  z-index: 10000;
}
.frame p {
  width: 200px;
}
.pay {
  display: flex;
  align-items: center;
}
.pay input {
  height: 16px;
  width: 16px;
  margin-right: 8px;
}
</style>
