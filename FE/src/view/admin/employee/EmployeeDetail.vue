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
          <h2>Thông tin nhà cung cấp</h2>
          <button @click="hideDetailRecord" class="btn btn-close mx-4"></button>
        </div>
        <div class="form-body mx-5">
          <div class="d-flex justify-content-between">
            <div style="width: 45%">
              <label class="mb-2 mt-3" for="">Mã tài khoản</label>
              <input
                readonly
                class="m-input"
                v-model="recordSelect.AccountCode"
                type="text"
              />
            </div>
            <div style="width: 45%">
              <label class="mb-2 mt-3" for="">Họ và tên</label>
              <input
                readonly
                class="m-input"
                v-model="recordSelect.FullName"
                type="text"
              />
            </div>
          </div>
          <div class="d-flex justify-content-between">
            <div style="width: 45%">
              <label class="mb-2 mt-3" for="">Tài khoản</label>
              <input
                readonly
                class="m-input"
                v-model="recordSelect.UserName"
                type="text"
              />
            </div>
            <div style="width: 45%">
              <label class="mb-2 mt-3" for="">Mật khẩu</label>
              <input
                readonly
                class="m-input"
                v-model="recordSelect.Password"
                type="text"
              />
            </div>
          </div>
          <div class="d-flex justify-content-between">
            <div style="width: 45%">
              <label class="mb-2 mt-3" for="">Email</label>
              <input
                readonly
                class="m-input"
                v-model="recordSelect.Email"
                type="text"
              />
            </div>
            <div style="width: 45%">
              <label class="mb-2 mt-3" for="">Số điện thoại</label>
              <input
                readonly
                class="m-input"
                v-model="recordSelect.PhoneNumber"
                type="text"
              />
            </div>
          </div>
          <div class="d-flex justify-content-between">
            <div style="width: 45%">
              <label class="mb-2 mt-3" for="">Giới tính</label>
              <input
                readonly
                class="m-input"
                v-model="recordSelect.Gender"
                type="text"
              />
            </div>
            <div style="width: 45%">
              <label class="mb-2 mt-3" for="">Trạng thái</label>
              <input
                readonly
                class="m-input"
                v-model="recordSelect.Status"
                type="text"
              />
            </div>
          </div>
          <div class="d-flex justify-content-between">
            <div style="width: 45%">
              <label class="mb-2 mt-3" for="">Ngày tạo</label>
              <input
                readonly
                class="m-input"
                v-model="recordSelect.CreatedDate"
                type="text"
              />
            </div>
            <div style="width: 45%">
              <label class="mb-2 mt-3" for="">Ngày sửa</label>
              <input
                readonly
                class="m-input"
                v-model="recordSelect.ModifiedDate"
                type="text"
              />
            </div>
          </div>
          <div>
            <label class="mb-2 mt-3" for="">Địa chỉ</label>
            <input
              readonly
              class="m-input"
              v-model="recordSelect.Address"
              type="text"
            />
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
        UserCode: "",
        UserName: "",
        Email: "",
        PhoneNumber: "",
        Address: "",
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
        if (me.common.validateInput(me.recordSelect.UserCode)) {
          me.errors.UserCode = "Mã nhà cung cấp không được phép bỏ trống";
          checkError = false;
        }

        // Kiểm tra fullname
        if (me.common.validateInput(me.recordSelect.UserName)) {
          me.errors.UserName = "Tên nhà cung cấp không được phép bỏ trống";
          checkError = false;
        }

        // Kiểm tra email
        if (!me.common.checkEmail(me.recordSelect.Email)) {
          me.errors.Email = "Email không đúng định dạng";
          checkError = false;
        }

        // Kiểm tra số điện thoại
        if (me.common.validateInput(me.recordSelect.PhoneNumber)) {
          me.errors.PhoneNumber = "Số điện thoại không được phép bỏ trống";
          checkError = false;
        }

        // Kiểm tra địa chỉ
        if (me.common.validateInput(me.recordSelect.PhoneNumber)) {
          me.errors.Address = "Địa chỉ không được phép bỏ trống";
          checkError = false;
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
            result = await this.apiService.post("User/post", this.recordSelect);
          } else if (this.statusCode === this.helper.Status.Update) {
            this.recordSelect.ModifiedDate = new Date();
            result = await this.apiService.update(
              "User/put",
              this.recordSelect.UserId,
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
