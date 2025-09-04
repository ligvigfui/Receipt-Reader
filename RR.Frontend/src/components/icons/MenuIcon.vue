<template>
  <span :class="['sandwich-icon', { open }]">
    <span></span>
    <span></span>
    <span></span>
  </span>
</template>
<script setup lang="ts">
const props = defineProps<{ open: boolean }>()
</script>
<style scoped>
.sandwich-icon {
  display: inline-block;
  width: 1em;
  height: 1em;
  position: relative;
  transition: 0.3s;
}
.sandwich-icon span {
  display: block;
  position: absolute;
  height: 0.125em;
  width: 100%;
  background: var(--color-heading);
  border-radius: 2px;
  opacity: 1;
  left: 0;
  transition: 0.3s;
}
.sandwich-icon span:nth-child(1) {
  top: 0.15em;
}
.sandwich-icon span:nth-child(2) {
  top: 0.45em;
}
.sandwich-icon span:nth-child(3) {
  top: 0.75em;
}
.sandwich-icon.open span:nth-child(1) {
  top: 0.45em;
  transform: rotate(45deg);
}
.sandwich-icon.open span:nth-child(2) {
  opacity: 0;
}
.sandwich-icon.open span:nth-child(3) {
  top: 0.45em;
  transform: rotate(-45deg);
}
</style>
