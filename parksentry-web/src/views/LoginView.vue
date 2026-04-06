<template>
  <div class="page">
    <div class="card">

      <!-- Logo & Title -->
      <div class="header">
        <span class="logo">🅿</span>
        <h1>ParkSentry</h1>
        <p>Sistem Manajemen Parkir</p>
      </div>

      <!-- Error -->
      <div v-if="errorMsg" class="alert-error" id="login-error">
        ⚠️ {{ errorMsg }}
      </div>

      <!-- Form -->
      <form id="login-form" @submit.prevent="handleLogin">

        <!-- Role -->
        <div class="field">
          <label for="role-select">Login Sebagai</label>
          <select id="role-select" v-model="selectedRole" :disabled="loading">
            <option value="admin">Admin</option>
            <option value="officer">Petugas</option>
            <option value="owner">Pemilik Kendaraan</option>
          </select>
        </div>

        <!-- Username -->
        <div class="field">
          <label for="username-input">Username</label>
          <input
            id="username-input"
            v-model="form.username"
            type="text"
            placeholder="Masukkan username"
            autocomplete="username"
            :disabled="loading"
            required
          />
        </div>

        <!-- Password -->
        <div class="field">
          <label for="password-input">Password</label>
          <div class="input-with-btn">
            <input
              id="password-input"
              v-model="form.password"
              :type="showPassword ? 'text' : 'password'"
              placeholder="Masukkan password"
              autocomplete="current-password"
              :disabled="loading"
              required
            />
            <button
              type="button"
              id="toggle-password"
              class="eye-btn"
              @click="showPassword = !showPassword"
              tabindex="-1"
            >{{ showPassword ? '🙈' : '👁️' }}</button>
          </div>
        </div>

        <!-- Submit -->
        <button
          id="login-submit"
          type="submit"
          class="btn-submit"
          :disabled="loading"
        >
          {{ loading ? 'Memverifikasi...' : 'Masuk' }}
        </button>
      </form>

      <div class="footer">
        Belum punya akun?
        <RouterLink id="link-to-register" to="/register">Daftar di sini</RouterLink>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router    = useRouter()
const authStore = useAuthStore()

const selectedRole  = ref('admin')
const showPassword  = ref(false)
const loading       = ref(false)
const errorMsg      = ref('')
const form          = ref({ username: '', password: '' })

async function handleLogin() {
  errorMsg.value = ''
  loading.value  = true
  try {
    await authStore.login(form.value.username, form.value.password)
    router.push('/dashboard')
  } catch (e) {
    errorMsg.value = e.response?.data?.message || e.message || 'Terjadi kesalahan.'
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
  max-width: 380px;
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

form {
  display: flex;
  flex-direction: column;
  gap: 14px;
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
