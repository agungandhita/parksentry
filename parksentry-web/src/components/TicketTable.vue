<template>
  <div class="table-wrap">
    <table class="data-table">
      <thead>
        <tr>
          <th>No. Tiket</th>
          <th>Plat</th>
          <th>Pelanggaran</th>
          <th>Zona</th>
          <th>Denda</th>
          <th>Tgl. Jatuh Tempo</th>
          <th>Status</th>
        </tr>
      </thead>
      <tbody>
        <tr v-if="loading">
          <td colspan="7" class="loading-row">
            <span class="spinner"></span> Memuat...
          </td>
        </tr>
        <tr v-else-if="!tickets.length">
          <td colspan="7">
            <div class="empty-state">
              <div class="empty-icon">🎫</div>
              <p>Tidak ada tiket</p>
            </div>
          </td>
        </tr>
        <tr v-for="t in tickets" :key="t.id">
          <td>
            <RouterLink :to="`/tickets/${t.id}`" style="color:var(--accent);font-weight:600">
              {{ t.ticketNo }}
            </RouterLink>
          </td>
          <td><strong>{{ t.vehicle?.plateNumber }}</strong></td>
          <td>{{ t.violationType?.name }}</td>
          <td>{{ t.zone?.zoneName }}</td>
          <td style="color:var(--accent-red);font-weight:600">{{ formatRp(t.fineAmount) }}</td>
          <td :style="isOverdue(t) ? 'color:var(--accent-red)' : ''">
            {{ formatDate(t.dueDate) }}
            <span v-if="isOverdue(t)">⚠️</span>
          </td>
          <td><span class="badge" :class="`badge-${t.status?.toLowerCase()}`">{{ t.status }}</span></td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
defineProps({
  tickets: { type: Array, default: () => [] },
  loading: { type: Boolean, default: false }
})

function formatRp(val) {
  if (!val && val !== 0) return '—'
  return 'Rp ' + Number(val).toLocaleString('id-ID')
}
function formatDate(d) {
  if (!d) return '—'
  return new Date(d).toLocaleDateString('id-ID', { day: '2-digit', month: 'short', year: 'numeric' })
}
function isOverdue(t) {
  return t.status?.toLowerCase() === 'unpaid' && new Date(t.dueDate) < new Date()
}
</script>
