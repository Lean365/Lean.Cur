import { http } from '@/utils/http'
import type { LeanNotice } from './types'

const baseUrl = '/api/leanNotice'

export const getNoticeList = (params?: any) => {
  return http.get<LeanNotice[]>(baseUrl, { params })
}

export const getNotice = (id: number) => {
  return http.get<LeanNotice>(`${baseUrl}/${id}`)
}

export const createNotice = (data: LeanNotice) => {
  return http.post<LeanNotice>(baseUrl, data)
}

export const updateNotice = (data: LeanNotice) => {
  return http.put<LeanNotice>(baseUrl, data)
}

export const deleteNotice = (id: number) => {
  return http.delete(`${baseUrl}/${id}`)
} 