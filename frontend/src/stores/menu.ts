import { defineStore } from 'pinia'
import { ref } from 'vue'
import { getMenuList } from '@/api/menu'
import type { MenuItem } from '@/types/menu'

export const useMenuStore = defineStore('menu', () => {
  const menus = ref<MenuItem[]>([])
  
  const fetchMenus = async () => {
    try {
      const response = await getMenuList()
      menus.value = response.data
    } catch (error) {
      console.error('获取菜单列表失败:', error)
    }
  }
  
  return {
    menus,
    fetchMenus
  }
}) 