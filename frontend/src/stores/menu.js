import { defineStore } from 'pinia'
import { ref } from 'vue'
import { getMenuList } from '@/api/menu'

interface MenuItem {
  path: string
  title: string
  icon?: string
  children?: MenuItem[]
}

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