<template>
  <div class="about">
    <input type="email" v-model="state.loginCredentials.email" />
    <input type="password" v-model="state.loginCredentials.password" />
    <button @click="OnSubmit">Submit</button>
  </div>
</template>
<script setup lang="ts">
import { reactive } from 'vue'
// import ApiClient
import { ApiClient } from '@/utils/ApiClient'
import { setToken } from '@/store/authStore'

const state = reactive({
  loginCredentials: {
    email: '',
    password: ''
  }
})

async function OnSubmit() {
  const result = await ApiClient.post<AuthResponse>(
    'account/login',
    state.loginCredentials,
  )
  if (result.isOk()) {
    setToken(result.token)
  } else {
    let errors: string[] = result.errors ?? [];
    alert(errors.join('\n'))
  }
}
</script>
<style>
@media (min-width: 1024px) {
  .about {
    min-height: 100vh;
    display: flex;
    align-items: center;
  }
}
</style>
