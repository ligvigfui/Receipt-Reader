<template>
  <div class="sidebar" :class="{ expanded: menuOpen }">
    <button class="sandwich" @click="toggleMenu">
      &#9776;
    </button>
    <div v-if="menuOpen || isMobile" class="menu">
      <slot name="login-profile">
        <button @click="onLoginProfileClick">
          <span class="icon">{{ isAuthenticated ? '👤' : '🔑' }}</span>
          <span v-if="showText">{{ isAuthenticated ? 'Profile' : 'Log In' }}</span>
        </button>
      </slot>
      <button @click="goReceipts">
        <span class="icon">🧾</span>
        <span v-if="showText">Receipts</span>
      </button>
      <!-- Add more menu items here if needed -->
    </div>
  </div>
</template>

<script setup lang="ts">
import { isAuthenticated } from '@/store/authStore'
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useRouter } from 'vue-router'

const menuOpen = ref(false)
const isMobile = ref(false)
const router = useRouter()

function toggleMenu() {
  menuOpen.value = !menuOpen.value
}

function onLoginProfileClick() {
  if (isAuthenticated.value) {
    router.push('/profile')
  } else {
    router.push('/login')
  }
}

function goReceipts() {
  router.push('/receipts')
}

function handleResize() {
  isMobile.value = window.innerWidth < 768
}
onMounted(() => {
  handleResize()
  window.addEventListener('resize', handleResize)
})
onUnmounted(() => {
  window.removeEventListener('resize', handleResize)
})

const showText = computed(() => isMobile.value || menuOpen.value)
</script>

<style scoped>
.sidebar {
  position: fixed;
  left: 0;
  top: 0;
  height: 100vh;
  width: 100vw;
  background: var(--color-background-soft);
  display: flex;
  flex-direction: column;
  align-items: center;
  z-index: 1000;
  transition: width 0.3s;
}
@media (min-width: 768px) {
  .sidebar {
    width: 4em;
  }
  .sidebar.expanded {
    width: 16em;
  }
  .menu {
    align-items: flex-start;
  }
}
.sandwich {
  background: none;
  border: none;
  color: var(--color-text);
  font-size: 2rem;
  margin: 1rem 0;
  cursor: pointer;
}
.menu {
  margin-top: 2rem;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}
.menu button {
  background: var(--color-button);
  color: var(--color-heading);
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  cursor: pointer;
}
.menu button:hover {
  background: var(--color-button-hover);
}
.icon {
  font-size: 1.5em;
}
</style>
