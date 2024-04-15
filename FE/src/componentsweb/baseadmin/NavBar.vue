<template>
  <div class="navbar">
    <div>
      <div>
        <i class="bi bi-columns"></i>
      </div>
      <div class="navbar-title">
        <router-link to="/admin/overview">Tổng quan</router-link>
      </div>
    </div>
    <div>
      <div>
        <i class="bi bi-person-gear"></i>
      </div>
      <div class="navbar-title">
        <router-link to="/admin/employee">Nhân viên</router-link>
      </div>
    </div>
    <div>
      <div>
        <i class="bi bi-phone"></i>
      </div>
      <div class="navbar-title">
        <router-link to="/admin/product">Sản phẩm</router-link>
      </div>
    </div>
    <div>
      <div>
        <i class="bi bi-menu-app"></i>
      </div>
      <div class="navbar-title">
        <router-link to="/admin/product-type">Loại sản phẩm</router-link>
      </div>
    </div>
    <div>
      <div>
        <i class="bi bi-truck"></i>
      </div>
      <div class="navbar-title">
        <router-link to="/admin/supplier">Nhà cung cấp</router-link>
      </div>
    </div>
    <div>
      <div>
        <i class="bi bi-ticket"></i>
      </div>
      <div class="navbar-title">
        <router-link to="/admin/discount-code">Mã giảm giá</router-link>
      </div>
    </div>
    <div>
      <div>
        <i class="bi bi-person-check"></i>
      </div>
      <div class="navbar-title">
        <router-link to="/admin/user">Khách hàng</router-link>
      </div>
    </div>
    <div>
      <div>
        <i class="bi bi-receipt"></i>
      </div>
      <div class="navbar-title">
        <router-link to="/admin/invoice">Đơn hàng</router-link>
      </div>
    </div>
    <div>
      <div>
        <i class="bi bi-chat"></i>
      </div>
      <div class="navbar-title">
        <router-link to="/admin/feedback">Phản hồi</router-link>
      </div>
    </div>
    <div>
      <div>
        <i class="bi bi-newspaper"></i>
      </div>
      <div class="navbar-title">
        <router-link to="/admin/news">Tin tức</router-link>
      </div>
    </div>
    <div style="height: 26%" class="navbar-bottom"></div>
  </div>
</template>
<script>
export default {
  setup() {},
};
</script>
<style></style>
