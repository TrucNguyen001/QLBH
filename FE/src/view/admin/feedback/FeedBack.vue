<template>
  <div class="content">
    <div class="content-header">
      <div class="content-title">Quản lý phản hồi</div>
      <div>
        <button v-on:click="addRecord" class="btn btn-success">
          Thêm phản hồi
        </button>
      </div>
    </div>
    <div class="content-main">
      <div class="header-table">
        <div class="header-table-left">
          <div class="table-left" v-if="listRecordId.length !== 0">
            <div>
              Số phản hồi đã chọn:
              <strong>{{ listRecordId.length }}</strong>
            </div>
            <div>Bỏ chọn</div>
            <button @click="deleteRecords" type="button" class="btn btn-danger">
              Xoá hết
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
              <th @click="selectAllRecord(filterObject.pageNumber)">
                <input
                  :checked="
                    listPageSelectAllRecord.includes(filterObject.pageNumber)
                  "
                  class="input-checkbox th fixed-column-left"
                  type="checkbox"
                />
              </th>
              <th>Mã phản hồi</th>
              <th>Người phản hồi</th>
              <th>Ngày phản hồi</th>
              <th>Phản hồi</th>
            </tr>
          </thead>
          <tbody>
            <tr
              :class="{
                selected: this.checkRowSelected(record),
              }"
              v-for="(record, index) in listRecord"
              :key="record.CommentId"
            >
              <td
                :class="{
                  selected: this.checkRowSelected(record),
                  background_default: !this.checkRowSelected(record),
                }"
                @click="selectedRecord(record.CommentId, index)"
              >
                <input
                  :checked="this.checkRowSelected(record)"
                  class="input-checkbox td fixed-column-left"
                  type="checkbox"
                />
              </td>
              <td>{{ record.CommentCode }}</td>
              <td>{{ record.CommentName }}</td>
              <td>{{ this.common.changeDisplayDate(record.CreatedDate) }}</td>
              <td>
                <button
                  @click="detailInfoRecord(record.CommentId)"
                  class="btn btn-primary px-4"
                >
                  Phản hồi
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
  <feed-back-detail
    :show="isShow"
    :record="recordSelected"
    :statusCode="statusCode"
    @loadData="loadData"
    @hideDetailRecord="hideDetailRecord"
  ></feed-back-detail>
</template>
<script>
import FeedBackDetail from "../feedback/FeedBackDetail.vue";
export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: "Comment",
  // eslint-disable-next-line vue/no-unused-components
  components: { FeedBackDetail },
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
     * Hàm Thực hiện tự động download file excel về
     * @author: Nguyễn Văn Trúc (21/1/2024)
     */
    async downloadFile() {
      let me = this;
      try {
        me.common.showLoading();
        await me.apiService.downloadFileExcel("Comment/ExportFile");
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
          "Comment",
          me.filterObject.pageSize,
          me.filterObject.pageNumber,
          me.InfoSearch
        );
        me.text = me.InfoSearch;
        me.listRecord = result.ListRecord;
        me.totalRecord = result.ToTalRecord;
        me.resultRecordIdList = [];

        if (Object.keys(this.listRecord).length !== 0) {
          // Duyệt qua từng đối tượng trong result và thu thập RecordId
          for (let i = 0; i < me.listRecord.length; i++) {
            me.resultRecordIdList.push(me.listRecord[i].CommentId);
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
     * Thông tin chi tiết bản ghi(Xem)
     * @param {bản ghi} record
     * @author: Nguyễn Văn Trúc (2/4/2024)
     */
    async detailInfoRecord(id) {
      try {
        let result = await this.apiService.getByInfo("Comment", id);
        this.isShow = true;
        // Gắn khách hàng gửi đi là khách hàng click chọn
        this.recordSelected = result;
      } catch (error) {
        console.log(error);
      }
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
      this.recordId = record.CommentId;
      this.recordCode = record.CommentCode;
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
              !this.listRecord.some((record) => record.CommentId === item)
          );

          this.listIndexSelected = this.listIndexSelected.filter(
            (item) =>
              !this.listRecord.some((record) => record.CommentId === item.key)
          );
        } else {
          // Nếu pageNumber chưa tồn tại trong mảng, thêm nó vào mảng
          this.listPageSelectAllRecord.push(pageNumber);
          this.listRecord.forEach((item) => {
            this.listRecordId.push(item.CommentId);
            this.listRecordId = this.common.uniqueArray(this.listRecordId);
            let element = {};
            element.key = item.CommentId;
            this.listIndexSelected.push(element);
          });
        }
      } catch (error) {
        console.log(error);
      }
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
          "Comment/delete",
          recordId
        );
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
     * Hàm Thực hiện lấy mã lớn nhất
     * @author: Nguyễn Văn Trúc (21/1/2024)
     */
    async getRecordCodeBiggest() {
      try {
        let me = this;
        if (this.listRecord == 0) {
          me.recordCodeBiggest = "FB-00001";
        } else {
          me.recordCodeBiggest = await me.apiService.get(
            "Comment/code-biggest"
          );
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm Thực hiện xoá nhiều nhân viên
     * @author: Nguyễn Văn Trúc (22/1/2024)
     */
    deleteRecords() {
      console.log(this.listRecordId);
      try {
        this.common.showDialog(
          this.resource.ConfirmDeleteRecord.ContentDeleteAll,
          this.resource.ConfirmDeleteRecord.Title,
          this.helper.Status.DeleteMultiple,
          "Comment/delete",
          this.listRecordId
        );
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
        this.listIndexSelected.filter((i) => i.key === record.CommentId)
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
    // Khi thay đổi số lượng bản ghi trên trang làm mới lại mảng chứa các trang
    this.emitter.on(this.helper.Emitter.ChangePageSize, () => {
      this.listPageSelectAllRecord = [];
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
