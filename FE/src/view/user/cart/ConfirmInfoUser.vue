<template>
  <div class="frame">
    <div class="p-5 container w-75" style="height: 75%; border-radius: 10px">
      <h2>Xác nhận thông tin khách hàng</h2>
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
      <div class="col-lg-12 mb-3">
        <label>Mã giảm giá</label>
        <input
          class="m-input mt-2"
          placeholder="Nhập mã giảm giá nếu có"
          type="text"
          v-model="discountCode"
        />
      </div>
      <div class="col-lg-12 d-flex justify-content-between pb-4">
        <button
          style="padding: 4px 12px; border: none; border-radius: 4px"
          class="bg-primary pull-right text-white"
          @click="goBack"
        >
          Quay về
        </button>
        <button
          style="padding: 4px 12px; border: none; border-radius: 4px"
          class="bg-primary pull-right text-white"
          @click="addInvoice"
        >
          Xác nhận
        </button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "ConfirmInfoUser",
  data() {
    return {
      record: {},
      invoice: {},
      discountCode: "",
      errors: {
        FullName: "",
        PhoneNumber: "",
        Address: "",
      },
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
        this.invoice.Total = sessionStorage.getItem("total") - 25000;
        if (this.discountCode.length > 0) {
          let resultDiscount = await this.apiService.getByInfo(
            "Discount/discountCode",
            this.discountCode
          );
          this.invoice.DiscountId = resultDiscount.DiscountId;
          this.invoice.Total =
            this.invoice.Total - resultDiscount.ReducedAmount;
        }

        this.invoice.InvoiceId = sessionStorage.getItem("invoiceId");
        this.invoice.InvoiceCode = sessionStorage.getItem("invoiceCode");
        this.invoice.StatusInvoice = 1;
        this.invoice.CreatedDate = new Date();

        let result = await this.apiService.update(
          "Invoice/put",
          this.invoice.InvoiceId + "/" + localStorage.getItem("AccountId"),
          this.invoice
        );

        if (result === 1) {
          this.$router.push("/user/home");
        }
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
</style>
