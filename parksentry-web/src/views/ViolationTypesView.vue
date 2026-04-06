<template>
  <div>
    <div class="page-header">
      <div>
        <h1 class="page-title">Jenis Pelanggaran</h1>
        <p class="page-subtitle">Daftar jenis pelanggaran dan tarif denda</p>
      </div>
      <button class="btn btn-primary" @click="showModal = true">+ Tambah Jenis</button>
    </div>
    <div class="card">
      <div class="table-wrap">
        <table class="data-table">
          <thead><tr><th>Kode</th><th>Nama Pelanggaran</th><th>Deskripsi</th><th>Tarif Denda</th><th>Aksi</th></tr></thead>
          <tbody>
            <tr v-if="loading"><td colspan="5" class="loading-row"><span class="spinner"></span></td></tr>
            <tr v-else-if="types.length === 0"><td colspan="5"><div class="empty-state"><div class="empty-icon">⚠️</div><p>Belum ada jenis pelanggaran</p></div></td></tr>
            <tr v-for="t in types" :key="t.id">
              <td><strong>{{ t.code }}</strong></td>
              <td>{{ t.name }}</td>
              <td style="color:var(--text-muted)">{{ t.description || '—' }}</td>
              <td style="font-weight:700;color:var(--accent-red)">{{ formatRp(t.fineAmount) }}</td>
              <td><button class="btn btn-danger btn-sm" @click="del(t)">Hapus</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <Transition name="fade">
      <div v-if="showModal" class="modal-overlay" @click.self="showModal = false">
        <div class="modal">
          <div class="modal-header"><h3 class="modal-title">⚠️ Tambah Jenis Pelanggaran</h3><button class="modal-close" @click="showModal = false">✕</button></div>
          <div class="modal-body">
            <div class="form-row">
              <div class="form-group"><label class="form-label">Kode *</label><input v-model="form.code" class="form-input" placeholder="PKR-001" /></div>
              <div class="form-group"><label class="form-label">Tarif Denda (Rp) *</label><input v-model.number="form.fineAmount" type="number" class="form-input" placeholder="500000" /></div>
            </div>
            <div class="form-group"><label class="form-label">Nama Pelanggaran *</label><input v-model="form.name" class="form-input" placeholder="Parkir di bahu jalan..." /></div>
            <div class="form-group"><label class="form-label">Deskripsi</label><textarea v-model="form.description" class="form-textarea" placeholder="Deskripsi lengkap..."></textarea></div>
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
import { violationTypeApi } from '../api'
import { useToastStore } from '../stores/toast'

const toast = useToastStore()
const types = ref([])
const loading = ref(true)
const showModal = ref(false)
const saving = ref(false)
const form = ref({ code: '', name: '', fineAmount: 0, description: '' })

function formatRp(val) { return 'Rp ' + Number(val || 0).toLocaleString('id-ID') }

async function submit() {
  if (!form.value.code || !form.value.name) { toast.error('Kode dan nama wajib diisi'); return }
  saving.value = true
  try {
    await violationTypeApi.create(form.value)
    toast.success('Jenis pelanggaran berhasil ditambahkan')
    showModal.value = false
    form.value = { code: '', name: '', fineAmount: 0, description: '' }
    await load()
  } catch (e) { toast.error('Gagal: ' + (e.response?.data?.title || e.message)) }
  finally { saving.value = false }
}

async function del(t) {
  if (!confirm(`Hapus jenis pelanggaran "${t.name}"?`)) return
  try { await violationTypeApi.delete(t.id); types.value = types.value.filter(x => x.id !== t.id); toast.success('Berhasil dihapus') }
  catch { toast.error('Gagal menghapus') }
}

async function load() {
  loading.value = true
  try { const r = await violationTypeApi.getAll(); types.value = r.data }
  finally { loading.value = false }
}
onMounted(load)
</script>
