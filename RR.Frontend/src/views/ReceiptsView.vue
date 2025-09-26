<template>
  <div class="receipts-view">
    <div class="receipts-header">
      <h1>Receipts</h1>
      <div class="nav-buttons">
        <button class="nav-btn" @click="prevReceipt" :disabled="currentReceiptIndex === 0">Previous</button>
        <button class="danger" @click="deleteReceipt">Delete</button>
        <button class="nav-btn" @click="nextReceipt" :disabled="currentReceiptIndex === receipts.length - 1 || receipts.length === 0">Next</button>
      </div>
    </div>
    <div v-if="receipts.length === 0">No receipts found.</div>
    <div v-else>
      <ReceiptComponent
        v-model:receipt="receipts[currentReceiptIndex]"
      />
    </div>
    <div class="upload-section">
      <div class="upload-btn-group">
        <button class="upload-btn" @click="openPhotoPopup">New from image</button>
        <button class="upload-btn" @click="addNewReceipt">New receipt</button>
        <button class="upload-btn" @click="saveReceipts">
          <span class="save-icon">💾</span>
          Save
        </button>
      </div>
      <div v-if="imageUrl" class="image-preview">
        <img :src="imageUrl" alt="Uploaded" />
      </div>
      <PhotoCapture :show="showPhotoPopup" @close="showPhotoPopup = false" @photo="onPhotoCaptured" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { Receipt } from '@/DTOs/Receipt'
import PhotoCapture from '@/components/PhotoCapture.vue'
import ReceiptComponent from '@/components/ReceiptComponent.vue'

const receipts = ref<Receipt[]>([])
const currentReceiptIndex = ref(0)

const RECEIPTS_STORAGE_KEY = 'receipts-draft-v1'

const showPhotoPopup = ref(false)
const imageUrl = ref<string | null>(null)

function openPhotoPopup() {
  showPhotoPopup.value = true
}

function onPhotoCaptured(dataUrl: string) {
  imageUrl.value = dataUrl
  showPhotoPopup.value = false
}

function deleteReceipt() {
  if (receipts.value.length === 0) return
  receipts.value.splice(currentReceiptIndex.value, 1)
  if (currentReceiptIndex.value >= receipts.value.length) {
    currentReceiptIndex.value = receipts.value.length - 1
  }
}

onMounted(() => {
  const saved = localStorage.getItem(RECEIPTS_STORAGE_KEY)
  if (saved) {
    try {
      receipts.value = JSON.parse(saved)
    } catch {
      receipts.value = [ new Receipt() ]
    }
  } else {
    receipts.value = [ new Receipt() ]
  }
})

watch(receipts, (val) => {
  localStorage.setItem(RECEIPTS_STORAGE_KEY, JSON.stringify(val))
}, { deep: true })

function addNewReceipt() {
  receipts.value.push(new Receipt())
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
