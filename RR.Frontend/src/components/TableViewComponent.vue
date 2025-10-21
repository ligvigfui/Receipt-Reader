<template>
  <div class="table-view">
    <table>
      <thead>
        <tr>
          <th v-for="col in columns" :key="col.key" :class="['header-cell', { sortable: col.sortable }]">
            <div class="header-content">
              <span>{{ col.label }}</span>
              <span v-if="col.sortable" class="sort-icons">
                <button @click="sortBy(col.key, 'asc')" :class="{ active: sortKey === col.key && sortOrder === 'asc' }">▲</button>
                <button @click="sortBy(col.key, 'desc')" :class="{ active: sortKey === col.key && sortOrder === 'desc' }">▼</button>
              </span>
            </div>
            <div v-if="col.filterBy !== null" class="filter-box">
              <input v-model="filters[col.key]" @input="onFilterChange" placeholder="Filter..." />
            </div>
          </th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="row in pagedRows" :key="rowKey(row)" @click="handleRowClick(row)" :class="{ clickable: !!props.grid.onClick }">
          <td v-for="col in columns" :key="col.key">
            <span v-if="col.render" v-html="col.render(row)"></span>
            <span v-else>{{ row[col.key] }}</span>
          </td>
        </tr>
      </tbody>
    </table>
    <div class="pagination">
      <button @click="prevPage" :disabled="page === 1">Prev</button>
      <span>Page {{ page }} / {{ totalPages }}</span>
      <button @click="nextPage" :disabled="page === totalPages">Next</button>
    </div>
  </div>
</template>

<script setup lang="ts"> 
import { ref, computed, watch, defineProps } from 'vue'

interface IGridColumn<T> {
  key: string;
  label: string;
  render?: (row: T) => string;
  sortable?: boolean;
  filterBy?: 'text' | null;
}
interface IGridDisplay<T> {
  getRows: () => Promise<T[]> | T[];
  defaultColumns: () => Array<IGridColumn<T>>;
  rowKey?: (row: T) => string | number;
  getData?: (options: {
    filters: Record<string, string>,
    sortKey: string | null,
    sortOrder: 'asc' | 'desc' | null,
    page: number,
    pageSize: number
  }) => Promise<T[]> | T[];
  onClick?: (row: T) => void;
}

const props = defineProps<{
  grid: IGridDisplay<any>,
  pageSize?: number
}>()

const page = ref(1)
const rows = ref<any[]>([])
const columns = ref(props.grid.defaultColumns().map(col => ({
  ...col,
  sortable: col.sortable !== false,
  filterBy: col.filterBy === undefined ? 'text' : col.filterBy
})))
const filters = ref<Record<string, string>>({})
const sortKey = ref<string | null>(null)
const sortOrder = ref<'asc' | 'desc' | null>(null)
const pageSize = computed(() => props.pageSize || 10)

const filteredRows = computed(() => {
  let result = rows.value
  columns.value.forEach(col => {
    const filterVal = filters.value[col.key]
    if (col.filterBy !== null && filterVal) {
      if (typeof col.render === 'function') {
        result = result.filter(row => String(col.render!(row) ?? '').toLowerCase().includes(filterVal.toLowerCase()))
      }
      else
        result = result.filter(row => String(row[col.key] ?? '').toLowerCase().includes(filterVal.toLowerCase()))
    }
  })
  if (sortKey.value) {
    result = [...result].sort((a, b) => {
      const aVal = a[sortKey.value!]
      const bVal = b[sortKey.value!]
      if (aVal == null && bVal == null) return 0
      if (aVal == null) return sortOrder.value === 'asc' ? -1 : 1
      if (bVal == null) return sortOrder.value === 'asc' ? 1 : -1
      if (aVal < bVal) return sortOrder.value === 'asc' ? -1 : 1
      if (aVal > bVal) return sortOrder.value === 'asc' ? 1 : -1
      return 0
    })
  }
  return result
})

const totalPages = computed(() => Math.ceil(filteredRows.value.length / pageSize.value) || 1)
const pagedRows = computed(() => {
  const start = (page.value - 1) * pageSize.value
  return filteredRows.value.slice(start, start + pageSize.value)
})
function sortBy(key: string, order: 'asc' | 'desc') {
  if (sortKey.value === key && sortOrder.value === order) {
    sortKey.value = null
    sortOrder.value = null
  } else {
    sortKey.value = key
    sortOrder.value = order
  }
  if (props.grid.getData) {
    loadRows()
  }
}

function onFilterChange() {
  page.value = 1
  if (props.grid.getData) {
    loadRows()
  }
}

function rowKey(row: any) {
  return props.grid.rowKey ? props.grid.rowKey(row) : row.id || row._id || JSON.stringify(row)
}

function handleRowClick(row: any) {
  if (props.grid.onClick) {
    props.grid.onClick(row)
  }
}

async function loadRows() {
  // If getData is defined, use it, otherwise fallback to getRows
  if (props.grid.getData) {
    try {
      const result = await props.grid.getData({
        filters: { ...filters.value },
        sortKey: sortKey.value,
        sortOrder: sortOrder.value,
        page: page.value,
        pageSize: pageSize.value
      })
      rows.value = Array.isArray(result) ? result : []
      return
    } catch (e) {
      // Fallback to frontend filtering/sorting
      // Optionally, you could emit an error event here
    }
  }
  // Fallback: use getRows and frontend filtering/sorting
  const result = await props.grid.getRows()
  rows.value = Array.isArray(result) ? result : []
  page.value = 1
}

function prevPage() {
  if (page.value > 1) {
    page.value--
    if (props.grid.getData) {
      loadRows()
    }
  }
}
function nextPage() {
  if (page.value < totalPages.value) {
    page.value++
    if (props.grid.getData) {
      loadRows()
    }
  }
}

watch(() => props.grid, loadRows, { immediate: true })
</script>

<style scoped>
.table-view {
  width: 100%;
  overflow-x: auto;
}
table {
  width: 100%;
  border-collapse: separate;
  border-spacing: 0;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 2px 8px var(--color-background-card-shadow);
}
th, td {
  border: 1px solid var(--color-border);
  padding: 0.5em 1em;
  text-align: left;
}
th.header-cell {
  background: var(--color-background-soft);
  color: var(--color-heading);
  font-weight: bold;
  font-size: 1.1em;
  border-bottom: 2px solid var(--color-border-hover);
  position: relative;
}
.header-content {
  display: flex;
  align-items: center;
  gap: 0.5em;
}
.sort-icons button {
  background: none;
  border: none;
  color: var(--color-heading);
  font-size: 1em;
  cursor: pointer;
  padding: 0 0.2em;
  transition: color 0.2s;
}
.sort-icons button.active {
  color: var(--color-accent);
}
.filter-box {
  margin-top: 0.3em;
}
.filter-box input {
  width: 100%;
  padding: 0.2em 0.4em;
  border-radius: 4px;
  border: 1px solid var(--color-border);
  font-size: 1em;
}
.pagination {
  margin-top: 1em;
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 1em;
}
.clickable {
  cursor: pointer;
  background: var(--color-background-hover);
}
</style>
