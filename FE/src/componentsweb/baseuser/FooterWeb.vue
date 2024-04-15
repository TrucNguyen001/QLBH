<template>
  <footer class="frame_footer bg-dark-subtle text-black">
    <div class="logo">
      <div class="name">
        <h1>Bamboo</h1>
        <div style="font-size: 20px; margin-top: 30px">
          Trang web bán điện thoại uy tín, chất lượng
        </div>
        <div style="font-size: 20px; margin-top: 5px">hàng đầu Việt Nam</div>
      </div>
    </div>
    <div
      style="
        border: 1px solid grey;
        height: 90%;
        margin-top: 10px;
        margin-left: -70px;
      "
    ></div>
    <div class="address">
      <div style="font-size: 20px; margin-bottom: 20px">Bạn cần hỗ trợ</div>
      <h2>1800 1009</h2>
      <div style="font-size: 17px; margin-top: 20px">
        <i
          >Địa chỉ: Số 298 đường Cầu Diễn, quận Bắc Từ Liêm, thành phố Hà Nội</i
        >
      </div>
      <div style="font-size: 17px; margin-top: 8px">
        <i>Email: support@gmail.com</i>
      </div>
      <div
        class="icon_x"
        style="
          margin-top: 10px;
          width: 150px;
          display: flex;
          justify-content: space-between;
        "
      >
        <i class="fa fa-facebook"></i>
        <i class="fa fa-youtube-play"></i>
        <i class="fa fa-google"></i>
        <i class="fa fa-github-alt"></i>
      </div>
    </div>
    <div class="support1">
      <div style="font-size: 20px; margin-bottom: 10px">Hỗ trợ mua hàng</div>
      <div style="margin-top: 8px">Trang chủ</div>
      <div style="margin-top: 8px">Danh mục</div>
      <div style="margin-top: 8px">Tìm kiếm</div>
      <div style="margin-top: 8px">Đăng nhập</div>
      <div style="margin-top: 8px">Hướng dẫn sử dụng</div>
    </div>
    <div class="support2">
      <div style="font-size: 20px; margin-bottom: 10px">Hỗ trợ khách hàng</div>
      <div style="margin-top: 8px">Trang chủ</div>
      <div style="margin-top: 8px">Danh mục</div>
      <div style="margin-top: 8px">Tìm kiếm</div>
      <div style="margin-top: 8px">Đăng nhập</div>
      <div style="margin-top: 8px">Hướng dẫn sử dụng</div>
    </div>
  </footer>
</template>

<script>
export default {};
</script>

<style scoped>
.frame_footer {
  height: 230px;
  display: flex;
  justify-content: space-between;
  font-size: 15px;
}

.frame_footer .logo {
  margin-left: 20px;
  width: 30%;
  margin-top: 35px;
}

.frame_footer .address {
  margin-top: 35px;
  margin-left: -20px;
}

.frame_footer .support1 {
  margin-top: 35px;
  font-size: 15px;
}

.frame_footer .support2 {
  margin-top: 35px;
  font-size: 15px;
  margin-right: 20px;
}
</style>
