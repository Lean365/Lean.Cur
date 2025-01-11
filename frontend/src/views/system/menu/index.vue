<template>
  <div class="menu-container">
    <a-card :bordered="false">
      <template #title>
        <a-space>
          <a-button type="primary" @click="handleAdd" v-permission="['system:menu:add']">
            <template #icon><plus-outlined /></template>
            {{ $t('common.add') }}
          </a-button>
          <a-button @click="handleExpandAll">
            <template #icon><node-expand-outlined /></template>
            {{ expanded ? $t('common.collapse') : $t('common.expand') }}
          </a-button>
        </a-space>
      </template>

      <a-table
        :columns="columns"
        :data-source="menuList"
        :loading="loading"
        :pagination="false"
        row-key="id"
        :expanded-row-keys="expandedKeys"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'action'">
            <a-space>
              <a-button type="link" @click="handleEdit(record)" v-permission="['system:menu:edit']">
                {{ $t('common.edit') }}
              </a-button>
              <a-popconfirm
                :title="$t('common.deleteConfirm')"
                @confirm="handleDelete(record)"
                v-permission="['system:menu:delete']"
              >
                <a-button type="link" danger>{{ $t('common.delete') }}</a-button>
              </a-popconfirm>
            </a-space>
          </template>
        </template>
      </a-table>
    </a-card>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { PlusOutlined, NodeExpandOutlined } from '@ant-design/icons-vue';

const loading = ref(false);
const menuList = ref([]);
const expanded = ref(false);
const expandedKeys = ref([]);

const columns = [
  {
    title: '菜单名称',
    dataIndex: 'name',
    key: 'name',
  },
  {
    title: '图标',
    dataIndex: 'icon',
    key: 'icon',
  },
  {
    title: '排序',
    dataIndex: 'orderNum',
    key: 'orderNum',
    width: 80,
  },
  {
    title: '权限标识',
    dataIndex: 'perms',
    key: 'perms',
  },
  {
    title: '操作',
    key: 'action',
    width: 200,
  },
];

const handleAdd = () => {
  // TODO: 实现添加菜单功能
};

const handleEdit = (record) => {
  // TODO: 实现编辑菜单功能
};

const handleDelete = (record) => {
  // TODO: 实现删除菜单功能
};

const handleExpandAll = () => {
  expanded.value = !expanded.value;
  // TODO: 实现展开/收起所有功能
};

onMounted(() => {
  // TODO: 加载菜单列表数据
});
</script>

<style scoped>
.menu-container {
  padding: 24px;
}
</style> 