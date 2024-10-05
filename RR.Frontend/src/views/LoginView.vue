<template>
  <div class="about">
    <input type="email" v-model="state.email">
    <input type="password">
    
  </div>
</template>
<script setup lang="ts">
const state = reactive({
  email: '',
});
</script>
<style>
@media (min-width: 1024px) {
  .about {
    min-height: 100vh;
    display: flex;
    align-items: center;
  }
}
</style>
