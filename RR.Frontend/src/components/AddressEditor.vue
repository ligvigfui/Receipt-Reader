<template>
    <div class="address-editor">
        <input v-model="address.country" placeholder="Country" />
        <input v-model="address.region" placeholder="Region" />
        <input v-model="address.postalCode" placeholder="Postal Code" />
    </div>
    <div class="address-editor">
        <input v-model="address.city" placeholder="City" />
        <input v-model="address.streetAddress" placeholder="Street Address" />
        <input v-model="address.note" placeholder="Note" />
    </div>
</template>

<script setup lang="ts">
import { defineModel } from 'vue';
import type { Address } from '@/DTOs/Address';
const address = defineModel<Address>('address', { required: true });
</script>

<style scoped>
.address-editor input {
    margin: 0 0.5em 0.5em 0;
    /* Only relation/spacing here, no color */
}
</style>
