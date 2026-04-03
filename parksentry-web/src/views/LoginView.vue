<template>
  <div class="login-page">
    <!-- Animated background -->
    <div class="bg-orbs">
      <div class="orb orb-1"></div>
      <div class="orb orb-2"></div>
      <div class="orb orb-3"></div>
    </div>

    <!-- Grid pattern overlay -->
    <div class="grid-overlay"></div>

    <div class="login-wrapper">
      <!-- Left Panel: Branding -->
      <div class="brand-panel">
        <div class="brand-content">
          <div class="brand-logo">
            <span class="logo-icon">🅿</span>
          </div>
          <h1 class="brand-name">ParkSentry</h1>
          <p class="brand-tagline">Sistem Manajemen Pelanggaran Parkir</p>

          <div class="brand-features">
            <div class="feature-item">
              <div class="feature-icon">🎫</div>
              <div>
                <div class="feature-title">Tiket Digital</div>
                <div class="feature-desc">Buat & kelola tiket pelanggaran secara real-time</div>
              </div>
            </div>
            <div class="feature-item">
              <div class="feature-icon">📊</div>
              <div>
                <div class="feature-title">Dashboard Analytics</div>
                <div class="feature-desc">Pantau statistik pelanggaran & pendapatan</div>
              </div>
            </div>
            <div class="feature-item">
              <div class="feature-icon">🗺️</div>
              <div>
                <div class="feature-title">Zona Parkir</div>
                <div class="feature-desc">Kelola ratusan zona parkir dengan mudah</div>
              </div>
            </div>
          </div>

          <div class="brand-badge">
            <span class="badge-dot"></span>
            Sistem Aktif & Beroperasi
          </div>
        </div>
      </div>

      <!-- Right Panel: Login Form -->
      <div class="form-panel">
        <div class="login-card">
          <div class="login-header">
            <h2>Selamat Datang</h2>
            <p>Masuk ke akun ParkSentry Anda</p>
          </div>

          <!-- Role Switcher -->
          <div class="role-switcher">
            <button
              id="role-admin"
              class="role-btn"
              :class="{ active: selectedRole === 'Admin' }"
              @click="selectedRole = 'Admin'"
            >
              <span class="role-emoji">👑</span>
              <span>Admin</span>
            </button>
            <button
              id="role-officer"
              class="role-btn"
              :class="{ active: selectedRole === 'Officer' }"
              @click="selectedRole = 'Officer'"
            >
              <span class="role-emoji">👮</span>
              <span>Petugas</span>
            </button>
          </div>

          <!-- Login Form -->
          <form id="login-form" @submit.prevent="handleLogin" class="login-form">
            <!-- Error message -->
            <Transition name="shake">
              <div v-if="errorMsg" class="error-banner" id="login-error">
                <span>⚠️</span> {{ errorMsg }}
              </div>
            </Transition>

            <div class="form-group">
              <label class="form-label" for="username-input">Username</label>
              <div class="input-wrap">
                <span class="input-icon">👤</span>
                <input
                  id="username-input"
                  v-model="form.username"
                  type="text"
                  class="form-input with-icon"
                  placeholder="Masukkan username"
                  autocomplete="username"
                  :disabled="loading"
                  required
                />
              </div>
            </div>

            <div class="form-group">
              <label class="form-label" for="password-input">Password</label>
              <div class="input-wrap">
                <span class="input-icon">🔒</span>
                <input
                  id="password-input"
                  v-model="form.password"
                  :type="showPassword ? 'text' : 'password'"
                  class="form-input with-icon"
                  placeholder="Masukkan password"
                  autocomplete="current-password"
                  :disabled="loading"
                  required
                />
                <button
                  type="button"
                  class="toggle-pass"
                  id="toggle-password"
                  @click="showPassword = !showPassword"
                  :title="showPassword ? 'Sembunyikan' : 'Tampilkan'"
                >
                  {{ showPassword ? '🙈' : '👁️' }}
                </button>
              </div>
            </div>

            <button
              id="login-submit"
              type="submit"
              class="btn-login"
              :class="{ loading }"
              :disabled="loading"
            >
              <span v-if="loading" class="btn-spinner"></span>
              <span v-else>🔐</span>
              {{ loading ? 'Memverifikasi...' : 'Masuk' }}
            </button>
          </form>

          <!-- Demo credentials hint -->
          <div class="demo-hint">
            <div class="hint-title">🧪 Akun Demo</div>
            <div v-if="selectedRole === 'Admin'" class="hint-creds">
              <code>admin</code> / <code>admin123</code>
            </div>
            <div v-else class="hint-creds">
              <code>officer1</code> / <code>officer123</code>
            </div>
          </div>
        </div>

        <div class="login-footer">
          © {{ new Date().getFullYear() }} ParkSentry — Sistem Manajemen Parkir
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const authStore = useAuthStore()

const selectedRole = ref('Admin')
const showPassword = ref(false)
const loading = ref(false)
const errorMsg = ref('')
const form = ref({ username: '', password: '' })

async function handleLogin() {
  errorMsg.value = ''
  loading.value = true
  try {
    await authStore.login(form.value.username, form.value.password, selectedRole.value)
    router.push('/dashboard')
  } catch (e) {
    errorMsg.value = e.message || 'Terjadi kesalahan. Coba lagi.'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
/* ── Page ─────────────────────────────────────────────────────── */
.login-page {
  min-height: 100vh;
  background: #070c18;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  overflow: hidden;
}

/* ── Animated orbs background ─────────────────────────────────── */
.bg-orbs { position: absolute; inset: 0; pointer-events: none; }
.orb {
  position: absolute;
  border-radius: 50%;
  filter: blur(80px);
  opacity: 0.25;
  animation: float 8s ease-in-out infinite;
}
.orb-1 {
  width: 500px; height: 500px;
  background: radial-gradient(circle, #6366f1, transparent);
  top: -150px; left: -100px;
  animation-delay: 0s;
}
.orb-2 {
  width: 400px; height: 400px;
  background: radial-gradient(circle, #8b5cf6, transparent);
  bottom: -100px; right: -80px;
  animation-delay: -3s;
}
.orb-3 {
  width: 300px; height: 300px;
  background: radial-gradient(circle, #3b82f6, transparent);
  top: 50%; left: 50%;
  animation-delay: -5s;
}
@keyframes float {
  0%, 100% { transform: translate(0, 0) scale(1); }
  33%       { transform: translate(20px, -30px) scale(1.05); }
  66%       { transform: translate(-15px, 20px) scale(0.95); }
}

/* ── Grid overlay ──────────────────────────────────────────────── */
.grid-overlay {
  position: absolute; inset: 0;
  background-image:
    linear-gradient(rgba(99,102,241,0.04) 1px, transparent 1px),
    linear-gradient(90deg, rgba(99,102,241,0.04) 1px, transparent 1px);
  background-size: 40px 40px;
  pointer-events: none;
}

/* ── Wrapper ───────────────────────────────────────────────────── */
.login-wrapper {
  display: flex;
  min-height: 100vh;
  width: 100%;
  position: relative;
  z-index: 1;
}

/* ── Brand Panel ───────────────────────────────────────────────── */
.brand-panel {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 60px 48px;
  background: linear-gradient(135deg, rgba(99,102,241,0.08), rgba(139,92,246,0.06));
  border-right: 1px solid rgba(99,102,241,0.15);
}
.brand-content { max-width: 420px; }

.brand-logo {
  width: 72px; height: 72px;
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  border-radius: 20px;
  display: flex; align-items: center; justify-content: center;
  font-size: 36px;
  margin-bottom: 24px;
  box-shadow: 0 8px 32px rgba(99,102,241,0.4);
  animation: logoFloat 4s ease-in-out infinite;
}
@keyframes logoFloat {
  0%, 100% { transform: translateY(0); }
  50%       { transform: translateY(-8px); }
}

.brand-name {
  font-size: 40px; font-weight: 900;
  background: linear-gradient(135deg, #a5b4fc, #c4b5fd, #f1f5f9);
  -webkit-background-clip: text; -webkit-text-fill-color: transparent;
  background-clip: text;
  margin-bottom: 8px;
}
.brand-tagline {
  font-size: 15px; color: #64748b;
  margin-bottom: 48px;
  line-height: 1.5;
}

.brand-features { display: flex; flex-direction: column; gap: 24px; margin-bottom: 48px; }
.feature-item {
  display: flex; align-items: flex-start; gap: 16px;
}
.feature-icon {
  width: 44px; height: 44px; flex-shrink: 0;
  background: rgba(99,102,241,0.12);
  border: 1px solid rgba(99,102,241,0.2);
  border-radius: 12px;
  display: flex; align-items: center; justify-content: center;
  font-size: 20px;
}
.feature-title { font-size: 14px; font-weight: 700; color: #e2e8f0; margin-bottom: 3px; }
.feature-desc { font-size: 13px; color: #475569; line-height: 1.4; }

.brand-badge {
  display: inline-flex; align-items: center; gap: 8px;
  padding: 8px 16px;
  background: rgba(16,185,129,0.1);
  border: 1px solid rgba(16,185,129,0.2);
  border-radius: 20px;
  font-size: 13px; font-weight: 600; color: #34d399;
}
.badge-dot {
  width: 8px; height: 8px; border-radius: 50%;
  background: #10b981;
  animation: pulse 2s ease-in-out infinite;
}
@keyframes pulse {
  0%, 100% { transform: scale(1); opacity: 1; }
  50%       { transform: scale(1.4); opacity: 0.6; }
}

/* ── Form Panel ────────────────────────────────────────────────── */
.form-panel {
  width: 480px; flex-shrink: 0;
  display: flex; flex-direction: column;
  align-items: center; justify-content: center;
  padding: 48px 40px;
  background: rgba(10,14,26,0.7);
  backdrop-filter: blur(20px);
}

.login-card {
  width: 100%; max-width: 400px;
  background: rgba(26,34,53,0.8);
  border: 1px solid rgba(30,45,69,0.8);
  border-radius: 20px;
  padding: 36px;
  box-shadow: 0 24px 64px rgba(0,0,0,0.5);
  backdrop-filter: blur(10px);
}

/* ── Login Header ──────────────────────────────────────────────── */
.login-header { text-align: center; margin-bottom: 28px; }
.login-header h2 {
  font-size: 26px; font-weight: 800;
  color: #f1f5f9; margin-bottom: 6px;
}
.login-header p { font-size: 14px; color: #64748b; }

/* ── Role Switcher ─────────────────────────────────────────────── */
.role-switcher {
  display: grid; grid-template-columns: 1fr 1fr;
  gap: 8px; margin-bottom: 28px;
  background: rgba(7,12,24,0.6);
  border: 1px solid #1e2d45;
  border-radius: 12px;
  padding: 6px;
}
.role-btn {
  display: flex; align-items: center; justify-content: center; gap: 8px;
  padding: 10px 16px;
  border-radius: 8px;
  border: none; cursor: pointer;
  font-size: 14px; font-weight: 600;
  color: #475569;
  background: transparent;
  transition: all 0.25s ease;
}
.role-btn:hover { color: #94a3b8; background: rgba(255,255,255,0.04); }
.role-btn.active {
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  color: white;
  box-shadow: 0 4px 12px rgba(99,102,241,0.35);
}
.role-emoji { font-size: 18px; }

/* ── Form ──────────────────────────────────────────────────────── */
.login-form { display: flex; flex-direction: column; gap: 18px; }

.error-banner {
  background: rgba(239,68,68,0.1);
  border: 1px solid rgba(239,68,68,0.3);
  border-radius: 8px;
  padding: 10px 14px;
  font-size: 13px; font-weight: 500;
  color: #f87171;
  display: flex; align-items: center; gap: 8px;
}

.form-group { display: flex; flex-direction: column; gap: 7px; }
.form-label { font-size: 12px; font-weight: 700; color: #64748b; text-transform: uppercase; letter-spacing: 0.6px; }

.input-wrap { position: relative; }
.input-icon {
  position: absolute; left: 14px; top: 50%; transform: translateY(-50%);
  font-size: 16px; pointer-events: none;
}
.form-input {
  width: 100%;
  background: rgba(7,12,24,0.7);
  border: 1px solid #1e2d45;
  border-radius: 10px;
  color: #f1f5f9;
  padding: 12px 14px;
  font-size: 14px; font-family: inherit;
  transition: all 0.2s ease;
}
.form-input.with-icon { padding-left: 44px; }
.form-input:focus {
  outline: none;
  border-color: #6366f1;
  box-shadow: 0 0 0 3px rgba(99,102,241,0.15);
  background: rgba(99,102,241,0.04);
}
.form-input::placeholder { color: #334155; }
.form-input:disabled { opacity: 0.5; cursor: not-allowed; }

.toggle-pass {
  position: absolute; right: 12px; top: 50%; transform: translateY(-50%);
  background: none; border: none; cursor: pointer; font-size: 16px;
  padding: 4px; border-radius: 4px;
  transition: opacity 0.2s;
}
.toggle-pass:hover { opacity: 0.7; }

/* ── Submit button ─────────────────────────────────────────────── */
.btn-login {
  width: 100%; padding: 14px;
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  color: white; border: none; border-radius: 10px;
  font-size: 15px; font-weight: 700; cursor: pointer;
  display: flex; align-items: center; justify-content: center; gap: 10px;
  transition: all 0.25s ease;
  margin-top: 4px;
  box-shadow: 0 4px 20px rgba(99,102,241,0.3);
}
.btn-login:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 8px 28px rgba(99,102,241,0.45);
}
.btn-login:active:not(:disabled) { transform: translateY(0); }
.btn-login:disabled { opacity: 0.6; cursor: not-allowed; transform: none !important; }
.btn-login.loading { background: linear-gradient(135deg, #4f52d9, #7c3fd8); }

.btn-spinner {
  width: 18px; height: 18px;
  border: 2px solid rgba(255,255,255,0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 0.7s linear infinite;
  flex-shrink: 0;
}
@keyframes spin { to { transform: rotate(360deg); } }

/* ── Demo hint ─────────────────────────────────────────────────── */
.demo-hint {
  margin-top: 20px;
  background: rgba(59,130,246,0.06);
  border: 1px solid rgba(59,130,246,0.15);
  border-radius: 10px;
  padding: 12px 16px;
  text-align: center;
}
.hint-title { font-size: 12px; font-weight: 700; color: #60a5fa; margin-bottom: 6px; }
.hint-creds { font-size: 13px; color: #64748b; }
.hint-creds code {
  background: rgba(255,255,255,0.06); border-radius: 4px;
  padding: 2px 6px; color: #94a3b8; font-size: 12px;
}

/* ── Footer ────────────────────────────────────────────────────── */
.login-footer {
  margin-top: 24px;
  font-size: 12px; color: #334155;
  text-align: center;
}

/* ── Transitions ───────────────────────────────────────────────── */
.shake-enter-active { animation: shakeIn 0.4s ease; }
@keyframes shakeIn {
  0%   { transform: translateX(-6px); opacity: 0; }
  30%  { transform: translateX(6px); }
  60%  { transform: translateX(-3px); }
  100% { transform: translateX(0); opacity: 1; }
}

/* ── Responsive ────────────────────────────────────────────────── */
@media (max-width: 900px) {
  .brand-panel { display: none; }
  .form-panel { width: 100%; min-height: 100vh; padding: 32px 24px; }
}
</style>
