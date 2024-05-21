<template>
  <router-view />
  <a
    ><df-messenger
      intent="WELCOME"
      chat-title="QLBanHang"
      agent-id="21eec9cd-3dbf-4a9a-88cf-fa0765c817b1"
      language-code="en"
    ></df-messenger
  ></a>
  <!-- <div v-if="this.toggle === false" class="open-form">
    <button @click="this.toggle = !this.toggle" class="btn btn-primary">
      Gợi ý sản phẩm
      <img
        style="height: 30px"
        src="../../assets/img/party-popper_3146600.png"
      />
      <img
        style="height: 30px"
        src="../../assets/img/party-popper_3146600.png"
      />
    </button>
  </div> -->
  <!-- <div
    v-if="this.listProduct.length > 0 && this.toggle === true"
    class="product shadow-lg p-3 mb-5 bg-body rounded"
  >
    <div class="d-flex justify-content-between">
      <h6 style="font-weight: 600">Top sản phẩm đã mua</h6>
      <i @click="toggleForm" class="bi bi-x-lg"></i>
    </div>
    <div v-for="product in listProduct" :key="product.ProductId" class="mt-4">
      <table style="height: 100px; position: relative" class="mt-2 mb-4">
        <tr>
          <th style="border: none" rowspan="2">
            <a href="#" @click="detailProduct(product.ProductId)"
              ><img
                style="height: 50px; width: 40px; margin-right: 14px"
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
                {{ this.common.changeDisplayDebitAmount(product.PriceBuy) }}
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
              <p class="bought">
                Đã mua {{ product.TotalQuantityPurchased }} sản phẩm
              </p>
            </div>
          </th>
        </tr>
      </table>
    </div>
  </div> -->
</template>
<script>
export default {
  name: "ContentWeb",
  data() {
    return {
      listProduct: [],
      toggle: true,
    };
  },
  methods: {
    toggleForm() {
      this.toggle = !this.toggle;
    },
    async loadData() {
      if (localStorage.getItem("AccountId")) {
        this.listProduct = await this.apiService.get(
          "Product/GetProductForUser" + "/" + localStorage.getItem("AccountId")
        );
      }
    },
    /**
     * Chuyển hướng đến trang chi tiết sản phẩm
     * @param {id sản phẩm} id
     */
    async detailProduct(id) {
      await this.$router.push(`/user/product-detail/${id}`);
      window.location.reload();
    },
  },
  created() {
    this.loadData();
  },
};
</script>
<style scoped>
.message {
  width: 150px;
  height: 50px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50px;
  color: white;
  font-size: 15px;
  position: fixed;
  right: 24px;
  bottom: 200px;
}
.message i {
  font-size: 32px;
  color: white;
  margin-right: 8px;
}
.product {
  position: fixed;
  height: 560px;
  width: 280px;
  top: 120px;
  right: 24px;
}
.bought {
  font-size: 10px;
  font-weight: 500;
  margin-left: 2px;
  color: darkgreen;
}
.bi-x-lg {
  background: rgb(255, 205, 205);
  display: flex;
  align-items: center;
  padding: 0 16px;
  margin-top: -14px;
  margin-right: -14px;
  border-top-right-radius: 4px;
}
.open-form {
  position: fixed;
  top: 220px;
  right: 24px;
}
</style>
