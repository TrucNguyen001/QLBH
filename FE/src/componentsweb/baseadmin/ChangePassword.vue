<template>
  <div class="curtain" v-if="show">
    <div style="padding: 20px 48px 40px 48px; width: 600px" class="form-login">
      <div class="d-flex justify-content-center mb-4" style="font-size: 36px">
        Đăng ký
      </div>
      <div class="form-login-body">
        <div class="login-account mt-4">
          <label class="mb-2" for="">Nhập mật khẩu cũ</label>
          <div style="position: relative" class="input-icon w-100">
            <div
              @click="togglePassword1"
              class="icon-pass icon-show-pass"
              :class="{
                icon_show_pass: iconPass1 === false,
                icon_hide_pass: iconPass1 === true,
              }"
            ></div>
            <input
              placeholder="Nhập mật khẩu cũ"
              class="m-input"
              :type="iconPass1 ? 'password' : 'text'"
              v-model="password1"
              id="password1"
              @keydown.enter="login"
              @click="common.blackenInput('password1')"
            />
            <p v-show="errors.password1 !== null" class="error-password">
              {{ errors.password1 }}
            </p>
          </div>
        </div>
        <div class="login-password mt-4">
          <label class="mb-2" for="">Nhập mật khẩu mới</label>
          <div style="position: relative" class="input-icon w-100">
            <div
              @click="togglePassword2"
              class="icon-pass icon-show-pass"
              :class="{
                icon_show_pass: iconPass2 === false,
                icon_hide_pass: iconPass2 === true,
              }"
            ></div>
            <input
              placeholder="Nhập mật khẩu mới"
              class="m-input"
              :type="iconPass2 ? 'password' : 'text'"
              v-model="password2"
              id="password2"
              @keydown.enter="login"
              @click="common.blackenInput('password2')"
            />
            <p v-show="errors.password2 !== null" class="error-password">
              {{ errors.password2 }}
            </p>
          </div>
        </div>
        <div class="login-password mt-4 mb-5">
          <label class="mb-2" for="">Nhập lại mật khẩu mới</label>
          <div style="position: relative" class="input-icon w-100">
            <div
              @click="togglePassword3"
              class="icon-pass icon-show-pass"
              :class="{
                icon_show_pass: iconPass3 === false,
                icon_hide_pass: iconPass3 === true,
              }"
            ></div>
            <input
              placeholder="Nhập lại mật khẩu mới"
              class="m-input"
              :type="iconPass3 ? 'password' : 'text'"
              v-model="password3"
              id="password3"
              @keydown.enter="login"
              @click="common.blackenInput('password3')"
            />
            <p v-show="errors.password3 !== null" class="error-password">
              {{ errors.password3 }}
            </p>
          </div>
        </div>
        <div @click="updatePassword" class="login">Thay đổi mật khẩu</div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "LoginApp",
  props: {
    show: Boolean,
    record: Object,
  },
  data() {
    return {
      password1: "",
      password2: "",
      password3: "",
      iconPass1: true,
      iconPass2: true,
      iconPass3: true,
      errors: {
        password1: null,
        password2: null,
        password3: null,
      },
      account: {},
    };
  },
  methods: {
    togglePassword1() {
      this.iconPass1 = !this.iconPass1;
    },
    togglePassword2() {
      this.iconPass2 = !this.iconPass2;
    },
    togglePassword3() {
      this.iconPass3 = !this.iconPass3;
    },
    async updatePassword() {
      if (this.checkPassword()) {
        this.account.Password = this.password2;
        await this.apiService.update(
          "Account/put",
          this.account.AccountId,
          this.account
        );
        this.$router.go();
      }
    },

    /**
     * Kiểm tra validate khi đăng ký
     */
    checkPassword() {
      let check = true;
      this.errors = {};
      if (this.common.validateInput(this.password1)) {
        this.errors.password1 = "Mật khẩu cũ không được phép bỏ trống";
        check = false;
      } else if (this.password1 !== this.account.Password) {
        this.errors.password1 =
          "Mật khẩu cũ không chính xác. Vui lòng kiểm tra lại";
      }
      console.log(this.account.Password);

      if (this.common.validateInput(this.password2)) {
        this.errors.password2 = "Mật khẩu mới không được phép bỏ trống";
        check = false;
      } else {
        if (!this.common.checkFormatPassword(this.password2)) {
          this.errors.password2 =
            this.resource.MForgetPassword.ErrorValidPassword;
          check = false;
        } else {
          this.errors.password2 = "";
        }
      }
      if (this.common.validateInput(this.password3)) {
        this.errors.password3 = "Xác nhận mật khẩu không được phép bỏ trống";
        check = false;
      } else {
        if (this.password2 !== this.password3) {
          this.errors.password3 =
            this.resource.MForgetPassword.PasswordDifferent;
          check = false;
        } else {
          this.errors.password3 = "";
        }
      }
      return check;
    },

    autoFocus: function () {
      try {
        //this.$refs.input_account.focus();
      } catch (error) {
        console.log(error);
      }
    },
  },

  mounted() {
    let me = this;
    this.autoFocus();
    // Khi có lỗi api
    me.emitter.on("errorLogin", (value) => {
      me.common.showLoading(false);
      me.errors.password = value;
    });
  },
  watch: {
    async show(value) {
      if (value === true) {
        this.errors = {};
        this.account = JSON.parse(JSON.stringify(this.record));
      }
    },
  },
};
</script>

<style scoped>
.is_error_password {
  margin-top: 30px;
}
.error-account,
.error-password,
.error-email {
  font-size: 12px;
  color: #ff1d1d;
  height: 20px;
  line-height: 20px;
  position: absolute;
}
.error-password {
  top: 50px;
}
.icon_show_pass {
  background: url("../../assets/img/icon-show-pass.svg") no-repeat;
}
.icon_hide_pass {
  background: url("../../assets/img/icon-hide-pass.svg") no-repeat;
}
.form-login {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -55%);
}
</style>
