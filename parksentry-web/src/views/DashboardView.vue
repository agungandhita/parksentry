<template>
  <div>
    <!-- Stats Grid -->
    <div class="stats-grid">
      <div class="stat-card primary">
        <div class="stat-icon primary">🎫</div>
        <div class="stat-value">{{ stats.total ?? '—' }}</div>
        <div class="stat-label">Total Tiket</div>
      </div>
      <div class="stat-card danger">
        <div class="stat-icon danger">⏰</div>
        <div class="stat-value">{{ stats.unpaid ?? '—' }}</div>
        <div class="stat-label">Belum Dibayar</div>
      </div>
      <div class="stat-card success">
        <div class="stat-icon success">✅</div>
        <div class="stat-value">{{ stats.paid ?? '—' }}</div>
        <div class="stat-label">Sudah Dibayar</div>
      </div>
      <div class="stat-card warning">
        <div class="stat-icon warning">💰</div>
        <div class="stat-value">{{ formatRp(stats.paidFine) }}</div>
        <div class="stat-label">Denda Terkumpul</div>
      </div>
    </div>

    <!-- Recent Tickets -->
    <div class="card">
      <div class="toolbar">
        <div>
          <h3 style="font-size:16px;font-weight:700">Tiket Terbaru</h3>
          <p style="font-size:13px;color:var(--text-secondary);margin-top:4px">10 tiket pelanggaran terakhir</p>
        </div>
        <RouterLink to="/tickets" class="btn btn-ghost btn-sm">Lihat Semua →</RouterLink>
      </div>
      <div class="table-wrap">
        <table class="data-table">
          <thead>
            <tr>
              <th>No. Tiket</th>
              <th>Kendaraan</th>
              <th>Pelanggaran</th>
              <th>Zona</th>
              <th>Denda</th>
              <th>Tanggal</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="loading"><td colspan="7" class="loading-row"><span class="spinner"></span> Memuat data...</td></tr>
            <tr v-else-if="tickets.length === 0"><td colspan="7"><div class="empty-state"><div class="empty-icon">🎫</div><p>Belum ada tiket pelanggaran</p></div></td></tr>
            <tr v-for="t in tickets.slice(0,10)" :key="t.id">
              <td><RouterLink :to="`/tickets/${t.id}`" style="color:var(--accent-primary);font-weight:600">{{ t.ticketNo }}</RouterLink></td>
              <td>
                <div style="font-weight:600">{{ t.vehicle?.plateNumber }}</div>
                <div style="font-size:12px;color:var(--text-muted)">{{ t.vehicle?.brand }} {{ t.vehicle?.model }}</div>
              </td>
              <td>{{ t.violationType?.name }}</td>
              <td>{{ t.zone?.zoneName }}</td>
              <td style="font-weight:600;color:var(--accent-warning)">{{ formatRp(t.fineAmount) }}</td>
              <td style="font-size:12px;color:var(--text-secondary)">{{ formatDate(t.violationDate) }}</td>
              <td><span class="badge" :class="`badge-${t.status?.toLowerCase()}`">{{ t.status }}</span></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { ticketApi } from '../api'

const stats = ref({})
const tickets = ref([])
const loading = ref(true)

function formatRp(val) {
  if (!val && val !== 0) return '—'
  return 'Rp ' + Number(val).toLocaleString('id-ID')
}

function formatDate(d) {
  if (!d) return '—'
  return new Date(d).toLocaleDateString('id-ID', { day: '2-digit', month: 'short', year: 'numeric' })
}

onMounted(async () => {
  try {
    const [statsRes, ticketsRes] = await Promise.all([
      ticketApi.getStats(),
      ticketApi.getAll()
    ])
    stats.value = statsRes.data
    tickets.value = ticketsRes.data
  } catch (e) {
    console.error(e)
  } finally {
    loading.value = false
  }
})
</script>
