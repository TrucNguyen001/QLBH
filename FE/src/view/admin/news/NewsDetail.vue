<template>
  <div class="curtain" v-if="show">
    <div
      class="m-form"
      style="
        position: absolute;
        border: 1px solid darkgreen;
        width: 1300px;
        height: 75%;
        z-index: 999;
        overflow-y: auto;
      "
    >
      <div class="form">
        <div
          class="form-header d-flex align-items-center justify-content-between"
        >
          <h2>Thông tin tin tức</h2>
          <button @click="hideDetailRecord" class="btn btn-close mx-4"></button>
        </div>
        <div class="form-body mx-5">
          <div class="d-flex justify-content-between align-items-center">
            <div style="width: 45%">
              <MInput
                :inputErrorFirst="nameInputErrorFirst"
                nameInput="NewsCode"
                v-model="recordSelect.NewsCode"
                label="Mã tin tức"
                :required="true"
                :isError="this.errors.NewsCode"
                @removeClassErrorInput="this.errors.NewsCode = null"
              ></MInput>
            </div>
            <div style="width: 45%">
              <div style="width: 22%; margin-bottom: -18px">
                <label style="font-weight: 600; color: #1f1f1f" for="img"
                  >Hình ảnh<i class="text-danger">*</i></label
                >
                <input
                  @change="changeImg"
                  class="mt-2"
                  name="img"
                  type="file"
                  style="height: 36px"
                />
                <div
                  v-if="
                    recordSelect.Image !== undefined &&
                    isImageFileName(recordSelect.Image)
                  "
                  class="mt-3"
                >
                  <img
                    style="height: 100px"
                    :src="require('@/assets/img/new/' + recordSelect.Image)"
                  />
                </div>
                <div
                  class="position-absolute"
                  style="color: red; font-size: 12px"
                >
                  {{ errors.Image }}
                </div>
              </div>
            </div>
          </div>
          <div class="d-flex justify-content-between">
            <div class="w-100">
              <MInput
                nameInput="NewsName"
                v-model="recordSelect.NewsName"
                label="Tiêu đề"
                :required="true"
                :isError="this.errors.NewsName"
                @removeClassErrorInput="this.errors.NewsName = null"
              ></MInput>
            </div>
          </div>
          <div class="d-flex justify-content-between">
            <div class="w-100 my-2">
              <label
                class="mt-3 mb-2"
                style="font-weight: 600; color: #1f1f1f"
                for="img"
                >Mô tả<i class="text-danger">*</i></label
              >
              <ckeditor
                :editor="editor"
                v-model="recordSelect.Content"
              ></ckeditor>
              <div
                class="position-absolute"
                style="color: red; font-size: 12px"
              >
                {{ errors.Content }}
              </div>
            </div>
          </div>
        </div>
        <div
          class="form-footer mx-5 px-0 mt-5 mb-4 d-flex justify-content-end"
          v-if="statusCode === this.helper.Status.See"
        >
          <button @click="hideDetailRecord" class="btn btn-primary px-4">
            Thoát
          </button>
        </div>
        <div v-else class="form-footer mx-5 px-0 mt-5 mb-4">
          <button @click="hideDetailRecord" class="btn btn-danger px-4">
            Huỷ
          </button>
          <button @click="saveRecord" class="btn btn-success px-4">
            Cất bản ghi
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
export default {
  components: {},
  props: {
    show: Boolean,
    record: Object,
    statusCode: Number,
  },
  data() {
    return {
      editor: ClassicEditor,
      editorData: "<h1>Content of the editor.</h1>",
      recordSelect: {},
      errors: {
        NewsCode: "",
        NewsName: "",
        Image: "",
        Content: "",
      },
    };
  },
  methods: {
    /**
     * Kiểm tra xem có phải ảnh không
     * @param {tên file} fileName
     * @returns: true: Đúng, false: Sai
     * @author: Nguyễn Văn Trúc(4/4/2024)
     */
    isImageFileName(fileName) {
      const imageExtensions = /\.(jpg|jpeg|png|gif)$/i; // Danh sách các phần mở rộng ảnh phổ biến
      return imageExtensions.test(fileName);
    },
    changeImg() {
      try {
        this.recordSelect.Image = event.target.files[0].name;
      } catch (error) {
        console.log(error);
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
        if (me.common.validateInput(me.recordSelect.NewsCode)) {
          me.errors.NewsCode = "Mã tin tức không được phép bỏ trống";
          checkError = false;
        }

        // Kiểm tra fullname
        if (me.common.validateInput(me.recordSelect.NewsName)) {
          me.errors.NewsName = "Tiêu đề không được phép bỏ trống";
          checkError = false;
        }

        // Kiểm tra hình ảnh
        if (me.common.validateInput(me.recordSelect.Image)) {
          me.errors.Image = "Hình ảnh không được phép bỏ trống";
          checkError = false;
        }

        // Kiểm tra mô tả
        if (me.common.validateInput(me.recordSelect.Content)) {
          me.errors.Content = "Nội dung không được phép bỏ trống";
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
            this.recordSelect.Status = 1;
            result = await this.apiService.post("News/post", this.recordSelect);
          } else if (this.statusCode === this.helper.Status.Update) {
            this.recordSelect.ModifiedDate = new Date();
            result = await this.apiService.update(
              "News/put",
              this.recordSelect.NewsId,
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
  },
  /**
   * Hàm Thực hiện khi có thay đổi về show
   * @author: Nguyễn Văn Trúc (9/12/2023)
   */
  watch: {
    async show(value) {
      if (value === true) {
        this.errors = {};
        this.recordSelect = JSON.parse(JSON.stringify(this.record));
      }
    },
  },

  /**
   * Hàm thực hiện nhận sự kiện bấm nút
   * @author: Nguyễn Văn Trúc (3/3/2024)
   */
  mounted() {
    let me = this;
    me.emitter.on(me.helper.Emitter.SendEvent, (value) => {
      // esc: Thoát form chi tiết nhân viên
      if (value === me.helper.Status.Exit && me.show === true) {
        me.hideDetailRecord();
      }

      // CTRL + S: Lưu và cất
      if (value === me.helper.Status.Save && me.show === true) {
        me.saveRecordAndExit();
      }
    });
  },
};
</script>
<style scoped></style>
