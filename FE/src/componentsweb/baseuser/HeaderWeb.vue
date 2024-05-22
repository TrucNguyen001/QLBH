<template>
  <div
    class="container-fluid fixed-top"
    style="height: 100px; color: white; background: #f06666"
  >
    <header class="row h-100 d-flex align-items-center">
      <div style="border-radius: 8px" class="col-md-1 logo-baboo"></div>
      <div class="col-md-1">
        <router-link to="/">Trang chủ</router-link>
      </div>
      <div class="col-md-1">
        <router-link to="/user/list-product">Sản phẩm</router-link>
      </div>
      <div class="col-md-1">
        <router-link to="/user/news">Tin tức</router-link>
      </div>
      <div class="col-md-1">
        <router-link to="/user/info">Giới thiệu</router-link>
      </div>
      <div class="col-md-4">
        <div style="margin-left: 100px" class="">
          <div class="input-icon">
            <input
              v-model="textSearch"
              type="text"
              class="m-input m-input-icon"
              @keydown.enter="searchRecord"
            />
            <div class="icon-search">
              <i @click="searchRecord" class="bi bi-search"></i>
            </div>
          </div>
        </div>
      </div>
      <div class="col-md-2 d-flex justify-content-between" style="width: 320px">
        <div class="">
          <router-link to="/user/cart"><i class="bi bi-cart3"></i></router-link>
        </div>
        <div v-if="fullName.length > 0" class="">
          {{ fullName }}
          <i
            @click="toggleSetting"
            style="font-size: 22px"
            class="bi bi-person-gear mx-2"
          ></i>
          <div
            v-if="isSetting"
            class="setting shadow p-3 mb-5 bg-body rounded text-black"
          >
            <div>
              <i style="font-size: 36px" class="bi bi-person-circle"></i>
            </div>
            <div>{{ fullName }}</div>
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
        <div v-else class="text-white">
          <router-link to="/login-user">Đăng nhập</router-link>
        </div>
      </div>
    </header>
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
import UpdateAccount from "../baseadmin/UpdateAccount.vue";
import ChangePassword from "../baseadmin/ChangePassword.vue";
export default {
  components: { UpdateAccount, ChangePassword },
  data() {
    return {
      fullName: "",
      textSearch: "",
      isSetting: false,
      email: "",
      isShow: false,
      isShowChangePassword: false,
      recordSelected: {},
    };
  },
  methods: {
    logout() {
      this.$router.push("/login-user");
    },
    async searchRecord() {
      // if (this.textSearch.trim().length > 0) {
      //   await sessionStorage.setItem("TextSearch", this.textSearch);
      //   this.emitter.emit("Search", this.textSearch);
      //   this.$router.push("/user/list-product");
      // } else {
      //   this.common.showToastError("Nhập nội dung để có thể tìm kiếm");
      // }
      await sessionStorage.setItem("TextSearch", this.textSearch);
      this.emitter.emit("Search", this.textSearch);
      this.$router.push("/user/list-product");
    },
    /**
     * Ẩn hiện đăng xuất
     * @author Nguyễn Văn Trúc(1/3/2024)
     */
    toggleSetting() {
      this.isSetting = !this.isSetting;
    },
    /**
     * Thông tin chi tiết bản ghi
     * @param {bản ghi} record
     * @author: Nguyễn Văn Trúc (2/4/2024)
     */
    async detailInfo() {
      try {
        this.isSetting = false;
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
        this.isSetting = false;
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
      this.$router.push("/login-user");
    },
  },
  created() {
    if (localStorage.getItem("FullName")) {
      this.fullName = localStorage.getItem("FullName");
    }
    if (localStorage.getItem("Email")) {
      this.email = localStorage.getItem("Email");
    }
  },
};
</script>

<style>
.logo-baboo {
  background: url("../../assets/img/logo/bamboo-font-icon-text-design-600nw-1504461878.webp")
    no-repeat;
  background-position: center;
  background-size: contain;
  height: 60px;
  width: 80px;
}
input {
  border-bottom-right-radius: 0;
  border-top-right-radius: 0;
}
.setting {
  position: absolute;
  margin-left: -50px;
  background: #ffffff;
  width: 280px;
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
header a {
  text-decoration: none;
  color: #ffffff;
}
</style>
