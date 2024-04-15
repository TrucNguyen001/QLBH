<template>
  <div class="content">
    <div class="content-header">
      <div class="content-title">Quản lý người dùng</div>
    </div>
    <div class="content-main">
      <div class="header-table">
        <div class="header-table-left"></div>
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
              <th style="width: 150px">Mã người dùng</th>
              <th>Tên người dùng</th>
              <th>Số điện thoại</th>
              <th>Giới tính</th>
              <th>Trạng thái</th>
              <th>Xem</th>
              <th>Khoá</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="record in listRecord"
              :key="record.AccountId"
              @mouseover="getRecordIdWhenMove(record)"
              @mouseleave="handleMouseLeave"
            >
              <td>{{ record.AccountCode }}</td>
              <td>{{ record.FullName }}</td>
              <td>{{ record.PhoneNumber }}</td>
              <td>{{ this.common.changeDisplayGender(record.Gender) }}</td>
              <td v-if="record.Status === 0">
                <span class="text-danger">Ngưng hoạt động</span>
              </td>
              <td v-else>
                <span class="text-success">Đang hoạt động</span>
              </td>
              <td>
                <button
                  @click="detailInfoRecord(record)"
                  class="btn btn-primary px-4"
                >
                  Xem
                </button>
              </td>
              <td>
                <button
                  v-if="record.Status === 0"
                  @click="activatedRecord(record)"
                  class="btn btn-success px-4"
                >
                  Kích hoạt
                </button>
                <button
                  v-else
                  @click="deleteRecord(record)"
                  class="btn btn-danger px-4"
                >
                  Vô hiệu hoá
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
  <user-detail
    :show="isShow"
    :record="recordSelected"
    :statusCode="statusCode"
    @loadData="loadData"
    @hideDetailRecord="hideDetailRecord"
  ></user-detail>
</template>
<script>
import UserDetail from "../user/UserDetail.vue";
export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: "User",
  // eslint-disable-next-line vue/no-unused-components
  components: { UserDetail },
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
    };
  },
  methods: {
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
      this.recordId = record.AccountId;
      this.recordCode = record.AccountCode;
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
        await me.apiService.downloadFileExcel("Account/ExportFile");
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
          "Account",
          me.filterObject.pageSize,
          me.filterObject.pageNumber,
          me.InfoSearch,
          "User"
        );
        me.text = me.InfoSearch;
        me.listRecord = result.ListRecord;
        me.totalRecord = result.ToTalRecord;

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
     * Thông tin chi tiết bản ghi(Xem)
     * @param {bản ghi} record
     * @author: Nguyễn Văn Trúc (2/4/2024)
     */
    detailInfoRecord(record) {
      try {
        this.statusCode = this.helper.Status.See;
        // Hiển thị form chi tiết khách hàng
        this.isShow = true;
        // Gắn khách hàng gửi đi là khách hàng click chọn
        this.recordSelected = record;
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
      this.recordId = record.AccountId;
      this.recordCode = record.AccountCode;
    },

    /**
     * Hàm Thực hiện khi chọn xoá khách hàng
     * @author: Nguyễn Văn Trúc (9/12/2023)
     */
    async deleteRecord(record) {
      try {
        record.Status = 0;
        await this.apiService.update("Account/put", record.AccountId, record);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm Thực hiện khi kích hoạt khách hàng
     * @author: Nguyễn Văn Trúc (9/12/2023)
     */
    async activatedRecord(record) {
      try {
        record.Status = 1;
        await this.apiService.update("Account/put", record.AccountId, record);
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
        me.deleteRecord();
      }
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
