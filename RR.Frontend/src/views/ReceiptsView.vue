<template>
  <div class="receipts-view">
    <div class="receipts-header">
      <h1>Receipts</h1>
      <div class="nav-buttons">
        <button class="nav-btn" @click="prevReceipt" :disabled="currentReceiptIndex === 0">Previous</button>
        <button class="nav-btn" @click="nextReceipt" :disabled="currentReceiptIndex === receipts.length - 1 || receipts.length === 0">Next</button>
      </div>
    </div>
    <div v-if="receipts.length === 0">No receipts found.</div>
    <div v-else>
      <div v-for="(receipt, rIdx) in receipts" :key="receipt.receiptId" v-show="rIdx === currentReceiptIndex" class="receipt-card">
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
    <div class="upload-section">
      <input ref="fileInput" type="file" accept="image/png, image/jpeg" style="display:none" @change="onFileChange" />
      <div class="upload-btn-group">
        <button class="upload-btn" @click="triggerFileInput">New from image</button>
        <button class="upload-btn" @click="addNewReceipt">New receipt</button>
        <button class="upload-btn" @click="saveReceipts">
          <span class="save-icon">💾</span>
          Save
        </button>
      </div>
      <div v-if="imageUrl" class="image-preview">
        <img :src="imageUrl" alt="Uploaded" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import type { Receipt } from '@/DTOs/Receipt'
import type { Measurement } from '@/DTOs/Measurement'
import type { ReceiptItem } from '@/DTOs/ReceiptItem'
import VendorEditor from '@/components/VendorEditor.vue'
import ReceiptItemEditor from '@/components/ReceiptItemEditor.vue'

const receipts = ref<Receipt[]>([])
const measurements: Measurement[] = ['grams','kilograms','liters','mililiters','pieces']
const currentReceiptIndex = ref(0)

const RECEIPTS_STORAGE_KEY = 'receipts-draft-v1'

onMounted(() => {
  const saved = localStorage.getItem(RECEIPTS_STORAGE_KEY)
  if (saved) {
    try {
      receipts.value = JSON.parse(saved)
    } catch {}
  } else {
    receipts.value = [
      {
        vendor: {
          name: 'Supermarket',
          taxNumber: '',
          address: {
            country: '', region: '', postalCode: '', city: '', streetAddress: '', note: ''
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
  }
})

watch(receipts, (val) => {
  localStorage.setItem(RECEIPTS_STORAGE_KEY, JSON.stringify(val))
}, { deep: true })

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

const fileInput = ref<HTMLInputElement | null>(null)
const imageUrl = ref<string | null>(null)

function triggerFileInput() {
  fileInput.value?.click()
}
function onFileChange(event: Event) {
  const files = (event.target as HTMLInputElement).files
  if (files && files[0]) {
    const reader = new FileReader()
    reader.onload = e => {
      imageUrl.value = e.target?.result as string
    }
    reader.readAsDataURL(files[0])
  }
}
function addNewReceipt() {
  receipts.value.push({
    vendor: {
      name: '',
      taxNumber: '',
      address: {
          country: '', region: '', postalCode: '', city: '', streetAddress: '', note: ''
      }
    },
    items: [],
    total: 0,
    receiptId: Math.random().toString(36).slice(2),
    dateTime: new Date().toISOString()
  })
}
function saveReceipts() {
  // TODO: Implement save logic (API call, etc.)
  alert('Receipts saved!')
}
function nextReceipt() {
  if (currentReceiptIndex.value < receipts.value.length - 1)
    currentReceiptIndex.value++
}
function prevReceipt() {
  if (currentReceiptIndex.value > 0)
    currentReceiptIndex.value--
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
.receipts-header {
  display: flex;
  align-items: center;
  gap: 1.5em;
  margin-bottom: 1.5em;
}
.nav-buttons {
  display: flex;
  gap: 0.5em;
}
.nav-btn {
  background: var(--color-button);
  color: var(--color-heading);
  border: none;
  border-radius: 6px;
  padding: 0.5em 1.2em;
  font-size: 1em;
  cursor: pointer;
  transition: background 0.2s;
}
.nav-btn:disabled {
  background: var(--color-border);
  color: var(--color-text);
  cursor: not-allowed;
}
.nav-btn:hover:not(:disabled) {
  background: var(--color-button-hover);
}
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
.add-item-btn {
  background: var(--color-button);
  color: var(--color-heading);
  border: none;
  border-radius: 50%;
  width: 2em;
  height: 2em;
  font-size: 1.5em;
  cursor: pointer;
  margin-top: 0.5em;
}
.add-item-btn:hover {
  background: var(--color-button-hover);
}
.upload-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-top: 2em;
}
.upload-btn-group {
  display: flex;
  gap: 1em;
  margin-bottom: 1em;
}
.upload-btn {
  background: var(--color-button);
  color: var(--color-heading);
  border: none;
  border-radius: 6px;
  padding: 0.7em 2em;
  font-size: 1.1rem;
  cursor: pointer;
  transition: background 0.2s;
}
.upload-btn:hover {
  background: var(--color-button-hover);
}
.image-preview img {
  max-width: 320px;
  max-height: 320px;
  border-radius: 8px;
  box-shadow: 0 2px 8px var(--color-background-card-shadow);
}
.save-icon {
  margin-right: 0.5em;
  font-size: 1.1em;
  vertical-align: middle;
}
</style>
