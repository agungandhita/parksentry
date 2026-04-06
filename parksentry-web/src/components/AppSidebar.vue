<template>
  <aside class="sidebar">
    <div class="sidebar-brand">
      <span>🅿</span>
      <div>
        <div class="brand-name">ParkSentry</div>
        <div class="brand-role">{{ roleLabel }}</div>
      </div>
    </div>

    <nav class="sidebar-nav">
      <RouterLink
        v-for="item in navItems"
        :key="item.path"
        :to="item.path"
        class="nav-item"
        :class="{ active: isActive(item.path) }"
      >
        <span>{{ item.icon }}</span>
        <span>{{ item.label }}</span>
      </RouterLink>
    </nav>

    <div class="sidebar-footer">
      <button id="logout-btn" class="logout-btn" @click="handleLogout">
        🚪 Keluar
      </button>
      <div class="user-info">
        <div class="user-name">{{ authStore.user?.fullName }}</div>
        <div class="user-role">{{ authStore.user?.username }}</div>
      </div>
    </div>
  </aside>
</template>

<script setup>
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const route     = useRoute()
const router    = useRouter()
const authStore = useAuthStore()

const roleLabel = computed(() => {
  const map = { admin: 'Administrator', officer: 'Petugas', owner: 'Pemilik Kendaraan' }
  return map[authStore.user?.role] || authStore.user?.role || ''
})

const navItems = computed(() => {
  const role = authStore.user?.role
  if (role === 'admin') {
    return [
      { path: '/dashboard',      icon: '📊', label: 'Dashboard' },
      { path: '/tickets',        icon: '🎫', label: 'Tiket Pelanggaran' },
      { path: '/vehicles',       icon: '🚗', label: 'Kendaraan' },
      { path: '/zones',          icon: '📍', label: 'Zona Parkir' },
      { path: '/violation-types', icon: '⚠️', label: 'Jenis Pelanggaran' },
    ]
  }
  if (role === 'officer') {
    return [
      { path: '/dashboard', icon: '📊', label: 'Dashboard' },
      { path: '/tickets',   icon: '🎫', label: 'Tiket Pelanggaran' },
      { path: '/vehicles',  icon: '🚗', label: 'Cari Kendaraan' },
    ]
  }
  if (role === 'owner') {
    return [
      { path: '/dashboard', icon: '📊', label: 'Dashboard' },
      { path: '/tickets',   icon: '🎫', label: 'Tiket Saya' },
      { path: '/vehicles',  icon: '🚗', label: 'Kendaraan Saya' },
    ]
  }
  return [{ path: '/dashboard', icon: '📊', label: 'Dashboard' }]
})

function isActive(path) {
  return route.path === path || route.path.startsWith(path + '/')
}

function handleLogout() {
  authStore.logout()
  router.push('/login')
}
</script>

<style scoped>
.sidebar {
  position: fixed; left: 0; top: 0; bottom: 0;
  width: var(--sidebar-width);
  background: #fff;
  border-right: 1px solid var(--border);
  display: flex; flex-direction: column;
  z-index: 50;
}

.sidebar-brand {
  display: flex; align-items: center; gap: 10px;
  padding: 16px;
  border-bottom: 1px solid var(--border);
  font-size: 22px;
}
.brand-name { font-size: 15px; font-weight: 700; color: var(--text); }
.brand-role { font-size: 11px; color: var(--text-muted); }

.sidebar-nav {
  flex: 1; overflow-y: auto;
  padding: 8px;
  display: flex; flex-direction: column; gap: 2px;
}

.nav-item {
  display: flex; align-items: center; gap: 8px;
  padding: 8px 10px;
  border-radius: var(--radius);
  color: var(--text-muted);
  font-size: 13px; font-weight: 500;
  transition: background 0.15s;
}
.nav-item:hover { background: var(--bg); color: var(--text); }
.nav-item.active {
  background: #dbeafe;
  color: var(--accent);
  font-weight: 600;
}

.sidebar-footer {
  padding: 12px 16px;
  border-top: 1px solid var(--border);
}
.logout-btn {
  width: 100%;
  padding: 7px;
  background: #fff0f0;
  border: 1px solid #fdd;
  border-radius: var(--radius);
  color: var(--accent-red);
  font-size: 13px; font-weight: 600;
  cursor: pointer; margin-bottom: 10px;
  text-align: center;
}
.logout-btn:hover { background: #fce8e8; }
.user-name { font-size: 13px; font-weight: 600; }
.user-role { font-size: 11px; color: var(--text-muted); }
</style>
