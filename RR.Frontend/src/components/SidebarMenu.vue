<template>
  <div class="sidebar" :class="{ expanded: menuOpen }">
    <button class="sandwich" @click="toggleMenu" :aria-label="menuOpen ? 'Close menu' : 'Open menu'">
      <MenuIcon :open="menuOpen" />
    </button>
    <div v-if="menuOpen || isMobile" class="menu">
      <slot name="login-profile">
        <div class="menu-link" @click="onLoginProfileClick">
          <span class="icon">{{ isAuthenticated ? '👤' : '🔑' }}</span>
          <span v-if="showText">{{ isAuthenticated ? 'Profile' : 'Log In' }}</span>
        </div>
      </slot>
      <div class="menu-divider"></div>
      <div class="menu-link" @click="goReceipts">
        <span class="icon">🧾</span>
        <span v-if="showText">Receipts</span>
      </div>
      <!-- Add more menu items here if needed -->
    </div>
  </div>
</template>

<script setup lang="ts">
import { isAuthenticated } from '@/store/authStore'
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useRouter } from 'vue-router'
import MenuIcon from '@/components/icons/MenuIcon.vue'

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
.sandwich {
  position: absolute;
  top: 1rem;
  left: 50%;
  transform: translateX(-50%);
  background: none;
  border: none;
  font-size: 1.7rem;
  display: flex;
  align-items: center;
  padding: 0;
  justify-content: center;
  width: 2em;
  height: 2em;
  z-index: 1100;
  transition: right 0.3s;
}
.sidebar.expanded .sandwich {
    align-self: flex-end;
    left: auto;
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
.menu {
  margin-top: 6em;
  display: flex;
  flex-direction: column;
  gap: 0;
  width: 100%;
}
.menu-link {
  display: flex;
  align-items: center;
  gap: 0.7em;
  width: 100%;
  padding: 0.7rem 1rem;
  color: var(--color-heading);
  background: none;
  border: none;
  cursor: pointer;
  font-size: 1.1em;
  transition: background 0.15s;
  user-select: none;
}
.menu-link:hover {
  background: var(--color-button);
}
.menu-divider {
  width: 90%;
  height: 1px;
  background: var(--color-border);
  margin: 0.2em auto;
}
.icon {
  font-size: 1.5em;
}
</style>
