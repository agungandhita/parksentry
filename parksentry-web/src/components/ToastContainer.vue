<template>
  <Teleport to="body">
    <div class="toast-container">
      <TransitionGroup name="toast-anim">
        <div v-for="toast in toastStore.toasts" :key="toast.id" class="toast" :class="toast.type">
          <span>{{ toastIcon(toast.type) }}</span>
          <span>{{ toast.message }}</span>
        </div>
      </TransitionGroup>
    </div>
  </Teleport>
</template>

<script setup>
import { useToastStore } from '../stores/toast'
const toastStore = useToastStore()

function toastIcon(type) {
  return { success: '✅', error: '❌', info: 'ℹ️' }[type] || 'ℹ️'
}
</script>

<style scoped>
.toast-anim-enter-active, .toast-anim-leave-active { transition: all 0.3s ease; }
.toast-anim-enter-from { transform: translateX(100%); opacity: 0; }
.toast-anim-leave-to { transform: translateX(100%); opacity: 0; }
</style>
