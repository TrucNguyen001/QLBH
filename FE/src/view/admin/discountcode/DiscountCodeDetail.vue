<template>
  <div class="curtain" v-if="show">
    <div
      class="m-form"
      style="
        position: absolute;
        border: 1px solid darkgreen;
        width: 800px;
        z-index: 999;
      "
    >
      <div class="form">
        <div
          class="form-header d-flex align-items-center justify-content-between"
        >
          <h2>Thông tin mã giảm giá</h2>
          <button @click="hideDetailRecord" class="btn btn-close mx-4"></button>
        </div>
        <div class="form-body mx-5">
          <div class="d-flex justify-content-between">
            <div style="width: 45%">
              <MInput
                :inputErrorFirst="nameInputErrorFirst"
                nameInput="DiscountCode"
                v-model="recordSelect.DiscountCode"
                label="Mã mã giảm giá"
                :required="true"
                :isError="this.errors.DiscountCode"
                @removeClassErrorInput="this.errors.DiscountCode = null"
              ></MInput>
            </div>
            <div style="width: 45%">
              <MInput
                nameInput="DiscountName"
                v-model="recordSelect.DiscountName"
                label="Tên mã giảm giá"
                :required="true"
                :isError="this.errors.DiscountName"
                @removeClassErrorInput="this.errors.DiscountName = null"
              ></MInput>
            </div>
          </div>
          <div class="d-flex justify-content-between">
            <div class="mt-2" style="width: 45%">
              <MDate
                nameInput="ExpirationDate"
                v-model="recordSelect.ExpirationDate"
                label="Ngày hết hạn"
                :isError="this.errors.ExpirationDate"
                @removeClassErrorInput="removeErrorExpirationDate"
                @isNaNDate="isNaNExpirationDate"
              ></MDate>
            </div>
            <div style="width: 45%">
              <MInput
                nameInput="InputNumber"
                v-model="recordSelect.InputNumber"
                label="Số lần nhập"
                :required="true"
                :isError="this.errors.InputNumber"
                @removeClassErrorInput="this.errors.InputNumber = null"
              ></MInput>
            </div>
          </div>
          <div class="d-flex justify-content-between">
            <div style="width: 45%">
              <MInput
                nameInput="ReducedAmount"
                v-model="recordSelect.ReducedAmount"
                label="Số tiền giảm"
                :required="true"
                :isError="this.errors.ReducedAmount"
                @removeClassErrorInput="this.errors.ReducedAmount = null"
              ></MInput>
            </div>
            <div style="width: 45%">
              <MInput
                nameInput="Minimum"
                v-model="recordSelect.Minimum"
                label="Đơn tối thiểu"
                :required="true"
                :isError="this.errors.Minimum"
                @removeClassErrorInput="this.errors.Minimum = null"
              ></MInput>
            </div>
          </div>
          <div
            v-if="statusCode === this.helper.Status.See"
            class="d-flex justify-content-between"
          >
            <div style="width: 45%">
              <MInput
                v-model="recordSelect.CreatedDate"
                label="Ngày tạo"
              ></MInput>
            </div>
            <div style="width: 45%">
              <MInput
                v-model="recordSelect.CreatedBy"
                label="Người tạo"
              ></MInput>
            </div>
          </div>
          <div
            v-if="statusCode === this.helper.Status.See"
            class="d-flex justify-content-between"
          >
            <div style="width: 45%">
              <MInput
                v-model="recordSelect.ModifiedDate"
                label="Ngày sửa"
              ></MInput>
            </div>
            <div style="width: 45%">
              <MInput
                v-model="recordSelect.ModifiedBy"
                label="Người sửa"
              ></MInput>
            </div>
          </div>
        </div>
        <div
          class="form-footer mx-5 px-0 mt-5 mb-4 d-flex justify-content-end"
          v-if="statusCode === this.helper.Status.See"
        >
          <button @click="hideDetailRecord" class="btn btn-primary px-4">
            Thoát
          </button>
        </div>
        <div v-else class="form-footer mx-5 px-0 mt-5 mb-4">
          <button @click="hideDetailRecord" class="btn btn-danger px-4">
            Huỷ
          </button>
          <button @click="saveRecord" class="btn btn-success px-4">
            Cất bản ghi
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  components: {},
  props: {
    show: Boolean,
    record: Object,
    statusCode: Number,
  },
  data() {
    return {
      recordSelect: {},
      errors: {
        DiscountCode: "",
        DiscountName: "",
        ReducedAmount: "",
        InputNumber: "",
        ExpirationDate: "",
        Minimum: "",
      },
    };
  },
  methods: {
    /**
     * Hàm ẩn form chi tiết
     * @author: Nguyễn Văn Trúc (9/12/2023)
     */
    hideDetailRecord: function () {
      this.$emit("hideDetailRecord", false);
    },

    /**
     * Hàm Thực hiện valide dữ liệu
     * @author: Nguyễn Văn Trúc (9/12/2023)
     */
    validateData: function () {
      try {
        let me = this;
        let checkError = true;
        me.errors = {};
        // Kiểm tra record code
        if (me.common.validateInput(me.recordSelect.DiscountCode)) {
          me.errors.DiscountCode = "Mã giảm giá không được phép bỏ trống";
          checkError = false;
        }

        // Kiểm tra fullname
        if (me.common.validateInput(me.recordSelect.DiscountName)) {
          me.errors.DiscountName = "Tên mã giảm giá không được phép bỏ trống";
          checkError = false;
        }

        if (me.common.validateInput(me.recordSelect.ReducedAmount)) {
          me.errors.ReducedAmount = "Số tiền giảm không được phép bỏ trống";
          checkError = false;
        }

        if (me.common.validateInput(me.recordSelect.InputNumber)) {
          me.errors.InputNumber = "Số lần nhập không được phép bỏ trống";
          checkError = false;
        }

        if (me.common.validateInput(me.recordSelect.Minimum)) {
          me.errors.Minimum = "Đơn tối thiểu không được phép bỏ trống";
          checkError = false;
        }

        if (
          !me.common.validateInputDate(me.recordSelect.ExpirationDate) &&
          me.recordSelect.ExpirationDate !== undefined
        ) {
          me.errors.ExpirationDate =
            "Ngày hết hạn không được nhỏ hơn ngày hiện tại";
          checkError = false;
          console.log("OK");
        }

        return checkError;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm thêm hoặc sửa nhân viên
     * @author: Nguyễn Văn Trúc(13/3/2024)
     */
    async insertOfUpdateRecord() {
      console.log(this.recordSelect);
      try {
        let result = 0;
        if (this.validateData()) {
          if (this.statusCode === this.helper.Status.Insert) {
            this.recordSelect.CreatedDate = new Date();
            this.recordSelect.ModifiedDate = null;
            result = await this.apiService.post(
              "Discount/post",
              this.recordSelect
            );
          } else if (this.statusCode === this.helper.Status.Update) {
            this.recordSelect.ModifiedDate = new Date();
            result = await this.apiService.update(
              "Discount/put",
              this.recordSelect.DiscountId,
              this.recordSelect
            );
          }
          if (result === 1) {
            this.dialogSaveSuccess();
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Thực hiện lưu nhân viên và giữ form
     * @author: Nguyễn Văn Trúc (13/3/2024)
     */
    saveRecord() {
      try {
        this.insertOfUpdateRecord();
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm thực hiện sau khi thao tác(Thêm, Sửa) thành công
     * @author: Nguyễn Văn Trúc (9/12/2023)
     */
    dialogSaveSuccess() {
      try {
        // load lại dữ liệu
        this.hideDetailRecord();
        this.$emit("loadData", true);
      } catch (error) {
        console.log(error);
      }
    },
  },
  /**
   * Hàm Thực hiện khi có thay đổi về show
   * @author: Nguyễn Văn Trúc (9/12/2023)
   */
  watch: {
    async show(value) {
      if (value === true) {
        this.errors = {};
        this.recordSelect = JSON.parse(JSON.stringify(this.record));
      }
    },
  },

  /**
   * Hàm thực hiện nhận sự kiện bấm nút
   * @author: Nguyễn Văn Trúc (3/3/2024)
   */
  mounted() {
    let me = this;
    me.emitter.on(me.helper.Emitter.SendEvent, (value) => {
      // esc: Thoát form chi tiết nhân viên
      if (value === me.helper.Status.Exit && me.show === true) {
        me.hideDetailRecord();
      }

      // CTRL + S: Lưu và cất
      if (value === me.helper.Status.Save && me.show === true) {
        me.saveRecordAndExit();
      }
    });
  },
};
</script>
<style scoped></style>
