<template>
  <div>
    <div class="page-header">
      <div>
        <div class="page-title">Tiket Pelanggaran</div>
        <div class="page-subtitle">Kelola semua tiket pelanggaran parkir</div>
      </div>
      <button v-if="canCreate" class="btn btn-primary" @click="showModal = true">+ Tiket Baru</button>
    </div>

    <div class="card">
      <div class="toolbar">
        <div class="search-wrap">
          <span class="search-icon">🔍</span>
          <input v-model="search" class="search-input" placeholder="Cari no. tiket, plat..." />
        </div>
        <select v-model="filterStatus" class="form-select" style="width:140px">
          <option value="">Semua Status</option>
          <option value="unpaid">Unpaid</option>
          <option value="paid">Paid</option>
          <option value="appealing">Appealing</option>
          <option value="cancelled">Cancelled</option>
        </select>
      </div>

      <div class="table-wrap">
        <table class="data-table">
          <thead>
            <tr>
              <th>No. Tiket</th>
              <th>Plat</th>
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
            <tr v-if="loading">
              <td colspan="9" class="loading-row"><span class="spinner"></span> Memuat...</td>
            </tr>
            <tr v-else-if="filtered.length === 0">
              <td colspan="9"><div class="empty-state"><div class="empty-icon">🎫</div><p>Tidak ada tiket</p></div></td>
            </tr>
            <tr v-for="t in filtered" :key="t.id">
              <td>
                <RouterLink :to="`/tickets/${t.id}`" style="color:var(--accent);font-weight:600">{{ t.ticketNo }}</RouterLink>
              </td>
              <td><strong>{{ t.vehicle?.plateNumber }}</strong></td>
              <td>{{ t.violationType?.name }}</td>
              <td style="color:var(--text-muted)">{{ t.officer?.fullName }}</td>
              <td style="color:var(--text-muted)">{{ t.zone?.zoneName }}</td>
              <td style="color:var(--accent-red);font-weight:600">{{ formatRp(t.fineAmount) }}</td>
              <td :style="isOverdue(t) ? 'color:var(--accent-red)' : ''">
                {{ formatDate(t.dueDate) }}<span v-if="isOverdue(t)"> ⚠️</span>
              </td>
              <td>
                <span class="badge" :class="`badge-${t.status?.toLowerCase()}`">{{ t.status }}</span>
              </td>
              <td>
                <div style="display:flex;gap:4px">
                  <RouterLink :to="`/tickets/${t.id}`" class="btn btn-ghost btn-sm">Detail</RouterLink>
                  <button v-if="t.status?.toLowerCase() === 'unpaid' && canPay" class="btn btn-primary btn-sm" @click="payTicket(t)">Bayar</button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Modal Buat Tiket -->
    <Transition name="fade">
      <div v-if="showModal" class="modal-overlay" @click.self="showModal = false">
        <div class="modal">
          <div class="modal-header">
            <div class="modal-title">Tambah Tiket Pelanggaran</div>
            <button class="modal-close" @click="showModal = false">✕</button>
          </div>
          <div class="modal-body">
            <div class="form-row">
              <div class="form-group">
                <label class="form-label">ID Petugas *</label>
                <input v-model.number="form.officerId" type="number" class="form-input" placeholder="1" />
              </div>
              <div class="form-group">
                <label class="form-label">ID Kendaraan *</label>
                <input v-model.number="form.vehicleId" type="number" class="form-input" placeholder="1" />
              </div>
            </div>
            <div class="form-row">
              <div class="form-group">
                <label class="form-label">Jenis Pelanggaran *</label>
                <select v-model.number="form.violationTypeId" class="form-select">
                  <option value="">Pilih...</option>
                  <option v-for="vt in violationTypes" :key="vt.id" :value="vt.id">
                    {{ vt.code }} — {{ vt.name }}
                  </option>
                </select>
              </div>
              <div class="form-group">
                <label class="form-label">Zona Parkir *</label>
                <select v-model.number="form.zoneId" class="form-select">
                  <option value="">Pilih...</option>
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
              <input v-model="form.locationDetail" class="form-input" placeholder="Jalan, area parkir..." />
            </div>
            <div class="form-group">
              <label class="form-label">Catatan</label>
              <textarea v-model="form.notes" class="form-textarea" placeholder="Catatan tambahan..."></textarea>
            </div>
          </div>
          <div class="modal-footer">
            <button class="btn btn-ghost" @click="showModal = false">Batal</button>
            <button class="btn btn-primary" @click="submitTicket" :disabled="saving">
              {{ saving ? 'Menyimpan...' : 'Simpan Tiket' }}
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
import { useAuthStore } from '../stores/auth'
import { useToastStore } from '../stores/toast'

const authStore = useAuthStore()
const toast = useToastStore()
const role = computed(() => authStore.user?.role)

const canCreate = computed(() => role.value === 'admin' || role.value === 'officer')
const canPay    = computed(() => role.value === 'admin' || role.value === 'officer')

const tickets        = ref([])
const zones          = ref([])
const violationTypes = ref([])
const loading        = ref(true)
const showModal      = ref(false)
const saving         = ref(false)
const search         = ref('')
const filterStatus   = ref('')

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
    const matchStatus = !filterStatus.value || t.status?.toLowerCase() === filterStatus.value
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
  return t.status?.toLowerCase() === 'unpaid' && new Date(t.dueDate) < new Date()
}

async function payTicket(t) {
  if (!confirm(`Tandai tiket ${t.ticketNo} sebagai LUNAS?`)) return
  try {
    await ticketApi.markAsPaid(t.id)
    t.status = 'Paid'
    toast.success(`Tiket ${t.ticketNo} lunas`)
  } catch {
    toast.error('Gagal memperbarui status')
  }
}

async function submitTicket() {
  if (!form.value.officerId || !form.value.vehicleId || !form.value.violationTypeId || !form.value.zoneId) {
    toast.error('Isi semua field wajib')
    return
  }
  saving.value = true
  try {
    await ticketApi.create({ ...form.value, violationDate: new Date(form.value.violationDate).toISOString() })
    toast.success('Tiket berhasil dibuat!')
    showModal.value = false
    await loadData()
  } catch (e) {
    toast.error('Gagal: ' + (e.response?.data?.title || e.message))
  } finally {
    saving.value = false
  }
}

async function loadData() {
  loading.value = true
  try {
    const [tr, zr, vtr] = await Promise.all([ticketApi.getAll(), zoneApi.getAll(), violationTypeApi.getAll()])
    tickets.value        = tr.data
    zones.value          = zr.data
    violationTypes.value = vtr.data
  } finally {
    loading.value = false
  }
}

onMounted(loadData)
</script>
