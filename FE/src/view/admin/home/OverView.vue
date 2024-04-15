<template>
  <div class="content">
    <div class="content-header">
      <div class="content-title">Quản lý sản phẩm</div>
      <div>
        <button
          v-on:click="addEmployee"
          class="m-button m-button-success btn-add"
        >
          Thêm mới sản phẩm
        </button>
      </div>
    </div>
    <div class="content-main">
      <div class="header-table">
        <div class="header-table-left">
          <div>Số sản phẩm đã chọn: <strong>10</strong></div>
          <div>Bỏ chọn</div>
          <button type="button" class="btn btn-danger">Xoá hết</button>
        </div>
        <div class="header-table-right">
          <div class="input-icon">
            <input type="text" class="m-input m-input-icon" />
            <div class="icon-search">
              <i class="bi bi-search"></i>
            </div>
          </div>
          <div class="icon">
            <i class="bi bi-file-earmark-arrow-down"></i>
          </div>
          <div class="icon">
            <i class="bi bi-file-earmark-arrow-up"></i>
          </div>
        </div>
      </div>
      <div class="table">
        <table style="min-width: 1400px" class="m-table tbl">
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
              <th>Mã sản phẩm</th>
              <th>Tên sản phẩm</th>
              <th>Hình ảnh</th>
              <th>Đơn giá</th>
              <th>Số lượng</th>
              <th>Số lượng mua</th>
              <th>Trạng thái</th>
              <th>Chức năng</th>
            </tr>
          </thead>
          <tbody>
            <tr
              :class="{
                selected: this.checkRowSelected(employee),
              }"
              v-for="(employee, index) in listEmployee"
              :key="employee.EmployeeId"
              @mouseover="getEmployeeIdWhenMove(employee)"
              @mouseleave="handleMouseLeave"
            >
              <td
                :class="{
                  selected: this.checkRowSelected(employee),
                  background_default: !this.checkRowSelected(employee),
                }"
                @click="selectedEmployee(employee.EmployeeId, index)"
              >
                <input
                  :checked="this.checkRowSelected(employee)"
                  class="input-checkbox td fixed-column-left"
                  type="checkbox"
                />
              </td>
              <td>{{ employee.EmployeeCode }}</td>
              <td>{{ employee.FullName }}</td>
              <td>{{ this.common.changeDisplayGender(employee.Gender) }}</td>
              <td style="text-align: center; padding-left: 0">
                {{ this.common.changeDisplayDate(employee.DateOfBirth) }}
              </td>
              <td>{{ employee.IdentificationCard }}</td>
              <td>
                {{ employee.PositionName }}
              </td>
              <td>
                {{ employee.DepartmentName }}
              </td>
              <td>{{ employee.BankAccount }}</td>
              <td>{{ employee.BankName }}</td>
              <td>{{ employee.Branch }}</td>
              <td
                @mouseleave="closeFormDuplicate"
                @click="showUpdate(employee)"
                class="function-update"
                style="text-align: center"
              >
                <div class="parent">
                  <div
                    style="cursor: pointer"
                    @click="detailEmployee(employee)"
                  >
                    {{ resource.MEmployeeList.Update }}
                  </div>
                  <div
                    @click="whenMove(employee)"
                    class="icon icon-caret-down-small"
                  ></div>
                </div>
                <div
                  v-show="
                    employee.EmployeeId === employeeSelected.EmployeeId &&
                    showChildUpdate === true
                  "
                  class="child-update"
                >
                  <div @click="replicationEmployee(employee)">
                    {{ resource.MEmployeeList.Replication }}
                  </div>
                  <div @click="deleteEmployee">
                    {{ resource.MEmployeeList.Delete }}
                  </div>
                  <div>{{ resource.MEmployeeList.Stop }}</div>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <MISAPagination
        v-model:pageNumber="filterObject.pageNumber"
        v-model:pageSize="filterObject.pageSize"
        :totalRecord="totalRecord"
      ></MISAPagination>
    </div>
  </div>
</template>
<script>
export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: "Product",
  data() {
    return {
      InfoSearch: "",
      isShow: false,
      listEmployee: {},
      employeeSelected: {},
      employeeId: "",
      employeeCode: "",
      listEmployeeId: [],
      listIndexSelected: [],
      indexSelected: null,
      statusCode: this.helper.Status.Insert,
      totalRecord: 0,
      filterObject: {
        pageNumber: 1,
        pageSize: 20,
      },
      isDelete: true,
      employeeCodeBiggest: "",
      showChildUpdate: false,
      listPageSelectAllRecord: [],
      text: "",
    };
  },
};
</script>
<style scoped>
@import url("../../../css/pages/product.css");
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
