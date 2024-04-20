<template>
  <div class="container scale_img mb-5" style="min-height: 100vh">
    <div class="row" style="height: 100%">
      <div class="col-md-3 bg-body-tertiary" style="height: 100%">
        <div class="product-type" style="margin-top: 20px">
          <h3>Danh sách sản phẩm</h3>
          <div
            class="p-2"
            v-for="item in listProductType"
            :key="item.ProductTypeId"
            @click="selectProductType(item.ProductTypeId)"
            :class="{
              product_type_checked: item.ProductTypeId === productTypeId,
            }"
          >
            {{ item.ProductTypeName }}
          </div>
        </div>
        <hr />
        <div>
          <h3>Sản phẩm nổi bật</h3>
          <div
            v-for="product in top5ProductSuperDiscount(listProduct)"
            :key="product.ProductId"
            class="mt-4"
          >
            <table style="height: 100px" class="mt-2 mb-4">
              <tr>
                <th style="border: none" rowspan="2">
                  <a href="#" @click="detailProduct(product.ProductId)"
                    ><img
                      style="height: 100px; width: 90px; margin-right: 14px"
                      class="card-img-top"
                      :src="require('@/assets/img/product/' + product.Avatar)"
                  /></a>
                </th>
                <th
                  style="
                    font-size: 12px;
                    border: none;
                    text-align: left;
                    font-size: 12px;
                  "
                >
                  {{ product.ProductName }}
                </th>
              </tr>
              <tr>
                <th style="border: none">
                  <div class="card-text" style="display: flex">
                    <p style="color: red; margin-right: 10px; font-size: 12px">
                      {{
                        this.common.changeDisplayDebitAmount(product.PriceBuy)
                      }}
                    </p>
                    <p
                      style="
                        text-decoration: line-through;
                        opacity: 0.6;
                        font-size: 11px;
                      "
                    >
                      {{ this.common.changeDisplayDebitAmount(product.Price) }}
                    </p>
                  </div>
                </th>
              </tr>
            </table>
          </div>
        </div>
      </div>
      <div class="col-md-9" style="height: 100%">
        <div class="d-flex justify-content-between">
          <h2 style="margin-top: 10px">Tất cả sản phẩm</h2>
          <select
            class="bg-body-secondary"
            style="
              width: 300px;
              height: 30px;
              border: none;
              outline: none;
              border-radius: 6px;
              margin-top: 20px;
            "
            v-model="valueOption"
            @change="handleChangeOption"
            onchange=""
          >
            <option>Lựa chọn chức năng</option>
            <option value="1">Giá tăng dần</option>
            <option value="2">Giá giảm dần</option>
            <option value="3">Bán chạy nhất</option>
            <option value="4">Sản phẩm mới nhất</option>
            <option value="5">Sản phẩm cũ nhất</option>
            <option value="6">Sắp xếp A -> Z</option>
            <option value="7">Sắp xếp Z -> A</option>
          </select>
        </div>
        <hr />
        <div style="width: 100%; margin-top: 10px">
          <div class="auto_wrap" style="width: 100%">
            <div class="d-flex justify-content-center flex-wrap w-100">
              <div
                v-for="product in listProduct"
                class="card scale_img m-4"
                style="border: none; width: 15%"
                :key="product.ProductId"
              >
                <a href="#" @click="detailProduct(product.ProductId)"
                  ><img
                    style="height: 140px; width: 120px"
                    class="card-img-top"
                    :src="require('@/assets/img/product/' + product.Avatar)"
                /></a>
                <div class="card-body" style="margin-top: 15px">
                  <div style="font-size: 13px" class="card-title">
                    {{ product.ProductName }}
                  </div>
                  <div class="card-text" style="display: flex">
                    <p style="color: red; margin-right: 10px; font-size: 13px">
                      {{
                        this.common.changeDisplayDebitAmount(product.PriceBuy)
                      }}
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
          <div
            class="container d-flex align-items-center justify-content-center"
            style="margin-top: 20px"
          >
            <nav aria-label="Page navigation">
              <ul class="pagination">
                <li class="page-item" :class="{ disabled: pageIndex === 1 }">
                  <a class="page-link" href="#" @click="prevPage">Previous</a>
                </li>
                <li
                  class="page-item"
                  v-for="pageNumber in totalPages"
                  :key="pageNumber"
                  :class="{ active: pageIndex === pageNumber }"
                >
                  <a
                    class="page-link"
                    href="#"
                    @click="changePage(pageNumber)"
                    >{{ pageNumber }}</a
                  >
                </li>
                <li
                  class="page-item"
                  :class="{ disabled: pageIndex === totalPages }"
                >
                  <a class="page-link" href="#" @click="nextPage">Next</a>
                </li>
              </ul>
            </nav>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      listProduct: {},
      pageIndex: 1,
      infoSearch: "",
      optionSelect: 0,
      productTypeId: "",
      listProductType: {},
      totalPages: 0,
    };
  },
  methods: {
    /**
     * Chuyển hướng đến trang chi tiết sản phẩm
     * @param {id sản phẩm} id
     */
    detailProduct(id) {
      this.$router.push(`product-detail/${id}`);
    },
    changePage(pageNumber) {
      this.pageIndex = pageNumber;
      this.loadData();
    },
    nextPage() {
      if (this.pageIndex < this.totalPages) {
        this.pageIndex++;
        this.loadData();
      }
    },
    prevPage() {
      if (this.pageIndex > 1) {
        this.pageIndex--;
        this.loadData();
      }
    },
    /**
     * Hàm lấy dữ liệu sản phẩm
     */
    async loadData() {
      this.common.showLoading();
      let result = await this.apiService.loadFilterUser(
        "Product",
        this.pageIndex,
        this.infoSearch,
        this.optionSelect,
        this.productTypeId
      );
      this.listProduct = result.ListRecord;
      this.totalPages = Math.ceil(result.ToTalRecord / 15);
      this.common.showLoading(false);
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
     * Chọn loại sản phẩm
     */
    selectProductType(id) {
      this.productTypeId = id;
      this.loadData();
    },

    handleChangeOption(event) {
      this.optionSelect = event.target.value;
      this.loadData();
    },

    async getListProductType() {
      this.listProductType = await this.apiService.get("ProductType/getAll/1");
    },
  },
  created() {
    this.getListProductType();
    this.loadData();
  },
  mounted() {
    this.emitter.on("Search", (value) => {
      this.infoSearch = value;
      this.loadData();
    });
  },
};
</script>

<style scoped>
.product-type > div:hover {
  background: #ceead9;
  border-radius: 4px;
  cursor: pointer;
}
.product_type_checked {
  background: #ceead9;
  border-radius: 4px;
}
img {
  transition: transform 0.3s ease; /* Thời gian và kiểu chuyển đổi */
}

img:hover {
  transform: scale(1.1); /* Scale ảnh lên 1.2 lần khi hover */
}
</style>
