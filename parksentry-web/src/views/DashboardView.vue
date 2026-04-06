<template>
  <div>
    <!-- Admin Dashboard -->
    <template v-if="role === 'admin'">
      <div class="page-header">
        <div>
          <div class="page-title">Dashboard Admin</div>
          <div class="page-subtitle">Ringkasan sistem ParkSentry</div>
        </div>
      </div>

      <div class="stats-grid">
        <div class="stat-card">
          <div class="stat-icon">🎫</div>
          <div class="stat-value">{{ stats.total ?? '—' }}</div>
          <div class="stat-label">Total Tiket</div>
        </div>
        <div class="stat-card">
          <div class="stat-icon">⏰</div>
          <div class="stat-value" style="color:var(--accent-red)">{{ stats.unpaid ?? '—' }}</div>
          <div class="stat-label">Belum Dibayar</div>
        </div>
        <div class="stat-card">
          <div class="stat-icon">✅</div>
          <div class="stat-value" style="color:var(--accent-green)">{{ stats.paid ?? '—' }}</div>
          <div class="stat-label">Sudah Dibayar</div>
        </div>
        <div class="stat-card">
          <div class="stat-icon">💰</div>
          <div class="stat-value" style="font-size:16px">{{ formatRp(stats.paidFine) }}</div>
          <div class="stat-label">Denda Terkumpul</div>
        </div>
      </div>

      <div class="card">
        <div class="toolbar">
          <strong>Tiket Terbaru</strong>
          <RouterLink to="/tickets" class="btn btn-ghost btn-sm">Lihat Semua →</RouterLink>
        </div>
        <TicketTable :tickets="tickets.slice(0,8)" :loading="loading" />
      </div>
    </template>

    <!-- Officer Dashboard -->
    <template v-else-if="role === 'officer'">
      <div class="page-header">
        <div>
          <div class="page-title">Dashboard Petugas</div>
          <div class="page-subtitle">Selamat datang, {{ authStore.user?.fullName }}</div>
        </div>
        <RouterLink to="/tickets" class="btn btn-primary">+ Buat Tiket</RouterLink>
      </div>

      <div class="stats-grid">
        <div class="stat-card">
          <div class="stat-icon">🎫</div>
          <div class="stat-value">{{ stats.total ?? '—' }}</div>
          <div class="stat-label">Total Tiket</div>
        </div>
        <div class="stat-card">
          <div class="stat-icon">⏰</div>
          <div class="stat-value" style="color:var(--accent-red)">{{ stats.unpaid ?? '—' }}</div>
          <div class="stat-label">Belum Terbayar</div>
        </div>
        <div class="stat-card">
          <div class="stat-icon">✅</div>
          <div class="stat-value" style="color:var(--accent-green)">{{ stats.paid ?? '—' }}</div>
          <div class="stat-label">Sudah Terbayar</div>
        </div>
      </div>

      <div class="card">
        <div class="toolbar">
          <strong>Tiket Terbaru</strong>
          <RouterLink to="/tickets" class="btn btn-ghost btn-sm">Semua Tiket →</RouterLink>
        </div>
        <TicketTable :tickets="tickets.slice(0,8)" :loading="loading" />
      </div>
    </template>

    <!-- Owner Dashboard -->
    <template v-else-if="role === 'owner'">
      <div class="page-header">
        <div>
          <div class="page-title">Dashboard Saya</div>
          <div class="page-subtitle">Halo, {{ authStore.user?.fullName }}</div>
        </div>
      </div>

      <div class="stats-grid">
        <div class="stat-card">
          <div class="stat-icon">🎫</div>
          <div class="stat-value">{{ stats.total ?? '—' }}</div>
          <div class="stat-label">Total Tiket Saya</div>
        </div>
        <div class="stat-card">
          <div class="stat-icon">⏰</div>
          <div class="stat-value" style="color:var(--accent-red)">{{ stats.unpaid ?? '—' }}</div>
          <div class="stat-label">Belum Dibayar</div>
        </div>
        <div class="stat-card">
          <div class="stat-icon">✅</div>
          <div class="stat-value" style="color:var(--accent-green)">{{ stats.paid ?? '—' }}</div>
          <div class="stat-label">Lunas</div>
        </div>
        <div class="stat-card">
          <div class="stat-icon">💸</div>
          <div class="stat-value" style="color:var(--accent-red);font-size:16px">{{ formatRp(stats.unpaidFine) }}</div>
          <div class="stat-label">Denda Belum Bayar</div>
        </div>
      </div>

      <!-- Unpaid tickets warning -->
      <div v-if="unpaidTickets.length" class="alert-unpaid card">
        <strong>⚠️ Anda memiliki {{ unpaidTickets.length }} tiket yang belum dibayar!</strong>
        <RouterLink to="/tickets" class="btn btn-danger btn-sm" style="margin-left:12px">Bayar Sekarang</RouterLink>
      </div>

      <div class="card" style="margin-top:12px">
        <div class="toolbar">
          <strong>Tiket Saya</strong>
          <RouterLink to="/tickets" class="btn btn-ghost btn-sm">Lihat Semua →</RouterLink>
        </div>
        <TicketTable :tickets="tickets.slice(0,8)" :loading="loading" />
      </div>
    </template>

    <!-- Fallback -->
    <template v-else>
      <div class="page-title">Dashboard</div>
    </template>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useAuthStore } from '../stores/auth'
import { ticketApi } from '../api'
import TicketTable from '../components/TicketTable.vue'

const authStore = useAuthStore()
const role      = computed(() => authStore.user?.role)

const stats   = ref({})
const tickets = ref([])
const loading = ref(true)

const unpaidTickets = computed(() =>
  tickets.value.filter(t => t.status?.toLowerCase() === 'unpaid')
)

function formatRp(val) {
  if (!val && val !== 0) return '—'
  return 'Rp ' + Number(val).toLocaleString('id-ID')
}

onMounted(async () => {
  try {
    const [statsRes, ticketsRes] = await Promise.all([
      ticketApi.getStats(),
      ticketApi.getAll()
    ])
    stats.value   = statsRes.data
    tickets.value = ticketsRes.data
  } catch (e) {
    console.error(e)
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
.alert-unpaid {
  display: flex; align-items: center;
  background: #fff8e1;
  border: 1px solid #ffe082;
  border-radius: var(--radius);
  padding: 12px 16px;
  font-size: 13px;
  color: var(--accent-yellow);
  margin-bottom: 0;
}
</style>
