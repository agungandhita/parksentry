import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const routes = [
  {
    path: '/login',
    component: () => import('../views/LoginView.vue'),
    meta: { title: 'Login', public: true }
  },
  {
    path: '/register',
    component: () => import('../views/RegisterView.vue'),
    meta: { title: 'Daftar Akun', public: true }
  },
  { path: '/', redirect: '/dashboard' },
  {
    path: '/dashboard',
    component: () => import('../views/DashboardView.vue'),
    meta: { title: 'Dashboard', requiresAuth: true }
  },
  {
    path: '/tickets',
    component: () => import('../views/TicketsView.vue'),
    meta: { title: 'Tiket Pelanggaran', requiresAuth: true }
  },
  {
    path: '/tickets/:id',
    component: () => import('../views/TicketDetailView.vue'),
    meta: { title: 'Detail Tiket', requiresAuth: true }
  },
  {
    path: '/vehicles',
    component: () => import('../views/VehiclesView.vue'),
    meta: { title: 'Kendaraan', requiresAuth: true }
  },
  {
    path: '/zones',
    component: () => import('../views/ZonesView.vue'),
    meta: { title: 'Zona Parkir', requiresAuth: true }
  },
  {
    path: '/violation-types',
    component: () => import('../views/ViolationTypesView.vue'),
    meta: { title: 'Jenis Pelanggaran', requiresAuth: true }
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to) => {
  const authStore = useAuthStore()
  if (to.meta.requiresAuth && !authStore.isLoggedIn) {
    return { path: '/login' }
  }
  if ((to.path === '/login' || to.path === '/register') && authStore.isLoggedIn) {
    return { path: '/dashboard' }
  }
})

router.afterEach((to) => {
  document.title = `${to.meta.title || 'ParkSentry'} — ParkSentry`
})

export default router
