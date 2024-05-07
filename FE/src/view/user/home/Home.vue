<template>
  <div class="body-home col-lg-11 m-xl-auto">
    <div class="banner">
      <img src="../../../assets/img/banner1.png" />
    </div>
    <div class="super-discount mt-5">
      <div>
        <h1 style="font-size: 45px; color: red">Siêu giảm giá</h1>
      </div>
      <hr />
      <div class="auto_wrap" style="width: 100%">
        <div class="d-flex justify-content-center w-100">
          <div
            v-for="product in top5ProductSuperDiscount(listProduct)"
            class="card scale_img m-4"
            style="border: none; width: 16%"
            :key="product.ProductId"
          >
            <a href="#" @click="detailProduct(product.ProductId)"
              ><img
                style="height: 200px"
                class="card-img-top"
                :src="require('@/assets/img/product/' + product.Avatar)"
            /></a>
            <div class="card-body" style="margin-top: 15px">
              <div class="card-title">{{ product.ProductName }}</div>
              <div class="card-text" style="display: flex">
                <p style="color: red; margin-right: 10px">
                  {{ this.common.changeDisplayDebitAmount(product.PriceBuy) }}
                </p>
                <p
                  style="
                    text-decoration: line-through;
                    opacity: 0.6;
                    font-size: 12px;
                  "
                >
                  {{ this.common.changeDisplayDebitAmount(product.Price) }}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div>
      <div>
        <h1 style="font-size: 45px; color: red">Sản phẩm bán chạy</h1>
      </div>
      <hr />
      <div class="auto_wrap" style="width: 100%">
        <div class="flex-wrap justify-content-center d-flex w-100">
          <div
            v-for="product in top10ProductSelling(listProduct)"
            class="card scale_img m-4"
            style="border: none; width: 16%"
            :key="product.ProductId"
          >
            <a href="#" @click="detailProduct(product.ProductId)"
              ><img
                style="height: 200px"
                class="card-img-top"
                :src="require('@/assets/img/product/' + product.Avatar)"
            /></a>
            <div class="card-body" style="margin-top: 15px">
              <div class="card-title">{{ product.ProductName }}</div>
              <div class="card-text" style="display: flex">
                <p style="color: red; margin-right: 10px">
                  {{ this.common.changeDisplayDebitAmount(product.PriceBuy) }}
                </p>
                <p
                  style="
                    text-decoration: line-through;
                    opacity: 0.6;
                    font-size: 12px;
                  "
                >
                  {{ this.common.changeDisplayDebitAmount(product.Price) }}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div style="margin-top: 20px">
      <div>
        <h1 style="font-size: 45px; color: red">Điện thoại nổi bật</h1>
      </div>
      <hr />
      <div class="auto_wrap" style="width: 100%">
        <div class="flex-wrap justify-content-center d-flex w-100">
          <div
            v-for="product in top15Product(
              listProduct,
              '34fe7e75-02be-46e3-9778-1805fb21f28c'
            )"
            class="card scale_img m-4"
            style="border: none; width: 16%"
            :key="product.ProductId"
          >
            <a href="#" @click="detailProduct(product.ProductId)"
              ><img
                style="height: 200px"
                class="card-img-top"
                :src="require('@/assets/img/product/' + product.Avatar)"
            /></a>
            <div class="card-body" style="margin-top: 15px">
              <div class="card-title">{{ product.ProductName }}</div>
              <div class="card-text" style="display: flex">
                <p style="color: red; margin-right: 10px">
                  {{ this.common.changeDisplayDebitAmount(product.PriceBuy) }}
                </p>
                <p
                  style="
                    text-decoration: line-through;
                    opacity: 0.6;
                    font-size: 12px;
                  "
                >
                  {{ this.common.changeDisplayDebitAmount(product.Price) }}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Máy tính -->
    <div class="laptop" style="margin-top: 20px">
      <div>
        <h1 style="font-size: 45px; color: red">Máy tính nổi bật</h1>
      </div>
      <hr />
      <div class="auto_wrap" style="width: 100%">
        <div class="flex-wrap justify-content-center d-flex w-100">
          <div
            v-for="product in top15Product(
              listProduct,
              'd9eeb5b8-0da1-4de1-bfef-08fba03bda91'
            )"
            class="card scale_img m-4"
            style="border: none; width: 16%"
            :key="product.ProductId"
          >
            <a href="#" @click="detailProduct(product.ProductId)"
              ><img
                style="height: 200px"
                class="card-img-top"
                :src="require('@/assets/img/product/' + product.Avatar)"
            /></a>
            <div class="card-body" style="margin-top: 15px">
              <div class="card-title">{{ product.ProductName }}</div>
              <div class="card-text" style="display: flex">
                <p style="color: red; margin-right: 10px">
                  {{ this.common.changeDisplayDebitAmount(product.PriceBuy) }}
                </p>
                <p
                  style="
                    text-decoration: line-through;
                    opacity: 0.6;
                    font-size: 12px;
                  "
                >
                  {{ this.common.changeDisplayDebitAmount(product.Price) }}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Phụ kiện -->
    <div style="margin-top: 20px">
      <div>
        <h1 style="font-size: 45px; color: red">Phụ Kiện nổi bật</h1>
      </div>
      <hr />
      <div class="auto_wrap" style="width: 100%">
        <div class="flex-wrap justify-content-center d-flex w-100">
          <div
            v-for="product in top15Product(
              listProduct,
              'c655d547-f06f-4ba7-a5f3-a615213fcae7'
            )"
            class="card scale_img m-4"
            style="border: none; width: 16%"
            :key="product.ProductId"
          >
            <a href="#" @click="detailProduct(product.ProductId)"
              ><img
                style="height: 200px"
                class="card-img-top"
                :src="require('@/assets/img/product/' + product.Avatar)"
            /></a>
            <div class="card-body" style="margin-top: 15px">
              <div class="card-title">{{ product.ProductName }}</div>
              <div class="card-text" style="display: flex">
                <p style="color: red; margin-right: 10px">
                  {{ this.common.changeDisplayDebitAmount(product.PriceBuy) }}
                </p>
                <p
                  style="
                    text-decoration: line-through;
                    opacity: 0.6;
                    font-size: 12px;
                  "
                >
                  {{ this.common.changeDisplayDebitAmount(product.Price) }}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: "Home",
  data() {
    return {
      listProduct: {},
    };
  },
  methods: {
    /**
     * Chuyển hướng đến trang chi tiết sản phẩm
     * @param {id sản phẩm} id
     */
    detailProduct(id) {
      this.$router.push(`user/product-detail/${id}`);
    },
    /**
     * Hàm lấy dữ liệu sản phẩm
     */
    async loadData() {
      this.listProduct = await this.apiService.get("Product");
    },

    /**
     * Lấy ra 5 sản phẩm giảm nhiều nhất
     * @param {danh sách sản phẩm} products
     */
    top5ProductSuperDiscount(products) {
      // Kiểm tra xem products có tồn tại và có chứa sản phẩm không
      if (
        !products ||
        typeof products !== "object" ||
        Object.keys(products).length === 0
      ) {
        return []; // Trả về một mảng trống nếu không có sản phẩm hoặc products không hợp lệ
      }

      let productArray = Object.values(products); // Chuyển đổi object thành một mảng các sản phẩm
      let productsWithReducedPrice = productArray.sort(
        (a, b) => b.PriceReduced - a.PriceReduced
      );

      // Trả về 5 sản phẩm đầu tiên hoặc tất cả sản phẩm nếu có ít hơn 5 sản phẩm
      return productsWithReducedPrice.slice(0, 5);
    },

    /**
     * Lấy ra 10 sản phẩm bán nhiều nhất
     * @param {danh sách sản phẩm} products
     */
    top10ProductSelling(products) {
      // Kiểm tra xem products có tồn tại và có chứa sản phẩm không
      if (
        !products ||
        typeof products !== "object" ||
        Object.keys(products).length === 0
      ) {
        return []; // Trả về một mảng trống nếu không có sản phẩm hoặc products không hợp lệ
      }

      let productArray = Object.values(products); // Chuyển đổi object thành một mảng các sản phẩm
      let productsWithSelling = productArray.sort(
        (a, b) => b.QuantityBuy - a.QuantityBuy
      );

      // Trả về 10 sản phẩm đầu tiên hoặc tất cả sản phẩm nếu có ít hơn 10 sản phẩm
      return productsWithSelling.slice(0, 10);
    },

    /**
     * Lấy ra 15 sản phẩm bán nhiều nhất
     * @param {danh sách sản phẩm} products
     */
    top15Product(products, productTypeId) {
      // Kiểm tra xem products có tồn tại và có chứa sản phẩm không
      if (
        !products ||
        typeof products !== "object" ||
        Object.keys(products).length === 0
      ) {
        return []; // Trả về một mảng trống nếu không có sản phẩm hoặc products không hợp lệ
      }

      let productArray = Object.values(products); // Chuyển đổi object thành một mảng các sản phẩm
      let productsWithSelling = productArray
        .filter((product) => product.ProductTypeId === productTypeId)
        .sort((a, b) => b.Hot - a.Hot)
        .slice(0, 15); // Lấy ra 10 bản ghi đầu tiên

      // Trả về 15 sản phẩm đầu tiên hoặc tất cả sản phẩm nếu có ít hơn 15 sản phẩm
      return productsWithSelling;
    },
  },
  created() {
    this.loadData();
  },
};
</script>

<style scoped>
.banner img {
  width: 100%;
  height: 360px;
  border-radius: 6px;
}
.auto_wrap img {
  transition: transform 0.3s ease; /* Thời gian và kiểu chuyển đổi */
}

.auto_wrap img:hover {
  transform: scale(1.1); /* Scale ảnh lên 1.2 lần khi hover */
}
</style>
