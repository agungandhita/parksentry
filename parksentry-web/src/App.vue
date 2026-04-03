<template>
  <!-- Layout dengan sidebar hanya jika sudah login -->
  <div v-if="authStore.isLoggedIn" class="app-layout">
    <AppSidebar />
    <div class="main-content">
      <AppHeader />
      <div class="page-container">
        <RouterView v-slot="{ Component }">
          <Transition name="fade" mode="out-in">
            <component :is="Component" />
          </Transition>
        </RouterView>
      </div>
    </div>
    <ToastContainer />
  </div>

  <!-- Layout tanpa sidebar saat di halaman login -->
  <div v-else>
    <RouterView />
  </div>
</template>

<script setup>
import { useAuthStore } from './stores/auth'
import AppSidebar from './components/AppSidebar.vue'
import AppHeader from './components/AppHeader.vue'
import ToastContainer from './components/ToastContainer.vue'

const authStore = useAuthStore()
</script>
