<template>
  <div class="page-container">
    <a-card :bordered="false">
      <!-- 搜索表单 -->
      <a-form layout="inline" :model="queryParams" @submit="handleSearch">
        <a-form-item label="用户名">
          <a-input
            v-model:value="queryParams.username"
            placeholder="请输入用户名"
            allow-clear
          />
        </a-form-item>
        <a-form-item label="手机号">
          <a-input
            v-model:value="queryParams.mobile"
            placeholder="请输入手机号"
            allow-clear
          />
        </a-form-item>
        <a-form-item label="状态">
          <a-select
            v-model:value="queryParams.status"
            placeholder="请选择状态"
            allow-clear
          >
            <a-select-option value="1">正常</a-select-option>
            <a-select-option value="0">禁用</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item>
          <a-space>
            <a-button type="primary" html-type="submit">查询</a-button>
            <a-button @click="handleReset">重置</a-button>
          </a-space>
        </a-form-item>
      </a-form>

      <!-- 操作按钮 -->
      <div class="table-operations">
        <a-space>
          <a-button type="primary" @click="handleAdd">
            <template #icon><plus-outlined /></template>
            新增
          </a-button>
          <a-button danger :disabled="!selectedRowKeys.length" @click="handleBatchDelete">
            <template #icon><delete-outlined /></template>
            批量删除
          </a-button>
        </a-space>
      </div>

      <!-- 数据表格 -->
      <a-table
        :columns="columns"
        :data-source="dataSource"
        :loading="loading"
        :pagination="pagination"
        :row-selection="rowSelection"
        @change="handleTableChange"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'status'">
            <a-tag :color="record.status === '1' ? 'success' : 'error'">
              {{ record.status === '1' ? '正常' : '禁用' }}
            </a-tag>
          </template>
          <template v-else-if="column.key === 'action'">
            <a-space>
              <a @click="handleEdit(record)">编辑</a>
              <a-divider type="vertical" />
              <a-popconfirm
                title="确定要删除此用户吗？"
                @confirm="handleDelete(record)"
              >
                <a class="text-danger">删除</a>
              </a-popconfirm>
            </a-space>
          </template>
        </template>
      </a-table>
    </a-card>

    <!-- 新增/编辑表单弹窗 -->
    <a-modal
      v-model:visible="modalVisible"
      :title="modalTitle"
      @ok="handleModalOk"
      @cancel="handleModalCancel"
    >
      <a-form
        ref="formRef"
        :model="formData"
        :rules="formRules"
        label-col="{ span: 6 }"
        wrapper-col="{ span: 16 }"
      >
        <a-form-item label="用户名" name="username">
          <a-input
            v-model:value="formData.username"
            placeholder="请输入用户名"
            :disabled="!!formData.id"
          />
        </a-form-item>
        <a-form-item
          label="密码"
          name="password"
          :rules="[{ required: !formData.id, message: '请输入密码' }]"
        >
          <a-input-password
            v-model:value="formData.password"
            placeholder="请输入密码"
          />
        </a-form-item>
        <a-form-item label="昵称" name="nickname">
          <a-input
            v-model:value="formData.nickname"
            placeholder="请输入昵称"
          />
        </a-form-item>
        <a-form-item label="手机号" name="mobile">
          <a-input
            v-model:value="formData.mobile"
            placeholder="请输入手机号"
          />
        </a-form-item>
        <a-form-item label="邮箱" name="email">
          <a-input
            v-model:value="formData.email"
            placeholder="请输入邮箱"
          />
        </a-form-item>
        <a-form-item label="状态" name="status">
          <a-radio-group v-model:value="formData.status">
            <a-radio value="1">正常</a-radio>
            <a-radio value="0">禁用</a-radio>
          </a-radio-group>
        </a-form-item>
      </a-form>
    </a-modal>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';
import { message } from 'ant-design-vue';
import type { TableColumnsType } from 'ant-design-vue';
import {
  PlusOutlined,
  DeleteOutlined
} from '@ant-design/icons-vue';

// 查询参数
const queryParams = reactive({
  username: '',
  mobile: '',
  status: undefined
});

// 表格列定义
const columns: TableColumnsType = [
  {
    title: '用户名',
    dataIndex: 'username',
    key: 'username',
  },
  {
    title: '昵称',
    dataIndex: 'nickname',
    key: 'nickname',
  },
  {
    title: '手机号',
    dataIndex: 'mobile',
    key: 'mobile',
  },
  {
    title: '邮箱',
    dataIndex: 'email',
    key: 'email',
  },
  {
    title: '状态',
    dataIndex: 'status',
    key: 'status',
  },
  {
    title: '创建时间',
    dataIndex: 'createTime',
    key: 'createTime',
  },
  {
    title: '操作',
    key: 'action',
    fixed: 'right',
    width: 120,
  }
];

// 表格数据
const loading = ref(false);
const dataSource = ref([]);
const pagination = reactive({
  current: 1,
  pageSize: 10,
  total: 0,
  showTotal: (total: number) => `共 ${total} 条`
});

// 表格选择
const selectedRowKeys = ref<string[]>([]);
const rowSelection = {
  selectedRowKeys,
  onChange: (keys: string[]) => {
    selectedRowKeys.value = keys;
  }
};

// 表单数据
const modalVisible = ref(false);
const modalTitle = ref('新增用户');
const formRef = ref();
const formData = reactive({
  id: '',
  username: '',
  password: '',
  nickname: '',
  mobile: '',
  email: '',
  status: '1'
});

// 表单校验规则
const formRules = {
  username: [
    { required: true, message: '请输入用户名' },
    { min: 4, max: 20, message: '用户名长度必须在4-20个字符之间' }
  ],
  nickname: [
    { required: true, message: '请输入昵称' }
  ],
  mobile: [
    { pattern: /^1[3-9]\d{9}$/, message: '请输入正确的手机号' }
  ],
  email: [
    { type: 'email', message: '请输入正确的邮箱地址' }
  ]
};

// 查询方法
const handleSearch = () => {
  pagination.current = 1;
  fetchData();
};

// 重置查询
const handleReset = () => {
  queryParams.username = '';
  queryParams.mobile = '';
  queryParams.status = undefined;
  handleSearch();
};

// 表格变化
const handleTableChange = (pag: any) => {
  pagination.current = pag.current;
  pagination.pageSize = pag.pageSize;
  fetchData();
};

// 获取数据
const fetchData = async () => {
  loading.value = true;
  try {
    // TODO: 调用后端API获取数据
    loading.value = false;
  } catch (error) {
    message.error('获取数据失败');
    loading.value = false;
  }
};

// 新增用户
const handleAdd = () => {
  modalTitle.value = '新增用户';
  Object.assign(formData, {
    id: '',
    username: '',
    password: '',
    nickname: '',
    mobile: '',
    email: '',
    status: '1'
  });
  modalVisible.value = true;
};

// 编辑用户
const handleEdit = (record: any) => {
  modalTitle.value = '编辑用户';
  Object.assign(formData, {
    ...record,
    password: ''
  });
  modalVisible.value = true;
};

// 删除用户
const handleDelete = async (record: any) => {
  try {
    // TODO: 调用后端API删除数据
    message.success('删除成功');
    fetchData();
  } catch (error) {
    message.error('删除失败');
  }
};

// 批量删除
const handleBatchDelete = async () => {
  if (!selectedRowKeys.value.length) {
    message.warning('请选择要删除的记录');
    return;
  }
  try {
    // TODO: 调用后端API批量删除数据
    message.success('删除成功');
    selectedRowKeys.value = [];
    fetchData();
  } catch (error) {
    message.error('删除失败');
  }
};

// 提交表单
const handleModalOk = () => {
  formRef.value?.validate().then(async () => {
    try {
      // TODO: 调用后端API保存数据
      message.success('保存成功');
      modalVisible.value = false;
      fetchData();
    } catch (error) {
      message.error('保存失败');
    }
  });
};

// 取消表单
const handleModalCancel = () => {
  modalVisible.value = false;
  formRef.value?.resetFields();
};

// 初始化
fetchData();
</script>

<style lang="less" scoped>
.page-container {
  padding: 24px;
  background-color: #f0f2f5;

  .table-operations {
    margin-bottom: 16px;
  }

  .text-danger {
    color: #ff4d4f;
  }
}
</style> 