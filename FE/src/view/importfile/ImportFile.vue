<template>
  <div class="import-Product">
    <div class="header-import">
      <div class=""></div>
    </div>
    <div class="body">
      <!-- Bước 1 -->
      <div v-if="pageIndex === 1" class="page1">
        <div class="main">
          <div>
            <div
              style="height: 36px"
              class="text-white d-flex align-items-center p-2 bg-primary"
            >
              1. Chọn tệp nguồn
            </div>
            <div
              class="d-flex align-items-center p-2 my-4 bg-light"
              style="height: 36px"
            >
              2. Kiểm tra dữ liệu
            </div>
            <div
              class="d-flex align-items-center p-2 bg-light"
              style="height: 36px"
            >
              3. Kiểm tra nhập khẩu
            </div>
          </div>
          <div class="content">
            <div>
              <div style="margin-top: 10px">
                {{ resource.MImportFile.SelectFile }}
              </div>
              <div style="margin: 10px 0; display: inline-flex">
                <input
                  v-model="nameFile"
                  class="m-input"
                  type="text"
                  style="width: 500px"
                  readonly
                />
                <input
                  ref="input_file"
                  type="file"
                  @change="inputFileChange"
                  hidden
                />
                <button
                  @click="importFile"
                  style="margin-left: 20px"
                  class="btn btn-primary px-4"
                >
                  {{ resource.MImportFile.Select }}
                </button>
              </div>
              <div>
                {{ resource.MImportFile.HaveFile }}
                <a href="/Danh_sach_nhan_vien.xlsx">{{
                  resource.MImportFile.Here
                }}</a>
              </div>
            </div>
          </div>
        </div>
        <div class="footer px-5 mt-2">
          <div class="footer d-flex justify-content-between">
            <div class="footer-left">
              <div class="m-button m-button-cancel"></div>
            </div>
            <div class="footer-right">
              <div class="btn btn-primary not_active">
                {{ resource.MImportFile.Back }}
              </div>
              <div @click="changePage2" class="btn btn-primary mx-4">
                {{ resource.MImportFile.Continue }}
              </div>
              <div class="btn btn-danger" @click="redirectToPageProduct">
                {{ resource.MImportFile.Cancel }}
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Bước 2 -->
      <div v-if="pageIndex === 2" class="page2">
        <div class="main">
          <div>
            <div
              style="height: 36px"
              class="d-flex align-items-center p-2 bg-light"
            >
              1. Chọn tệp nguồn
            </div>
            <div
              class="text-white d-flex align-items-center p-2 my-4 bg-primary"
              style="height: 36px"
            >
              2. Kiểm tra dữ liệu
            </div>
            <div
              class="d-flex align-items-center p-2 bg-light"
              style="height: 36px"
            >
              3. Kiểm tra nhập khẩu
            </div>
          </div>
          <div class="content">
            <div class="content-header">
              <div class="content-title">
                {{ numberOfSuccess }} / {{ listProductImport.length }}
                {{ resource.MImportFile.RowValid }}
              </div>
              <div>
                {{ numberOfFail }} / {{ listProductImport.length }}
                {{ resource.MImportFile.RowNoValid }}
              </div>
            </div>
            <div class="content-main">
              <div class="table-product">
                <table style="width: 1800px" class="m-table tbl-product">
                  <thead>
                    <tr>
                      <th style="width: 150px">Mã sản phẩm</th>
                      <th>Tên sản phẩm</th>
                      <th>Avatar</th>
                      <th>Số lượng</th>
                      <th>Giá</th>
                      <th>Giá giảm</th>
                      <th>Mã nhà cung cấp</th>
                      <th>Mã loại sản phẩm</th>
                      <th>Độ hot</th>
                      <th>Trạng thái</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr
                      v-for="Product in listProductImport"
                      :key="Product.ProductId"
                    >
                      <td
                        style="text-align: left; padding-left: 15px"
                        class="Product-code"
                      >
                        {{ Product.ProductCode }}
                      </td>
                      <td>{{ Product.ProductName }}</td>
                      <td>
                        {{ Product.Avatar }}
                      </td>
                      <td>
                        {{ Product.Quantity }}
                      </td>
                      <td>
                        {{ Product.Price }}
                      </td>
                      <td>
                        {{ Product.PriceReduced }}
                      </td>
                      <td>{{ Product.SupplierId }}</td>
                      <td>{{ Product.ProductTypeId }}</td>
                      <td>{{ Product.Hot }}</td>
                      <td
                        v-if="Product.ImportInvalidErrors.length !== 0"
                        :class="{
                          error_record: Product.IsImported === false,
                          success_record: Product.IsImported === true,
                        }"
                      >
                        <p
                          v-for="(error, index) in Product.ImportInvalidErrors"
                          :key="index"
                        >
                          {{ error }}
                        </p>
                      </td>
                      <td style="color: darkgreen" v-else>Hợp lệ</td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        </div>
        <div class="d-flex justify-content-between px-5 mt-2">
          <div class="footer-left">
            <div></div>
          </div>
          <div class="footer-right d-flex">
            <div @click="changePage1" class="btn btn-primary">
              {{ resource.MImportFile.Back }}
            </div>
            <div
              class="btn btn-primary mx-4"
              v-if="numberOfSuccess === 0"
              @click="noChangePage3"
            >
              {{ resource.MImportFile.Continue }}
            </div>
            <div v-else>
              <div @click="changePage3" class="btn btn-primary mx-4">
                {{ resource.MImportFile.Continue }}
              </div>
            </div>
            <div @click="redirectToPageProduct" class="btn btn-danger">
              {{ resource.MImportFile.Cancel }}
            </div>
          </div>
        </div>
      </div>

      <!-- Bước 3 -->
      <div v-if="pageIndex === 3" class="page3">
        <div class="main">
          <div>
            <div
              style="height: 36px"
              class="d-flex align-items-center p-2 bg-light"
            >
              1. Chọn tệp nguồn
            </div>
            <div
              class="d-flex align-items-center p-2 my-4 bg-light"
              style="height: 36px"
            >
              2. Kiểm tra dữ liệu
            </div>
            <div
              class="text-white d-flex align-items-center p-2 bg-primary"
              style="height: 36px"
            >
              3. Kiểm tra nhập khẩu
            </div>
          </div>
          <div class="content">
            <div>
              <div style="margin-top: 10px">
                <h2 class="title">
                  {{ resource.MImportFile.ResultImport }}
                </h2>
              </div>
              <div style="margin: 10px 0">
                {{ resource.MImportFile.DownloadFile }}
                <strong @click="downloadFile(listProductImport)">{{
                  resource.MImportFile.Here
                }}</strong
                >.
              </div>
              <div>
                <p>
                  {{ resource.MImportFile.RowSuccess }}{{ numberOfSuccess }}
                </p>
                <p>{{ resource.MImportFile.RowError }}{{ numberOfFail }}</p>
              </div>
            </div>
          </div>
        </div>
        <div class="footer px-5 mt-2">
          <div class="footer-left">
            <div class="m-button m-button-cancel"></div>
          </div>
          <div class="footer-right">
            <div></div>
            <div @click="redirectToPageProduct" class="btn btn-danger px-4">
              {{ resource.MImportFile.Close }}
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "ImportFile",
  data() {
    return {
      listProductImport: [],
      listEmployeeInsertFalse: {},
      numberOfSuccess: 0,
      numberOfFail: 0,
      pageIndex: 1,
      nameFile: "",
      file: null,
      toke: "",
      language: "",
    };
  },
  methods: {
    /**
     * Trở về trang danh sách nhân viên
     * @author: Nguyễn Văn Trúc (31/1/2024)
     */
    redirectToPageProduct() {
      this.$router.push("/admin/product");
    },

    /**
     * Hàm Thực hiện import file excel
     * @author: Nguyễn Văn Trúc (29/1/2024)
     */
    importFile: function () {
      this.$refs["input_file"].click();
    },
    /**Hàm thực hiện kiểm tra xem file chọn đúng định dạng không
     * @param {Tên file truyền vào để kiểm tra} fileName
     * @author: Nguyễn Văn Trúc(30/01/2024)
     */
    isExcelFile(fileName) {
      try {
        // Chuyển đổi tên tệp thành chữ thường
        let lowerCaseFileName = fileName.toLowerCase();
        // Kiểm tra phần mở rộng
        return (
          lowerCaseFileName.endsWith(".xls") ||
          lowerCaseFileName.endsWith(".xlsx")
        );
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Hàm thực hiện lấy file đầu tiên
     * @author: Nguyễn Văn Trúc(30/01/2024)
     */
    inputFileChange() {
      try {
        this.selectedFile = event.target.files[0];
        this.uploadFile(this.selectedFile);
        this.selectedFile = null;
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Hàm thực hiện import dữ liệu từ file
     * @param {*file lựa chọn} file
     * @author: Nguyễn Văn Trúc(30/01/2024)
     */
    uploadFile(file) {
      try {
        let me = this;
        if (!me.isExcelFile(file.name)) {
          // Hiển thị dialog
          me.common.showDialog(
            me.resource.ConfirmImportFileError.Content,
            me.resource.ConfirmImportFileError.Title,
            me.helper.TypeIcon.Error
          );
        } else {
          this.nameFile = file.name;
          this.file = file;
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Thay đổi trang
     * @author: Nguyễn Văn Trúc(30/01/2024)
     */
    changePage1() {
      this.pageIndex = 1;
    },
    /**
     * load dữ liệu kiểm tra lỗi
     * @author: Nguyễn Văn Trúc(30/01/2024)
     */
    changePage2() {
      try {
        var me = this;
        console.log(me.nameFile);
        if (me.nameFile === "") {
          // Hiển thị dialog
          me.common.showDialog(
            me.resource.NotFoundFile.Content,
            me.resource.NotFoundFile.Title,
            me.helper.TypeIcon.Error
          );
        } else {
          me.pageIndex = 2;
          me.commitData(false);
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Thêm bản ghi vào trong database
     * @author: Nguyễn Văn Trúc(31/01/2024)
     */
    changePage3() {
      try {
        this.commitData(true);
        this.pageIndex = 3;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Thông báo thi không có bản ghi nào thành công
     * @author: Nguyễn Văn Trúc(16/03/2024)
     */
    noChangePage3() {
      try {
        this.common.showToast(
          this.resource.MImportFile.ToastError.Content,
          this.helper.TypeIcon.Error
        );
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Gửi api để thêm dữ liệu
     * @param {Biến bool dùng kiểm tra có commit hay không} isCommit
     * @author: Nguyễn Văn Trúc(30/01/2024)
     */
    async commitData(isCommit) {
      try {
        let me = this;
        me.common.showLoading();
        me.listProductImport = await me.apiService.importFile(
          "Product/import",
          isCommit,
          me.file
        );
        me.listEmployeeInsertFalse = me.listProductImport.filter(
          (Product) => Product.IsImported === false
        );
        me.numberOfFail = me.listEmployeeInsertFalse.length;
        me.numberOfSuccess = me.listProductImport.length - me.numberOfFail;
        me.common.showLoading(false);
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Tải những bản ghi thất bại
     * @author: Nguyễn Văn Trúc(29/2/2024)
     */
    async downloadFile(listEmployeeExport) {
      this.common.showLoading();
      listEmployeeExport.forEach(function (number) {
        number.Email = "";
      });
      await this.apiService.downloadFileExcel(listEmployeeExport);
      this.common.showLoading(false);
    },
  },
  created() {
    this.language = localStorage.getItem("language");
  },
};
</script>

<style scoped>
@import url(../../css/pages/importfile.css);
.page2 strong {
  margin-left: 4px;
}
strong {
  cursor: pointer;
}
.page1 a {
  text-decoration: none;
  color: black;
  font-weight: 700;
}
.navbar div {
  cursor: default;
}
.navbar div:hover {
  background-color: initial;
}
.navbar .is-active:hover {
  background: #66afff;
}
.error_record {
  color: red;
}
.success_record {
  color: green;
}
.m-button {
  display: flex;
  align-items: center;
  justify-content: center;
  border: 1px solid #bdbdbd;
}
.import-Product .navbar {
  padding-top: 0;
  padding-left: 0;
}
.import-Product .navbar > div {
  border-radius: 0;
}
.content-main {
  overflow-x: auto;
}
.content-main table {
  height: 100%;
}
</style>
