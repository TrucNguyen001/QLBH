<template>
  <!-- <div>
    <input class="m-input" type="date" />
    <input class="m-input" type="date" />
  </div> -->
  <div v-show="this.isShowRevenueByDay === true" class="h-100 w-100">
    <div class="mt-4 d-flex">
      <select @change="handleChangeOption" v-model="yearSelect">
        <option :value="item" v-for="(item, index) in year" :key="index">
          Năm {{ item }}
        </option>
      </select>
    </div>
    <div class="d-flex justify-content-between" style="height: 250px">
      <div class="demo" style="height: 240px; width: 500px">
        <div class="chart" style="height: 180px; width: 400px">
          <canvas id="myChart" width="800" height="300px"></canvas>
        </div>
        <h3 class="d-flex justify-content-center">
          Thống kê doanh thu năm {{ yearSelect }}
        </h3>
      </div>
      <div style="height: 400px; width: 500px">
        <div class="chart" style="height: 180px; width: 180px">
          <canvas id="myChartTypeProduct" width="180px" height="180px"></canvas>
        </div>
        <h3 class="mt-2">Thống kê sản phẩm năm {{ yearSelect }}</h3>
      </div>
    </div>
    <div style="margin-left: 10%">
      <div style="height: 280px; width: 800px">
        <div class="chart" style="height: 280px; width: 800px">
          <canvas id="myChartTopProduct" width="700" height="280"></canvas>
        </div>
        <h3 style="margin-left: 34%">
          Top sản phẩm bán chạy nhất năm {{ yearSelect }}
        </h3>
      </div>
    </div>
  </div>
  <div v-show="this.isShowRevenueByDay === false" class="mt-5">
    <div @click="this.isShowRevenueByDay = true" style="margin-left: 24px">
      <button class="btn btn-primary px-4">
        <i style="margin-right: 10px" class="bi bi-arrow-90deg-left"></i>Quay
        lại
      </button>
    </div>
    <div>
      <div style="height: 380px; width: 1000px">
        <div class="chart" style="height: 100%; width: 1000px">
          <canvas id="myChartRevenueByMonth" width="1000" height="280"></canvas>
        </div>
        <h2 class="d-flex justify-content-center" style="width: 1250px">
          Thống kê tháng {{ monthSelect }} năm {{ yearSelect }}
        </h2>
      </div>
    </div>
  </div>
</template>

<script>
import Chart from "chart.js/auto";
export default {
  name: "OverView",
  data() {
    return {
      isShowRevenueByDay: true,
      listInvoice: {},
      data: [],
      dataProductType: [],
      year: [],
      yearSelect: "",
      top10ProductAmount: [],
      top10ProductCode: [],
      demo: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"],
      listDay: [],
      listTotalByDay: [],
      monthSelect: "",
    };
  },
  methods: {
    async handleChangeOption(event) {
      this.yearSelect = await event.target.value;
      this.getStatistics();
    },
    loadChartTypeProduct() {
      let ctx = document.getElementById("myChartTypeProduct");

      // Kiểm tra xem có biểu đồ nào đã được tạo trên canvas không
      if (ctx) {
        // Nếu có, hủy bỏ biểu đồ cũ (nếu tồn tại)
        if (ctx.myChart) {
          ctx.myChart.destroy();
        }
      }

      ctx.myChart = new Chart(ctx, {
        type: "pie",
        data: {
          labels: ["Máy tính", "Điện thoại", "Phụ kiện"],
          datasets: [
            {
              label: "Tổng sản phẩm:",
              data: this.dataProductType,
              backgroundColor: [
                "rgb(255, 99, 132)",
                "rgb(54, 162, 235)",
                "rgb(255, 205, 86)",
              ],
              hoverOffset: 4,
            },
          ],
        },
      });
    },

    // Biêu đồ thống kê theo tháng của năm
    loadChartTotal() {
      let ctx = document.getElementById("myChart");
      var me = this;

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
          labels: this.demo,
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
          onClick: (event, elements) => {
            if (elements.length > 0) {
              const chartElement = elements[0];
              const label = ctx.myChart.data.labels[chartElement.index];
              this.monthSelect = label;
              me.getRevenueByMonth(label, this.yearSelect);
            }
          },
        },
      });
    },

    async getRevenueByMonth(month, year) {
      //this.common.showLoading();
      let result = await this.apiService.getByInfo(
        "Invoice/Revenue",
        month + "/" + year
      );
      this.listDay = result.Day;
      this.listTotalByDay = result.Total;
      this.loadRevenueByMonth();
      this.isShowRevenueByDay = false;
      //this.common.showLoading();
    },

    // Biểu đồ thống kê theo ngày
    loadRevenueByMonth() {
      let ctx = document.getElementById("myChartRevenueByMonth");

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
          labels: this.listDay,
          datasets: [
            {
              label: "Doanh thu từng ngày",
              data: this.listTotalByDay,
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

    loadChartTopProduct() {
      let ctx = document.getElementById("myChartTopProduct");

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
          labels: this.top10ProductCode,
          datasets: [
            {
              label: "Số lượng bán",
              data: this.top10ProductAmount,
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

    async getTotalByMonth(invoices) {
      let totalByMonth = invoices.map((obj) => obj.TotalSUM);

      return totalByMonth;
    },
    async getTop10ProductCode(list) {
      let result = list.map((obj) => obj.ProductCode);

      return result;
    },
    async getTop10ProductAmount(list) {
      let result = list.map((obj) => obj.TotalSales);

      return result;
    },
    async getArrayYears(yearsObjects) {
      // Tạo một mảng chứa các năm
      let yearsArray = yearsObjects.map((obj) => obj.FullYear);

      // Sắp xếp mảng theo thứ tự giảm dần
      yearsArray.sort((a, b) => b - a);

      return yearsArray;
    },

    processInvoiceData(queryResult) {
      // Khởi tạo mảng để lưu trữ Total cho mỗi loại sản phẩm
      let totalsArray = [];

      // Khởi tạo đối tượng để lưu trữ Total cho mỗi loại sản phẩm
      let resultObject = {};

      // Lặp qua kết quả từ truy vấn và xây dựng đối tượng kết quả
      for (let i = 0; i < queryResult.length; i++) {
        // Lưu giá trị Total vào đối tượng kết quả, sắp xếp theo ProductTypeName
        resultObject[queryResult[i].ProductTypeName] = queryResult[i].Total;
      }

      // Thêm các loại sản phẩm còn thiếu vào đối tượng kết quả với Total là 0
      const productTypes = ["Máy tính", "Điện thoại", "Phụ kiện"];
      for (const productType of productTypes) {
        if (!(productType in resultObject)) {
          resultObject[productType] = 0;
        }
      }

      // Lặp qua mảng đối tượng kết quả để lấy giá trị Total và lưu vào mảng totalsArray
      for (const productType of productTypes) {
        totalsArray.push(resultObject[productType]);
      }

      return totalsArray;
    },

    async getStatistics() {
      let year = await new Date().getFullYear();
      if (this.yearSelect === "") {
        this.yearSelect = year;
      }
      let result = this.apiService.get("Invoice/getAll/" + this.yearSelect);
      result.then((response) => {
        // Lấy ra những năm có hoá đơn
        let result = this.getArrayYears(response.FullYear);
        result.then((y) => {
          this.year = y;
        });
        this.dataProductType = this.processInvoiceData(response.ProductType);
        this.loadChartTypeProduct();

        let product = this.getTotalByMonth(response.Revenue);
        product.then((p) => {
          this.data = p;
          this.loadChartTotal();
        });

        let topByCode = this.getTop10ProductCode(response.Product);
        topByCode.then((p) => {
          this.top10ProductCode = p;
          this.loadChartTopProduct();
        });

        let topByAmount = this.getTop10ProductAmount(response.Product);
        topByAmount.then((p) => {
          this.top10ProductAmount = p;
          this.loadChartTopProduct();
        });
      });
    },
  },
  async mounted() {
    await this.getStatistics();
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
.demo {
  overflow-x: auto;
}
</style>
