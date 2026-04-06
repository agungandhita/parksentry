<template>
  <div>
    <div class="page-header">
      <div>
        <div class="page-title">Data Kendaraan</div>
        <div class="page-subtitle">Semua kendaraan yang terdaftar</div>
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
            <tr v-if="loading"><td colspan="6" class="loading-row"><span class="spinner"></span> Memuat...</td></tr>
            <tr v-else-if="filtered.length === 0"><td colspan="6"><div class="empty-state"><div class="empty-icon">🚗</div><p>Tidak ada kendaraan</p></div></td></tr>
            <tr v-for="v in filtered" :key="v.id">
              <td><strong>{{ v.plateNumber }}</strong></td>
              <td>{{ v.brand }} {{ v.model }}</td>
              <td>{{ v.color }}</td>
              <td><span class="badge badge-appealing">{{ v.vehicleType }}</span></td>
              <td>{{ v.year || '—' }}</td>
              <td>
                <div>{{ v.owner?.fullName }}</div>
                <div style="font-size:11px;color:var(--text-muted)">{{ v.owner?.idCardNo }}</div>
              </td>
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
