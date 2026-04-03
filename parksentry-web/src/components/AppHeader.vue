<template>
  <header class="app-header">
    <div class="header-left">
      <h2 class="header-title">{{ pageTitle }}</h2>
    </div>
    <div class="header-right">
      <span class="header-time">{{ currentTime }}</span>
      <div class="header-user">
        <div class="avatar">{{ authStore.user?.avatar || '👤' }}</div>
        <div class="user-info">
          <span class="user-name">{{ authStore.user?.name }}</span>
          <span class="user-role">{{ authStore.user?.role }}</span>
        </div>
      </div>
      <button id="logout-btn" class="logout-btn" @click="handleLogout" title="Keluar">
        🚪 Keluar
      </button>
    </div>
  </header>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()

function handleLogout() {
  authStore.logout()
  router.push('/login')
}
const currentTime = ref('')

const pageTitle = computed(() => {
  const titles = {
    '/dashboard': 'Dashboard',
    '/tickets': 'Tiket Pelanggaran',
    '/vehicles': 'Data Kendaraan',
    '/zones': 'Zona Parkir',
    '/violation-types': 'Jenis Pelanggaran',
  }
  return titles[route.path] || route.meta.title || 'ParkSentry'
})

let timer
function updateTime() {
  const now = new Date()
  currentTime.value = now.toLocaleString('id-ID', {
    weekday: 'short', day: '2-digit', month: 'short',
    hour: '2-digit', minute: '2-digit'
  })
}

onMounted(() => {
  updateTime()
  timer = setInterval(updateTime, 1000)
})
onUnmounted(() => clearInterval(timer))
</script>

<style scoped>
.app-header {
  height: 64px;
  background: var(--bg-secondary);
  border-bottom: 1px solid var(--border);
  display: flex; align-items: center; justify-content: space-between;
  padding: 0 32px;
  position: sticky; top: 0; z-index: 40;
}
.header-title { font-size: 18px; font-weight: 700; }
.header-right { display: flex; align-items: center; gap: 20px; }
.header-time { font-size: 13px; color: var(--text-muted); }
.header-user {
  display: flex; align-items: center; gap: 10px;
  font-size: 14px; font-weight: 600; color: var(--text-secondary);
}
.avatar {
  width: 36px; height: 36px;
  background: var(--gradient-primary);
  border-radius: 50%;
  display: flex; align-items: center; justify-content: center;
  font-size: 18px;
}
.user-info { display: flex; flex-direction: column; }
.user-name { font-size: 13px; font-weight: 700; color: var(--text-primary); line-height: 1.2; }
.user-role { font-size: 11px; color: var(--text-muted); font-weight: 500; }
.logout-btn {
  display: flex; align-items: center; gap: 6px;
  padding: 7px 14px;
  background: rgba(239,68,68,0.1);
  border: 1px solid rgba(239,68,68,0.25);
  border-radius: 8px;
  font-size: 13px; font-weight: 600; color: #f87171;
  cursor: pointer; transition: all 0.2s;
}
.logout-btn:hover { background: rgba(239,68,68,0.2); border-color: rgba(239,68,68,0.4); }
</style>
