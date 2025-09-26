<template>
    <VendorHQEditor v-model:vendorHQ="vendor.vendorHQ">
        <div class="vendor-editor">
            <input v-model="vendor.name" placeholder="Vendor Name" />
            <AddressEditor v-model:address="vendor.address" />
        </div>
    </VendorHQEditor>
</template>
<script setup lang="ts">
import type { Vendor } from '@/DTOs/Vendor'
import VendorHQEditor from './VendorHQEditor.vue'
import AddressEditor from './AddressEditor.vue'
import { defineModel } from 'vue';
const vendor = defineModel<Vendor>('vendor', { required: true });
</script>
<style scoped>
.vendor-editor input {
    margin: 0 0.5em 0.5em 0;
}
</style>
