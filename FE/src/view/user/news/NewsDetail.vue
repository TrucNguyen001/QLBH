<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <div class="container" style="min-height: 80vh">
    <div>
      <a href="/user/home"> Trang chủ </a> /
      <a href="/user/info"> Tin tức </a>
    </div>
    <h4 class="my-4">Tin tức</h4>
    <h5>Giới thiệu chung</h5>
    <h2>{{ record.NewsName }}</h2>
    <div v-html="record.Content"></div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      record: {},
    };
  },
  methods: {
    async getDetailNews() {
      let newsId = this.$route.params.id;
      let result = await this.apiService.getByInfo("News", newsId);
      this.record = result;
    },
  },
  created() {
    this.getDetailNews();
  },
};
</script>

<style scoped>
img {
  height: 160px;
  width: 160px;
}
</style>
