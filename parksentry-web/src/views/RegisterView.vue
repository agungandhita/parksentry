<template>
  <div class="page">
    <div class="card">

      <div class="header">
        <span class="logo">🅿</span>
        <h1>ParkSentry</h1>
        <p>Daftar Akun Baru</p>
      </div>

      <!-- Success -->
      <div v-if="success" class="alert-success" id="register-success">
        ✅ Akun <strong>{{ form.username }}</strong> berhasil dibuat.
        <RouterLink to="/login" id="btn-goto-login" class="btn-link">→ Masuk sekarang</RouterLink>
      </div>

      <!-- Error -->
      <div v-if="errorMsg && !success" class="alert-error" id="register-error">
        ⚠️ {{ errorMsg }}
      </div>

      <!-- Form -->
      <form v-if="!success" id="register-form" @submit.prevent="handleRegister">

        <!-- Role -->
        <div class="field">
          <label for="reg-role">Daftar Sebagai</label>
          <select id="reg-role" v-model="form.role" :disabled="loading">
            <option value="owner">Pemilik Kendaraan</option>
            <option value="officer">Petugas</option>
            <option value="admin">Admin</option>
          </select>
        </div>

        <!-- Full Name -->
        <div class="field">
          <label for="reg-fullname">Nama Lengkap</label>
          <input
            id="reg-fullname"
            v-model="form.fullName"
            type="text"
            placeholder="cth. Budi Santoso"
            required
            :disabled="loading"
          />
        </div>

        <!-- Username -->
        <div class="field">
          <label for="reg-username">Username</label>
          <input
            id="reg-username"
            v-model="form.username"
            type="text"
            placeholder="cth. budi.s"
            autocomplete="username"
            required
            :disabled="loading"
          />
        </div>

        <!-- Email -->
        <div class="field">
          <label for="reg-email">Email <span class="opt">(opsional)</span></label>
          <input
            id="reg-email"
            v-model="form.email"
            type="email"
            placeholder="cth. budi@email.com"
            :disabled="loading"
          />
        </div>

        <!-- Phone -->
        <div class="field">
          <label for="reg-phone">No. Telepon <span class="opt">(opsional)</span></label>
          <input
            id="reg-phone"
            v-model="form.phone"
            type="tel"
            placeholder="cth. 08xxxxxxxxxx"
            :disabled="loading"
          />
        </div>

        <!-- Owner-only fields -->
        <template v-if="form.role === 'owner'">
          <div class="field">
            <label for="reg-idcard">No. KTP <span class="opt">(opsional)</span></label>
            <input
              id="reg-idcard"
              v-model="form.idCardNo"
              type="text"
              placeholder="cth. 3517-xxxx-xxxx"
              :disabled="loading"
            />
          </div>
          <div class="field">
            <label for="reg-address">Alamat <span class="opt">(opsional)</span></label>
            <input
              id="reg-address"
              v-model="form.address"
              type="text"
              placeholder="cth. Jl. Merdeka No.1 Surabaya"
              :disabled="loading"
            />
          </div>
        </template>

        <!-- Password -->
        <div class="field">
          <label for="reg-password">Password</label>
          <div class="input-with-btn">
            <input
              id="reg-password"
              v-model="form.password"
              :type="showPass ? 'text' : 'password'"
              placeholder="Min. 6 karakter"
              autocomplete="new-password"
              required
              :disabled="loading"
            />
            <button type="button" class="eye-btn" @click="showPass = !showPass" tabindex="-1">
              {{ showPass ? '🙈' : '👁️' }}
            </button>
          </div>
        </div>

        <!-- Confirm Password -->
        <div class="field">
          <label for="reg-confirm">Konfirmasi Password</label>
          <input
            id="reg-confirm"
            v-model="form.confirmPassword"
            type="password"
            placeholder="Ulangi password"
            autocomplete="new-password"
            required
            :disabled="loading"
            :class="{ 'input-error': form.confirmPassword && !passwordsMatch }"
          />
          <span v-if="form.confirmPassword && !passwordsMatch" class="field-err">
            Password tidak cocok
          </span>
        </div>

        <!-- Submit -->
        <button
          id="btn-register-submit"
          type="submit"
          class="btn-submit"
          :disabled="loading || !canSubmit"
        >
          {{ loading ? 'Mendaftarkan...' : 'Daftar Sekarang' }}
        </button>
      </form>

      <div class="footer">
        Sudah punya akun?
        <RouterLink id="link-to-login" to="/login">Masuk di sini</RouterLink>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useAuthStore } from '../stores/auth'

const authStore = useAuthStore()

const loading  = ref(false)
const errorMsg = ref('')
const success  = ref(false)
const showPass = ref(false)

const form = ref({
  role:            'owner',
  fullName:        '',
  username:        '',
  email:           '',
  phone:           '',
  idCardNo:        '',
  address:         '',
  password:        '',
  confirmPassword: '',
})

const passwordsMatch = computed(() =>
  form.value.password === form.value.confirmPassword && form.value.confirmPassword !== ''
)

const canSubmit = computed(() =>
  form.value.fullName &&
  form.value.username &&
  form.value.password.length >= 6 &&
  passwordsMatch.value
)

async function handleRegister() {
  errorMsg.value = ''
  if (!canSubmit.value) {
    errorMsg.value = 'Lengkapi semua field dengan benar'
    return
  }
  loading.value = true
  try {
    await authStore.register({
      fullName: form.value.fullName,
      username: form.value.username,
      password: form.value.password,
      email:    form.value.email    || undefined,
      phone:    form.value.phone    || undefined,
      role:     form.value.role,
      idCardNo: form.value.idCardNo || undefined,
      address:  form.value.address  || undefined,
    })
    success.value = true
  } catch (e) {
    errorMsg.value = e.response?.data?.message || e.message || 'Registrasi gagal.'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
* { box-sizing: border-box; }

.page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #f0f2f5;
  padding: 16px;
}

.card {
  background: #fff;
  border: 1px solid #d0d7de;
  border-radius: 8px;
  padding: 32px 28px;
  width: 100%;
  max-width: 400px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.08);
}

.header {
  text-align: center;
  margin-bottom: 24px;
}
.logo {
  font-size: 32px;
  display: block;
  margin-bottom: 6px;
}
.header h1 {
  font-size: 20px;
  font-weight: 700;
  margin: 0 0 4px;
  color: #1f2328;
}
.header p {
  font-size: 13px;
  color: #656d76;
  margin: 0;
}

.alert-error {
  background: #fff0f0;
  border: 1px solid #f5c6c6;
  border-radius: 6px;
  padding: 10px 12px;
  font-size: 13px;
  color: #c62828;
  margin-bottom: 16px;
}
.alert-success {
  background: #f0fff4;
  border: 1px solid #b2dfdb;
  border-radius: 6px;
  padding: 14px 16px;
  font-size: 14px;
  color: #1b5e20;
  display: flex;
  flex-direction: column;
  gap: 10px;
}
.btn-link {
  color: #0969da;
  font-weight: 600;
  text-decoration: none;
  font-size: 14px;
}
.btn-link:hover { text-decoration: underline; }

form {
  display: flex;
  flex-direction: column;
  gap: 13px;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 5px;
}
.field label {
  font-size: 13px;
  font-weight: 600;
  color: #1f2328;
}
.opt {
  font-weight: 400;
  color: #656d76;
  font-size: 12px;
}

.field input,
.field select {
  border: 1px solid #d0d7de;
  border-radius: 6px;
  padding: 8px 10px;
  font-size: 14px;
  width: 100%;
  background: #fff;
  color: #1f2328;
  transition: border-color 0.15s;
}
.field input:focus,
.field select:focus {
  outline: none;
  border-color: #0969da;
  box-shadow: 0 0 0 3px rgba(9,105,218,0.12);
}
.field input:disabled,
.field select:disabled {
  background: #f6f8fa;
  cursor: not-allowed;
  opacity: 0.7;
}
.field input.input-error {
  border-color: #cf222e;
}

.field-err {
  font-size: 12px;
  color: #cf222e;
}

.input-with-btn {
  position: relative;
}
.input-with-btn input {
  padding-right: 38px;
}
.eye-btn {
  position: absolute;
  right: 8px;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  cursor: pointer;
  font-size: 15px;
  padding: 2px;
  line-height: 1;
}

.btn-submit {
  padding: 9px;
  background: #1f883d;
  color: #fff;
  border: none;
  border-radius: 6px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  margin-top: 4px;
  transition: background 0.15s;
}
.btn-submit:hover:not(:disabled) { background: #1a7f37; }
.btn-submit:disabled { opacity: 0.6; cursor: not-allowed; }

.footer {
  margin-top: 18px;
  text-align: center;
  font-size: 13px;
  color: #656d76;
}
.footer a {
  color: #0969da;
  text-decoration: none;
  font-weight: 500;
}
.footer a:hover { text-decoration: underline; }
</style>
