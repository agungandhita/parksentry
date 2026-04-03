<template>
  <div>
    <div class="page-header">
      <div>
        <h1 class="page-title">Data Kendaraan</h1>
        <p class="page-subtitle">Semua kendaraan yang terdaftar</p>
      </div>
    </div>
    <div class="card">
      <div class="toolbar">
        <div class="search-wrap">
          <span class="search-icon">🔍</span>
          <input v-model="search" class="search-input" placeholder="Cari plat, merek, model..." />
        </div>
      </div>
      <div class="table-wrap">
        <table class="data-table">
          <thead><tr><th>Plat Nomor</th><th>Merek / Model</th><th>Warna</th><th>Tipe</th><th>Tahun</th><th>Pemilik</th></tr></thead>
          <tbody>
            <tr v-if="loading"><td colspan="6" class="loading-row"><span class="spinner"></span></td></tr>
            <tr v-else-if="filtered.length === 0"><td colspan="6"><div class="empty-state"><div class="empty-icon">🚗</div><p>Tidak ada kendaraan</p></div></td></tr>
            <tr v-for="v in filtered" :key="v.id">
              <td><span style="font-weight:800;background:rgba(99,102,241,0.1);padding:4px 10px;border-radius:6px;letter-spacing:1px">{{ v.plateNumber }}</span></td>
              <td><div style="font-weight:600">{{ v.brand }}</div><div style="font-size:12px;color:var(--text-muted)">{{ v.model }}</div></td>
              <td><span style="font-size:13px;color:var(--text-secondary)">{{ v.color }}</span></td>
              <td><span class="badge" style="background:rgba(59,130,246,0.12);color:#60a5fa">{{ v.vehicleType }}</span></td>
              <td style="color:var(--text-secondary)">{{ v.year || '—' }}</td>
              <td><div style="font-size:13px;font-weight:600">{{ v.owner?.fullName }}</div><div style="font-size:11px;color:var(--text-muted)">{{ v.owner?.idCardNo }}</div></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { vehicleApi } from '../api'

const vehicles = ref([])
const loading = ref(true)
const search = ref('')

const filtered = computed(() => {
  const s = search.value.toLowerCase()
  return vehicles.value.filter(v =>
    !s || v.plateNumber?.toLowerCase().includes(s) ||
    v.brand?.toLowerCase().includes(s) || v.model?.toLowerCase().includes(s)
  )
})

onMounted(async () => {
  try { const r = await vehicleApi.getAll(); vehicles.value = r.data }
  finally { loading.value = false }
})
</script>
