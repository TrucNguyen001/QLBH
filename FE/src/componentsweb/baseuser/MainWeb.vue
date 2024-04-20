<template>
  <header-web />
  <div style="margin-top: 160px">
    <content-web-vue />
    <hr class="container" />
  </div>
  <footer-web-vue />
</template>
<script>
import ContentWebVue from "./ContentWeb.vue";
import HeaderWeb from "./HeaderWeb.vue";
import FooterWebVue from "./FooterWeb.vue";
export default {
  name: "MainWeb",
  components: { ContentWebVue, FooterWebVue, HeaderWeb },
};
</script>
<style></style>
