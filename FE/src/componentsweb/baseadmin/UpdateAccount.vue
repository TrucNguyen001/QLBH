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
          <h2>Thông tin tài khoản</h2>
          <button @click="hideDetailRecord" class="btn btn-close mx-4"></button>
        </div>
        <div class="form-body mx-5">
          <div class="d-flex justify-content-between">
            <div style="width: 45%">
              <MInput
                :inputErrorFirst="nameInputErrorFirst"
                nameInput="FullName"
                v-model="recordSelect.FullName"
                label="Họ và tên"
              ></MInput>
            </div>
            <div style="width: 45%">
              <MInput
                nameInput="Email"
                v-model="recordSelect.Email"
                label="Email"
                :required="true"
                :isError="this.errors.Email"
                @removeClassErrorInput="this.errors.Email = null"
              ></MInput>
            </div>
          </div>
          <div class="d-flex justify-content-between">
            <div style="width: 45%">
              <MInput
                nameInput="PhoneNumber"
                v-model="recordSelect.PhoneNumber"
                label="Số điện thoại"
                :isError="this.errors.PhoneNumber"
                @removeClassErrorInput="this.errors.PhoneNumber = null"
              ></MInput>
            </div>
            <div style="width: 45%">
              <MCheckBox
                v-model="recordSelect.Gender"
                label="Giới tính"
              ></MCheckBox>
            </div>
          </div>
          <div>
            <MInput
              nameInput="Address"
              v-model="recordSelect.Address"
              label="Địa chỉ"
            ></MInput>
          </div>
        </div>
        <div class="form-footer mx-5 px-0 mt-5 mb-4">
          <button @click="hideDetailRecord" class="btn btn-danger px-4">
            Thoát
          </button>
          <button @click="saveRecord" class="btn btn-success px-4">
            Cập nhật
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
  },
  data() {
    return {
      recordSelect: {},
      errors: {
        Email: "",
        PhoneNumber: "",
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

        // Kiểm tra email
        if (me.common.validateInput(me.recordSelect.Email)) {
          me.errors.Email = "Email không được phép bỏ trống";
          checkError = false;
        } else if (!me.common.checkEmail(me.recordSelect.Email)) {
          me.errors.Email = "Email không đúng định dạng";
          checkError = false;
        }

        if (
          me.recordSelect.PhoneNumber &&
          !me.common.checkPhoneNumber(me.recordSelect.PhoneNumber)
        ) {
          me.errors.PhoneNumber = "Số điện thoại không đúng định dạng";
          checkError = false;
        }

        return checkError;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Thực hiện lưu nhân viên và giữ form
     * @author: Nguyễn Văn Trúc (13/3/2024)
     */
    async saveRecord() {
      try {
        if (this.validateData()) {
          var result = await this.apiService.update(
            "Account/put",
            this.recordSelect.AccountId,
            this.recordSelect
          );
          console.log(result);
          if (this.recordSelect.FullName !== "") {
            localStorage.setItem("FullName", this.recordSelect.FullName);
          }
        }
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
