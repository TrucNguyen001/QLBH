<template>
  <router-view />
  <a href="https://m.me/308650905658557" target="_blank"
    ><div class="message btn btn-primary">
      <i class="bi bi-chat-dots-fill"></i>Trợ giúp
    </div></a
  >
</template>
<script>
export default {
  name: "ContentWeb",
};
</script>
<style scoped>
.message {
  width: 150px;
  height: 50px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50px;
  color: white;
  font-size: 15px;
  position: fixed;
  right: 24px;
  bottom: 100px;
}
.message i {
  font-size: 32px;
  color: white;
  margin-right: 8px;
}
</style>
