import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import api from '../api/index'

export const useAuthStore = defineStore('auth', () => {
  const user  = ref(JSON.parse(localStorage.getItem('ps_user')  || 'null'))
  const token = ref(localStorage.getItem('ps_token') || null)

  const isLoggedIn = computed(() => !!user.value)
  const isAdmin    = computed(() => user.value?.role === 'admin')
  const isOfficer  = computed(() => user.value?.role === 'officer')
  const isOwner    = computed(() => user.value?.role === 'owner')

  // ── Login ─────────────────────────────────────────────────────
  async function login(username, password) {
    const res = await api.post('/auth/login', { username, password })
    const userData = res.data

    // Generate a simple client-side token (replace with real JWT when backend provides)
    const mockToken = btoa(`${userData.username}:${userData.role}:${Date.now()}`)

    user.value  = userData
    token.value = mockToken
    localStorage.setItem('ps_user',  JSON.stringify(userData))
    localStorage.setItem('ps_token', mockToken)

    return userData
  }

  // ── Register ──────────────────────────────────────────────────
  async function register({ fullName, username, password, email, phone, role, idCardNo, address }) {
    const res = await api.post('/auth/register', {
      fullName,
      username,
      password,
      email,
      phone,
      role,
      idCardNo,
      address,
    })
    return res.data
  }

  // ── Logout ────────────────────────────────────────────────────
  function logout() {
    user.value  = null
    token.value = null
    localStorage.removeItem('ps_user')
    localStorage.removeItem('ps_token')
  }

  return { user, token, isLoggedIn, isAdmin, isOfficer, isOwner, login, logout, register }
})
