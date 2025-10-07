<template>
  <div class="receipts-view">
    <div class="receipts-header">
      <h1>Receipts</h1>
      <div class="button-group">
        <button @click="prevReceipt" :disabled="currentReceiptIndex === 0">Previous</button>
        <button class="danger" @click="deleteReceipt">Delete</button>
        <button @click="nextReceipt" :disabled="currentReceiptIndex === receipts.length - 1 || receipts.length === 0">Next</button>
      </div>
    </div>
    <div v-if="receipts.length === 0">No receipts found.</div>
    <div v-else>
      <ReceiptComponent
        v-model:receipt="receipts[currentReceiptIndex]"
      />
    </div>
    <div class="upload-section">
      <div class="button-group">
        <button @click="openPhotoPopup">New from image</button>
        <button @click="addNewReceipt" :disabled="hasEmptyReceipt">New receipt</button>
        <button @click="saveReceipts">
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
import { ref, onMounted, computed } from 'vue'
import { Receipt } from '@/DTOs/Receipt'
import PhotoCapture from '@/components/PhotoCapture.vue'
import ReceiptComponent from '@/components/ReceiptComponent.vue'
import { ApiClient } from '@/utils/ApiClient'

const receipts = ref<Receipt[]>([])
const currentReceiptIndex = ref(0)

const RECEIPTS_STORAGE_KEY = 'receipts-draft-v1'

const showPhotoPopup = ref(false)
const imageUrl = ref<string | null>(null)

const hasEmptyReceipt = computed(() => {
  var result = receipts.value.some(r => !r.items || r.isEmpty());
  return result;
})

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
      const arr = JSON.parse(saved)
      receipts.value = arr.map((r: any) => Receipt.from(r));
    } catch {
      receipts.value = [ new Receipt() ]
    }
  } else {
    receipts.value = [ new Receipt() ]
  }
})

function addNewReceipt() {
  receipts.value.push(new Receipt())
}
async function saveReceipts() {
  // TODO: Implement save logic (API call, etc.)
  localStorage.setItem(RECEIPTS_STORAGE_KEY, JSON.stringify(receipts.value))
  const result = await ApiClient.post<AuthResponse>(
    'account/login',
    state.loginCre
  )
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
.receipts-header {
  display: flex;
  align-items: center;
  gap: 1.5em;
  margin-bottom: 1.5em;
}
.upload-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-top: 2em;
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
