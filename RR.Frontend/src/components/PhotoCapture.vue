<template>
  <div v-if="show" class="photo-capture-modal">
    <div class="modal-content">
      <button class="close-btn" @click="close">&times;</button>
      <div v-if="!photoDataUrl && !showCamera" class="options-view">
        <div v-if="cameraError" class="camera-error">{{ cameraError }}</div>
        <div class="option-row">
          <div class="option-rect" @click="startCameraMode">
            <span class="option-icon">📷</span>
            <span class="option-text">Take a Photo</span>
          </div>
          <label class="option-rect file-option">
            <input type="file" accept="image/*" style="display:none" @change="onFileChange" />
            <span class="option-icon">📁</span>
            <span class="option-text">Choose from Device</span>
          </label>
        </div>
      </div>
      <div v-else-if="showCamera && !photoDataUrl">
        <template v-if="!cameraError">
          <video ref="video" autoplay playsinline class="camera-preview"> </video>
          <div class="option-row">
            <button class="capture-btn" @click="capturePhoto">Capture Photo</button>
            <label class="option-rect file-option always-file">
              <input type="file" accept="image/*" style="display:none" @change="onFileChange" />
              <span class="option-icon">📁</span>
              <span class="option-text">Choose from Device</span>
            </label>
          </div>
        </template>
      </div>
      <div v-else>
        <ImageCropper v-if="showCropper && photoDataUrl" :src="photoDataUrl" @crop="onCrop" @close="onCropperClose" />
        <template v-else>
          <div class="final-photo-view">
            <img :src="photoDataUrl ?? undefined" class="captured-photo" />
            <div class="option-row">
              <button class="retake-btn" @click="retakePhoto">Retake</button>
              <button class="crop-action-btn" @click="showCropper = true">Crop</button>
              <button class="confirm-btn" @click="confirmPhoto">Confirm</button>
            </div>
          </div>
        </template>
      </div>
    </div>
  </div>
</template>


<script setup lang="ts">
import { ref, onUnmounted, watch, defineProps, defineEmits, nextTick } from 'vue'
import ImageCropper from './ImageCropper.vue'

const props = defineProps<{ show: boolean }>()
const emit = defineEmits(['close', 'photo'])

const video = ref<HTMLVideoElement | null>(null)
const stream = ref<MediaStream | null>(null)
const photoDataUrl = ref<string | null>(null)
const showCamera = ref(false)
const cameraError = ref<string | null>(null)
const showCropper = ref(false)

watch(() => props.show, (val) => {
  if (!val) {
    stopCamera()
    showCamera.value = false
    photoDataUrl.value = null
  }
})

onUnmounted(() => {
  stopCamera()
})

function onCropperClose() {
  showCropper.value = false
  close()
}

async function startCameraMode() {
  cameraError.value = null
  await nextTick()
  const success = await startCamera()
  if (success) {
    showCamera.value = true
  }
}

async function startCamera() {
  if (!video.value) {
    cameraError.value = 'Camera not ready.'
    return false
  }
  try {
    const s = await navigator.mediaDevices.getUserMedia({ video: true })
    stream.value = s
    if (video.value) video.value.srcObject = s
    cameraError.value = null
    return true
  } catch (err: any) {
    if (err.name === 'NotAllowedError') {
      cameraError.value = 'Camera access denied. Please allow camera permissions.'
    } else if (err.name === 'NotFoundError') {
      cameraError.value = 'No camera device found.'
    } else {
      cameraError.value = 'Could not access camera: ' + err.message
    }
    return false
  }
}

function stopCamera() {
  if (stream.value) {
    stream.value.getTracks().forEach(track => track.stop())
    stream.value = null
  }
  if (video.value) {
    video.value.srcObject = null
  }
  cameraError.value = null
}

function capturePhoto() {
  if (!video.value) return
  const canvas = document.createElement('canvas')
  canvas.width = video.value.videoWidth
  canvas.height = video.value.videoHeight
  const ctx = canvas.getContext('2d')!
  ctx.drawImage(video.value, 0, 0, canvas.width, canvas.height)
  photoDataUrl.value = canvas.toDataURL('image/png')
  stopCamera()
  showCropper.value = true
}

function retakePhoto() {
  photoDataUrl.value = null
  showCamera.value = false
}

function confirmPhoto() {
  emit('photo', photoDataUrl.value)
  close()
}

function close() {
  stopCamera()
  photoDataUrl.value = null
  showCamera.value = false
  emit('close')
}

function onFileChange(event: Event) {
  const files = (event.target as HTMLInputElement).files
  if (files && files[0]) {
    const reader = new FileReader()
    reader.onload = e => {
      photoDataUrl.value = e.target?.result as string
      showCamera.value = false
      stopCamera()
      showCropper.value = true
    }
    reader.readAsDataURL(files[0])
  }
}

function onCrop(croppedDataUrl: string) {
  photoDataUrl.value = croppedDataUrl
  showCropper.value = false
}
</script>

<style scoped>
.options-view {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-start;
  height: auto;
  width: 100%;
}

/* Make the error message 100% wide in the options view */
.options-view .camera-error {
  width: 100%;
  box-sizing: border-box;
  margin-bottom: 1em;
}
.photo-capture-modal {
  position: fixed;
  top: 0; left: 0; right: 0; bottom: 0;
  background: rgba(0,0,0,0.7);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}
.modal-content {
  background: #fff;
  border-radius: 10px;
  padding: 2em;
  position: relative;
  min-width: 340px;
  min-height: 340px;
  display: flex;
  flex-direction: column;
  align-items: center;
}
.close-btn {
  position: absolute;
  top: 0.5em;
  right: 0.5em;
  background: none;
  border: none;
  font-size: 2em;
  cursor: pointer;
}

/* Option rectangles */
.option-row {
  display: flex;
  gap: 2em;
  margin: 2em 0 1em 0;
  justify-content: center;
}
.option-rect {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  width: 160px;
  height: 140px;
  background: #f0f0f0;
  border-radius: 12px;
  border: 2px solid #007bff;
  cursor: pointer;
  transition: background 0.2s, border 0.2s;
  font-size: 1em;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
  user-select: none;
  text-align: center;
}
.option-rect:hover {
  background: #e6f0ff;
  border-color: #0056b3;
}
.option-icon {
  font-size: 2.5em;
  margin-bottom: 0.5em;
}
.option-text {
  font-size: 1.2em;
  font-weight: bold;
  width: 100%;
  text-align: center;
}
.file-option input[type="file"] {
  display: none;
}
.always-file {
  width: 160px;
  height: 60px;
  margin-left: 1em;
  margin-top: 0;
  margin-bottom: 0;
  flex-direction: row;
  justify-content: flex-start;
  align-items: center;
  font-size: 1em;
}
.always-file .option-icon {
  font-size: 1.5em;
  margin-bottom: 0;
  margin-right: 0.5em;
}
.file-option .option-icon  {
    margin-bottom: 0;
}
.always-file .option-text {
  font-size: 1em;
  font-weight: normal;
}

/* Camera and photo styles */
.camera-preview {
  width: 320px;
  height: 240px;
  background: #222;
  border-radius: 8px;
}
.captured-photo {
  max-width: 320px;
  max-height: 320px;
  border-radius: 8px;
  margin-bottom: 1em;
}
.capture-btn, .retake-btn, .confirm-btn {
  margin-top: 1em;
  padding: 0.7em 2em;
  border: none;
  border-radius: 6px;
  background: #007bff;
  color: #fff;
  font-size: 1.1em;
  cursor: pointer;
  transition: background 0.2s;
}
.capture-btn:hover, .retake-btn:hover, .confirm-btn:hover {
  background: #0056b3;
}

/* Camera error message */
.camera-error {
  color: #b00020;
  background: #ffeaea;
  border: 1px solid #b00020;
  border-radius: 6px;
  padding: 0.7em 1em;
  margin: 1em 0;
  font-size: 1.1em;
  text-align: center;
  max-width: 320px;
}
/* Center the image and buttons in the final view */
.final-photo-view {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

/* Make crop button match others */
.crop-action-btn {
  margin-top: 1em;
  padding: 0.7em 2em;
  border: none;
  border-radius: 6px;
  background: #007bff;
  color: #fff;
  font-size: 1.1em;
  cursor: pointer;
  transition: background 0.2s;
}
.crop-action-btn:hover {
  background: #0056b3;
}
</style>