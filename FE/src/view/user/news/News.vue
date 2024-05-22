<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <div class="container" style="min-height: 80vh">
    <div>
      <a href="/"> Trang chủ </a> /
      <a href="/user/news"> Tin tức </a>
    </div>
    <h4 class="my-4">Tin tức</h4>
    <h5>Giới thiệu chung</h5>
    <div
      @click="detailNews(item.NewsId)"
      class="m-5"
      style="cursor: pointer"
      v-for="item in listRecord"
      :key="item.NewsId"
    >
      <div class="d-flex">
        <div>
          <img :src="require('@/assets/img/new/' + item.Image)" />
        </div>
        <div class="m-4">
          <h3>{{ item.NewsName }}</h3>
          <p>{{ this.common.changeDisplayDate(item.CreatedDate) }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      listRecord: {},
    };
  },
  methods: {
    detailNews(id) {
      this.$router.push(`news-detail/${id}`);
    },
    async loadNews() {
      let result = await this.apiService.get("News/getAll/1");
      this.listRecord = result;
    },
  },
  created() {
    this.loadNews();
  },
};
</script>

<style scoped>
img {
  height: 160px;
  width: 160px;
}
</style>
