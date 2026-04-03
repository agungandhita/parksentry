<template>
  <aside class="sidebar">
    <div class="sidebar-logo">
      <div class="logo-icon">🛡️</div>
      <div class="logo-text">
        <span class="logo-name">ParkSentry</span>
        <span class="logo-sub">Management System</span>
      </div>
    </div>

    <nav class="sidebar-nav">
      <div class="nav-section-label">Main Menu</div>
      <RouterLink v-for="item in navItems" :key="item.path" :to="item.path" class="nav-item" :class="{ active: isActive(item.path) }">
        <span class="nav-icon">{{ item.icon }}</span>
        <span class="nav-label">{{ item.label }}</span>
        <span v-if="item.badge" class="nav-badge">{{ item.badge }}</span>
      </RouterLink>
    </nav>

    <div class="sidebar-footer">
      <div class="api-status" :class="{ online: apiOnline }">
        <span class="status-dot"></span>
        <span>API {{ apiOnline ? 'Online' : 'Offline' }}</span>
      </div>
    </div>
  </aside>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import api from '../api'

const route = useRoute()
const apiOnline = ref(false)

const navItems = [
  { path: '/dashboard', icon: '📊', label: 'Dashboard' },
  { path: '/tickets', icon: '🎫', label: 'Tiket Pelanggaran' },
  { path: '/vehicles', icon: '🚗', label: 'Kendaraan' },
  { path: '/zones', icon: '📍', label: 'Zona Parkir' },
  { path: '/violation-types', icon: '⚠️', label: 'Jenis Pelanggaran' },
]

function isActive(path) {
  return route.path === path || route.path.startsWith(path + '/')
}

onMounted(async () => {
  try {
    await api.get('/violationtickets/stats')
    apiOnline.value = true
  } catch {
    apiOnline.value = false
  }
})
</script>

<style scoped>
.sidebar {
  position: fixed; left: 0; top: 0; bottom: 0;
  width: var(--sidebar-width);
  background: var(--bg-sidebar);
  border-right: 1px solid var(--border);
  display: flex; flex-direction: column;
  z-index: 50;
  padding: 0 0 16px;
}

.sidebar-logo {
  display: flex; align-items: center; gap: 12px;
  padding: 20px 20px 16px;
  border-bottom: 1px solid var(--border);
  margin-bottom: 8px;
}
.logo-icon { font-size: 28px; }
.logo-name { display: block; font-size: 16px; font-weight: 800; color: var(--text-primary); }
.logo-sub { display: block; font-size: 10px; color: var(--text-muted); font-weight: 500; text-transform: uppercase; letter-spacing: 0.5px; }

.sidebar-nav { flex: 1; padding: 8px 12px; overflow-y: auto; }
.nav-section-label {
  font-size: 10px; font-weight: 700; text-transform: uppercase;
  letter-spacing: 1px; color: var(--text-muted);
  padding: 8px 8px 6px;
}
.nav-item {
  display: flex; align-items: center; gap: 10px;
  padding: 9px 12px;
  border-radius: var(--radius-sm);
  color: var(--text-secondary);
  font-size: 14px; font-weight: 500;
  transition: var(--transition);
  margin-bottom: 2px;
  position: relative;
}
.nav-item:hover { background: rgba(255,255,255,0.05); color: var(--text-primary); }
.nav-item.active {
  background: rgba(99,102,241,0.12);
  color: var(--accent-primary);
  font-weight: 600;
}
.nav-item.active::before {
  content: '';
  position: absolute; left: 0; top: 20%; bottom: 20%;
  width: 3px; border-radius: 0 3px 3px 0;
  background: var(--accent-primary);
}
.nav-icon { font-size: 16px; width: 20px; text-align: center; }
.nav-badge {
  margin-left: auto;
  background: var(--accent-danger);
  color: white; font-size: 10px; font-weight: 700;
  padding: 1px 6px; border-radius: 10px;
}

.sidebar-footer { padding: 16px 20px 0; border-top: 1px solid var(--border); }
.api-status {
  display: flex; align-items: center; gap: 8px;
  font-size: 12px; color: var(--text-muted);
}
.status-dot {
  width: 8px; height: 8px; border-radius: 50%;
  background: var(--accent-danger);
  animation: pulse 2s infinite;
}
.api-status.online .status-dot { background: var(--accent-success); }
@keyframes pulse { 0%, 100% { opacity: 1; } 50% { opacity: 0.4; } }
</style>
