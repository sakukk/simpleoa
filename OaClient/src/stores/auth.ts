import { defineStore } from 'pinia'
import api from '../utils/axios'

interface LoginPayload {
  email: string
  password: string
}

interface User {
  id: string
  email: string
  roles: string[]
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token'),
    user: null as User | null
  }),

  actions: {
    async login(payload: LoginPayload) {
      try {
        const response = await api.post('/auth/login', payload)
        this.token = response.data.token
        localStorage.setItem('token', response.data.token)
        await this.fetchUser()
        return true
      } catch (error) {
        return false
      }
    },

    async fetchUser() {
      try {
        const response = await api.get('/user/info')
        this.user = response.data
      } catch (error) {
        this.logout()
      }
    },

    logout() {
      this.token = null
      this.user = null
      localStorage.removeItem('token')
    }
  },

  getters: {
    isAuthenticated: (state) => !!state.token,
    isManager: (state) => state.user?.roles.includes('Manager'),
    isStaff: (state) => state.user?.roles.includes('Staff')
  }
}) 