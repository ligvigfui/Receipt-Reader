<template>
    <div class="vendorhq-editor">
    <input v-model="vendorHQ.name" placeholder="Vendor HQ Name" />
    <AddressEditor v-model:address="vendorHQ.address" />
    <slot />
    <input v-model="vendorHQ.taxNumber" placeholder="Tax Number" />
    </div>
</template>
<script setup lang="ts">
import { defineModel } from 'vue';
import type { VendorHQ } from '@/DTOs/VendorHQ';
import AddressEditor from './AddressEditor.vue';
const vendorHQ = defineModel<VendorHQ>('vendorHQ', { required: true });
</script>
<style scoped>
.vendorhq-editor input {
    margin: 0 0.5em 0.5em 0;
}
</style>
