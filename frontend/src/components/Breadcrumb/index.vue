<template>
  <a-breadcrumb>
    <template v-for="item in breadcrumbItems" :key="item.path">
      <a-breadcrumb-item>
        <router-link v-if="item.path" :to="item.path">
          <component v-if="item.icon" :is="item.icon" />
          <span>{{ $t(item.title) }}</span>
        </router-link>
        <template v-else>
          <component v-if="item.icon" :is="item.icon" />
          <span>{{ $t(item.title) }}</span>
        </template>
      </a-breadcrumb-item>
    </template>
  </a-breadcrumb>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import { HomeOutlined } from '@ant-design/icons-vue';

interface BreadcrumbItem {
  title: string;
  path?: string;
  icon?: any;
}

const route = useRoute();

const breadcrumbItems = computed<BreadcrumbItem[]>(() => {
  const items: BreadcrumbItem[] = [
    {
      title: 'menu.home',
      path: '/',
      icon: HomeOutlined,
    },
  ];

  const matched = route.matched.filter(item => item.meta?.title);
  matched.forEach(item => {
    items.push({
      title: item.meta.title as string,
      path: item.path,
      icon: item.meta.icon,
    });
  });

  return items;
});
</script>

<style lang="less" scoped>
:deep(.ant-breadcrumb) {
  .anticon {
    margin-right: 4px;
  }
}
</style> 