<template>
    <form class="product-editor" @submit.prevent="onSave">
        <div class="form-group">
            <label>Name</label>
            <input v-model="product.name" required />
            <div v-if="product.imageUrl" class="product-image-preview">
                <img :src="product.imageUrl" alt="Product Image"
                    style="max-width: 100%; max-height: 200px; margin-top: 0.5em; border-radius: 6px;" />
            </div>
        </div>
        <div class="form-group">
            <label>Description</label>
            <textarea v-model="product.description" />
        </div>
        <div class="form-group">
            <label>Quantity</label>
            <input type="number" v-model.number="product.quantity" />
        </div>
        <div class="form-group">
            <label>Measurement</label>
            <select v-model="product.measurement">
                <option value="">--</option>
                <option v-for="m in measurements" :key="m" :value="m">{{ m }}</option>
            </select>
        </div>
        <div class="form-group">
            <label>Image URL</label>
            <input v-model="product.imageUrl" />
        </div>
        <div class="form-group">
            <label>Is Public</label>
            <input type="checkbox" v-model="product.isPublic" />
        </div>
        <!-- Add more fields as needed -->
        <button type="submit">Save</button>
    </form>
</template>

<script setup lang="ts">

import { Product } from '@/DTOs/Product'
const product = defineModel<Product>('modelValue', { required: true });
const emit = defineEmits(['save']);

const measurements = [
    'grams', 'kilograms', 'liters', 'mililiters', 'pieces'
]

function onSave() {
    emit('save', product.value)
}
</script>

<style scoped>
.product-editor {
    display: flex;
    flex-direction: column;
    gap: 1em;
    max-width: 400px;
    margin: 2em auto;
}

.form-group {
    display: flex;
    flex-direction: column;
    gap: 0.3em;
}
</style>
