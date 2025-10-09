<template>
  <div class="product-editor-view">
    <h1>Edit Product</h1>
    <ProductEditor v-model="product" @save="onSave" />
  </div>
</template>

<script setup lang="ts">

import { ref } from 'vue'
import ProductEditor from '@/components/ProductEditor.vue'
import { Product } from '@/DTOs/Product'

const product = ref(new Product())

function onSave(updated: any) {
  // TODO: Save logic (API call, etc.)
  alert('Product saved!')
}
</script>

<style scoped>
.product-editor-view {
  max-width: 600px;
  margin: 2em auto;
  background: var(--color-background-soft);
  padding: 2em;
  border-radius: 8px;
  box-shadow: 0 2px 8px var(--color-background-card-shadow);
}
</style>
