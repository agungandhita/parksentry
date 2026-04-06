<template>
  <div>
    <div class="page-header">
      <div>
        <button class="btn btn-ghost btn-sm" @click="$router.back()" style="margin-bottom:12px">← Kembali</button>
        <h1 class="page-title">Detail Tiket</h1>
      </div>
      <div style="display:flex;gap:8px" v-if="ticket">
        <button v-if="ticket.status === 'Unpaid'" class="btn btn-success" @click="pay">✅ Tandai Lunas</button>
        <button v-if="ticket.status === 'Unpaid'" class="btn btn-danger" @click="cancel">❌ Batalkan</button>
      </div>
    </div>

    <div v-if="loading" style="text-align:center;padding:80px;color:var(--text-muted)">
      <span class="spinner"></span> Memuat...
    </div>

    <div v-else-if="ticket" style="display:grid;grid-template-columns:2fr 1fr;gap:20px">
      <!-- Main Info -->
      <div style="display:flex;flex-direction:column;gap:20px">
        <div class="card">
          <div style="display:flex;align-items:center;justify-content:space-between;margin-bottom:20px">
            <h3 style="font-size:20px;font-weight:800">{{ ticket.ticketNo }}</h3>
            <span class="badge" :class="`badge-${ticket.status?.toLowerCase()}`" style="font-size:13px;padding:6px 14px">{{ ticket.status }}</span>
          </div>
          <div style="display:grid;grid-template-columns:1fr 1fr;gap:16px">
            <div class="info-item">
              <div class="info-label">Tanggal Pelanggaran</div>
              <div class="info-value">{{ formatDate(ticket.violationDate) }}</div>
            </div>
            <div class="info-item">
              <div class="info-label">Jatuh Tempo</div>
              <div class="info-value" :style="isOverdue ? 'color:var(--accent-danger)' : ''">
                {{ formatDate(ticket.dueDate) }}
                <span v-if="isOverdue"> ⚠️ Lewat Jatuh Tempo</span>
              </div>
            </div>
            <div class="info-item">
              <div class="info-label">Jumlah Denda</div>
              <div class="info-value" style="font-size:22px;font-weight:800;color:var(--accent-warning)">
                {{ formatRp(ticket.fineAmount) }}
              </div>
            </div>
            <div class="info-item">
              <div class="info-label">Lokasi</div>
              <div class="info-value">{{ ticket.locationDetail || '—' }}</div>
            </div>
          </div>
          <div v-if="ticket.notes" style="margin-top:16px;padding:12px;background:#f6f8fa;border-radius:var(--radius);border:1px solid var(--border)">
            <div style="font-size:11px;color:var(--text-muted);margin-bottom:4px">CATATAN</div>
            <div style="font-size:14px">{{ ticket.notes }}</div>
          </div>
        </div>

        <!-- Violation Detail -->
        <div class="card">
          <h4 style="font-size:16px;font-weight:700;margin-bottom:16px">⚠️ Jenis Pelanggaran</h4>
          <div style="display:grid;grid-template-columns:1fr 1fr;gap:16px">
            <div class="info-item"><div class="info-label">Kode</div><div class="info-value" style="color:var(--accent)">{{ ticket.violationType?.code }}</div></div>
            <div class="info-item"><div class="info-label">Nama Pelanggaran</div><div class="info-value">{{ ticket.violationType?.name }}</div></div>
            <div class="info-item"><div class="info-label">Zona</div><div class="info-value">{{ ticket.zone?.zoneName }}</div></div>
            <div class="info-item"><div class="info-label">Alamat Zona</div><div class="info-value" style="color:var(--text-muted)">{{ ticket.zone?.address || '—' }}</div></div>
          </div>
        </div>
      </div>

      <!-- Side Info -->
      <div style="display:flex;flex-direction:column;gap:20px">
        <!-- Vehicle -->
        <div class="card">
          <h4 style="font-size:16px;font-weight:700;margin-bottom:16px">🚗 Kendaraan</h4>
          <div class="plate-badge">{{ ticket.vehicle?.plateNumber }}</div>
          <div style="display:flex;flex-direction:column;gap:12px;margin-top:16px">
            <div class="info-item"><div class="info-label">Merek / Model</div><div class="info-value">{{ ticket.vehicle?.brand }} {{ ticket.vehicle?.model }}</div></div>
            <div class="info-item"><div class="info-label">Warna</div><div class="info-value">{{ ticket.vehicle?.color }}</div></div>
          </div>
        </div>

        <!-- Owner -->
        <div class="card">
          <h4 style="font-size:16px;font-weight:700;margin-bottom:16px">👤 Pemilik Kendaraan</h4>
          <div style="display:flex;flex-direction:column;gap:12px">
            <div class="info-item"><div class="info-label">Nama</div><div class="info-value">{{ ticket.owner?.fullName }}</div></div>
            <div class="info-item"><div class="info-label">No. KTP</div><div class="info-value" style="color:var(--text-secondary)">{{ ticket.owner?.idCardNo }}</div></div>
            <div class="info-item"><div class="info-label">Telepon</div><div class="info-value">{{ ticket.owner?.phone || '—' }}</div></div>
          </div>
        </div>

        <!-- Officer -->
        <div class="card">
          <h4 style="font-size:16px;font-weight:700;margin-bottom:16px">👮 Petugas</h4>
          <div style="display:flex;flex-direction:column;gap:12px">
            <div class="info-item"><div class="info-label">Nama</div><div class="info-value">{{ ticket.officer?.fullName }}</div></div>
            <div class="info-item"><div class="info-label">No. Badge</div><div class="info-value" style="color:var(--accent)">{{ ticket.officer?.badgeNo }}</div></div>
            <div class="info-item"><div class="info-label">Zona Tugas</div><div class="info-value">{{ ticket.officer?.zone || '—' }}</div></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ticketApi } from '../api'
import { useToastStore } from '../stores/toast'

const route = useRoute()
const router = useRouter()
const toast = useToastStore()
const ticket = ref(null)
const loading = ref(true)

const isOverdue = computed(() =>
  ticket.value?.status === 'Unpaid' && new Date(ticket.value?.dueDate) < new Date()
)

function formatRp(val) {
  return 'Rp ' + Number(val || 0).toLocaleString('id-ID')
}
function formatDate(d) {
  if (!d) return '—'
  return new Date(d).toLocaleDateString('id-ID', { day: '2-digit', month: 'long', year: 'numeric', hour: '2-digit', minute: '2-digit' })
}

async function pay() {
  if (!confirm('Tandai tiket ini sebagai LUNAS?')) return
  try {
    await ticketApi.markAsPaid(ticket.value.id)
    ticket.value.status = 'Paid'
    toast.success('Tiket berhasil dilunasi!')
  } catch { toast.error('Gagal memperbarui status') }
}

async function cancel() {
  if (!confirm('Batalkan tiket ini?')) return
  try {
    await ticketApi.cancel(ticket.value.id)
    ticket.value.status = 'Cancelled'
    toast.success('Tiket berhasil dibatalkan')
  } catch { toast.error('Gagal membatalkan tiket') }
}

onMounted(async () => {
  try {
    const res = await ticketApi.getById(route.params.id)
    ticket.value = res.data
  } catch { router.push('/tickets') }
  finally { loading.value = false }
})
</script>

<style scoped>
.info-item { display: flex; flex-direction: column; gap: 4px; }
.info-label { font-size: 11px; font-weight: 700; text-transform: uppercase; letter-spacing: 0.5px; color: var(--text-muted); }
.info-value { font-size: 14px; font-weight: 600; color: var(--text); }
.plate-badge {
  background: #dbeafe;
  color: var(--accent); font-size: 20px; font-weight: 800;
  text-align: center; padding: 12px; border-radius: var(--radius);
  letter-spacing: 3px; border: 2px solid #bee3f8;
}
</style>
