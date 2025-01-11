<template>
  <a-layout class="layout">
    <a-layout-sider
      v-model:collapsed="collapsed"
      :trigger="null"
      collapsible
      class="sider"
    >
      <div class="logo">
        <img :src="logoSrc" alt="logo" />
      </div>
      <Menu theme="dark" />
    </a-layout-sider>
    
    <a-layout>
      <a-layout-header class="header">
        <div class="header-left">
          <menu-unfold-outlined
            v-if="collapsed"
            class="action-icon"
            @click="() => (collapsed = !collapsed)"
          />
          <menu-fold-outlined
            v-else
            class="action-icon"
            @click="() => (collapsed = !collapsed)"
          />
          <a-breadcrumb>
            <a-breadcrumb-item v-for="item in breadcrumbItems" :key="item.path">
              {{ item.title }}
            </a-breadcrumb-item>
          </a-breadcrumb>
        </div>
        <div class="header-right">
          <locale-picker class="action-icon" />
          <theme-picker class="action-icon" />
          <a-dropdown placement="bottomRight">
            <span class="action-icon">
              <user-outlined />
            </span>
            <template #overlay>
              <a-menu>
                <a-menu-item key="profile" @click="router.push({ name: 'Profile' })">
                  <template #icon><user-outlined /></template>
                  <span>{{ $t('app.menu.profile') }}</span>
                </a-menu-item>
                <a-menu-item key="settings" @click="router.push({ name: 'Settings' })">
                  <template #icon><setting-outlined /></template>
                  <span>{{ $t('app.menu.settings') }}</span>
                </a-menu-item>
                <a-menu-item key="logout" @click="handleLogout">
                  <template #icon><logout-outlined /></template>
                  <span>{{ $t('app.menu.logout') }}</span>
                </a-menu-item>
              </a-menu>
            </template>
          </a-dropdown>
        </div>
      </a-layout-header>
      
      <a-layout-content class="content">
        <router-view></router-view>
      </a-layout-content>
    </a-layout>
  </a-layout>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { useAppStore } from '@/stores/app'
import { useUserStore } from '@/stores/user'
import { useMenuStore } from '@/stores/menu'
import { 
  MenuUnfoldOutlined,
  MenuFoldOutlined,
  UserOutlined,
  LogoutOutlined,
  SettingOutlined
} from '@ant-design/icons-vue'
import Menu from '@/components/Menu/index.vue'
import LocalePicker from '@/components/LocalePicker/index.vue'
import ThemePicker from '@/components/ThemePicker/index.vue'
import LogoLight from '@/assets/images/logo/logo.svg'
import LogoDark from '@/assets/images/logo/logo-dark.svg'

const router = useRouter()
const route = useRoute()
const { t } = useI18n()
const appStore = useAppStore()
const userStore = useUserStore()
const menuStore = useMenuStore()

const collapsed = ref(false)

const logoSrc = computed(() => {
  const theme = appStore.theme
  return theme === 'dark' ? LogoDark : LogoLight
})

const breadcrumbItems = computed(() => {
  return route.matched.map(item => {
    const title = item.meta?.title
    return {
      title: title ? t(title) : '',
      path: item.path
    }
  })
})

const handleLogout = async () => {
  await userStore.logout()
  router.push('/login')
}

onMounted(() => {
  menuStore.fetchMenus()
})
</script>

<style scoped lang="less">
.layout {
  min-height: 100vh;
}

.sider {
  .logo {
    height: 64px;
    padding: 16px;
    text-align: center;
    
    img {
      height: 32px;
    }
  }
}

.header {
  padding: 0 24px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: var(--ant-layout-header-background);
  
  .header-left {
    display: flex;
    align-items: center;
    gap: 12px;
  }
  
  .header-right {
    display: flex;
    align-items: center;
    gap: 8px;
  }
  
  .action-icon {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    width: 32px;
    height: 32px;
    cursor: pointer;
    color: var(--ant-text-color);
    
    &:hover {
      color: var(--ant-primary-color);
    }

    :deep(.anticon) {
      font-size: 18px;
      line-height: 1;
    }

    :deep(.svg-icon) {
      width: 18px;
      height: 18px;
      line-height: 1;
      vertical-align: 0;
    }
  }
}

:deep(.ant-dropdown-menu-item) {
  display: flex;
  align-items: center;
  gap: 12px;
  min-width: 160px;
  padding: 10px 16px;

  .svg-icon {
    font-size: 16px;
  }
}

.content {
  margin: 24px;
  padding: 24px;
  background: var(--ant-component-background);
}
</style> 