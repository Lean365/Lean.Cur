<template>
  <div class="dict-container">
    <a-card :bordered="false">
      <template #title>
        <a-space>
          <a-button type="primary" @click="handleAdd" v-permission="['system:dict:add']">
            <template #icon><plus-outlined /></template>
            {{ $t('common.add') }}
          </a-button>
          <a-button @click="handleRefresh">
            <template #icon><reload-outlined /></template>
            {{ $t('common.refresh') }}
          </a-button>
        </a-space>
      </template>

      <a-table
        :columns="columns"
        :data-source="dictList"
        :loading="loading"
        :pagination="pagination"
        @change="handleTableChange"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'status'">
            <a-tag :color="record.status === 1 ? 'success' : 'error'">
              {{ record.status === 1 ? $t('common.enable') : $t('common.disable') }}
            </a-tag>
          </template>
          <template v-if="column.key === 'action'">
            <a-space>
              <a-button type="link" @click="handleViewItems(record)" v-permission="['system:dict:list']">
                {{ $t('common.detail') }}
              </a-button>
              <a-button type="link" @click="handleEdit(record)" v-permission="['system:dict:edit']">
                {{ $t('common.edit') }}
              </a-button>
              <a-popconfirm
                :title="$t('common.deleteConfirm')"
                @confirm="handleDelete(record)"
                v-permission="['system:dict:delete']"
              >
                <a-button type="link" danger>{{ $t('common.delete') }}</a-button>
              </a-popconfirm>
            </a-space>
          </template>
        </template>
      </a-table>
    </a-card>

    <!-- 添加/编辑字典对话框 -->
    <a-modal
      v-model:visible="modalVisible"
      :title="modalTitle"
      :confirm-loading="modalLoading"
      @ok="handleModalOk"
    >
      <a-form
        ref="formRef"
        :model="formData"
        :rules="formRules"
        :label-col="{ span: 4 }"
        :wrapper-col="{ span: 18 }"
      >
        <a-form-item label="字典名称" name="name">
          <a-input v-model:value="formData.name" placeholder="请输入字典名称" />
        </a-form-item>
        <a-form-item label="字典类型" name="type">
          <a-input v-model:value="formData.type" placeholder="请输入字典类型" />
        </a-form-item>
        <a-form-item label="状态" name="status">
          <a-radio-group v-model:value="formData.status">
            <a-radio :value="1">正常</a-radio>
            <a-radio :value="0">停用</a-radio>
          </a-radio-group>
        </a-form-item>
        <a-form-item label="备注" name="remark">
          <a-textarea v-model:value="formData.remark" placeholder="请输入备注" :rows="4" />
        </a-form-item>
      </a-form>
    </a-modal>

    <!-- 字典数据列表抽屉 -->
    <a-drawer
      v-model:visible="drawerVisible"
      :title="drawerTitle"
      :width="720"
      placement="right"
    >
      <template #extra>
        <a-button type="primary" @click="handleAddItem" v-permission="['system:dict:add']">
          <template #icon><plus-outlined /></template>
          {{ $t('common.add') }}
        </a-button>
      </template>

      <a-table
        :columns="itemColumns"
        :data-source="dictItemList"
        :loading="itemLoading"
        :pagination="itemPagination"
        @change="handleItemTableChange"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'status'">
            <a-tag :color="record.status === 1 ? 'success' : 'error'">
              {{ record.status === 1 ? $t('common.enable') : $t('common.disable') }}
            </a-tag>
          </template>
          <template v-if="column.key === 'action'">
            <a-space>
              <a-button type="link" @click="handleEditItem(record)" v-permission="['system:dict:edit']">
                {{ $t('common.edit') }}
              </a-button>
              <a-popconfirm
                :title="$t('common.deleteConfirm')"
                @confirm="handleDeleteItem(record)"
                v-permission="['system:dict:delete']"
              >
                <a-button type="link" danger>{{ $t('common.delete') }}</a-button>
              </a-popconfirm>
            </a-space>
          </template>
        </template>
      </a-table>

      <!-- 添加/编辑字典数据对话框 -->
      <a-modal
        v-model:visible="itemModalVisible"
        :title="itemModalTitle"
        :confirm-loading="itemModalLoading"
        @ok="handleItemModalOk"
      >
        <a-form
          ref="itemFormRef"
          :model="itemFormData"
          :rules="itemFormRules"
          :label-col="{ span: 4 }"
          :wrapper-col="{ span: 18 }"
        >
          <a-form-item label="数据标签" name="label">
            <a-input v-model:value="itemFormData.label" placeholder="请输入数据标签" />
          </a-form-item>
          <a-form-item label="数据键值" name="value">
            <a-input v-model:value="itemFormData.value" placeholder="请输入数据键值" />
          </a-form-item>
          <a-form-item label="显示排序" name="orderNum">
            <a-input-number v-model:value="itemFormData.orderNum" :min="0" style="width: 100%" />
          </a-form-item>
          <a-form-item label="状态" name="status">
            <a-radio-group v-model:value="itemFormData.status">
              <a-radio :value="1">正常</a-radio>
              <a-radio :value="0">停用</a-radio>
            </a-radio-group>
          </a-form-item>
          <a-form-item label="备注" name="remark">
            <a-textarea v-model:value="itemFormData.remark" placeholder="请输入备注" :rows="4" />
          </a-form-item>
        </a-form>
      </a-modal>
    </a-drawer>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { PlusOutlined, ReloadOutlined } from '@ant-design/icons-vue';
import { message } from 'ant-design-vue';

// 字典列表相关
const loading = ref(false);
const dictList = ref([]);
const pagination = ref({
  current: 1,
  pageSize: 10,
  total: 0,
  showTotal: total => `共 ${total} 条`
});

// 字典表单相关
const modalVisible = ref(false);
const modalLoading = ref(false);
const modalTitle = ref('');
const formRef = ref();
const formData = ref({
  id: undefined,
  name: '',
  type: '',
  status: 1,
  remark: ''
});

const formRules = {
  name: [{ required: true, message: '请输入字典名称', trigger: 'blur' }],
  type: [{ required: true, message: '请输入字典类型', trigger: 'blur' }]
};

// 字典数据列表相关
const drawerVisible = ref(false);
const drawerTitle = ref('');
const currentDict = ref(null);
const itemLoading = ref(false);
const dictItemList = ref([]);
const itemPagination = ref({
  current: 1,
  pageSize: 10,
  total: 0,
  showTotal: total => `共 ${total} 条`
});

// 字典数据表单相关
const itemModalVisible = ref(false);
const itemModalLoading = ref(false);
const itemModalTitle = ref('');
const itemFormRef = ref();
const itemFormData = ref({
  id: undefined,
  dictId: undefined,
  label: '',
  value: '',
  orderNum: 0,
  status: 1,
  remark: ''
});

const itemFormRules = {
  label: [{ required: true, message: '请输入数据标签', trigger: 'blur' }],
  value: [{ required: true, message: '请输入数据键值', trigger: 'blur' }],
  orderNum: [{ required: true, message: '请输入显示排序', trigger: 'blur' }]
};

const columns = [
  {
    title: '字典名称',
    dataIndex: 'name',
    key: 'name'
  },
  {
    title: '字典类型',
    dataIndex: 'type',
    key: 'type'
  },
  {
    title: '状态',
    dataIndex: 'status',
    key: 'status',
    width: 100
  },
  {
    title: '备注',
    dataIndex: 'remark',
    key: 'remark'
  },
  {
    title: '创建时间',
    dataIndex: 'createTime',
    key: 'createTime',
    width: 180
  },
  {
    title: '操作',
    key: 'action',
    width: 220
  }
];

const itemColumns = [
  {
    title: '数据标签',
    dataIndex: 'label',
    key: 'label'
  },
  {
    title: '数据键值',
    dataIndex: 'value',
    key: 'value'
  },
  {
    title: '显示排序',
    dataIndex: 'orderNum',
    key: 'orderNum',
    width: 100
  },
  {
    title: '状态',
    dataIndex: 'status',
    key: 'status',
    width: 100
  },
  {
    title: '备注',
    dataIndex: 'remark',
    key: 'remark'
  },
  {
    title: '操作',
    key: 'action',
    width: 180
  }
];

// 字典列表方法
const loadDictList = async (params = {}) => {
  try {
    loading.value = true;
    // TODO: 调用获取字典列表API
    // const response = await getDictList(params);
    // dictList.value = response.data.list;
    // pagination.value.total = response.data.total;
  } catch (error) {
    message.error('获取字典列表失败');
  } finally {
    loading.value = false;
  }
};

const handleTableChange = (pag) => {
  pagination.value.current = pag.current;
  pagination.value.pageSize = pag.pageSize;
  loadDictList({
    pageNum: pag.current,
    pageSize: pag.pageSize
  });
};

const handleRefresh = () => {
  pagination.value.current = 1;
  loadDictList({
    pageNum: 1,
    pageSize: pagination.value.pageSize
  });
};

const handleAdd = () => {
  modalTitle.value = '添加字典';
  formData.value = {
    name: '',
    type: '',
    status: 1,
    remark: ''
  };
  modalVisible.value = true;
};

const handleEdit = (record) => {
  modalTitle.value = '编辑字典';
  formData.value = { ...record };
  modalVisible.value = true;
};

const handleDelete = async (record) => {
  try {
    loading.value = true;
    // TODO: 调用删除字典API
    message.success('删除成功');
    await loadDictList({
      pageNum: pagination.value.current,
      pageSize: pagination.value.pageSize
    });
  } catch (error) {
    message.error('删除失败');
  } finally {
    loading.value = false;
  }
};

const handleModalOk = () => {
  formRef.value?.validate().then(async () => {
    try {
      modalLoading.value = true;
      if (formData.value.id) {
        // TODO: 调用更新字典API
      } else {
        // TODO: 调用新增字典API
      }
      message.success(formData.value.id ? '更新成功' : '添加成功');
      modalVisible.value = false;
      await loadDictList({
        pageNum: pagination.value.current,
        pageSize: pagination.value.pageSize
      });
    } catch (error) {
      message.error(formData.value.id ? '更新失败' : '添加失败');
    } finally {
      modalLoading.value = false;
    }
  });
};

// 字典数据列表方法
const loadDictItemList = async (params = {}) => {
  try {
    itemLoading.value = true;
    // TODO: 调用获取字典数据列表API
    // const response = await getDictItemList({ dictId: currentDict.value.id, ...params });
    // dictItemList.value = response.data.list;
    // itemPagination.value.total = response.data.total;
  } catch (error) {
    message.error('获取字典数据列表失败');
  } finally {
    itemLoading.value = false;
  }
};

const handleViewItems = (record) => {
  currentDict.value = record;
  drawerTitle.value = `字典数据 - ${record.name}`;
  drawerVisible.value = true;
  itemPagination.value.current = 1;
  loadDictItemList({
    pageNum: 1,
    pageSize: itemPagination.value.pageSize
  });
};

const handleItemTableChange = (pag) => {
  itemPagination.value.current = pag.current;
  itemPagination.value.pageSize = pag.pageSize;
  loadDictItemList({
    pageNum: pag.current,
    pageSize: pag.pageSize
  });
};

const handleAddItem = () => {
  itemModalTitle.value = '添加字典数据';
  itemFormData.value = {
    dictId: currentDict.value.id,
    label: '',
    value: '',
    orderNum: 0,
    status: 1,
    remark: ''
  };
  itemModalVisible.value = true;
};

const handleEditItem = (record) => {
  itemModalTitle.value = '编辑字典数据';
  itemFormData.value = { ...record };
  itemModalVisible.value = true;
};

const handleDeleteItem = async (record) => {
  try {
    itemLoading.value = true;
    // TODO: 调用删除字典数据API
    message.success('删除成功');
    await loadDictItemList({
      pageNum: itemPagination.value.current,
      pageSize: itemPagination.value.pageSize
    });
  } catch (error) {
    message.error('删除失败');
  } finally {
    itemLoading.value = false;
  }
};

const handleItemModalOk = () => {
  itemFormRef.value?.validate().then(async () => {
    try {
      itemModalLoading.value = true;
      if (itemFormData.value.id) {
        // TODO: 调用更新字典数据API
      } else {
        // TODO: 调用新增字典数据API
      }
      message.success(itemFormData.value.id ? '更新成功' : '添加成功');
      itemModalVisible.value = false;
      await loadDictItemList({
        pageNum: itemPagination.value.current,
        pageSize: itemPagination.value.pageSize
      });
    } catch (error) {
      message.error(itemFormData.value.id ? '更新失败' : '添加失败');
    } finally {
      itemModalLoading.value = false;
    }
  });
};

onMounted(() => {
  loadDictList({
    pageNum: pagination.value.current,
    pageSize: pagination.value.pageSize
  });
});
</script>

<style scoped>
.dict-container {
  padding: 24px;
}
</style> 