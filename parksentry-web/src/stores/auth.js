import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useAuthStore = defineStore('auth', () => {
  const user = ref(JSON.parse(localStorage.getItem('ps_user') || 'null'))
  const token = ref(localStorage.getItem('ps_token') || null)

  const isLoggedIn = computed(() => !!token.value && !!user.value)
  const isAdmin = computed(() => user.value?.role === 'Admin')
  const isOfficer = computed(() => user.value?.role === 'Officer')

  // Simulasi login (ganti dengan API call nyata saat backend auth sudah siap)
  async function login(username, password, role) {
    // Mock users — ganti ini dengan fetch ke /api/auth/login
    const mockUsers = {
      Admin: [
        { username: 'admin', password: 'admin123', name: 'Super Admin', email: 'admin@parksentry.id', avatar: '👑' },
      ],
      Officer: [
        { username: 'officer1', password: 'officer123', name: 'Budi Santoso', email: 'budi@parksentry.id', avatar: '👮' },
        { username: 'officer2', password: 'officer123', name: 'Siti Rahma', email: 'siti@parksentry.id', avatar: '👮' },
      ],
    }

    const found = mockUsers[role]?.find(
      (u) => u.username === username && u.password === password
    )

    if (!found) throw new Error('Username atau password salah')

    const userData = {
      id: Math.random().toString(36).slice(2),
      username: found.username,
      name: found.name,
      email: found.email,
      avatar: found.avatar,
      role,
    }
    const mockToken = btoa(`${username}:${role}:${Date.now()}`)

    user.value = userData
    token.value = mockToken
    localStorage.setItem('ps_user', JSON.stringify(userData))
    localStorage.setItem('ps_token', mockToken)

    return userData
  }

  function logout() {
    user.value = null
    token.value = null
    localStorage.removeItem('ps_user')
    localStorage.removeItem('ps_token')
  }

  return { user, token, isLoggedIn, isAdmin, isOfficer, login, logout }
})
