<template>
  <div class="img-background">
    <div class="language-nation"></div>
    <div>
      <div v-if="type === 'login'" class="form-login">
        <div class="d-flex justify-content-center mb-4" style="font-size: 36px">
          Đăng nhập
        </div>
        <CKEditor></CKEditor>
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
            <div
              class="forget-password"
              :class="{ is_error_password: errors.password !== null }"
              @click="this.type = 'register'"
            >
              Đăng ký tài khoản
            </div>
          </div>
          <div @click="login" class="login">Đăng nhập</div>
        </div>
        <div class="form-login-footer">
          <div class="login-with">
            <div class="line-right"></div>
            <div class="form-login-footer-title">Hoặc</div>
            <div class="line-right"></div>
          </div>
          <div class="icon-logo-login">
            <button
              style="height: 40px"
              class="btn btn-light d-flex align-items-center px-5"
              @click="loginWithGoogle"
            >
              <i style="font-size: 24px" class="bi bi-google mx-2"></i>
              <p class="mt-3">Đăng nhập với Google</p>
            </button>
          </div>
        </div>
      </div>
      <div
        style="padding: 20px 48px 40px 48px; width: 600px"
        v-if="type === 'register'"
        class="form-login"
      >
        <div class="d-flex justify-content-center mb-4" style="font-size: 36px">
          Đăng ký
        </div>
        <div class="form-login-body">
          <div class="login-account">
            <input
              placeholder="Nhập tài khoản"
              class="m-input"
              type="text"
              v-model="accountRegister.UserName"
              ref="input_account"
              id="account"
              @click="common.blackenInput('account')"
            />
            <p v-show="errorsRegister.account !== null" class="error-account">
              {{ errorsRegister.account }}
            </p>
          </div>
          <div class="login-account mt-4">
            <input
              placeholder="Nhập địa chỉ email"
              class="m-input"
              type="text"
              v-model="accountRegister.Email"
              ref="input_email"
              id="email"
              @click="common.blackenInput('email')"
            />
            <p v-show="errorsRegister.email !== null" class="error-email">
              {{ errorsRegister.email }}
            </p>
          </div>
          <div class="login-password mt-4">
            <div style="position: relative" class="input-icon w-100">
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
                v-model="accountRegister.PassWord"
                id="password"
                @keydown.enter="login"
                @click="common.blackenInput('password')"
              />
              <p
                v-show="errorsRegister.password !== null"
                class="error-password"
              >
                {{ errorsRegister.password }}
              </p>
            </div>
          </div>
          <div class="login-password mt-4">
            <div style="position: relative" class="input-icon w-100">
              <div
                @click="togglePasswordConfirm"
                class="icon-pass icon-show-pass"
                :class="{
                  icon_show_pass: iconPassRegister === false,
                  icon_hide_pass: iconPassRegister === true,
                }"
              ></div>
              <input
                placeholder="Nhập xác nhận mật khẩu"
                class="m-input"
                :type="isPasswordConfirm ? 'password' : 'text'"
                v-model="confirmNewPassword"
                id="confirmPassword"
                @keydown.enter="login"
                @click="common.blackenInput('confirmPassword')"
              />
              <p
                v-show="errorsRegister.confirmPassword !== null"
                class="error-password"
              >
                {{ errorsRegister.confirmPassword }}
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
            <div
              class="forget-password"
              :class="{ is_error_password: errors.password !== null }"
              @click="this.type = 'login'"
            >
              Quay lại đăng nhập
            </div>
          </div>
          <div @click="register" class="login">Đăng ký tài khoản</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
//import passwordHash from "pbkdf2-password-hash";
export default {
  name: "LoginApp",
  data() {
    return {
      type: "login",
      account: "",
      password: "",
      iconPass: true,
      iconPassRegister: true,
      isPassword: true,
      isPasswordConfirm: true,
      errors: {
        account: null,
        password: null,
      },
      confirmNewPassword: "",
      errorsRegister: {
        account: null,
        password: null,
        confirmPassword: null,
        email: null,
      },
      accountLogin: {},
      accountRegister: {},
      showChangeLanguage: false,
    };
  },
  methods: {
    async loginWithGoogle() {
      try {
        // Đăng nhập bằng Google
        const user = await this.$gAuth.signIn();

        this.common.showLoading();
        const idUser = user.Ca;

        // Ví dụ: lấy email của người dùng
        const userEmail = user.getBasicProfile().getEmail();

        this.password = await this.apiService.getByInfo(
          "Account/GetByUserName",
          idUser
        );

        localStorage.setItem("FullName", userEmail);

        if (this.password !== "") {
          console.log(this.password);
          this.accountLogin = {
            Account: idUser,
            Password: this.password,
          };
          this.callApiLogin();
        } else {
          this.accountRegister = {
            UserName: idUser,
            Password: userEmail,
            Email: userEmail,
          };

          let register = await this.apiService.post(
            "Account/register",
            this.accountRegister
          );

          if (register === 1) {
            this.accountLogin = {
              Account: idUser,
              Password: userEmail,
            };
            this.callApiLogin();
          }
        }

        this.common.showLoading(false);
        this.$router.push("/");
      } catch (error) {
        // Xảy ra lỗi trong quá trình đăng nhập
        console.error("Đăng nhập thất bại:", error);
      }
    },

    callback(response) {
      console.log("Handle the response", response);
    },
    async register() {
      if (this.checkAccountRegister()) {
        let result = await this.apiService.post(
          "Account/register",
          this.accountRegister
        );
        if (result === 1) {
          this.type = "login";
        }
      }
    },
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

    togglePasswordConfirm() {
      this.iconPassRegister = !this.iconPassRegister;
      this.isPasswordConfirm = !this.isPasswordConfirm;
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
     * Kiểm tra validate khi đăng ký
     */
    checkAccountRegister() {
      let check = true;
      this.errorsRegister = {};
      if (this.common.validateInput(this.accountRegister.UserName)) {
        this.errorsRegister.account = "Tài khoản không được phép bỏ trống";
        check = false;
      }

      if (this.common.validateInput(this.accountRegister.PassWord)) {
        this.errorsRegister.password = "Mật khẩu không được phép bỏ trống";
        check = false;
      } else {
        if (!this.common.checkFormatPassword(this.accountRegister.PassWord)) {
          this.errorsRegister.password =
            this.resource.MForgetPassword.ErrorValidPassword;
          check = false;
        } else {
          this.errorsRegister.password = "";
        }
      }
      if (this.confirmNewPassword === "") {
        this.errorsRegister.confirmPassword =
          "Xác nhận mật khẩu không được phép bỏ trống";
        check = false;
      } else {
        if (this.accountRegister.PassWord !== this.confirmNewPassword) {
          this.errorsRegister.confirmPassword =
            this.resource.MForgetPassword.PasswordDifferent;
          check = false;
        } else {
          this.errorsRegister.confirmPassword = "";
        }
      }

      if (this.common.validateInput(this.accountRegister.Email)) {
        this.errorsRegister.email = "Email không được phép bỏ trống";
        this.check = false;
      } else if (!this.common.checkEmail(this.accountRegister.Email)) {
        this.errorsRegister.email = "Email không đúng định dạng";
        check = false;
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
    async callApiLogin() {
      let me = this;
      let response = await me.apiService.post(
        "Account/login/User",
        me.accountLogin
      );
      localStorage.setItem("token", response.Model.AccessToken);
      localStorage.setItem("refreshToken", response.Model.RefreshToken);
      localStorage.setItem("expiration", response.Model.Expiration);
      localStorage.setItem("isLogin", true);
      localStorage.setItem("account", this.accountLogin.Account);
      localStorage.setItem("FullName", response.FullName);
      localStorage.setItem("AccountId", response.AccountId);
      localStorage.setItem("Email", response.Email);

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

      this.$router.push("/");
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
          me.callApiLogin();
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
    // passwordHash.hash("password").then((hash) => {
    //   console.log(hash);
    // });

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
