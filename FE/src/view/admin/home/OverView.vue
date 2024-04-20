<template>
  <div class="h-100 w-100">
    <div class="d-flex justify-content-center mt-4">
      <select @change="handleChangeOption" v-model="yearSelect">
        <option :value="item" v-for="(item, index) in year" :key="index">
          Năm {{ item }}
        </option>
      </select>
    </div>
    <div class="chart h-75 w-75">
      <canvas id="myChart" width="800" height="400"></canvas>
    </div>
    <h2 class="d-flex justify-content-center mt-4">
      Thống kê doanh thu năm {{ yearSelect }}
    </h2>
  </div>
</template>

<script>
import Chart from "chart.js/auto";
export default {
  name: "OverView",
  data() {
    return {
      listInvoice: {},
      data: [],
      year: [],
      yearSelect: "",
    };
  },
  methods: {
    async handleChangeOption(event) {
      this.yearSelect = await event.target.value;
      this.getInvoice();
    },
    loadChart() {
      let ctx = document.getElementById("myChart");

      // Kiểm tra xem có biểu đồ nào đã được tạo trên canvas không
      if (ctx) {
        // Nếu có, hủy bỏ biểu đồ cũ (nếu tồn tại)
        if (ctx.myChart) {
          ctx.myChart.destroy();
        }
      }

      // Tạo biểu đồ mới
      ctx.myChart = new Chart(ctx, {
        type: "bar",
        data: {
          labels: [
            "Tháng 1",
            "Tháng 2",
            "Tháng 3",
            "Tháng 4",
            "Tháng 5",
            "Tháng 6",
            "Tháng 7",
            "Tháng 8",
            "Tháng 9",
            "Tháng 10",
            "Tháng 11",
            "Tháng 12",
          ],
          datasets: [
            {
              label: "Doanh thu từng tháng",
              data: this.data,
              borderWidth: 1,
            },
          ],
        },
        options: {
          scales: {
            y: {
              beginAtZero: true,
            },
          },
        },
      });
    },

    async getInvoice() {
      this.listInvoice = await this.apiService.get(
        "Invoice/getAll/" + this.yearSelect
      );
      this.data = await this.getTotalByMonth(this.listInvoice);
      this.loadChart();
    },
    async getTotalByMonth(invoices) {
      // Khởi tạo mảng lưu tổng tiền của từng tháng với giá trị mặc định là 0
      let totalByMonth = new Array(12).fill(0);

      // Duyệt qua mảng hóa đơn
      invoices.forEach((invoice) => {
        // Kiểm tra nếu CreatedDate là một chuỗi, chuyển đổi thành đối tượng Date
        let createdDate =
          typeof invoice.CreatedDate === "string"
            ? new Date(invoice.CreatedDate)
            : invoice.CreatedDate;

        // Lấy chỉ số tháng của hóa đơn (từ 0 đến 11)
        let monthIndex = createdDate.getMonth();

        // Cộng thêm số tiền của hóa đơn vào tổng tiền của tháng tương ứng
        totalByMonth[monthIndex] += invoice.Total;
      });

      return totalByMonth;
    },
    async getUniqueYears(invoices) {
      let uniqueYears = [];

      // Duyệt qua danh sách các hóa đơn
      invoices.forEach((invoice) => {
        // Trích xuất năm từ hóa đơn
        let year = new Date(invoice.CreatedDate).getFullYear();

        // Nếu năm chưa được thêm vào mảng uniqueYears, thêm vào
        if (!uniqueYears.includes(year)) {
          uniqueYears.push(year);
        }
      });

      // Sắp xếp các năm theo thứ tự giảm dần
      uniqueYears.sort((a, b) => b - a);

      return uniqueYears;
    },
  },
  async mounted() {
    this.yearSelect = await new Date().getFullYear();
    this.year = await this.getUniqueYears(
      await this.apiService.get("Invoice/getAll/0")
    );
    await this.getInvoice();
  },
};
</script>
<style scoped>
.chart {
  display: flex;
  align-items: center;
  justify-content: center;
  margin-left: 12%;
}
select {
  height: 36px;
  width: 200px;
  border-radius: 4px;
  border: 1px solid #e0e0e0;
  outline: none;
  margin-left: 40%;
}
</style>
