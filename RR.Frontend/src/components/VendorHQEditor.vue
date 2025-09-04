<template>
    <div class="vendorhq-editor">
        <input v-model="localVendorHQ.name" placeholder="Vendor HQ Name" @input="emitUpdate" />
        <AddressEditor v-model:address="localVendorHQ.address" />
    </div>
</template>
<script setup lang="ts">
import { reactive, watch } from 'vue'
import type { VendorHQ } from '@/DTOs/VendorHQ'
import AddressEditor from './AddressEditor.vue'
const props = defineProps<{ vendorHQ: VendorHQ }>()
const emit = defineEmits(['update:vendorHQ'])
const localVendorHQ = reactive({ 
    ...props.vendorHQ, 
    address: props.vendorHQ.address ?? { country: '', region: '', postalCode: '', city: '', streetAddress: '', note: '' }
})
watch(localVendorHQ, (val) => emit('update:vendorHQ', { ...val }))
function emitUpdate() { emit('update:vendorHQ', { ...localVendorHQ }) }
</script>
<style scoped>
.vendorhq-editor input {
    margin: 0 0.5em 0.5em 0;
}
</style>
