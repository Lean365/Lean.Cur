<template>
  <div class="dept-container">
    <a-card :bordered="false">
      <template #title>
        <a-space>
          <a-button type="primary" @click="handleAdd" v-permission="['system:dept:add']">
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
        :data-source="deptList"
        :loading="loading"
        :pagination="false"
        row-key="id"
        :expanded-row-keys="expandedKeys"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'status'">
            <a-tag :color="record.status === 1 ? 'success' : 'error'">
              {{ record.status === 1 ? $t('common.enable') : $t('common.disable') }}
            </a-tag>
          </template>
          <template v-if="column.key === 'action'">
            <a-space>
              <a-button type="link" @click="handleAdd(record)" v-permission="['system:dept:add']">
                {{ $t('common.add') }}
              </a-button>
              <a-button type="link" @click="handleEdit(record)" v-permission="['system:dept:edit']">
                {{ $t('common.edit') }}
              </a-button>
              <a-popconfirm
                :title="$t('common.deleteConfirm')"
                @confirm="handleDelete(record)"
                v-permission="['system:dept:delete']"
              >
                <a-button type="link" danger>{{ $t('common.delete') }}</a-button>
              </a-popconfirm>
            </a-space>
          </template>
        </template>
      </a-table>
    </a-card>

    <!-- 添加/编辑部门对话框 -->
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
        <a-form-item label="上级部门" name="parentId">
          <a-tree-select
            v-model:value="formData.parentId"
            :tree-data="deptTreeData"
            :field-names="{ children: 'children', label: 'name', value: 'id' }"
            placeholder="请选择上级部门"
            allow-clear
            tree-default-expand-all
          />
        </a-form-item>
        <a-form-item label="部门名称" name="name">
          <a-input v-model:value="formData.name" placeholder="请输入部门名称" />
        </a-form-item>
        <a-form-item label="显示排序" name="orderNum">
          <a-input-number v-model:value="formData.orderNum" :min="0" style="width: 100%" />
        </a-form-item>
        <a-form-item label="负责人" name="leader">
          <a-input v-model:value="formData.leader" placeholder="请输入负责人" />
        </a-form-item>
        <a-form-item label="联系电话" name="phone">
          <a-input v-model:value="formData.phone" placeholder="请输入联系电话" />
        </a-form-item>
        <a-form-item label="邮箱" name="email">
          <a-input v-model:value="formData.email" placeholder="请输入邮箱" />
        </a-form-item>
        <a-form-item label="部门状态" name="status">
          <a-radio-group v-model:value="formData.status">
            <a-radio :value="1">正常</a-radio>
            <a-radio :value="0">停用</a-radio>
          </a-radio-group>
        </a-form-item>
      </a-form>
    </a-modal>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { PlusOutlined, NodeExpandOutlined } from '@ant-design/icons-vue';
import { message } from 'ant-design-vue';

const loading = ref(false);
const deptList = ref([]);
const expanded = ref(false);
const expandedKeys = ref([]);
const modalVisible = ref(false);
const modalLoading = ref(false);
const modalTitle = ref('');
const formRef = ref();
const deptTreeData = ref([]);

const formData = ref({
  id: undefined,
  parentId: undefined,
  name: '',
  orderNum: 0,
  leader: '',
  phone: '',
  email: '',
  status: 1
});

const formRules = {
  name: [{ required: true, message: '请输入部门名称', trigger: 'blur' }],
  orderNum: [{ required: true, message: '请输入显示排序', trigger: 'blur' }],
  phone: [{ pattern: /^1[3-9]\d{9}$/, message: '请输入正确的手机号码', trigger: 'blur' }],
  email: [{ type: 'email', message: '请输入正确的邮箱地址', trigger: 'blur' }]
};

const columns = [
  {
    title: '部门名称',
    dataIndex: 'name',
    key: 'name'
  },
  {
    title: '排序',
    dataIndex: 'orderNum',
    key: 'orderNum',
    width: 80
  },
  {
    title: '状态',
    dataIndex: 'status',
    key: 'status',
    width: 100
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

const handleAdd = (record) => {
  modalTitle.value = '添加部门';
  formData.value = {
    parentId: record?.id,
    name: '',
    orderNum: 0,
    leader: '',
    phone: '',
    email: '',
    status: 1
  };
  modalVisible.value = true;
};

const handleEdit = (record) => {
  modalTitle.value = '编辑部门';
  formData.value = { ...record };
  modalVisible.value = true;
};

const handleDelete = async (record) => {
  try {
    loading.value = true;
    // TODO: 调用删除API
    message.success('删除成功');
    await loadDeptList();
  } catch (error) {
    message.error('删除失败');
  } finally {
    loading.value = false;
  }
};

const handleExpandAll = () => {
  expanded.value = !expanded.value;
  if (expanded.value) {
    // 展开所有节点
    const keys = [];
    const traverse = (list) => {
      list.forEach(item => {
        keys.push(item.id);
        if (item.children) {
          traverse(item.children);
        }
      });
    };
    traverse(deptList.value);
    expandedKeys.value = keys;
  } else {
    // 收起所有节点
    expandedKeys.value = [];
  }
};

const handleModalOk = () => {
  formRef.value?.validate().then(async () => {
    try {
      modalLoading.value = true;
      if (formData.value.id) {
        // TODO: 调用更新API
      } else {
        // TODO: 调用新增API
      }
      message.success(formData.value.id ? '更新成功' : '添加成功');
      modalVisible.value = false;
      await loadDeptList();
    } catch (error) {
      message.error(formData.value.id ? '更新失败' : '添加失败');
    } finally {
      modalLoading.value = false;
    }
  });
};

const loadDeptList = async () => {
  try {
    loading.value = true;
    // TODO: 调用获取部门列表API
    // deptList.value = response.data;
    // 构建树形选择数据
    deptTreeData.value = JSON.parse(JSON.stringify(deptList.value));
  } catch (error) {
    message.error('获取部门列表失败');
  } finally {
    loading.value = false;
  }
};

onMounted(() => {
  loadDeptList();
});
</script>

<style scoped>
.dept-container {
  padding: 24px;
}
</style> 