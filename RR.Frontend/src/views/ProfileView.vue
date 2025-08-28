<template>
  <div class="profile-view">
    <h1>Profile</h1>
    <p>Welcome to your profile page.</p>
    <button @click="logout">Log Out</button>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import { setToken } from '@/store/authStore'

const router = useRouter()

function logout() {
  setToken(null)
  router.push('/login')
}
</script>

<style scoped>
.profile-view {
  padding: 2rem;
}
button {
  margin-top: 1rem;
  padding: 0.5rem 1.5rem;
  background: #444;
  color: #fff;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
button:hover {
  background: #666;
}
</style>
