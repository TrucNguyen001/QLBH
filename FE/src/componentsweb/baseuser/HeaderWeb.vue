<template>
  <div
    class="container-fluid bg-primary-subtle fixed-top my-auto"
    style="height: 128px; color: white"
  >
    <header class="row h-100">
      <div class="col-md-1 logo-baboo my-auto m-lg-4"></div>
      <div class="col-md-1 my-auto">
        <router-link to="/user/home">Trang chủ</router-link>
      </div>
      <div class="col-md-1 my-auto">
        <router-link to="/user/list-product">Sản phẩm</router-link>
      </div>
      <div class="col-md-1 my-auto">Tin tức</div>
      <div class="col-md-1 my-auto">Giới thiệu</div>
      <div class="col-md-4 my-auto">
        <div style="margin-left: 100px" class="">
          <div class="input-icon">
            <input type="text" class="m-input m-input-icon" />
            <div class="icon-search">
              <i class="bi bi-search"></i>
            </div>
          </div>
        </div>
      </div>
      <div class="col-md-2 my-auto d-flex justify-content-between">
        <div v-if="fullName.length > 0" class="">
          {{ fullName }}
          <i style="margin-left: 4px" class="bi bi-box-arrow-in-left"></i>
        </div>
        <div v-else class="text-white">
          <router-link to="/login">Đăng nhập</router-link>
        </div>
        <div class="">
          <router-link to="/user/cart"><i class="bi bi-cart3"></i></router-link>
        </div>
      </div>
    </header>
  </div>
</template>

<script>
export default {
  data() {
    return {
      fullName: "",
    };
  },
  created() {
    if (localStorage.getItem("FullName")) {
      this.fullName = localStorage.getItem("FullName");
    }
  },
};
</script>

<style>
.logo-baboo {
  background: url("../../assets/img/bamboo-logo.jpg") no-repeat;
  background-position: center;
  background-size: contain;
  height: 80px;
  width: 80px;
  border-radius: 10px;
}
</style>
