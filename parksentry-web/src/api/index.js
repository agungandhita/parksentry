import axios from 'axios'

const api = axios.create({
  baseURL: '/api',
  headers: { 'Content-Type': 'application/json' }
})

export const authApi = {
  login:    (data) => api.post('/auth/login',    data),
  register: (data) => api.post('/auth/register', data),
}

export const ticketApi = {
  getAll: (status) => api.get('/violationtickets', { params: status ? { status } : {} }),
  getById: (id) => api.get(`/violationtickets/${id}`),
  create: (data) => api.post('/violationtickets', data),
  markAsPaid: (id) => api.patch(`/violationtickets/${id}/pay`),
  cancel: (id) => api.patch(`/violationtickets/${id}/cancel`),
  getStats: () => api.get('/violationtickets/stats'),
}

export const vehicleApi = {
  getAll: () => api.get('/vehicles'),
  getById: (id) => api.get(`/vehicles/${id}`),
  getByPlate: (plate) => api.get(`/vehicles/plate/${plate}`),
}

export const zoneApi = {
  getAll: () => api.get('/parkingzones'),
  getById: (id) => api.get(`/parkingzones/${id}`),
  create: (data) => api.post('/parkingzones', data),
  delete: (id) => api.delete(`/parkingzones/${id}`),
}

export const violationTypeApi = {
  getAll: () => api.get('/violationtypes'),
  getById: (id) => api.get(`/violationtypes/${id}`),
  create: (data) => api.post('/violationtypes', data),
  delete: (id) => api.delete(`/violationtypes/${id}`),
}

export const roleApi = {
  getAll: () => api.get('/roles'),
  create: (data) => api.post('/roles', data),
  delete: (id) => api.delete(`/roles/${id}`),
}

export default api
