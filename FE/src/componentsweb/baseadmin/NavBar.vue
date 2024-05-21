<template>
  <div class="navbar">
    <div :class="{ 'is-active': status === 0 }">
      <div>
        <i class="bi bi-columns"></i>
      </div>
      <div @click="this.status = 0" class="navbar-title">
        <router-link to="/admin/overview">Tổng quan</router-link>
      </div>
    </div>
    <div :class="{ 'is-active': status === 2 }">
      <div>
        <i class="bi bi-phone"></i>
      </div>
      <div @click="this.status = 2" class="navbar-title">
        <router-link to="/admin/product">Sản phẩm</router-link>
      </div>
    </div>
    <div v-if="role === 'Admin'" :class="{ 'is-active': status === 3 }">
      <div>
        <i class="bi bi-menu-app"></i>
      </div>
      <div @click="this.status = 3" class="navbar-title">
        <router-link to="/admin/product-type">Loại sản phẩm</router-link>
      </div>
    </div>
    <div v-else>
      <div>
        <i class="bi bi-menu-app"></i>
      </div>
      <div @click="messageNotAlow" class="navbar-title">Loại sản phẩm</div>
    </div>
    <div v-if="role === 'Admin'" :class="{ 'is-active': status === 4 }">
      <div>
        <i class="bi bi-truck"></i>
      </div>
      <div @click="this.status = 4" class="navbar-title">
        <router-link to="/admin/supplier">Nhà cung cấp</router-link>
      </div>
    </div>
    <div v-else>
      <div>
        <i class="bi bi-truck"></i>
      </div>
      <div @click="messageNotAlow" class="navbar-title">Nhà cung cấp</div>
    </div>
    <div :class="{ 'is-active': status === 5 }">
      <div>
        <i class="bi bi-ticket"></i>
      </div>
      <div @click="this.status = 5" class="navbar-title">
        <router-link to="/admin/discount-code">Mã giảm giá</router-link>
      </div>
    </div>
    <div :class="{ 'is-active': status === 6 }">
      <div>
        <i class="bi bi-person-check"></i>
      </div>
      <div @click="this.status = 6" class="navbar-title">
        <router-link to="/admin/user">Khách hàng</router-link>
      </div>
    </div>
    <div :class="{ 'is-active': status === 7 }">
      <div>
        <i class="bi bi-receipt"></i>
      </div>
      <div @click="this.status = 7" class="navbar-title">
        <router-link to="/admin/invoice">Đơn hàng</router-link>
      </div>
    </div>
    <div :class="{ 'is-active': status === 8 }">
      <div>
        <i class="bi bi-chat"></i>
      </div>
      <div @click="this.status = 8" class="navbar-title">
        <router-link to="/admin/feedback">Phản hồi</router-link>
      </div>
    </div>
    <div :class="{ 'is-active': status === 9 }">
      <div>
        <i class="bi bi-newspaper"></i>
      </div>
      <div @click="this.status = 9" class="navbar-title">
        <router-link to="/admin/news">Tin tức</router-link>
      </div>
    </div>
    <div style="height: 26%" class="navbar-bottom"></div>
  </div>
</template>
<script>
export default {
  name: "NavBar",
  data() {
    return {
      status: 0,
      role: "",
    };
  },
  methods: {
    messageNotAlow() {
      this.common.showToastError("Bạn không có quyền truy cập");
    },
  },
  created() {
    if (localStorage.getItem("Role")) {
      this.role = localStorage.getItem("Role");
    }
  },
};
</script>
<style scoped>
.navbar > div {
  padding-left: 12px;
}
.navbar > div a {
  text-decoration: none;
  color: #869ab8;
}
.is-active > div a {
  color: #ffffff;
}
.navbar > div a:hover {
  color: #ffffff;
}
.is-active {
  background: rgba(167, 196, 255, 0.16);
}
</style>
