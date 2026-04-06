<template>
  <div>
    <div class="page-header">
      <div>
        <h1 class="page-title">Zona Parkir</h1>
        <p class="page-subtitle">Kelola zona-zona parkir yang aktif</p>
      </div>
      <button class="btn btn-primary" @click="showModal = true">+ Tambah Zona</button>
    </div>
    <div class="card">
      <div class="table-wrap">
        <table class="data-table">
          <thead><tr><th>Kode</th><th>Nama Zona</th><th>Alamat</th><th>Kota</th><th>Status</th><th>Aksi</th></tr></thead>
          <tbody>
            <tr v-if="loading"><td colspan="6" class="loading-row"><span class="spinner"></span></td></tr>
            <tr v-else-if="zones.length === 0"><td colspan="6"><div class="empty-state"><div class="empty-icon">📍</div><p>Belum ada zona parkir</p></div></td></tr>
            <tr v-for="z in zones" :key="z.id">
              <td><strong style="color:var(--accent)">{{ z.zoneCode }}</strong></td>
              <td>{{ z.zoneName }}</td>
              <td style="color:var(--text-muted)">{{ z.address || '—' }}</td>
              <td style="color:var(--text-muted)">{{ z.city || '—' }}</td>
              <td><span class="badge" :class="z.isActive ? 'badge-paid' : 'badge-cancelled'">{{ z.isActive ? 'Aktif' : 'Nonaktif' }}</span></td>
              <td><button class="btn btn-danger btn-sm" @click="deleteZone(z)">Nonaktifkan</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <Transition name="fade">
      <div v-if="showModal" class="modal-overlay" @click.self="showModal = false">
        <div class="modal">
          <div class="modal-header"><h3 class="modal-title">📍 Tambah Zona Parkir</h3><button class="modal-close" @click="showModal = false">✕</button></div>
          <div class="modal-body">
            <div class="form-row">
              <div class="form-group"><label class="form-label">Kode Zona *</label><input v-model="form.zoneCode" class="form-input" placeholder="Z-001" /></div>
              <div class="form-group"><label class="form-label">Nama Zona *</label><input v-model="form.zoneName" class="form-input" placeholder="Nama lokasi..." /></div>
            </div>
            <div class="form-row">
              <div class="form-group"><label class="form-label">Kota</label><input v-model="form.city" class="form-input" placeholder="Jakarta" /></div>
              <div class="form-group"><label class="form-label">Alamat</label><input v-model="form.address" class="form-input" placeholder="Jalan..." /></div>
            </div>
          </div>
          <div class="modal-footer">
            <button class="btn btn-ghost" @click="showModal = false">Batal</button>
            <button class="btn btn-primary" @click="submit" :disabled="saving">{{ saving ? 'Menyimpan...' : 'Simpan' }}</button>
          </div>
        </div>
      </div>
    </Transition>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { zoneApi } from '../api'
import { useToastStore } from '../stores/toast'

const toast = useToastStore()
const zones = ref([])
const loading = ref(true)
const showModal = ref(false)
const saving = ref(false)
const form = ref({ zoneCode: '', zoneName: '', city: '', address: '' })

async function submit() {
  if (!form.value.zoneCode || !form.value.zoneName) { toast.error('Kode dan nama zona wajib diisi'); return }
  saving.value = true
  try {
    await zoneApi.create(form.value)
    toast.success('Zona berhasil ditambahkan')
    showModal.value = false
    form.value = { zoneCode: '', zoneName: '', city: '', address: '' }
    await load()
  } catch (e) { toast.error('Gagal: ' + (e.response?.data?.title || e.message)) }
  finally { saving.value = false }
}

async function deleteZone(z) {
  if (!confirm(`Nonaktifkan zona ${z.zoneName}?`)) return
  try { await zoneApi.delete(z.id); z.isActive = false; toast.success('Zona dinonaktifkan') }
  catch { toast.error('Gagal menonaktifkan zona') }
}

async function load() {
  loading.value = true
  try { const r = await zoneApi.getAll(); zones.value = r.data }
  finally { loading.value = false }
}
onMounted(load)
</script>
