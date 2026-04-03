<template>
  <div>
    <div class="page-header">
      <div>
        <h1 class="page-title">Tiket Pelanggaran</h1>
        <p class="page-subtitle">Kelola semua tiket pelanggaran parkir</p>
      </div>
      <button class="btn btn-primary" @click="showModal = true">+ Tiket Baru</button>
    </div>

    <div class="card">
      <div class="toolbar">
        <div class="search-wrap">
          <span class="search-icon">🔍</span>
          <input v-model="search" class="search-input" placeholder="Cari nomor tiket, plat..." />
        </div>
        <div style="display:flex;gap:8px">
          <select v-model="filterStatus" class="form-select" style="width:150px">
            <option value="">Semua Status</option>
            <option value="Unpaid">Unpaid</option>
            <option value="Paid">Paid</option>
            <option value="Appealing">Appealing</option>
            <option value="Cancelled">Cancelled</option>
          </select>
        </div>
      </div>

      <div class="table-wrap">
        <table class="data-table">
          <thead>
            <tr>
              <th>No. Tiket</th>
              <th>Plat Nomor</th>
              <th>Pelanggaran</th>
              <th>Petugas</th>
              <th>Zona</th>
              <th>Denda</th>
              <th>Jatuh Tempo</th>
              <th>Status</th>
              <th>Aksi</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="loading"><td colspan="9" class="loading-row"><span class="spinner"></span></td></tr>
            <tr v-else-if="filtered.length === 0"><td colspan="9"><div class="empty-state"><div class="empty-icon">🎫</div><p>Tidak ada tiket ditemukan</p></div></td></tr>
            <tr v-for="t in filtered" :key="t.id">
              <td>
                <RouterLink :to="`/tickets/${t.id}`" style="color:var(--accent-primary);font-weight:700;font-size:13px">
                  {{ t.ticketNo }}
                </RouterLink>
              </td>
              <td>
                <span style="font-weight:700;background:rgba(99,102,241,0.1);padding:3px 8px;border-radius:6px;font-size:13px">
                  {{ t.vehicle?.plateNumber }}
                </span>
              </td>
              <td>
                <div style="font-size:13px;font-weight:600">{{ t.violationType?.name }}</div>
              </td>
              <td style="font-size:13px;color:var(--text-secondary)">{{ t.officer?.fullName }}</td>
              <td style="font-size:13px;color:var(--text-secondary)">{{ t.zone?.zoneName }}</td>
              <td style="font-weight:700;color:var(--accent-warning)">{{ formatRp(t.fineAmount) }}</td>
              <td style="font-size:12px" :style="isOverdue(t) ? 'color:var(--accent-danger)' : 'color:var(--text-secondary)'">
                {{ formatDate(t.dueDate) }}
                <span v-if="isOverdue(t)" style="font-size:10px;font-weight:700"> ⚠️</span>
              </td>
              <td><span class="badge" :class="`badge-${t.status?.toLowerCase()}`">{{ t.status }}</span></td>
              <td>
                <div style="display:flex;gap:6px">
                  <RouterLink :to="`/tickets/${t.id}`" class="btn btn-ghost btn-sm">Detail</RouterLink>
                  <button v-if="t.status === 'Unpaid'" class="btn btn-success btn-sm" @click="payTicket(t)">Bayar</button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Modal Tambah Tiket -->
    <Transition name="fade">
      <div v-if="showModal" class="modal-overlay" @click.self="showModal = false">
        <div class="modal">
          <div class="modal-header">
            <h3 class="modal-title">🎫 Tambah Tiket Pelanggaran</h3>
            <button class="modal-close" @click="showModal = false">✕</button>
          </div>
          <div class="modal-body">
            <div class="form-row">
              <div class="form-group">
                <label class="form-label">ID Petugas *</label>
                <input v-model.number="form.officerId" type="number" class="form-input" placeholder="contoh: 1" />
              </div>
              <div class="form-group">
                <label class="form-label">ID Kendaraan *</label>
                <input v-model.number="form.vehicleId" type="number" class="form-input" placeholder="contoh: 1" />
              </div>
            </div>
            <div class="form-row">
              <div class="form-group">
                <label class="form-label">Jenis Pelanggaran *</label>
                <select v-model.number="form.violationTypeId" class="form-select">
                  <option value="">Pilih jenis...</option>
                  <option v-for="vt in violationTypes" :key="vt.id" :value="vt.id">
                    {{ vt.code }} — {{ vt.name }} ({{ formatRp(vt.fineAmount) }})
                  </option>
                </select>
              </div>
              <div class="form-group">
                <label class="form-label">Zona Parkir *</label>
                <select v-model.number="form.zoneId" class="form-select">
                  <option value="">Pilih zona...</option>
                  <option v-for="z in zones" :key="z.id" :value="z.id">{{ z.zoneCode }} — {{ z.zoneName }}</option>
                </select>
              </div>
            </div>
            <div class="form-row">
              <div class="form-group">
                <label class="form-label">Jumlah Denda *</label>
                <input v-model.number="form.fineAmount" type="number" class="form-input" placeholder="0" />
              </div>
              <div class="form-group">
                <label class="form-label">Tanggal Pelanggaran *</label>
                <input v-model="form.violationDate" type="datetime-local" class="form-input" />
              </div>
            </div>
            <div class="form-group">
              <label class="form-label">Detail Lokasi</label>
              <input v-model="form.locationDetail" class="form-input" placeholder="Jalan, area parkir, dll..." />
            </div>
            <div class="form-group">
              <label class="form-label">Catatan</label>
              <textarea v-model="form.notes" class="form-textarea" placeholder="Catatan tambahan..."></textarea>
            </div>
          </div>
          <div class="modal-footer">
            <button class="btn btn-ghost" @click="showModal = false">Batal</button>
            <button class="btn btn-primary" @click="submitTicket" :disabled="saving">
              {{ saving ? 'Menyimpan...' : '+ Simpan Tiket' }}
            </button>
          </div>
        </div>
      </div>
    </Transition>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { ticketApi, zoneApi, violationTypeApi } from '../api'
import { useToastStore } from '../stores/toast'

const toast = useToastStore()
const tickets = ref([])
const zones = ref([])
const violationTypes = ref([])
const loading = ref(true)
const showModal = ref(false)
const saving = ref(false)
const search = ref('')
const filterStatus = ref('')

const form = ref({
  officerId: '', vehicleId: '', violationTypeId: '',
  zoneId: '', fineAmount: 0, violationDate: '',
  locationDetail: '', notes: ''
})

const filtered = computed(() =>
  tickets.value.filter(t => {
    const s = search.value.toLowerCase()
    const matchSearch = !s ||
      t.ticketNo?.toLowerCase().includes(s) ||
      t.vehicle?.plateNumber?.toLowerCase().includes(s) ||
      t.violationType?.name?.toLowerCase().includes(s)
    const matchStatus = !filterStatus.value || t.status === filterStatus.value
    return matchSearch && matchStatus
  })
)

function formatRp(val) {
  if (!val && val !== 0) return '—'
  return 'Rp ' + Number(val).toLocaleString('id-ID')
}
function formatDate(d) {
  if (!d) return '—'
  return new Date(d).toLocaleDateString('id-ID', { day: '2-digit', month: 'short', year: 'numeric' })
}
function isOverdue(t) {
  return t.status === 'Unpaid' && new Date(t.dueDate) < new Date()
}

async function payTicket(t) {
  if (!confirm(`Tandai tiket ${t.ticketNo} sebagai LUNAS?`)) return
  try {
    await ticketApi.markAsPaid(t.id)
    t.status = 'Paid'
    toast.success(`Tiket ${t.ticketNo} berhasil dilunasi`)
  } catch {
    toast.error('Gagal memperbarui status')
  }
}

async function submitTicket() {
  if (!form.value.officerId || !form.value.vehicleId || !form.value.violationTypeId || !form.value.zoneId) {
    toast.error('Harap isi semua field wajib')
    return
  }
  saving.value = true
  try {
    await ticketApi.create({
      ...form.value,
      violationDate: new Date(form.value.violationDate).toISOString()
    })
    toast.success('Tiket berhasil dibuat!')
    showModal.value = false
    await loadData()
  } catch (e) {
    toast.error('Gagal membuat tiket: ' + (e.response?.data?.title || e.message))
  } finally {
    saving.value = false
  }
}

async function loadData() {
  loading.value = true
  try {
    const [tr, zr, vtr] = await Promise.all([
      ticketApi.getAll(),
      zoneApi.getAll(),
      violationTypeApi.getAll()
    ])
    tickets.value = tr.data
    zones.value = zr.data
    violationTypes.value = vtr.data
  } finally {
    loading.value = false
  }
}

onMounted(loadData)
</script>
