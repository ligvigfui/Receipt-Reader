<template>
  <div class="login-container">
    <form class="login-form" @submit.prevent="OnSubmit">
      <h2>Log In</h2>
      <input type="email" v-model="state.loginCredentials.email" placeholder="Email" required />
      <input type="password" v-model="state.loginCredentials.password" placeholder="Password" required />
      <button type="submit">Submit</button>
    </form>
  </div>
</template>
<script setup lang="ts">
import { reactive } from 'vue'
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
.login-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: var(--color-background-soft);
}
.login-form {
  background: var(--color-background-card);
  color: var(--color-text);
  padding: 2rem 2.5rem;
  border-radius: 12px;
  box-shadow: var(--color-background-card-shadow);
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
  min-width: 320px;
}
.login-form h2 {
  margin: 0 0 1rem 0;
  text-align: center;
  color: var(--color-accent);
}
.login-form input {
  font-size: 1rem;
}
.login-form button {
  background: var(--color-accent);
  color: #fff;
  border: none;
  border-radius: 6px;
  padding: 0.7em 0;
  font-size: 1.1rem;
  cursor: pointer;
  transition: background 0.2s;
}
.login-form button:hover {
  background: var(--color-button-hover);
}
</style>
