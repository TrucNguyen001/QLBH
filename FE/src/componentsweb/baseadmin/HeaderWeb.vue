<template>
  <div class="header">
    <div class="header-left">
      <div>
        <i
          style="color: white; margin-left: 10px"
          class="bi bi-grid-3x3-gap"
        ></i>
      </div>
      <div class="icon-logo"></div>
      <div class="name">Bamboo</div>
    </div>
    <div class="header-right">
      <div class="header-right-start">
        <div>
          <i
            style="font-size: 24px; margin-left: 24px; margin-right: 12px"
            class="bi bi-list-task"
          ></i>
        </div>
        <div class="name-company">
          Trang web cung cấp đồ điện tử số 1 Việt Nam
        </div>
      </div>
      <div class="header-right-end">
        <div class="mx-2"><i class="bi bi-bell"></i></div>
        <div class="mx-2">{{ userName }}</div>
        <div class="mx-2" style="width: 50px">
          <i
            @click="toggleSetting"
            style="font-size: 22px"
            class="bi bi-person-gear"
          ></i>
          <div
            v-if="isSetting"
            class="setting shadow p-3 mb-5 bg-body rounded text-black"
          >
            <div>
              <i style="font-size: 36px" class="bi bi-person-circle"></i>
            </div>
            <div>{{ userName }}</div>
            <div v-if="email !== ''">
              {{ email }}
            </div>
            <div class="text-danger" v-else>Chưa cập nhật email</div>
            <div @click="detailInfo" class="canClick">
              Cập nhật <i class="bi bi-gear mx-2"></i>
            </div>
            <div @click="changePassword" class="canClick">
              Thay đổi mật khẩu <i class="bi bi-lock-fill mx-2"></i>
            </div>
            <div @click="logout" class="canClick">
              Đăng xuất <i class="bi bi-box-arrow-right mx-2"></i>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <update-account
    :show="isShow"
    :record="recordSelected"
    @loadData="loadData"
    @hideDetailRecord="hideDetailRecord"
  ></update-account>
  <change-password
    :show="isShowChangePassword"
    :record="recordSelected"
    @loadData="loadData"
    @hideDetailRecord="hideChangePassword"
  ></change-password>
</template>
<script>
import UpdateAccount from "./UpdateAccount.vue";
import ChangePassword from "./ChangePassword.vue";
export default {
  name: "HeaderWeb",
  components: { UpdateAccount, ChangePassword },
  data() {
    return {
      userName: "Admin",
      isHideLogout: true,
      showLoading: false,
      email: "",
      isSetting: false,
      isShow: false,
      isShowChangePassword: false,
      recordSelected: {},
    };
  },
  methods: {
    logout() {
      this.$router.push("/login-admin");
    },
    hideChangePassword() {
      this.isShowChangePassword = false;
    },
    /**
     * Thông tin chi tiết bản ghi
     * @param {bản ghi} record
     * @author: Nguyễn Văn Trúc (2/4/2024)
     */
    async detailInfo() {
      try {
        let record = await this.apiService.getByInfo(
          "Account/getById",
          localStorage.getItem("AccountId")
        );
        this.isShow = true;

        this.recordSelected = record;
      } catch (error) {
        console.log(error);
      }
    },
    async changePassword() {
      try {
        let record = await this.apiService.getByInfo(
          "Account/getById",
          localStorage.getItem("AccountId")
        );
        this.isShowChangePassword = true;

        this.recordSelected = record;
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Hàm Thực hiện khi huỷ hoặc thoát form cập nhật
     * @author: Nguyễn Văn Trúc (9/12/2023)
     */
    hideDetailRecord: function () {
      this.isShow = false;
    },
    /**
     * Chuyển đến trang đăng nhập
     * @author Nguyễn Văn Trúc(18/2/2024)
     */
    loginPage() {
      this.$router.push("/login");
    },
    /**
     * Ẩn hiện đăng xuất
     * @author Nguyễn Văn Trúc(1/3/2024)
     */
    toggleSetting() {
      this.isSetting = !this.isSetting;
    },
    /**
     * Đăng xuất tài khoản
     * @author Nguyễn Văn Trúc(1/3/2024)
     */
    logoutAccount() {
      this.showLoading = true;
      this.isHideLogout = true;
      this.removeRefreshToken();
    },

    /**
     * Xoá refresh token khi đăng xuất
     * @author: Nguyễn Văn Trúc(2/3/2024)
     */
    async removeRefreshToken() {
      try {
        await this.apiService.getByInfo(this.helper.MApi.Login, this.userName);
        this.showLoading = false;
        this.$router.push("/login");
      } catch (error) {
        console.log(error);
      }
    },
  },
  /**
   * Lấy giá trị từ local
   * @author Nguyễn Văn Trúc(18/2/2024)
   */
  created() {
    this.userName = localStorage.getItem("FullName");
    if (localStorage.getItem("Email")) {
      this.email = localStorage.getItem("Email");
    }
  },
};
</script>
<style scoped>
.setting {
  position: absolute;
  margin-left: -170px;
  background: #ffffff;
  width: 200px;
  z-index: 999;
  padding: 12px;
  border-radius: 4px;
}
.setting > div {
  height: 40px;
  display: flex;
  justify-content: center;
  align-items: center;
}
.setting > .canClick:hover {
  background: #e0e0e0;
  cursor: pointer;
  border-radius: 4px;
}
</style>
