<template>
  <div class="receipt-card">
    <div>
      <strong>Vendor:</strong>
      <VendorEditor v-model:vendor="receipt.vendor" />
    </div>
    <ul>
      <li v-for="(item, iIdx) in receipt.items" :key="iIdx">
        <ReceiptItemEditor v-model:item="receipt.items[iIdx]" @delete="deleteItem(iIdx)" />
      </li>
      <li>
        <button class="circle" @click="addItem()">+</button>
      </li>
    </ul>
    <div>
      <strong>Total:</strong> {{ calcTotal().toFixed(2) }}
    </div>
    <div>
      <strong>Date:</strong>
      <input type="datetime-local" v-model="receipt.dateTime" />
    </div>
  </div>
</template>

<script setup lang="ts">
import VendorEditor from '@/components/VendorEditor.vue'
import ReceiptItemEditor from '@/components/ReceiptItemEditor.vue'
import type { Receipt } from '@/DTOs/Receipt'

const receipt = defineModel<Receipt>('receipt', { required: true });

function addItem() {
  receipt.value.items.push({
    name: '',
    originalRecognizedName: '',
    quantity: 1,
    measurement: 'pieces',
    pricePerQuantity: 0,
    price: 0
  })
}
function calcTotal() {
  return receipt.value.items.reduce((sum, i) => sum + (i.quantity * i.pricePerQuantity), 0)
}
function deleteItem(iIdx: number) {
  receipt.value.items.splice(iIdx, 1)
}
</script>
<style scoped>

.receipt-card {
  background: var(--color-background-card);
  color: var(--color-text);
  border-radius: 8px;
  padding: 1rem;
  margin-bottom: 1.5rem;
  box-shadow: 0 2px 8px var(--color-background-card-shadow);
}
.receipt-card strong {
  color: var(--color-heading);
}
.receipt-card ul {
  padding-left: 1.2em;
}
.receipt-card li {
  margin-bottom: 0.3em;
}
</style>