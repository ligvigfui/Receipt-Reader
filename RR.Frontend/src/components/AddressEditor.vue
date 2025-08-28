<template>
  <div class="address-editor">
    <input v-model="localAddress.country" placeholder="Country" @input="emitUpdate" />
    <input v-model="localAddress.city" placeholder="City" @input="emitUpdate" />
    <input v-model="localAddress.region" placeholder="Region" @input="emitUpdate" />
    <input v-model="localAddress.line1" placeholder="Address Line 1" @input="emitUpdate" />
    <input v-model="localAddress.line2" placeholder="Address Line 2" @input="emitUpdate" />
    <input v-model="localAddress.number" placeholder="Number" @input="emitUpdate" />
    <input v-model="localAddress.apartment" placeholder="Apartment" @input="emitUpdate" />
    <input v-model="localAddress.postalCode" placeholder="Postal Code" @input="emitUpdate" />
    <input v-model="localAddress.note" placeholder="Note" @input="emitUpdate" />
  </div>
</template>

<script setup lang="ts">
import { reactive, watch } from 'vue'
import type { Address } from '@/DTOs/Address'
const props = defineProps<{ address: Address }>();
const emit = defineEmits(['update:address'])

const localAddress = reactive({ ...props.address })

watch(localAddress, (val) => {
  emit('update:address', { ...val })
})

function emitUpdate() {
  emit('update:address', { ...localAddress })
}
</script>

<style scoped>
.address-editor input {
  margin: 0 0.5em 0.5em 0;
  /* Only relation/spacing here, no color */
}
</style>
