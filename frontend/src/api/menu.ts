import request from '@/utils/request'
import type { MenuItem } from '@/types/menu'

export interface MenuParams {
  id?: number
  path: string
  title: string
  icon?: string
  parentId?: number
  sort?: number
}

export function getMenuList() {
  return request<MenuItem[]>({
    url: '/api/menu/list',
    method: 'get'
  })
}

export function addMenu(data: MenuParams) {
  return request<void>({
    url: '/api/menu/add',
    method: 'post',
    data
  })
}

export function updateMenu(data: MenuParams) {
  return request<void>({
    url: '/api/menu/update',
    method: 'put',
    data
  })
}

export function deleteMenu(id: number) {
  return request<void>({
    url: `/api/menu/delete/${id}`,
    method: 'delete'
  })
} 