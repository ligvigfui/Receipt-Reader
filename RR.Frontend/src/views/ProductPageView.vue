<template>
  <div class="product-page-view">
    <h1>Products</h1>
    <TableViewComponent
      :grid="productGrid"
      :page-size="1"
    />
  </div>
</template>

<script setup lang="ts">
import TableViewComponent from '@/components/TableViewComponent.vue'
import { Product } from '@/DTOs/Product'
import { ref } from 'vue'

const products = ref<Product[]>([
    Product.from({
        id: '1',
        name: 'Milk',
        description: '1 liter of whole milk',
        quantity: 1,
        measurement: 'liters',
        imageUrl: 'https://upload.wikimedia.org/wikipedia/commons/a/a5/Glass_of_Milk_%2833657535532%29.jpg',
        isPublic: false
    }),
    Product.from({
        id: '2',
        name: 'Bread',
        description: 'Whole grain bread',
        quantity: 1,
        measurement: 'pieces',
        imageUrl: 'https://upload.wikimedia.org/wikipedia/commons/c/c7/Korb_mit_Br%C3%B6tchen.JPG',
        isPublic: true
    }),
])

async function openRow(row: Product) {
  alert(`Clicked on product: ${row.name}`)
}

// Example: fetch from API or local data
async function getRows() {
  // TODO: Replace with real API call
  return products.value
}

function defaultColumns() {
  return [
    { key: 'name', label: 'Name' },
    { key: 'description', label: 'Description' },
    { key: 'quantity', label: 'Quantity' },
    { key: 'measurement', label: 'Measurement' },
    { key: 'imageUrl', label: 'Image', render: (row: Product) => row.imageUrl ? `<img src='${row.imageUrl}' style='max-width:60px;max-height:60px;'/>` : '', sortable: false, filterBy: null },
    { key: 'isPublic', label: 'Public', render: (row: Product) => row.isPublic ? 'Yes' : 'No' },
  ]
}

const productGrid = {
  getRows,
  defaultColumns,
  rowKey: (row: Product) => row.id || row.name,
  getData: async (options: {
    filters: Record<string, string>,
    sortKey: string | null,
    sortOrder: 'asc' | 'desc' | null,
    page: number,
    pageSize: number
  }) => {
    alert(JSON.stringify(options, null, 2))
    // For simplicity, ignoring options and returning all products
    return products.value
  },
  onClick: openRow
}
</script>

<style scoped>
.product-page-view {
  max-width: 900px;
  margin: 2em auto;
  background: var(--color-background-soft);
  padding: 2em;
  border-radius: 8px;
  box-shadow: 0 2px 8px var(--color-background-card-shadow);
}
</style>
