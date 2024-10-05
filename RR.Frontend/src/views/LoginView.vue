<template>
  <div class="about">
    <input type="email" v-model="state.email" />
    <input type="password" v-model="state.password" />
    <button @click="OnSubmit">Submit</button>
  </div>
</template>
<script setup lang="ts">
import { reactive } from 'vue'

const state = reactive({
  email: '',
  password: ''
})

async function OnSubmit() {
  const response = await fetch('https://localhost:5001/account/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      email: state.email,
      password: state.password
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
