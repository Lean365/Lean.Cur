export interface MenuItem {
  id: number
  path: string
  title: string
  icon?: string
  parentId?: number
  sort?: number
  children?: MenuItem[]
}

export interface MenuState {
  menus: MenuItem[]
  selectedKeys: string[]
  openKeys: string[]
} 