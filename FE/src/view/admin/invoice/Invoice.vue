<template>
  <div class="content">
    <div class="content-header">
      <div class="content-title">Quản lý đơn hàng</div>
      <div>
        <button
          :class="{
            'btn-info': status === 1,
            'btn-outline-info': status !== 1,
          }"
          v-on:click="confirmInvoiceWait"
          class="mx-2 btn"
        >
          Đơn chờ xử lý
        </button>
        <button
          :class="{
            'btn-primary': status === 2,
            'btn-outline-primary': status !== 2,
          }"
          v-on:click="delivering"
          class="btn"
        >
          Đơn hàng đang giao
        </button>
        <button
          :class="{
            'btn-danger': status === 3,
            'btn-outline-danger': status !== 3,
          }"
          v-on:click="invoiceFalse"
          class="btn mx-2"
        >
          Đơn giao thất bại
        </button>
        <button
          :class="{
            'btn-success': status === 4,
            'btn-outline-success': status !== 4,
          }"
          v-on:click="invoiceSuccess"
          class="btn"
        >
          Đơn giao thành công
        </button>
      </div>
    </div>
    <div class="content-main">
      <div class="header-table">
        <div class="header-table-left">
          <div class="table-left" v-if="listRecordId.length !== 0">
            <div>
              Số đơn hàng đã chọn:
              <strong>{{ listRecordId.length }}</strong>
            </div>
            <div>Bỏ chọn</div>
            <button
              @click="confirmInvoices"
              type="button"
              class="btn btn-danger"
            >
              {{ this.changeMultipleExecute() }}
            </button>
          </div>
        </div>
        <div style="width: 29%" class="header-table-right">
          <div class="input-icon">
            <input
              placeholder="Tìm kiếm theo tên"
              type="text"
              class="m-input m-input-icon"
              v-model="InfoSearch"
              @keydown.enter="searchRecord"
            />
            <div @click="searchRecord" class="icon-search">
              <i class="bi bi-search"></i>
            </div>
          </div>
          <div @click="downloadFile" class="icon">
            <i class="bi bi-file-earmark-arrow-up"></i>
          </div>
        </div>
      </div>
      <div class="table">
        <table class="m-table tbl">
          <thead>
            <tr>
              <th
                v-if="this.status === 1 || this.status === 2"
                @click="selectAllRecord(filterObject.pageNumber)"
              >
                <input
                  :checked="
                    listPageSelectAllRecord.includes(filterObject.pageNumber)
                  "
                  class="input-checkbox th fixed-column-left"
                  type="checkbox"
                />
              </th>
              <th style="width: 140px">Mã đơn hàng</th>
              <th>Tên khách hàng</th>
              <th>Số điện thoại</th>
              <th>Ngày đặt</th>
              <th>Tổng tiền</th>
              <th>Trạng thái</th>
              <th>Xem</th>
              <th v-if="this.status === 1 || this.status === 2">Xác nhận</th>
            </tr>
          </thead>
          <tbody>
            <tr
              :class="{
                selected: this.checkRowSelected(record),
              }"
              v-for="(record, index) in listRecord"
              :key="record.InvoiceId"
              @mouseover="getRecordIdWhenMove(record)"
              @mouseleave="handleMouseLeave"
            >
              <td
                v-if="this.status === 1 || this.status === 2"
                :class="{
                  selected: this.checkRowSelected(record),
                  background_default: !this.checkRowSelected(record),
                }"
                @click="selectedRecord(record.InvoiceId, index)"
              >
                <input
                  :checked="this.checkRowSelected(record)"
                  class="input-checkbox td fixed-column-left"
                  type="checkbox"
                />
              </td>
              <td>{{ record.InvoiceCode }}</td>
              <td>{{ record.UserName }}</td>
              <td>{{ record.PhoneNumber }}</td>
              <td>
                {{ this.common.changeDisplayDate(record.CreatedDate) }}
              </td>
              <td>{{ this.common.changeDisplayDebitAmount(record.Total) }}</td>
              <td>{{ this.changeDisplayStatus(record.StatusInvoice) }}</td>
              <td>
                <button
                  @click="detailInvoice(record.InvoiceId)"
                  class="btn btn-primary px-4"
                >
                  Xem
                </button>
              </td>
              <td v-if="this.status === 1 || this.status === 2">
                <button
                  @click="confirmInvoice(record)"
                  class="btn btn-primary px-4"
                >
                  {{ changeButtonDisplayStatus(record.StatusInvoice) }}
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <MPagination
        v-model:pageNumber="filterObject.pageNumber"
        v-model:pageSize="filterObject.pageSize"
        :totalRecord="totalRecord"
      ></MPagination>
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
      isShow: false,
      listRecord: {},
      recordSelected: {},
      recordId: "",
      recordCode: "",
      listRecordId: [],
      listIndexSelected: [],
      indexSelected: null,
      statusCode: this.helper.Status.Insert,
      totalRecord: 0,
      filterObject: {
        pageNumber: 1,
        pageSize: 20,
      },
      recordCodeBiggest: "",
      listPageSelectAllRecord: [],
      text: "",
      resultRecordIdList: [],
      status: 1,
    };
  },
  methods: {
    /**
     * Chi tiết hoá đơn
     */
    detailInvoice(id) {
      this.$router.push(`/invoice-detail/${id}`);
    },
    reset() {
      this.listRecordId = [];
      this.listIndexSelected = [];
      this.listPageSelectAllRecord = [];
      this.filterObject.pageNumber = 1;
    },
    confirmInvoiceWait() {
      this.status = 1;
      this.loadData();
      this.reset();
    },
    /**
     * Đơn hàng đang giao
     */
    delivering() {
      this.status = 2;
      this.loadData();
      this.reset();
    },
    invoiceSuccess() {
      this.status = 4;
      this.loadData();
      this.reset();
    },
    invoiceFalse() {
      this.status = 3;
      this.loadData();
      this.reset();
    },
    /**
     * Hiển thị trạng thái dạng chữ
     * @param {trạng thái đơn hàng} status
     */
    changeDisplayStatus(status) {
      return status === 1
        ? "Chờ xác nhận"
        : status === 2
        ? "Đang giao hàng"
        : status === 3
        ? "Giao hàng thất bại"
        : "Giao hàng thành công";
    },

    changeButtonDisplayStatus(status) {
      return status === 1 ? "Xác nhận đơn" : status === 2 ? "Huỷ đơn hàng" : "";
    },

    changeMultipleExecute() {
      return this.status === 1 ? "Xác nhận hết" : "Huỷ hết đơn";
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
      this.recordId = record.InvoiceId;
      this.recordCode = record.InvoiceCode;
      this.recordSelected = record;
    },

    /**
     * Hàm Thực hiện tự động download file excel về
     * @author: Nguyễn Văn Trúc (21/1/2024)
     */
    async downloadFile() {
      let me = this;
      try {
        me.common.showLoading();
        await me.apiService.downloadFileExcel("Invoice/ExportFile");
        me.common.showLoading(false);
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Hàm Thực hiện khi nhấn tìm kiếm
     * @author: Nguyễn Văn Trúc (21/1/2024)
     */
    searchRecord: function () {
      try {
        this.common.showLoading();
        this.getFilterRecord();
        this.filterObject.pageNumber = 1;
        this.listPageSelectAllRecord = [];
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Hàm Thực hiện khi nhấn enter
     * @author: Nguyễn Văn Trúc (21/1/2024)
     */
    enterSearchInput: function () {
      try {
        this.common.showLoading();
        this.getFilterRecord();
        this.filterObject.pageNumber = 1;
        this.listPageSelectAllRecord = [];
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm Thực hiện gọi api phân trang
     * @author: Nguyễn Văn Trúc (21/1/2024)
     */
    async getFilterRecord() {
      try {
        let me = this;
        let result = await me.apiService.loadFilter(
          "Invoice",
          me.filterObject.pageSize,
          me.filterObject.pageNumber,
          me.InfoSearch,
          this.status
        );
        me.text = me.InfoSearch;
        me.listRecord = result.ListRecord;
        me.totalRecord = result.ToTalRecord;
        me.resultRecordIdList = [];

        if (Object.keys(this.listRecord).length !== 0) {
          // Duyệt qua từng đối tượng trong result và thu thập RecordId
          for (let i = 0; i < me.listRecord.length; i++) {
            me.resultRecordIdList.push(me.listRecord[i].InvoiceId);
          }

          // Kiểm tra xem tất cả các recordId của bạn có tồn tại trong danh sách resultRecordIdList không
          let allExist = me.resultRecordIdList.every((recordId) =>
            this.listRecordId.includes(recordId)
          );

          if (allExist) {
            this.listPageSelectAllRecord.push(this.filterObject.pageNumber);
          } else {
            // Nếu bỏ chọn 1 trong tất cả các dòng khi chọn tất cả thì xoá bỏ dấu tích chọn tất cả
            this.listPageSelectAllRecord = this.listPageSelectAllRecord.filter(
              (i) => i !== this.filterObject.pageNumber
            );
          }
        }

        me.common.showLoading(false);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm Thực hiện khi huỷ hoặc thoát form chi tiết
     * @author: Nguyễn Văn Trúc (9/12/2023)
     */
    hideDetailRecord: function () {
      this.isShow = false;
      this.indexSelected = null;
    },

    /**
     * Hàm Thực hiện khi nhấn nút Sửa
     * @param {*nhân viên muốn sửa} record
     * @author: Nguyễn Văn Trúc (22/1/2024)
     */
    showUpdate: function (record) {
      this.statusCode = this.helper.Status.Update;
      this.recordSelected = record;
    },

    /**
     * Hàm Thực hiện khi click  hàng trong table
     * @param {*Id của nhân viên chọn} recordIdSelected
     * @param {*vị trí chọn} index
     * @author: Nguyễn Văn Trúc (9/12/2023)
     */
    selectedRecord: function (recordIdSelected, index) {
      try {
        // Nếu vị trí đã được chọn
        if (
          this.listRecordId.filter((recordId) => recordId === recordIdSelected)
            .length != 0
        ) {
          // huỷ id được chọn
          this.recordId = null;
          // huỷ vị trí được chọn
          this.indexSelected = null;

          this.listRecordId = this.listRecordId.filter(
            (recordId) => recordId !== recordIdSelected
          );

          this.listIndexSelected = this.listIndexSelected.filter(
            (i) => i.key !== recordIdSelected
          );

          // Nếu bỏ chọn 1 trong tất cả các dòng khi chọn tất cả thì xoá bỏ dấu tích chọn tất cả
          this.listPageSelectAllRecord = this.listPageSelectAllRecord.filter(
            (i) => i !== this.filterObject.pageNumber
          );
        }
        // Nếu vị trí chưa đuược chọn
        else {
          let element = {};
          element.key = recordIdSelected;
          this.listIndexSelected.push(element);
          // Lấy id của khách hàng khi click chọn
          this.recordId = recordIdSelected;
          // Lấy vị trị chọn
          this.indexSelected = index;

          this.listRecordId.push(recordIdSelected);

          // Kiểm tra xem tất cả các recordId của bạn có tồn tại trong danh sách resultRecordIdList không
          let allExist = this.resultRecordIdList.every((recordId) =>
            this.listRecordId.includes(recordId)
          );

          if (allExist) {
            this.listPageSelectAllRecord.push(this.filterObject.pageNumber);
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm Thực hiện lấy recordId
     * @param {*nhân viên muốn lấy Id} record
     * @author: Nguyễn Văn Trúc (21/1/2024)
     */
    whenMove: function (record) {
      this.recordId = record.InvoiceId;
      this.recordCode = record.InvoiceCode;
    },

    /**
     * Hàm Thực hiện chọn tất cả bản ghi
     * @param {Vị trí trang} pageNumber
     * @author: Nguyễn Văn Trúc (22/1/2024)
     */
    selectAllRecord: function (pageNumber) {
      try {
        if (this.listPageSelectAllRecord.includes(pageNumber)) {
          // Nếu pageNumber đã tồn tại trong mảng, thì xóa nó ra khỏi mảng
          this.listPageSelectAllRecord = this.listPageSelectAllRecord.filter(
            (item) => item !== pageNumber
          );
          this.listRecordId = this.listRecordId.filter(
            (item) =>
              !this.listRecord.some((record) => record.InvoiceId === item)
          );

          this.listIndexSelected = this.listIndexSelected.filter(
            (item) =>
              !this.listRecord.some((record) => record.InvoiceId === item.key)
          );
        } else {
          // Nếu pageNumber chưa tồn tại trong mảng, thêm nó vào mảng
          this.listPageSelectAllRecord.push(pageNumber);
          this.listRecord.forEach((item) => {
            this.listRecordId.push(item.InvoiceId);
            this.listRecordId = this.common.uniqueArray(this.listRecordId);
            let element = {};
            element.key = item.InvoiceId;
            this.listIndexSelected.push(element);
          });
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Hàm Thực hiện khi chọn thực hiện hàng loạt
     * @author: Nguyễn Văn Trúc (9/12/2023)
     */
    async confirmInvoice(record) {
      try {
        record.StatusInvoice = record.StatusInvoice + 1;
        await this.apiService.update("Invoice/put", record.InvoiceId, record);
        this.loadData();
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm Thực hiện load dữ liệu lên bảng
     * @author: Nguyễn Văn Trúc (9/12/2023)
     */
    loadData: function () {
      try {
        this.common.showLoading();
        this.getFilterRecord();
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm Thực hiện xoá nhiều nhân viên
     * @author: Nguyễn Văn Trúc (22/1/2024)
     */
    async confirmInvoices() {
      try {
        let status = this.listRecord[0].StatusInvoice + 1;
        await this.apiService.multipleUpdate(
          "Invoice/put/multipleUpdate",
          status,
          this.listRecordId
        );
        this.reset();
        this.loadData();
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Bôi đen ô input search khi click vào
     * @author: Nguyễn Văn Trúc (22/1/2024)
     */
    blackenInputSearch() {
      try {
        this.common.blackenInput(this.helper.Ref.InputSearch);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Kiểm tra dòng đã được chọn chưa
     * @returns: true: Được chọn, false: Chưa được chọn
     * @author: Nguyễn Văn Trúc (22/1/2024)
     */
    checkRowSelected(record) {
      return (
        this.listIndexSelected.filter((i) => i.key === record.InvoiceId)
          .length != 0
      );
    },
  },
  /**
   * Hàm Thực hiện load lại dữ liệu khi tạo ra
   * @author: Nguyễn Văn Trúc (22/1/2024)
   */
  created() {
    this.loadData();
  },
  /**
   * Hàm thực hiện nhận sự kiện bấm nút
   * @author: Nguyễn Văn Trúc (3/3/2024)
   */
  mounted() {
    let me = this;
    // Nhận dữ liệu từ sự kiện bấm nút
    me.emitter.on(me.helper.Emitter.SendEvent, (value) => {
      // CTRL + D: Xoá
      if (value === me.helper.Status.Delete && me.recordCode !== "") {
        me.confirmInvoice();
      }

      // CTRL + 1 : Thêm mới
      if (value === me.helper.Status.Insert) {
        me.addRecord();
      }

      // CTRL + E: Cập nhật
      if (value === me.helper.Status.Update && me.recordCode !== "") {
        me.detailRecord(me.recordSelected);
      }
    });

    me.emitter.on("Status", (value) => {
      if (
        value === me.helper.Status.Delete ||
        value === me.helper.Status.DeleteMultiple
      ) {
        me.loadData();
        me.listRecordId = [];
        me.listIndexSelected = [];
        me.listPageSelectAllRecord = [];
      }
    });

    // Khi thay đổi số lượng bản ghi trên trang làm mới lại mảng chứa các trang
    me.emitter.on(me.helper.Emitter.ChangePageSize, () => {
      me.listPageSelectAllRecord = [];
    });
  },
  watch: {
    /**
     * Hàm Thực hiện khi phân trang
     * @author: Nguyễn Văn Trúc (21/1/2024)
     */
    filterObject: {
      handler: function hange() {
        // Khi thông tin tìm kiếm bị thay đổi mà phân trang chuyển về trang 1
        if (this.InfoSearch !== this.text) {
          this.filterObject.pageNumber = 1;
        }
        this.common.showLoading();
        this.getFilterRecord();
      },
      deep: true,
    },
  },
};
</script>
<style scoped>
.selected {
  background: #eeeeee;
}
.background_default {
  background: white;
}
.delete {
  display: none;
}
.isSuccess {
  background: rgb(206, 234, 217);
}

.isError {
  background: #facccc;
}
.icon-file {
  right: 24px;
  position: absolute;
}
</style>
