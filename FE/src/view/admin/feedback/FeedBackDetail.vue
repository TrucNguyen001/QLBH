<template>
  <div class="curtain" v-if="show">
    <div
      class="m-form"
      style="
        position: absolute;
        border: 1px solid darkgreen;
        width: 1200px;
        z-index: 999;
      "
    >
      <div class="form">
        <div
          class="form-header d-flex align-items-center justify-content-between"
        >
          <h2>Thông tin phản hồi</h2>
          <button @click="hideDetailRecord" class="btn btn-close mx-4"></button>
        </div>
        <div class="form-body mx-5">
          <div class="d-flex justify-content-between my-2">
            <div style="width: 24%">
              <label>Mã phản hồi</label>
              <input
                class="m-input mt-2"
                type="text"
                v-model="recordSelectUnique.CommentCode"
                readonly
              />
            </div>
            <div style="width: 24%">
              <label>Tên khách hàng</label>
              <input
                class="m-input mt-2"
                type="text"
                v-model="recordSelectUnique.CommentName"
                readonly
              />
            </div>
            <div style="width: 24%">
              <label>Mã sản phẩm</label>
              <input
                class="m-input mt-2"
                type="text"
                v-model="recordSelectUnique.ProductCode"
                readonly
              />
            </div>
            <div style="width: 24%">
              <label>Ngày phản hồi</label>
              <input
                class="m-input mt-2"
                type="text"
                v-model="recordSelectUnique.CreatedDate"
                readonly
              />
            </div>
          </div>
          <div class="d-flex justify-content-between my-2">
            <div style="width: 100%">
              <label>Tên sản phẩm</label>
              <input
                class="m-input mt-2"
                type="text"
                v-model="recordSelectUnique.ProductName"
                readonly
              />
            </div>
          </div>
          <div class="d-flex justify-content-between my-2">
            <div style="width: 100%">
              <label>Nội dung bình luận</label>
              <textarea
                readonly
                v-model="recordSelectUnique.Content"
                class="w-100 m-input mt-2"
                style="height: 100px"
              ></textarea>
            </div>
          </div>
        </div>
        <div class="mx-5">
          <hr />
          <h2>Phản hồi khách hàng</h2>
          <div class="d-flex justify-content-between my-2">
            <div style="width: 100%">
              <label>Nội dung phản hồi</label>
              <textarea
                v-model="feedbackUser"
                class="w-100 m-input mt-2"
                style="height: 100px"
              ></textarea>
              <span :class="{ error: errorFeedback !== '' }">{{
                errorFeedback
              }}</span>
            </div>
          </div>
        </div>
        <div class="form-footer mx-5 px-0 mt-5 mb-4">
          <button @click="hideDetailRecord" class="btn btn-danger px-4">
            Huỷ
          </button>
          <button @click="addFeedback" class="btn btn-primary px-4">
            Phản hồi
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  components: {},
  props: {
    show: Boolean,
    record: Object,
    statusCode: Number,
  },
  data() {
    return {
      recordSelect: {},
      recordSelectUnique: {},
      feedbackUser: "",
      errorFeedback: "",
    };
  },
  methods: {
    /**
     * Thêm phản hồi
     */
    async addFeedback() {
      if (this.feedbackUser === "") {
        this.errorFeedback = "Nỗi dung phản hồi không được phép bỏ trống";
      } else {
        let recordInsert = {
          FeedbackContent: this.feedbackUser,
          CommentId: this.recordSelectUnique.CommentId,
          Status: 1,
          CreatedDateFeedback: new Date(),
          CreatedBy: localStorage.getItem("FullName"),
          FeedbackFor: this.recordSelectUnique.CommentName,
        };
        let result = await this.apiService.post("Feedback/post", recordInsert);
        console.log(result);
      }
    },
    /**
     * Hàm ẩn form chi tiết
     * @author: Nguyễn Văn Trúc (9/12/2023)
     */
    hideDetailRecord: function () {
      this.$emit("hideDetailRecord", false);
    },

    /**
     * Hàm Thực hiện valide dữ liệu
     * @author: Nguyễn Văn Trúc (9/12/2023)
     */
    validateData: function () {
      try {
        let me = this;
        let checkError = true;
        me.errors = {};
        // Kiểm tra record code
        if (me.common.validateInput(me.recordSelect.CommentCode)) {
          me.errors.CommentCode = "Mã phản hồi không được phép bỏ trống";
          checkError = false;
        }

        // Kiểm tra fullname
        if (me.common.validateInput(me.recordSelect.CommentName)) {
          me.errors.CommentName = "Tên phản hồi không được phép bỏ trống";
          checkError = false;
        }

        // Kiểm tra email
        if (!me.common.checkEmail(me.recordSelect.Email)) {
          me.errors.Email = "Email không đúng định dạng";
          checkError = false;
        }

        // Kiểm tra số điện thoại
        if (me.common.validateInput(me.recordSelect.PhoneNumber)) {
          me.errors.PhoneNumber = "Số điện thoại không được phép bỏ trống";
          checkError = false;
        }

        // Kiểm tra địa chỉ
        if (me.common.validateInput(me.recordSelect.PhoneNumber)) {
          me.errors.Address = "Địa chỉ không được phép bỏ trống";
          checkError = false;
        }

        return checkError;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm thêm hoặc sửa nhân viên
     * @author: Nguyễn Văn Trúc(13/3/2024)
     */
    async insertOfUpdateRecord() {
      console.log(this.recordSelect);
      try {
        let result = 0;
        if (this.validateData()) {
          if (this.statusCode === this.helper.Status.Insert) {
            this.recordSelect.CreatedDate = new Date();
            result = await this.apiService.post(
              "Comment/post",
              this.recordSelect
            );
          } else if (this.statusCode === this.helper.Status.Update) {
            this.recordSelect.ModifiedDate = new Date();
            result = await this.apiService.update(
              "Comment/put",
              this.recordSelect.CommentId,
              this.recordSelect
            );
          }
          if (result === 1) {
            this.dialogSaveSuccess();
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Thực hiện lưu nhân viên và giữ form
     * @author: Nguyễn Văn Trúc (13/3/2024)
     */
    saveRecord() {
      try {
        this.insertOfUpdateRecord();
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm thực hiện sau khi thao tác(Thêm, Sửa) thành công
     * @author: Nguyễn Văn Trúc (9/12/2023)
     */
    dialogSaveSuccess() {
      try {
        // load lại dữ liệu
        this.hideDetailRecord();
        this.$emit("loadData", true);
      } catch (error) {
        console.log(error);
      }
    },
    filterUniqueFeedback(data) {
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
  },
  /**
   * Hàm Thực hiện khi có thay đổi về show
   * @author: Nguyễn Văn Trúc (9/12/2023)
   */
  watch: {
    async show(value) {
      if (value === true) {
        this.recordSelect = JSON.parse(JSON.stringify(this.record));
        let listRecord = this.filterUniqueFeedback(this.recordSelect);
        this.recordSelectUnique = listRecord[0];
      }
    },
  },
};
</script>
<style scoped>
.error {
  color: red;
}
</style>
