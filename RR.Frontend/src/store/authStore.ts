import { ref } from 'vue'

export const token = ref<string | null>(localStorage.getItem('token'))
export const isAuthenticated = ref<boolean>(localStorage.getItem('isAuthenticated') === 'true')

export function setToken(newToken: string | null) {
  token.value = newToken
  if (newToken) {
    localStorage.setItem('token', newToken)
    isAuthenticated.value = true
    localStorage.setItem('isAuthenticated', 'true')
  } else {
    localStorage.removeItem('token')
    isAuthenticated.value = false
    localStorage.setItem('isAuthenticated', 'false')
  }
}
