<template>
  <div class="page-container">
    <a-card :bordered="false">
      <!-- 搜索表单 -->
      <a-form layout="inline" :model="queryParams" @submit="handleSearch">
        <a-form-item label="角色名称">
          <a-input
            v-model:value="queryParams.roleName"
            placeholder="请输入角色名称"
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
              <a @click="handlePermission(record)">权限</a>
              <a-divider type="vertical" />
              <a-popconfirm
                title="确定要删除此角色吗？"
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
        <a-form-item label="角色名称" name="roleName">
          <a-input
            v-model:value="formData.roleName"
            placeholder="请输入角色名称"
          />
        </a-form-item>
        <a-form-item label="角色编码" name="roleCode">
          <a-input
            v-model:value="formData.roleCode"
            placeholder="请输入角色编码"
            :disabled="!!formData.id"
          />
        </a-form-item>
        <a-form-item label="排序" name="sort">
          <a-input-number
            v-model:value="formData.sort"
            placeholder="请输入排序"
            :min="0"
            :max="999"
          />
        </a-form-item>
        <a-form-item label="状态" name="status">
          <a-radio-group v-model:value="formData.status">
            <a-radio value="1">正常</a-radio>
            <a-radio value="0">禁用</a-radio>
          </a-radio-group>
        </a-form-item>
        <a-form-item label="备注" name="remark">
          <a-textarea
            v-model:value="formData.remark"
            placeholder="请输入备注"
            :rows="4"
          />
        </a-form-item>
      </a-form>
    </a-modal>

    <!-- 权限分配弹窗 -->
    <a-modal
      v-model:visible="permissionVisible"
      title="分配权限"
      @ok="handlePermissionOk"
      @cancel="handlePermissionCancel"
    >
      <a-tree
        v-model:checkedKeys="checkedKeys"
        :tree-data="permissionTree"
        checkable
        :defaultExpandAll="true"
      />
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

interface QueryParams {
  roleName: string;
  status: string | undefined;
}

interface RoleData {
  id: string;
  roleName: string;
  roleCode: string;
  sort: number;
  status: string;
  remark: string;
  createTime?: string;
}

// 查询参数
const queryParams = reactive<QueryParams>({
  roleName: '',
  status: undefined
});

// 表格列定义
const columns: TableColumnsType = [
  {
    title: '角色名称',
    dataIndex: 'roleName',
    key: 'roleName',
  },
  {
    title: '角色编码',
    dataIndex: 'roleCode',
    key: 'roleCode',
  },
  {
    title: '排序',
    dataIndex: 'sort',
    key: 'sort',
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
    title: '备注',
    dataIndex: 'remark',
    key: 'remark',
  },
  {
    title: '操作',
    key: 'action',
    fixed: 'right',
    width: 180,
  }
];

// 表格数据
const loading = ref(false);
const dataSource = ref<RoleData[]>([]);
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
const modalTitle = ref('新增角色');
const formRef = ref();
const formData = reactive<RoleData>({
  id: '',
  roleName: '',
  roleCode: '',
  sort: 0,
  status: '1',
  remark: ''
});

// 表单校验规则
const formRules = {
  roleName: [
    { required: true, message: '请输入角色名称' }
  ],
  roleCode: [
    { required: true, message: '请输入角色编码' },
    { pattern: /^[A-Z_]{1,50}$/, message: '角色编码只能包含大写字母和下划线' }
  ],
  sort: [
    { required: true, message: '请输入排序' }
  ]
};

// 权限树数据
const permissionVisible = ref(false);
const permissionTree = ref([]);
const checkedKeys = ref<string[]>([]);

// 查询方法
const handleSearch = () => {
  pagination.current = 1;
  fetchData();
};

// 重置查询
const handleReset = () => {
  queryParams.roleName = '';
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

// 新增角色
const handleAdd = () => {
  modalTitle.value = '新增角色';
  Object.assign(formData, {
    id: '',
    roleName: '',
    roleCode: '',
    sort: 0,
    status: '1',
    remark: ''
  });
  modalVisible.value = true;
};

// 编辑角色
const handleEdit = (record: RoleData) => {
  modalTitle.value = '编辑角色';
  Object.assign(formData, record);
  modalVisible.value = true;
};

// 删除角色
const handleDelete = async (record: RoleData) => {
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

// 打开权限分配
const handlePermission = async (record: RoleData) => {
  try {
    // TODO: 调用后端API获取权限树数据和已选权限
    permissionVisible.value = true;
  } catch (error) {
    message.error('获取权限数据失败');
  }
};

// 保存权限分配
const handlePermissionOk = async () => {
  try {
    // TODO: 调用后端API保存权限数据
    message.success('保存成功');
    permissionVisible.value = false;
  } catch (error) {
    message.error('保存失败');
  }
};

// 取消权限分配
const handlePermissionCancel = () => {
  permissionVisible.value = false;
  checkedKeys.value = [];
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