<template>
  <header class="app-header">
    <div class="header-title">{{ pageTitle }}</div>
    <div class="header-right">
      <span class="header-user">
        {{ authStore.user?.fullName }}
        <span class="role-badge">{{ authStore.user?.role }}</span>
      </span>
    </div>
  </header>
</template>

<script setup>
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const route     = useRoute()
const authStore = useAuthStore()

const pageTitle = computed(() => {
  const titles = {
    '/dashboard':       'Dashboard',
    '/tickets':         'Tiket Pelanggaran',
    '/vehicles':        'Data Kendaraan',
    '/zones':           'Zona Parkir',
    '/violation-types': 'Jenis Pelanggaran',
  }
  return titles[route.path] || route.meta?.title || 'ParkSentry'
})
</script>

<style scoped>
.app-header {
  height: 52px;
  background: #fff;
  border-bottom: 1px solid var(--border);
  display: flex; align-items: center; justify-content: space-between;
  padding: 0 24px;
  position: sticky; top: 0; z-index: 40;
}
.header-title { font-size: 15px; font-weight: 700; }
.header-right { display: flex; align-items: center; gap: 12px; }
.header-user { font-size: 13px; color: var(--text-muted); display: flex; align-items: center; gap: 8px; }
.role-badge {
  background: #dbeafe;
  color: var(--accent);
  font-size: 11px; font-weight: 700;
  padding: 2px 8px; border-radius: 10px;
  text-transform: capitalize;
}
</style>
