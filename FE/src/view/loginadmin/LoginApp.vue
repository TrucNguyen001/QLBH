<template>
  <div class="img-background">
    <div class="language-nation">
      <div @click="showLanguage" class="change-language">
        <div
          :class="{
            icon_vn: resource.MLogin.VN === 'VN',
            icon_en: resource.MLogin.VN === 'EN',
          }"
          class="icon-nation"
        ></div>
        <div class="language">{{ resource.MLogin.Language }}</div>
      </div>
      <div v-show="showChangeLanguage" class="select-language">
        <div @click="changeLanguageVN">Tiếng Việt</div>
        <div @click="changeLanguageEN">English</div>
      </div>
    </div>
    <div>
      <div class="form-login">
        <div class="d-flex justify-content-center mb-4" style="font-size: 36px">
          Đăng nhập
        </div>
        <div class="form-login-body">
          <div class="login-account">
            <input
              placeholder="Nhập tài khoản"
              class="m-input"
              type="text"
              v-model="accountLogin.Account"
              ref="input_account"
              id="account"
              @click="common.blackenInput('account')"
            />
            <p v-show="errors.account !== null" class="error-account">
              {{ errors.account }}
            </p>
          </div>
          <div class="login-password mt-4">
            <div style="position: relative" class="input-icon">
              <div
                @click="togglePassword"
                class="icon-pass icon-show-pass"
                :class="{
                  icon_show_pass: iconPass === false,
                  icon_hide_pass: iconPass === true,
                }"
              ></div>
              <input
                placeholder="Nhập mật khẩu"
                class="m-input"
                :type="isPassword ? 'password' : 'text'"
                v-model="accountLogin.PassWord"
                id="password"
                @keydown.enter="login"
                @click="common.blackenInput('password')"
              />
              <p v-show="errors.password !== null" class="error-password">
                {{ errors.password }}
              </p>
            </div>
          </div>
          <div class="d-flex justify-content-between">
            <div
              @click="recoverPassword"
              :class="{ is_error_password: errors.password !== null }"
              class="forget-password"
            >
              Quên mật khẩu?
            </div>
          </div>
          <div @click="login" class="login">Đăng nhập</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "LoginApp",
  data() {
    return {
      account: "",
      password: "",
      iconPass: true,
      isPassword: true,
      errors: {
        account: null,
        password: null,
      },
      accountLogin: {},
      showChangeLanguage: false,
    };
  },
  methods: {
    /**
     * Lấy lại mật khẩu
     * @author Nguyễn Văn Trúc (10/3/2024)
     */
    recoverPassword() {
      this.$router.push(this.helper.Router.ForgetPassword);
    },
    /**
     * Ẩn hiển mật khẩu
     * @author Nguyễn Văn Trúc (16/2/2024)
     */
    togglePassword() {
      this.iconPass = !this.iconPass;
      this.isPassword = !this.isPassword;
    },

    /**
     * Kiểm tra tài khoản mật khẩu đã nhập chưa
     * @author Nguyễn Văn Trúc (17/2/2024)
     */
    checkAccount() {
      let me = this;
      let check = true;
      if (me.accountLogin.Account === "") {
        me.errors.account = me.resource.ErrorLogin.AccountNotEmpty;
        check = false;
      } else {
        me.errors.account = null;
      }
      if (me.accountLogin.PassWord === "") {
        me.errors.password = me.resource.ErrorLogin.PasswordNotEmpty;
        check = false;
      } else {
        me.errors.password = null;
      }
      return check;
    },

    /**
     * Thay đổi thành tiếng việt
     * @author Nguyễn Văn Trúc (16/3/2024)
     */
    changeLanguageVN() {
      localStorage.setItem("language", "VN");
      this.showChangeLanguage = false;
      this.$router.go();
    },
    /**
     * Thay đổi thành tiếng anh
     * @author Nguyễn Văn Trúc (16/3/2024)
     */
    changeLanguageEN() {
      localStorage.setItem("language", "EN");
      this.showChangeLanguage = false;
      this.$router.go();
    },
    /**
     * Hiển thị chọn ngôn ngữ
     * @author Nguyễn Văn Trúc (16/3/2024)
     */
    showLanguage() {
      this.showChangeLanguage = !this.showChangeLanguage;
    },
    /**
     * Hàm đăng nhập
     * @author Nguyễn Văn Trúc (17/2/2024)
     */
    async login() {
      try {
        let me = this;
        me.common.showLoading();
        me.checkAccount();
        if (me.checkAccount() === true) {
          let response = await me.apiService.post(
            "Account/login/Admin",
            me.accountLogin
          );
          localStorage.setItem("token", response.Model.AccessToken);
          localStorage.setItem("refreshToken", response.Model.RefreshToken);
          localStorage.setItem("expiration", response.Model.Expiration);
          localStorage.setItem("isLogin", true);
          localStorage.setItem("account", this.accountLogin.Account);
          localStorage.setItem("FullName", response.FullName);
          localStorage.setItem("AccountId", response.AccountId);
          let email = response.Email === null ? "" : response.Email;
          localStorage.setItem("Email", email);

          // Phân tích AccessToken thành các phần tử
          let tokenParts = localStorage.getItem("token").split(".");
          let payload = JSON.parse(atob(tokenParts[1]));

          // Truy cập thông tin từ payload của token
          let userName = payload.UserName;
          let role = payload.Roles;
          let accountCode = payload.AccountCode;
          localStorage.setItem("userName", userName);
          localStorage.setItem("AccountCode", accountCode);
          localStorage.setItem("Role", role);

          this.$router.push("/admin/overview");
        }
        me.common.showLoading(false);
      } catch (error) {
        console.log(error);
      } finally {
        this.common.showLoading(false);
      }
    },
    /**
     * auto focus vào ô đầu tiên khi trang hiển thị
     * @author: Nguyễn Văn Trúc (3/3/2024)
     */
    autoFocus: function () {
      try {
        this.$refs.input_account.focus();
      } catch (error) {
        console.log(error);
      }
    },
  },
  /**
   * Tạo đối tượng đăng nhập
   * @author Nguyễn Văn Trúc (18/2/2024)
   */
  created() {
    this.accountLogin = {
      Account: "",
      PassWord: "",
    };

    localStorage.setItem("account", "");
    localStorage.setItem("FullName", "");
    localStorage.setItem("AccountId", "");

    localStorage.setItem("token", "");
    localStorage.setItem("refreshToken", "");
    localStorage.setItem("expiration", "");
    localStorage.setItem("userName", "");
    localStorage.setItem("isLogin", false);
    localStorage.setItem("Email", "");

    // Gán email cho accout sau khi lấy lại mật khẩu xong
    // if (localStorage.getItem("email")) {
    //   this.accountLogin.Account = localStorage.getItem("email");
    // }
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
</style>
