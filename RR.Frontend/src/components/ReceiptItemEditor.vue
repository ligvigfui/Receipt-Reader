<template>
    <div class="receipt-item-editor">
        <input v-model="item.name" placeholder="Item Name" />
        <input v-model="item.originalRecognizedName" placeholder="Original Name" />
        <input type="number" v-model.number="item.quantity" min="0" step="0.01" style="width:5em" placeholder="Qty" />
        <select v-model="item.measurement">
            <option v-for="m in measurements" :key="m" :value="m">{{ m }}</option>
        </select>
        <input type="number" v-model.number="item.pricePerQuantity" min="0" step="0.01" style="width:6em" placeholder="Price/Qty" />
        <span>= {{ (item.quantity * item.pricePerQuantity).toFixed(2) }}</span>
        <button class="circle danger" @click="emit('delete')">🗑️</button>
    </div>
</template>
<script setup lang="ts">
import type { ReceiptItem } from '@/DTOs/ReceiptItem'
import type { Measurement } from '@/DTOs/Measurement'
const item = defineModel<ReceiptItem>('item', { required: true });
const emit = defineEmits(['update:item', 'delete'])
const measurements: Measurement[] = ['grams','kilograms','liters','mililiters','pieces'];
</script>
<style scoped>
.receipt-item-editor input,
.receipt-item-editor select {
    margin: 0 0.5em 0.5em 0;
}
.danger {
    margin-left: 0.5em;
}
</style>
