<template>
  <div
    class="container bg-body-tertiary mb-4"
    style="min-height: 70vh; font-size: 14px"
    id="test"
  >
    <div class="row">
      <div class="col-md-12">
        <h2 class="mx-2 my-2">Thông tin đơn hàng</h2>
        <hr />
        <div v-if="this.listRecord.length > 0" class="content-main">
          <div class="header-table d-flex justify-content-end">
            <div class="">
              <button @click="getInvoiceBuySuccess" class="btn btn-primary">
                Danh sách đơn hàng đã mua
              </button>
            </div>
          </div>
          <div class="table">
            <table class="m-table tbl">
              <thead>
                <tr>
                  <th style="width: 140px; background: #f5f5f5">Mã đơn hàng</th>
                  <th>Tên khách hàng</th>
                  <th>Email</th>
                  <th>Số điện thoại</th>
                  <th>Ngày đặt</th>
                  <th>Trạng thái</th>
                  <th>Xem</th>
                  <th>Xoá</th>
                </tr>
              </thead>
              <tbody>
                <tr
                  v-for="record in listRecord"
                  :key="record.CartId"
                  @mouseover="getRecordIdWhenMove(record)"
                  @mouseleave="handleMouseLeave"
                >
                  <td style="background: white">{{ record.InvoiceCode }}</td>
                  <td>{{ record.UserName }}</td>
                  <td>{{ record.Email }}</td>
                  <td>{{ record.PhoneNumber }}</td>
                  <td>
                    {{ this.common.changeDisplayDate(record.CreatedDate) }}
                  </td>
                  <td>
                    {{ this.getStatus(record.StatusInvoice) }}
                  </td>
                  <td @click="detailInvoice(record.InvoiceId)">
                    <button class="btn btn-primary">Xem</button>
                  </td>
                  <td>
                    <button
                      @click="deleteRecord(record.CartId)"
                      class="btn btn-danger px-4"
                    >
                      Xoá
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
        <div
          class="d-flex justify-content-center align-items-center"
          v-if="
            this.listRecord.length <= 0 || this.listRecord.length === undefined
          "
        >
          <a href="/user/list-product"
            ><img
              src="../../../assets/img/empty_invoice.webp"
              alt="Không có đơn hàng nào"
              style="height: 300px"
          /></a>
        </div>
        <div v-if="this.listRecord.length > 0" class="my-2">
          <a
            href="/user/list-product"
            style="width: 220px; margin-bottom: 20px"
            class="d-flex align-items-center justify-content-center text-white btn bg-primary"
            >Tiếp tục mua hàng</a
          >
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: "Invoice",
  data() {
    return {
      InfoSearch: "",
      listRecord: {},
      recordId: "",
      recordCode: "",
      filterObject: {
        pageNumber: 1,
      },
      total: 0,
      isLogin: false,
      status: "",
    };
  },
  methods: {
    /**
     * Chi tiết hoá đơn
     */
    detailInvoice(id) {
      this.$router.push(`/invoice-detail/${id}`);
    },
    /**
     * Hàm lấy những hoá đơn thành công
     */
    async getInvoiceBuySuccess() {
      this.listRecord = await this.apiService.getByInfo(
        "Invoice/user-success",
        localStorage.getItem("AccountId")
      );
      this.listRecord = this.filterUniqueInvoice(this.listRecord);
    },
    /**
     * Làm mới lại recordId khi con trỏ chuột chỉ ra ngoài
     * @author: Nguyễn Văn Trúc(3/3/2024)
     */
    handleMouseLeave() {
      this.recordCode = "";
    },

    /**
     * Lấy id nhân viên khi con trỏ chuột ở dòng nhân viên ấy
     * @param {nhân viên} record
     * @author: Nguyễn Văn Trúc(3/3/2024)
     */
    getRecordIdWhenMove(record) {
      this.recordId = record.CartId;
      this.recordCode = record.ProductCode;
      this.recordSelected = record;
    },

    /**
     * Hàm Thực hiện khi chọn xoá khách hàng
     * @author: Nguyễn Văn Trúc (9/12/2023)
     */
    deleteRecord(recordId) {
      try {
        this.common.showDialog(
          `${this.resource.ConfirmDeleteRecord.ContentDelete} [${this.recordCode}] ${this.resource.Question.Content}`,
          this.resource.ConfirmDeleteRecord.Title,
          this.helper.Status.Delete,
          "Invoice/delete",
          recordId + "/" + localStorage.getItem("AccountId")
        );
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Lấy sản phẩm mua
     */
    async getInvoices() {
      this.common.showLoading();
      this.listRecord = await this.apiService.getByInfo(
        "Invoice/user",
        localStorage.getItem("AccountId")
      );
      console.log(this.listRecord);
      this.common.showLoading(false);
    },

    /**
     * Hiển thị trạng thái đơn hàng
     * @param {trạng thái} status
     */
    getStatus(status) {
      if (status === 1) {
        return "Chờ xác nhận";
      }
      return "Đang giao hàng";
    },
    filterUniqueInvoice(data) {
      // Tạo một đối tượng dùng để lưu trữ các bản ghi không trùng CommentId
      let uniqueComments = {};

      // Lặp qua dữ liệu và thêm các bản ghi không trùng CommentId vào mảng kết quả
      let uniqueRecords = [];
      data.forEach((item) => {
        if (!uniqueComments[item.InvoiceId]) {
          uniqueComments[item.InvoiceId] = true;
          uniqueRecords.push(item);
        }
      });
      return uniqueRecords; // Trả về mảng chứa các bản ghi không trùng CommentId
    },
  },
  async created() {
    if (localStorage.getItem("AccountId")) {
      await this.getInvoices();
      this.listRecord = this.filterUniqueInvoice(this.listRecord);
      this.isLogin = true;
    }
  },
  mounted() {
    this.emitter.on("Status", (value) => {
      if (
        value === this.helper.Status.Delete ||
        value === this.helper.Status.DeleteMultiple
      ) {
        this.getInvoices();
      }
    });
  },
};
</script>

<style scoped></style>
