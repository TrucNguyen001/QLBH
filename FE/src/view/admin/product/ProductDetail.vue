<template>
  <div class="curtain" v-if="show">
    <div
      class="m-form"
      style="
        position: absolute;
        border: 1px solid darkgreen;
        z-index: 999;
        height: 95%;
        width: 95%;
        overflow-y: auto;
      "
    >
      <div class="form">
        <div
          class="form-header d-flex align-items-center justify-content-between"
        >
          <h2>Thông tin sản phẩm</h2>
          <button @click="hideDetailRecord" class="btn btn-close mx-4"></button>
        </div>
        <div class="form-body mx-5">
          <div class="d-flex justify-content-between align-items-center">
            <div style="width: 48%" class="d-flex justify-content-between">
              <div style="width: 35%">
                <MInput
                  :inputErrorFirst="nameInputErrorFirst"
                  nameInput="ProductCode"
                  v-model="recordSelect.ProductCode"
                  label="Mã sản phẩm"
                  :required="true"
                  :isError="this.errors.ProductCode"
                  @removeClassErrorInput="this.errors.ProductCode = null"
                ></MInput>
              </div>
              <div style="width: 60%">
                <MInput
                  nameInput="ProductName"
                  v-model="recordSelect.ProductName"
                  label="Tên sản phẩm"
                  :required="true"
                  :isError="this.errors.ProductName"
                  @removeClassErrorInput="this.errors.ProductName = null"
                ></MInput>
              </div>
            </div>
            <div style="width: 48%" class="d-flex justify-content-between">
              <div style="width: 22%; margin-bottom: -18px">
                <label style="font-weight: 600; color: #1f1f1f" for="avatar"
                  >Ảnh đại diện<i style="color: red">*</i></label
                >
                <input
                  @change="inputFileChange"
                  class="mt-2"
                  name="avatar"
                  type="file"
                />
                <div
                  v-if="
                    recordSelect.Avatar !== undefined &&
                    isImageFileName(recordSelect.Avatar)
                  "
                  class="mt-3"
                >
                  <img
                    style="height: 100px"
                    :src="
                      require('@/assets/img/product/' + recordSelect.Avatar)
                    "
                  />
                </div>
                <div
                  class="position-absolute"
                  style="color: red; font-size: 12px"
                >
                  {{ errors.Avatar }}
                </div>
              </div>
              <div style="width: 48%; margin-bottom: -18px">
                <label style="font-weight: 600; color: #1f1f1f" for="avatar"
                  >Hình ảnh khác</label
                >
                <input
                  @change="inputFilesChange"
                  multiple
                  class="mt-3"
                  name="avatar"
                  type="file"
                />
                <div
                  v-if="
                    recordSelect.Image !== undefined &&
                    isImageFileName(recordSelect.Image)
                  "
                  class="mt-3"
                >
                  <img
                    class="m-2"
                    v-for="(item, index) in recordSelect.Image.split(' ')"
                    :key="index"
                    style="height: 100px"
                    :src="require('@/assets/img/product/' + item)"
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
            <div style="width: 48%" class="d-flex justify-content-between">
              <div style="width: 24%">
                <MInput
                  nameInput="Quantity"
                  v-model="recordSelect.Quantity"
                  label="Số lượng"
                  :required="true"
                  :isError="this.errors.Quantity"
                  @removeClassErrorInput="this.errors.Quantity = null"
                ></MInput>
              </div>
              <div class="mt-2" style="width: 24%">
                <MInput
                  nameInput="Hot"
                  v-model="recordSelect.Hot"
                  label="Độ hot"
                  :isError="this.errors.Hot"
                  @removeClassErrorInput="this.errors.Hot = null"
                ></MInput>
              </div>
              <div style="width: 24%">
                <MInput
                  nameInput="Price"
                  v-model="recordSelect.Price"
                  label="Giá"
                  :required="true"
                  :isError="this.errors.Price"
                  @removeClassErrorInput="this.errors.Price = null"
                ></MInput>
              </div>
              <div class="mt-2" style="width: 24%">
                <MInput
                  nameInput="PriceBuy"
                  v-model="recordSelect.PriceBuy"
                  label="Giá bán"
                  :isError="this.errors.PriceBuy"
                  @removeClassErrorInput="this.errors.PriceBuy = null"
                ></MInput>
              </div>
            </div>
            <div style="width: 48%" class="d-flex justify-content-between">
              <div style="width: 45%" class="product-type">
                <MCombobox
                  name="ProductType"
                  apiData="ProductType/getAll/1"
                  :entity="productType"
                  entityName="ProductTypeName"
                  entityId="ProductTypeId"
                  @emtitySelect="productTypeSelected"
                  label="Loại sản phẩm"
                  @removeClassErrorInput="this.errors.ProductType = null"
                  :required="true"
                  :isError="this.errors.ProductType"
                  @errorNoFind="errorNoFindProductType"
                ></MCombobox>
              </div>
              <div style="width: 45%; margin-right: 20px">
                <MCombobox
                  apiData="Supplier/getAll/1"
                  entityName="SupplierName"
                  entityId="SupplierId"
                  @emtitySelect="supplierSelected"
                  label="Nhà cung cấp"
                  :entityIdSelected="this.recordSelect.SupplierId"
                  name="Supplier"
                  :entity="supplier"
                  @removeClassErrorInput="this.errors.Supplier = null"
                  :required="true"
                  :isError="this.errors.Supplier"
                  @errorNoFind="errorNoFindSupplier"
                ></MCombobox>
              </div>
            </div>
          </div>
          <div class="describe">
            <MTextArea
              nameInput="Description"
              v-model="recordSelect.Description"
              label="Mô tả"
              :required="true"
              :isError="this.errors.Description"
              @removeClassErrorInput="this.errors.Description = null"
            ></MTextArea>
          </div>
          <div
            v-if="statusCode === this.helper.Status.See"
            class="d-flex justify-content-between"
          >
            <div style="width: 45%">
              <MInput
                v-model="recordSelect.CreatedDate"
                label="Ngày tạo"
              ></MInput>
            </div>
            <div style="width: 45%">
              <MInput
                v-model="recordSelect.CreatedBy"
                label="Người tạo"
              ></MInput>
            </div>
          </div>
          <div
            v-if="
              recordSelect.ProductTypeId !==
                'c655d547-f06f-4ba7-a5f3-a615213fcae7' &&
              this.type === 'product'
            "
          >
            <h3 class="mt-4">Cấu hình sản phẩm:</h3>
            <div class="d-flex justify-content-between">
              <div style="width: 48%" class="d-flex justify-content-between">
                <div style="width: 32%">
                  <MInput
                    v-model="recordSelect.Screen"
                    label="Màn hình"
                  ></MInput>
                </div>
                <div style="width: 32%">
                  <MInput
                    v-model="recordSelect.Resolution"
                    label="Độ phân giải"
                  ></MInput>
                </div>
                <div style="width: 32%">
                  <MInput
                    v-model="recordSelect.ScreenTechnology"
                    label="Công nghệ màn hình"
                  ></MInput>
                </div>
              </div>
              <div style="width: 48%" class="d-flex justify-content-between">
                <div style="width: 32%">
                  <MInput v-model="recordSelect.Memory" label="Bộ nhớ"></MInput>
                </div>
                <div style="width: 32%">
                  <MInput
                    v-model="recordSelect.Material"
                    label="Chất liệu"
                  ></MInput>
                </div>
                <div style="width: 32%">
                  <MInput
                    v-model="recordSelect.Size"
                    label="Kích thước"
                  ></MInput>
                </div>
              </div>
            </div>
            <div class="d-flex justify-content-between">
              <div style="width: 48%" class="d-flex justify-content-between">
                <div style="width: 32%">
                  <MInput v-model="recordSelect.Camera" label="Camera"></MInput>
                </div>
                <div style="width: 32%">
                  <MInput v-model="recordSelect.Pin" label="Pin"></MInput>
                </div>
                <div style="width: 32%">
                  <MInput v-model="recordSelect.CPU" label="CPU"></MInput>
                </div>
              </div>
              <div style="width: 48%" class="d-flex justify-content-between">
                <div style="width: 32%">
                  <MInput v-model="recordSelect.Chip" label="Chip"></MInput>
                </div>
                <div style="width: 32%">
                  <MInput v-model="recordSelect.RAM" label="RAM"></MInput>
                </div>
                <div style="width: 32%">
                  <MInput
                    v-model="recordSelect.Weight"
                    label="Trọng lượng"
                  ></MInput>
                </div>
              </div>
            </div>
            <div class="d-flex justify-content-between align-items-center mt-4">
              <div style="width: 48%" class="d-flex justify-content-between">
                <div style="width: 22%; margin-bottom: -18px">
                  <label style="font-weight: 600; color: #1f1f1f" for="imgcpu"
                    >Hình ảnh CPU</label
                  >
                  <input
                    @change="inputFileChangeCPU"
                    class="mt-2"
                    name="imgcpu"
                    type="file"
                  />
                  <div
                    v-if="
                      recordSelect.ImageCPU !== undefined &&
                      isImageFileName(recordSelect.ImageCPU)
                    "
                    class="mt-3"
                  >
                    <img
                      style="height: 100px"
                      :src="
                        require('@/assets/img/product/' + recordSelect.ImageCPU)
                      "
                    />
                  </div>
                  <div
                    class="position-absolute"
                    style="color: red; font-size: 12px"
                  >
                    {{ errors.ImageCPU }}
                  </div>
                </div>
                <div style="width: 48%; margin-bottom: -18px">
                  <label style="font-weight: 600; color: #1f1f1f" for="imgram"
                    >Hình ảnh RAM</label
                  >
                  <input
                    @change="inputFileChangeRAM"
                    class="mt-2"
                    name="imgram"
                    type="file"
                  />
                  <div
                    v-if="
                      recordSelect.ImageRAM !== undefined &&
                      isImageFileName(recordSelect.ImageRAM)
                    "
                    class="mt-3"
                  >
                    <img
                      style="height: 100px"
                      :src="
                        require('@/assets/img/product/' + recordSelect.ImageRAM)
                      "
                    />
                  </div>
                  <div
                    class="position-absolute"
                    style="color: red; font-size: 12px"
                  >
                    {{ errors.ImageRAM }}
                  </div>
                </div>
              </div>
              <div style="width: 48%" class="d-flex justify-content-between">
                <div style="width: 22%; margin-bottom: -18px">
                  <label
                    style="font-weight: 600; color: #1f1f1f"
                    for="imgcamera"
                    >Hình ảnh Camera</label
                  >
                  <input
                    @change="inputFileChangeCAMERA"
                    class="mt-2"
                    name="imgcamera"
                    type="file"
                  />
                  <div
                    v-if="
                      recordSelect.ImageCamera !== undefined &&
                      isImageFileName(recordSelect.ImageCamera)
                    "
                    class="mt-3"
                  >
                    <img
                      style="height: 100px"
                      :src="
                        require('@/assets/img/product/' +
                          recordSelect.ImageCamera)
                      "
                    />
                  </div>
                  <div
                    class="position-absolute"
                    style="color: red; font-size: 12px"
                  >
                    {{ errors.ImageCamera }}
                  </div>
                </div>
                <div style="width: 48%; margin-bottom: -18px">
                  <label style="font-weight: 600; color: #1f1f1f" for="imgpin"
                    >Hình ảnh Pin</label
                  >
                  <input
                    @change="inputFileChangePIN"
                    class="mt-2"
                    name="imgpin"
                    type="file"
                  />
                  <div
                    v-if="
                      recordSelect.ImagePin !== undefined &&
                      isImageFileName(recordSelect.ImagePin)
                    "
                    class="mt-3"
                  >
                    <img
                      style="height: 100px"
                      :src="
                        require('@/assets/img/product/' + recordSelect.ImagePin)
                      "
                    />
                  </div>
                  <div
                    class="position-absolute"
                    style="color: red; font-size: 12px"
                  >
                    {{ errors.ImagePin }}
                  </div>
                </div>
              </div>
            </div>
            <div class="describe mt-5">
              <MTextArea
                v-model="recordSelect.ContentCPU"
                label="Mô tả CPU"
              ></MTextArea>
            </div>
            <div class="describe">
              <MTextArea
                v-model="recordSelect.ContentPin"
                label="Mô tả Pin"
              ></MTextArea>
            </div>
            <div class="describe">
              <MTextArea
                v-model="recordSelect.ContentRAM"
                label="Mô tả RAM"
              ></MTextArea>
            </div>
            <div class="describe">
              <MTextArea
                v-model="recordSelect.ContentCamera"
                label="Mô tả Camera"
              ></MTextArea>
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
export default {
  components: {},
  props: {
    show: Boolean,
    record: Object,
    statusCode: Number,
    type: String,
  },
  data() {
    return {
      recordSelect: {},
      errors: {
        ProductCode: "",
        ProductName: "",
        Avatar: "",
        Image: "",
        Description: "",
        Quantity: "",
        Price: "",
        Supplier: "",
        ProductType: "",
        Hot: "",
        ImageCPU: "",
        ImageRAM: "",
        ImageCamera: "",
        ImagePin: "",
      },
      productTypeSelect: {},
      supplierSelect: {},
      productType: {},
      supplier: {},
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
        if (me.common.validateInput(me.recordSelect.ProductCode)) {
          me.errors.ProductCode = "Mã sản phẩm không được phép bỏ trống";
          checkError = false;
        }

        // Kiểm tra fullname
        if (me.common.validateInput(me.recordSelect.ProductName)) {
          me.errors.ProductName = "Tên sản phẩm không được phép bỏ trống";
          checkError = false;
        }

        if (me.common.validateInput(me.recordSelect.Avatar)) {
          me.errors.Avatar = "Ảnh đại điện không được phép bỏ trống";
          checkError = false;
        } else if (!this.isImageFileName(me.recordSelect.Avatar)) {
          me.errors.Avatar = "Ảnh đại điện không đúng định dạng";
          checkError = false;
        }

        if (
          !this.isImageFileName(me.recordSelect.Image) &&
          !me.common.validateInput(me.recordSelect.Image)
        ) {
          me.errors.Image = "Hình ảnh khác không đúng định dạng";
          checkError = false;
        }

        if (
          !this.isImageFileName(me.recordSelect.ImageCPU) &&
          !me.common.validateInput(me.recordSelect.ImageCPU)
        ) {
          me.errors.ImageCPU = "Ảnh CPU không đúng định dạng";
          checkError = false;
        }

        if (
          !this.isImageFileName(me.recordSelect.ImageCamera) &&
          !me.common.validateInput(me.recordSelect.ImageCamera)
        ) {
          me.errors.ImageCamera = "Ảnh Camera không đúng định dạng";
          checkError = false;
        }

        if (
          !this.isImageFileName(me.recordSelect.ImagePin) &&
          !me.common.validateInput(me.recordSelect.ImagePin)
        ) {
          me.errors.ImagePin = "Ảnh Pin không đúng định dạng";
          checkError = false;
        }

        if (
          !this.isImageFileName(me.recordSelect.ImageRAM) &&
          !me.common.validateInput(me.recordSelect.ImageRAM)
        ) {
          me.errors.ImageRAM = "Ảnh RAM không đúng định dạng";
          checkError = false;
        }

        if (me.common.validateInput(me.recordSelect.Description)) {
          me.errors.Description = "Mô tả không được phép bỏ trống";
          checkError = false;
        }

        if (me.common.validateInput(me.recordSelect.Quantity)) {
          me.errors.Quantity = "Số lượng không được phép bỏ trống";
          checkError = false;
        }

        if (me.common.validateInput(me.recordSelect.Price)) {
          me.errors.Price = "Giá không được phép bỏ trống";
          checkError = false;
        }

        if (me.common.validateInput(me.recordSelect.SupplierId)) {
          me.errors.SupplierId = "Nhà cung cấp không được phép bỏ trống";
          checkError = false;
        }

        if (me.common.validateInput(me.recordSelect.ProductTypeId)) {
          me.errors.ProductTypeId = "Loại sản phẩm không được phép bỏ trống";
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
      try {
        let result = 0;
        if (this.validateData()) {
          if (this.statusCode === this.helper.Status.Insert) {
            this.recordSelect.CreatedDate = new Date();
            result = await this.apiService.post(
              "Product/post",
              this.recordSelect
            );
          } else if (this.statusCode === this.helper.Status.Update) {
            this.recordSelect.ModifiedDate = new Date();
            result = await this.apiService.update(
              "Product/put",
              this.recordSelect.ProductId,
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

    /**
     * Hàm Thực hiện lấy productTypeSelected lựa chọn
     * @param(Chức vụ lựa chọn) productTypeSelected
     * @author: Nguyễn Văn Trúc (25/1/2023)
     */
    productTypeSelected: function (productType) {
      this.productTypeSelect = productType;
      this.recordSelect.ProductTypeId = this.productTypeSelect.ProductTypeId;
    },

    /**
     * Hàm Thực hiện lấy supplierSelected lựa chọn
     * @param(Chức vụ lựa chọn) supplierSelected
     * @author: Nguyễn Văn Trúc (25/1/2023)
     */
    supplierSelected: function (supplier) {
      this.supplierSelect = supplier;
      this.recordSelect.SupplierId = this.supplierSelect.SupplierId;
    },

    /**
     * Hàm thực hiện lấy file đầu tiên
     * @author: Nguyễn Văn Trúc(30/01/2024)
     */
    inputFileChange() {
      try {
        this.recordSelect.Avatar = event.target.files[0].name;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm thực hiện lấy file đầu tiên
     * @author: Nguyễn Văn Trúc(30/01/2024)
     */
    inputFilesChange() {
      try {
        let listNameImg = "";
        let files = Array.from(event.target.files);
        files.forEach((element) => {
          listNameImg = listNameImg + element.name + " ";
        });
        listNameImg = listNameImg.trim();
        this.recordSelect.Image = listNameImg;
        console.log(listNameImg);
      } catch (error) {
        console.log(error);
      }
    },
    inputFileChangePIN() {
      try {
        this.recordSelect.ImagePin = event.target.files[0].name;
      } catch (error) {
        console.log(error);
      }
    },
    inputFileChangeRAM() {
      try {
        this.recordSelect.ImageRAM = event.target.files[0].name;
      } catch (error) {
        console.log(error);
      }
    },
    inputFileChangeCPU() {
      try {
        this.recordSelect.ImageCPU = event.target.files[0].name;
      } catch (error) {
        console.log(error);
      }
    },
    inputFileChangeCAMERA() {
      try {
        this.recordSelect.ImageCamera = event.target.files[0].name;
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
        let me = this;
        me.errors = {};
        me.recordSelect = JSON.parse(JSON.stringify(this.record));

        if (me.recordSelect.ProductTypeId) {
          me.productType = await me.apiService.getByInfo(
            "ProductType",
            me.recordSelect.ProductTypeId
          );
        }

        if (me.recordSelect.SupplierId) {
          me.supplier = await me.apiService.getByInfo(
            "Supplier",
            me.recordSelect.SupplierId
          );
        }
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
<style scoped>
.product-type input {
  width: 30%;
}
</style>
