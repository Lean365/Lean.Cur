<template>
  <a-menu
    v-model:selectedKeys="selectedKeys"
    v-model:openKeys="openKeys"
    :theme="theme"
    mode="inline"
    @click="handleMenuClick"
  >
    <template v-for="menu in menuList" :key="menu.path">
      <template v-if="!menu.children || menu.children.length === 0">
        <a-menu-item :key="menu.path">
          <template #icon>
            <component :is="menu.icon" v-if="menu.icon" />
          </template>
          <span>{{ t(menu.title) }}</span>
        </a-menu-item>
      </template>
      <template v-else>
        <a-sub-menu :key="menu.path">
          <template #icon>
            <component :is="menu.icon" v-if="menu.icon" />
          </template>
          <template #title>{{ t(menu.title) }}</template>
          <template v-for="child in menu.children" :key="child.path">
            <a-menu-item :key="child.path">
              <template #icon>
                <component :is="child.icon" v-if="child.icon" />
              </template>
              <span>{{ t(child.title) }}</span>
            </a-menu-item>
          </template>
        </a-sub-menu>
      </template>
    </template>
  </a-menu>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { useMenuStore } from '@/stores/menu'

const router = useRouter()
const route = useRoute()
const { t } = useI18n()
const menuStore = useMenuStore()

const props = defineProps({
  theme: {
    type: String,
    default: 'dark'
  }
})

const selectedKeys = ref<string[]>([])
const openKeys = ref<string[]>([])

const menuList = computed(() => menuStore.menus)

const handleMenuClick = ({ key }: { key: string }) => {
  router.push(key)
}

// 根据当前路由设置选中的菜单项
const updateSelectedKeys = () => {
  const matched = route.matched
  if (matched.length > 0) {
    const lastMatched = matched[matched.length - 1]
    selectedKeys.value = [lastMatched.path]
    
    // 设置展开的子菜单
    const parentKeys = matched
      .slice(0, -1)
      .map(item => item.path)
    openKeys.value = parentKeys
  }
}

// 监听路由变化
watch(
  () => route.path,
  () => {
    updateSelectedKeys()
  },
  { immediate: true }
)
</script> 