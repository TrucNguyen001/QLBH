<template>
  <div class="container" style="min-height: 800px">
    <div class="d-flex" style="height: 60px">
      <h4><a href="#" @click="redirectListProduct">Sản phẩm </a></h4>
      <h4 style="font-size: 12px; margin-top: 8px">
        /
        {{ product.ProductName }}
      </h4>
    </div>
    <div class="row">
      <div class="col-md-4">
        <div class="img-avatar" v-if="imageShow !== ''">
          <img
            style="height: 400px"
            class="card-img-top"
            :src="require('@/assets/img/product/' + imageShow)"
          />
        </div>
        <div
          class="d-flex justify-content-center align-items-center scale_img"
          style="height: 80px; margin-top: 20px"
        >
          <p class="mx-2 my-auto">
            <img
              class="m-2"
              v-for="(item, index) in listImage"
              :key="index"
              style="height: 80px; cursor: pointer"
              @click="changeAvatar(item)"
              :src="require('@/assets/img/product/' + item)"
            />
          </p>
        </div>
      </div>
      <div class="col-md-5 buy_product">
        <h2>{{ product.ProductName }}</h2>
        <div class="d-flex" style="height: 80px">
          <p class="my-auto mx-4">Giá bán:</p>
          <h1 class="my-auto text-danger">
            {{ this.common.changeDisplayDebitAmount(product.PriceBuy) }}
          </h1>
          <p
            class="my-auto mx-3"
            style="text-decoration: line-through; opacity: 0.6; font-size: 22px"
          >
            {{ this.common.changeDisplayDebitAmount(product.Price) }}
          </p>
        </div>
        <div class="d-flex" style="height: 40px">
          <p class="mx-4 my-auto">Tình trạng:</p>
          <h3 v-if="product.Status === 1" class="text-success">Còn hàng</h3>
          <h3 v-else class="text-danger">Hết hàng</h3>
        </div>
        <div class="d-flex" style="height: 40px">
          <p class="mx-4 my-auto">Đã bán:</p>
          <h3 class="text-success">{{ product.QuantityBuy }}</h3>
          <p class="mx-4 my-auto">Sản phẩm</p>
        </div>
        <div class="d-flex">
          <p style="width: 100px" class="my-auto mx-4">Số lượng:</p>
          <div>
            <button
              @click="downQuantity"
              style="width: 100px"
              class="btn btn-info"
            >
              <i class="bi bi-dash-lg"></i>
            </button>
            <input
              class="my-auto mx-4"
              readonly
              style="
                width: 50px;
                height: 50px;
                font-size: 30px;
                padding-left: 16px;
                border: none;
              "
              v-model="quantity"
            />
            <button
              @click="upQuantity"
              style="width: 100px"
              class="btn btn-info"
            >
              <i class="bi bi-plus-lg"></i>
            </button>
          </div>
        </div>
        <div class="d-flex mt-4" style="height: 100px">
          <button
            @click="addToCart"
            class="bg-danger"
            style="height: 40px; width: 200px; border: none; border-radius: 4px"
          >
            Thêm vào giỏ hàng <i class="bi bi-cart-dash"></i>
          </button>
          <button
            @click="buyProduct"
            class="bg-danger"
            style="
              height: 40px;
              width: 200px;
              border: none;
              margin-left: 40px;
              border-radius: 4px;
            "
          >
            Mua hàng
          </button>
        </div>
      </div>
      <div class="col-md-3">
        <table style="text-align: center">
          <caption style="margin-bottom: 20px">
            <h3>Chúng tôi luôn sẵn sàng để giúp đỡ bạn</h3>
          </caption>
          <tr>
            <th class="help-user" style="border: none">
              <img
                style="height: 160px; width: 230px; margin-bottom: 10px"
                src="./../../../assets/img/product/help-user.jpg"
              />
            </th>
          </tr>
          <tr>
            <th style="border: none"><p>Để được hỗ trợ tốt nhất hãy gọi</p></th>
          </tr>
          <tr>
            <th style="border: none">
              <i style="color: limegreen; font-size: 30px">1800 1009</i>
            </th>
          </tr>
          <tr>
            <th style="border: none"><p>Hoặc</p></th>
          </tr>
          <tr>
            <th style="margin-top: 4px; border: none">Chát hỗ trợ trực tiếp</th>
          </tr>
          <tr>
            <th style="border: none">
              <p style="margin-top: 8px; line-height: 20px">
                Chúng tôi luôn sẵn sàng giúp đỡ bạn
              </p>
            </th>
          </tr>
        </table>
      </div>
    </div>

    <div class="row" style="margin-top: 50px; font-size: 20px">
      <div class="col-md-7">
        <h1>Mô tả sản phẩm:</h1>
        <hr />
        <p>{{ product.Description }}</p>
        <img style="width: 100%" src="" />
        <p>{{ product.ContentCamera }}</p>
      </div>
      <div class="col-md-1"></div>
      <div
        v-if="product.ProductTypeId !== 'c655d547-f06f-4ba7-a5f3-a615213fcae7'"
        class="col-md-4 shadow"
        style="font-size: 18px"
      >
        <table class="table config">
          <tr>
            <td style="width: 150px">Màn hình</td>
            <td>{{ product.Screen }}</td>
          </tr>
          <tr>
            <td>Độ phân giải</td>
            <td>{{ product.Resolution }}</td>
          </tr>
          <tr>
            <td>Công nghệ màn hình</td>
            <td>{{ product.ScreenTechnology }}</td>
          </tr>
          <tr>
            <td>RAM</td>
            <td>{{ product.RAM }}</td>
          </tr>
          <tr>
            <td>Bộ nhớ</td>
            <td>{{ product.Memory }}</td>
          </tr>
          <tr>
            <td>Chất liệu</td>
            <td>{{ product.Material }}</td>
          </tr>
          <tr>
            <td>Kích thước</td>
            <td>{{ product.Size }}</td>
          </tr>
          <tr>
            <td>Trọng lượng</td>
            <td>{{ product.Weight }}</td>
          </tr>
          <tr>
            <td>Camera</td>
            <td>{{ product.Camera }}</td>
          </tr>
          <tr>
            <td>Pin</td>
            <td>{{ product.Pin }}</td>
          </tr>
          <tr>
            <td>CPU</td>
            <td>{{ product.CPU }}</td>
          </tr>
          <tr>
            <td>Chip</td>
            <td>{{ product.Chip }}</td>
          </tr>
        </table>
      </div>
    </div>
    <div
      v-if="
        product.ImageCPU !== undefined &&
        product.ProductTypeId !== 'c655d547-f06f-4ba7-a5f3-a615213fcae7'
      "
      class="row img-config"
      style="font-size: 20px"
    >
      <div class="col-md-12">
        <h3>CPU:</h3>
        <hr />
        <img
          style="width: 90%; margin-left: 5%"
          :src="require('@/assets/img/product/' + product.ImageCPU)"
        />
        <p style="width: 90%; margin-left: 5%; margin-top: 20px">
          {{ product.ContentCPU }}
        </p>
      </div>
    </div>
    <div
      v-if="
        product.Pin !== undefined &&
        product.ProductTypeId !== 'c655d547-f06f-4ba7-a5f3-a615213fcae7'
      "
      class="row img-config"
      style="font-size: 20px"
    >
      <div class="col-md-12">
        <h3>Pin:</h3>
        <hr />
        <img
          style="width: 90%; margin-left: 5%"
          :src="require('@/assets/img/product/' + product.ImagePin)"
        />
        <p style="width: 90%; margin-left: 5%; margin-top: 20px">
          {{ product.ContentPin }}
        </p>
      </div>
    </div>
    <div
      v-if="
        product.RAM !== undefined &&
        product.ProductTypeId !== 'c655d547-f06f-4ba7-a5f3-a615213fcae7'
      "
      class="row img-config"
      style="font-size: 20px"
    >
      <div class="col-md-12">
        <h3>RAM:</h3>
        <hr />
        <img
          style="width: 90%; margin-left: 5%"
          :src="require('@/assets/img/product/' + product.ImageRAM)"
        />
        <p style="width: 90%; margin-left: 5%; margin-top: 20px">
          {{ product.ContentRAM }}
        </p>
      </div>
    </div>
    <div
      v-if="
        product.Camera !== undefined &&
        product.ProductTypeId !== 'c655d547-f06f-4ba7-a5f3-a615213fcae7'
      "
      class="row img-config"
      style="font-size: 20px"
    >
      <div class="col-md-12">
        <h3>Camera:</h3>
        <hr />
        <img
          style="width: 90%; margin-left: 5%"
          :src="require('@/assets/img/product/' + product.ImageCamera)"
        />
        <p style="width: 90%; margin-left: 5%; margin-top: 20px">
          {{ product.ContentPin }}
        </p>
      </div>
    </div>

    <div class="row" style="margin-top: 50px">
      <div class="col-md-12" style="width: 100%">
        <h2 class="text-danger">Top sản phẩm bán chạy nhất</h2>
        <hr />
        <div class="auto_wrap" style="width: 100%">
          <div class="d-flex justify-content-center w-100">
            <div
              v-for="product in top5ProductSelling(listProduct)"
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
    <div class="row" id="comment">
      <div class="col-md-12">
        <h2 class="text-danger">Phản hồi sản phẩm</h2>
        <hr />
        <div>
          <div class="w-100 d-flex">
            <textarea
              class="form-control"
              style="width: 80%; border-radius: 10px"
              rows="5"
              placeholder="Nhập nội dung bạn muốn phản hồi"
              v-model="contentComment"
            ></textarea>
            <button
              class="btn px-3 bg-danger text-white mx-4"
              style="height: 40px; width: 90px"
              @click="addComment"
            >
              <i class="bi bi-send-check"></i> Gửi
            </button>
          </div>
        </div>
        <hr />
        <div
          class="shadow mb-5"
          style="min-height: 400px; width: 100%; border-radius: 8px"
          v-if="this.listCommentUnique.length > 0"
        >
          <div
            v-for="item in listCommentUnique"
            :key="item.CommentId"
            class="w-100"
          >
            <div class="w-100 d-flex align-items-center">
              <div class="mx-4">
                <i style="font-size: 46px" class="bi bi-tencent-qq"></i>
              </div>
              <div class="w-75">
                <p style="margin-top: 50px">{{ item.CommentName }}</p>
                <div>
                  {{ item.Content }}
                  <div class="d-flex" style="font-size: 12px; opacity: 0.8">
                    {{ this.common.changeDisplayDate(item.CreatedDate) }}
                    <p
                      @click="getCommentId(item.CommentId, item.CommentName)"
                      class="mx-4"
                      style="cursor: pointer"
                    >
                      Phản hồi
                    </p>
                  </div>
                </div>
              </div>
            </div>
            <div
              class="form-feedback"
              :class="{ open_feedback: item.CommentId === this.commentId }"
            >
              <div class="w-100 d-flex mx-4">
                <textarea
                  v-model="contentFeedback"
                  class="form-control"
                  style="width: 60%; border-radius: 10px"
                  rows="5"
                  placeholder="Nhập nội dung bạn muốn phản hồi"
                ></textarea>
                <button
                  @click="addFeedback"
                  class="btn bg-danger text-white mx-4"
                  style="height: 40px; width: 90px"
                >
                  <i class="bi bi-send-check"></i> Gửi
                </button>
              </div>
              <hr />
            </div>
            <div>
              <div
                v-for="(feedback, index) in filterFeedbackByCommentId(
                  item.CommentId
                )"
                :key="index"
              >
                <div
                  v-if="feedback.FeedbackContent !== null"
                  class="w-100 d-flex align-items-center"
                  style="margin-top: -40px; margin-left: 40px"
                >
                  <div class="mx-4">
                    <i
                      v-if="feedback.Status === 1"
                      style="font-size: 46px"
                      class="bi bi-person-circle"
                    ></i>
                    <i
                      v-else
                      style="font-size: 46px"
                      class="bi bi-tencent-qq"
                    ></i>
                  </div>
                  <div class="w-75">
                    <p style="margin-top: 50px">
                      {{ feedback.CreatedBy
                      }}<span
                        v-if="feedback.Status === 1"
                        class="mx-2"
                        style="font-size: 12px"
                        >(nhân viên)</span
                      >
                    </p>
                    <div>
                      <strong>{{ feedback.FeedbackFor }}</strong>
                      {{ feedback.FeedbackContent }}
                      <div class="d-flex" style="font-size: 12px; opacity: 0.8">
                        {{
                          this.common.changeDisplayDate(
                            feedback.CreatedDateFeedback
                          )
                        }}
                        <p
                          @click="
                            getCommentIdEmployee(
                              feedback.FeedbackId,
                              item.CommentId,
                              feedback.CreatedBy
                            )
                          "
                          class="mx-4"
                          style="cursor: pointer"
                        >
                          Phản hồi
                        </p>
                      </div>
                    </div>
                  </div>
                </div>
                <div
                  class="form-feedback"
                  :class="{
                    open_feedback: feedback.FeedbackId === this.commentId,
                  }"
                >
                  <div class="w-100 d-flex mx-4">
                    <textarea
                      v-model="contentFeedback"
                      class="form-control"
                      style="width: 60%; border-radius: 10px"
                      rows="5"
                      placeholder="Nhập nội dung bạn muốn phản hồi"
                    ></textarea>
                    <button
                      @click="addFeedback"
                      class="btn bg-danger text-white mx-4"
                      style="height: 40px; width: 90px"
                    >
                      <i class="bi bi-send-check"></i> Gửi
                    </button>
                  </div>
                  <hr />
                </div>
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
  data() {
    return {
      product: {},
      comment: {},
      feedback: {},
      imageShow: "",
      listImage: [],
      listProduct: {},
      quantity: 1,
      listComment: {},
      listCommentUnique: {},
      showFeedback: false,
      commentId: "",
      cart: {
        ProductId: "",
        InvoiceId: "",
        AccountId: "",
        QuantityPurchased: "",
        Status: 1,
      },
      commentIdSelect: "",
      contentComment: "",
      contentFeedback: "",
      fullNameFeedback: "",
    };
  },
  methods: {
    /**
     * Thêm phản hồi
     */
    async addFeedback() {
      if (this.contentFeedback !== "" && localStorage.getItem("FullName")) {
        this.feedback = {
          FeedbackContent: this.contentFeedback,
          CommentId: this.commentIdSelect,
          Status: 0,
          CreatedDateFeedback: new Date(),
          CreatedBy: localStorage.getItem("FullName"),
          FeedbackFor: this.fullNameFeedback,
        };

        let result = await this.apiService.post("Feedback/post", this.feedback);
        console.log(result);
      }
    },
    /**
     * Lấy commentId
     * @param {id comment} id
     */
    getCommentId(id, name) {
      this.commentId = id;
      this.commentIdSelect = id;
      this.fullNameFeedback = name;
    },

    /**
     * Lấy commentId
     * @param {id comment} id
     */
    getCommentIdEmployee(id, idSelect, name) {
      this.commentId = id;
      this.commentIdSelect = idSelect;
      this.fullNameFeedback = name;
    },
    /**
     * Thêm bình luận
     */
    async addComment() {
      if (this.contentComment !== "" && localStorage.getItem("AccountId")) {
        this.comment = {
          CommentName: localStorage.getItem("FullName"),
          Content: this.contentComment,
          ProductId: this.product.ProductId,
          AccountId: localStorage.getItem("AccountId"),
        };

        var result = await this.apiService.post("Comment/post", this.comment);
        console.log(result);
      }
    },
    upQuantity() {
      if (this.quantity < this.product.Quantity) {
        this.quantity++;
      }
    },
    downQuantity() {
      if (this.quantity > 1) {
        this.quantity--;
      }
    },
    /**
     * Thêm sản phẩm vào giỏ hàng
     */
    addToCart() {
      this.cart.ProductId = this.product.ProductId;
      this.cart.AccountId = localStorage.getItem("AccountId");
      this.cart.InvoiceId = null;
      this.cart.QuantityPurchased = this.quantity;

      var result = this.apiService.post("Cart/post", this.cart);
      console.log(result);
    },
    async buyProduct() {
      this.cart.ProductId = this.product.ProductId;
      this.cart.AccountId = localStorage.getItem("AccountId");
      this.cart.InvoiceId = null;
      this.cart.QuantityPurchased = this.quantity;

      await this.apiService.post("Cart/post", this.cart);

      this.$router.push("/user/cart");
    },
    /**
     * Quay về trang sản phẩm
     */
    redirectListProduct() {
      this.$router.push("/user/list-product");
    },
    /**
     * Lấy Id trên url và lấy product
     */
    async getProduct() {
      let productId = this.$route.params.id;
      let result = await this.apiService.getByInfo("Product", productId);
      this.imageShow = result.Avatar;
      this.listImage = result.Image.split(" ");
      this.product = result;
    },
    async loadAllProduct() {
      this.listProduct = await this.apiService.get("Product");
    },

    /**
     * Chuyển hướng đến trang chi tiết sản phẩm
     * @param {id sản phẩm} id
     */
    async detailProduct(id) {
      await this.$router.push(`/user/product-detail/${id}`);
      window.location.reload();
    },
    /**
     * Lấy ra 5 sản phẩm bán nhiều nhất
     * @param {danh sách sản phẩm} products
     */
    top5ProductSelling(products) {
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

      // Trả về 5 sản phẩm đầu tiên hoặc tất cả sản phẩm nếu có ít hơn 5 sản phẩm
      return productsWithSelling.slice(0, 5);
    },

    changeAvatar(img) {
      this.imageShow = img;
    },

    async getCommentAndFeedBack() {
      this.listComment = await this.apiService.getByInfo(
        "Comment/comment-feedback",
        this.product.ProductId
      );
    },
    filterUniqueComments(data) {
      // Tạo một đối tượng dùng để lưu trữ các bản ghi không trùng CommentId
      let uniqueComments = {};

      // Lặp qua dữ liệu và thêm các bản ghi không trùng CommentId vào mảng kết quả
      let uniqueRecords = [];
      data.forEach((item) => {
        if (!uniqueComments[item.CommentId]) {
          uniqueComments[item.CommentId] = true;
          uniqueRecords.push(item);
        }
      });

      return uniqueRecords; // Trả về mảng chứa các bản ghi không trùng CommentId
    },

    filterFeedbackByCommentId(commentId) {
      let feedbacksWithSameCommentId = this.listComment.filter((item) => {
        return item.CommentId === commentId;
      });

      return feedbacksWithSameCommentId;
    },
  },
  async created() {
    await this.getProduct();
    this.loadAllProduct();
    await this.getCommentAndFeedBack();
    this.listCommentUnique = await this.filterUniqueComments(this.listComment);
    console.log(this.product);
  },
};
</script>

<style scoped>
img {
  transition: transform 0.3s ease; /* Thời gian và kiểu chuyển đổi */
}

img:hover {
  transform: scale(1.1); /* Scale ảnh lên 1.2 lần khi hover */
}
.img-avatar img:hover {
  transform: scale(1.05);
}
.help-user img:hover,
.img-config img:hover {
  transform: scale(1.02);
}
a {
  text-decoration: none;
  color: #1f1f1f;
  font-weight: 600;
  margin-right: 4px;
}
.config tr td:nth-child(2) {
  padding-left: 10px;
  border-right: none;
}
.form-feedback {
  display: none;
}
.open_feedback {
  display: block;
}
</style>
