<template>
  <div class="cropper-modal">
    <div class="cropper-content">
      <div class="cropper-header">
        <span>Crop your image</span>
        <button class="close-btn" title="Close popup" @click="$emit('close')">&times;</button>
      </div>
      <div class="cropper-body">
        <img ref="image" :src="src" alt="To crop" class="cropper-img" />
      </div>
      <div class="cropper-footer">
        <button class="crop-btn" @click="applyCrop">Apply Crop</button>
        <button class="crop-btn no-crop-btn" @click="noCrop">No crop</button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount, watch, defineProps, defineEmits } from 'vue'
import Cropper from 'cropperjs'

const props = defineProps<{ src: string }>()
const emit = defineEmits(['crop', 'close'])
const image = ref<HTMLImageElement | null>(null)
let cropper: Cropper | null = null

function createCropper() {
  if (cropper) {
    (cropper as any).$destroy?.()
    cropper = null
  }
  if (image.value) {
    cropper = new Cropper(image.value, {
    })
  }
}

onMounted(() => {
  createCropper()
})

function noCrop() {
  emit('crop', props.src)
}
onBeforeUnmount(() => {
  if (cropper) {
    (cropper as any).$destroy?.() // fallback for v2+ API
    cropper = null
  }
})

watch(() => props.src, () => {
  createCropper()
})

function applyCrop() {
  if (cropper) {
    const selection = cropper.getCropperSelection()
    if (selection) {
      selection.$toCanvas().then((canvas: HTMLCanvasElement) => {
        emit('crop', canvas.toDataURL('image/png'))
      })
    }
  }
}
</script>

<style scoped>
.cropper-modal {
  position: fixed;
  top: 0; left: 0; right: 0; bottom: 0;
  background: rgba(0,0,0,0.7);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 2000;
  height: 100vh;
}
.cropper-content {
  background: #fff;
  border-radius: 10px;
  padding: 1.5em;
  min-width: 360px;
  min-height: 360px;
  width: 80vw;
  max-width: 700px;
  height: 80vh;
  max-height: 700px;
  display: flex;
  flex-direction: column;
  align-items: stretch;
  flex: 1 1 auto;
  min-height: 0;
}
.cropper-header {
  width: 100%;
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1em;
}
.close-btn {
  background: none;
  border: none;
  font-size: 2em;
  cursor: pointer;
}
.cropper-body {
  flex: 1 1 0;
  min-height: 0;
  min-width: 0;
  position: relative;
  margin-bottom: 1em;
  overflow: hidden;
  display: flex;
  justify-content: center;
  align-items: center;
}
.cropper-body :deep(cropper-canvas) {
  height: 100%;
  width: 100%;
}

/* don't absolutely position the source <img> — let Cropper create its own view and scale it */
.cropper-img {
  max-width: 100%;
  max-height: 100%;
  display: block;
  width: auto;
  height: auto;
}

.cropper-footer {
  display: flex;
  justify-content: center;
  width: 100%;
}
.crop-btn {
  padding: 0.7em 2em;
  border: none;
  border-radius: 6px;
  background: #007bff;
  color: #fff;
  font-size: 1.1em;
  cursor: pointer;
  transition: background 0.2s;
  margin: 0 0.5em;
}
.crop-btn:hover {
  background: #0056b3;
}
.no-crop-btn {
  background: #6c757d;
}
.no-crop-btn:hover {
  background: #444b50;
}
</style>
