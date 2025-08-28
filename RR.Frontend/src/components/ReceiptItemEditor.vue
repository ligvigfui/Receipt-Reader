<template>
  <div class="receipt-item-editor">
    <input v-model="localItem.name" placeholder="Item Name" @input="emitUpdate" />
    <input v-model="localItem.originalRecognizedName" placeholder="Original Name" @input="emitUpdate" />
    <input type="number" v-model.number="localItem.quantity" min="0" step="0.01" style="width:5em" placeholder="Qty" @input="emitUpdate" />
    <select v-model="localItem.measurement" @change="emitUpdate">
      <option v-for="m in measurements" :key="m" :value="m">{{ m }}</option>
    </select>
    <input type="number" v-model.number="localItem.pricePerQuantity" min="0" step="0.01" style="width:6em" placeholder="Price/Qty" @input="emitUpdate" />
    <span>= {{ (localItem.quantity * localItem.pricePerQuantity).toFixed(2) }}</span>
    <button class="delete-item-btn" @click="emit('delete')">🗑️</button>
  </div>
</template>
<script setup lang="ts">
import { reactive, watch } from 'vue'
import type { ReceiptItem } from '@/DTOs/ReceiptItem'
import type { Measurement } from '@/DTOs/Measurement'
const props = defineProps<{ item: ReceiptItem, measurements: Measurement[] }>()
const emit = defineEmits(['update:item', 'delete'])
const localItem = reactive({ ...props.item })
watch(localItem, (val) => emit('update:item', { ...val }))
function emitUpdate() { emit('update:item', { ...localItem }) }
</script>
<style scoped>
.receipt-item-editor input,
.receipt-item-editor select {
    margin: 0 0.5em 0.5em 0;
}

.delete-item-btn {
    border-radius: 50%;
    width: 2em;
    height: 2em;
    font-size: 1.2em;
    cursor: pointer;
    margin-left: 0.5em;
}
</style>
