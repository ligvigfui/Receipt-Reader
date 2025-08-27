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

const state = reactive({
  loginCredentials: {
    email: '',
    password: ''
  }
})

async function OnSubmit() {
  const result1 = await ApiClient.get<string>(
    'account/login',
    null,
    state.loginCredentials
  )
  const response = await fetch('https://localhost:5001/account/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      email: state.loginCredentials.email,
      password: state.loginCredentials.password
    })
  })
  const result = await response.json()
  if (response.ok) {
    alert(result.token)
  } else {
    let errors: string[] = result.errors;
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
