<template>
  <div class="receipts-view">
    <h1>Receipts</h1>
    <div v-if="receipts.length === 0">No receipts found.</div>
    <div v-for="(receipt, ) in receipts" :key="receipt.receiptId" class="receipt-card">
      <input v-model="receipt.groupName" placeholder="Group Name" />
      <div>
        <strong>Vendor:</strong>
        <VendorEditor v-model:vendor="receipt.vendor" />
      </div>
      <ul>
          <li v-for="(item, iIdx) in receipt.items" :key="iIdx">
              <ReceiptItemEditor v-model:item="receipt.items[iIdx]" :measurements="measurements" @delete="receipt.items.splice(iIdx, 1)" />
            </li>
            <li>
                <button class="add-item-btn" @click="addItem(receipt)">+</button>
            </li>
        </ul>
        <div>
          <strong>Total:</strong> {{ calcTotal(receipt).toFixed(2) }}
        </div>
        <div>
          <strong>Date:</strong>
          <input type="datetime-local" v-model="receipt.dateTime" />
        </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { Receipt } from '@/DTOs/Receipt'
import type { Measurement } from '@/DTOs/Measurement'
import type { ReceiptItem } from '@/DTOs/ReceiptItem'
import VendorEditor from '@/components/VendorEditor.vue'
import ReceiptItemEditor from '@/components/ReceiptItemEditor.vue'

const receipts = ref<Receipt[]>([])
const measurements: Measurement[] = ['grams','kilograms','liters','mililiters','pieces']

onMounted(async () => {
  receipts.value = [
    {
      groupName: 'Groceries',
      vendor: {
        name: 'Supermarket',
        taxNumber: '',
        address: {
          country: '', city: '', region: '', line1: '', line2: '', number: '', apartment: '', postalCode: '', note: ''
        }
      },
      items: [
        { name: 'Milk', originalRecognizedName: '', quantity: 1, measurement: 'liters', pricePerQuantity: 2.5, price: 2.5 },
        { name: 'Bread', originalRecognizedName: '', quantity: 2, measurement: 'pieces', pricePerQuantity: 1.2, price: 2.4 }
      ],
      total: 4.9,
      receiptId: 'abc123',
      dateTime: new Date().toISOString()
    }
  ]
})

function calcTotal(receipt: Receipt) {
  return receipt.items.reduce((sum, i) => sum + (i.quantity * i.pricePerQuantity), 0)
}

function addItem(receipt: Receipt) {
  const newItem: ReceiptItem = {
    name: '',
    originalRecognizedName: '',
    quantity: 1,
    measurement: 'pieces',
    pricePerQuantity: 0,
    price: 0
  }
  receipt.items.push(newItem)
}
</script>

<style scoped>
.receipts-view {
  padding: 2rem;
  min-height: 100vh;
  background: var(--color-background-soft);
  color: var(--color-background);
}
h1 {
  color: var(--color-heading);
}
.receipt-card {
  background: var(--color-background-card);
  color: var(--color-text);
  border-radius: 8px;
  padding: 1rem;
  margin-bottom: 1.5rem;
  box-shadow: 0 2px 8px rgba(0,0,0,0.15);
}
.receipt-card strong {
  color: var(--color-text-secondary);
}
.receipt-card ul {
  padding-left: 1.2em;
}
.receipt-card li {
  margin-bottom: 0.3em;
}
.add-item-btn {
  background: #444;
  color: #fff;
  border: none;
  border-radius: 50%;
  width: 2em;
  height: 2em;
  font-size: 1.5em;
  cursor: pointer;
  margin-top: 0.5em;
}
.add-item-btn:hover {
  background: #666;
}
</style>
