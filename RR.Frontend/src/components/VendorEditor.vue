<template>
  <div class="vendor-editor">
    <input v-model="localVendor.name" placeholder="Vendor Name" @input="emitUpdate" />
    <input v-model="localVendor.taxNumber" placeholder="Tax Number" @input="emitUpdate" />
    <AddressEditor v-model:address="localVendor.address" />
    <VendorHQEditor v-model:vendorHQ="localVendor.vendorHQ" />
  </div>
</template>
<script setup lang="ts">
import { reactive, watch } from 'vue'
import type { Vendor } from '@/DTOs/Vendor'
import VendorHQEditor from './VendorHQEditor.vue'
import AddressEditor from './AddressEditor.vue'
const props = defineProps<{ vendor: Vendor }>()
const emit = defineEmits(['update:vendor'])
const localVendor = reactive({ ...props.vendor })
watch(localVendor, (val) => emit('update:vendor', { ...val }))
function emitUpdate() { emit('update:vendor', { ...localVendor }) }
</script>
<style scoped>
.vendor-editor input { margin: 0 0.5em 0.5em 0; }
</style>
