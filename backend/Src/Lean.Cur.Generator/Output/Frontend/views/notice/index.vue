<template>
  <div class="lean-notice-container">
    <a-card :bordered="false">
      <!-- 搜索区域 -->
      <div class="table-page-search-wrapper">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="8" :sm="24">
              <a-form-item :label="$t('notice.keyword')">
                <a-input v-model:value="queryParam.keyword" :placeholder="$t('common.pleaseInput')" allow-clear />
              </a-form-item>
            </a-col>
            <a-col :md="8" :sm="24">
              <span class="table-page-search-submitButtons">
                <a-button type="primary" @click="handleSearch">
                  <template #icon><SearchOutlined /></template>
                  {{ $t('common.search') }}
                </a-button>
                <a-button style="margin-left: 8px" @click="handleReset">
                  <template #icon><ReloadOutlined /></template>
                  {{ $t('common.reset') }}
                </a-button>
              </span>
            </a-col>
          </a-row>
        </a-form>
      </div>

      <!-- 操作按钮区域 -->
      <div class="table-operator">
        <a-button type="primary" @click="handleCreate">
          <template #icon><PlusOutlined /></template>
          {{ $t('common.create') }}
        </a-button>
      </div>

      <!-- 表格区域 -->
      <a-table
        :columns="columns"
        :data-source="dataSource"
        :loading="loading"
        :pagination="pagination"
        @change="handleTableChange"
        rowKey="id"
      >
        <!-- 操作列 -->
        <template #action="{ record }">
          <a-space>
            <a @click="handleEdit(record)">{{ $t('common.edit') }}</a>
            <a-popconfirm
              :title="$t('common.deleteConfirm')"
              @confirm="handleDelete(record)"
            >
              <a class="danger-text">{{ $t('common.delete') }}</a>
            </a-popconfirm>
          </a-space>
        </template>
      </a-table>

      <!-- 创建/编辑弹窗 -->
      <a-modal
        :title="modalTitle"
        :visible="modalVisible"
        @ok="handleModalOk"
        @cancel="handleModalCancel"
      >
        <a-form
          ref="formRef"
          :model="formData"
          :rules="rules"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 16 }"
        >
          <a-form-item :label="$t('notice.title')" name="title">
            <a-input v-model:value="formData.title" :placeholder="$t('common.pleaseInput')" />
          </a-form-item>
          <a-form-item :label="$t('notice.content')" name="content">
            <a-input v-model:value="formData.content" :placeholder="$t('common.pleaseInput')" />
          </a-form-item>
          <a-form-item :label="$t('notice.type')" name="type">
            <a-input-number v-model:value="formData.type" :placeholder="$t('common.pleaseInput')" style="width: 100%" />
          </a-form-item>
          <a-form-item :label="$t('notice.status')" name="status">
            <a-switch v-model:checked="formData.status" />
          </a-form-item>
          <a-form-item :label="$t('notice.publishtime')" name="publishtime">
            <a-date-picker v-model:value="formData.publishtime" show-time style="width: 100%" />
          </a-form-item>
          <a-form-item :label="$t('notice.publisher')" name="publisher">
            <a-input v-model:value="formData.publisher" :placeholder="$t('common.pleaseInput')" />
          </a-form-item>
        </a-form>
      </a-modal>
    </a-card>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted, computed } from 'vue'
import { message } from 'ant-design-vue'
import type { TablePaginationConfig } from 'ant-design-vue'
import { SearchOutlined, ReloadOutlined, PlusOutlined } from '@ant-design/icons-vue'
import { getNoticeList, getNotice, createNotice, updateNotice, deleteNotice } from '@/api/notice'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

// 表格列定义
const columns = [
  {
    title: computed(() => t('notice.title')),
    dataIndex: 'title',
  },
  {
    title: computed(() => t('notice.content')),
    dataIndex: 'content',
  },
  {
    title: computed(() => t('notice.type')),
    dataIndex: 'type',
  },
  {
    title: computed(() => t('notice.status')),
    dataIndex: 'status',
    customRender: ({ text }) => text ? t('common.yes') : t('common.no')
  },
  {
    title: computed(() => t('notice.publishtime')),
    dataIndex: 'publishtime',
    customRender: ({ text }) => text ? dayjs(text).format('YYYY-MM-DD HH:mm:ss') : ''
  },
  {
    title: computed(() => t('notice.publisher')),
    dataIndex: 'publisher',
  },
  {
    title: computed(() => t('common.action')),
    key: 'action',
    slots: { customRender: 'action' }
  }
]

// 查询参数
const queryParam = ref({
  keyword: ''
})

// 表格数据
const dataSource = ref([])
const loading = ref(false)
const pagination = ref({
  current: 1,
  pageSize: 10,
  total: 0,
  showTotal: (total: number) => t('common.total', { total })
})

// 弹窗相关
const modalVisible = ref(false)
const modalTitle = ref('')
const formRef = ref()
const formData = ref({})
const rules = {
  title: [{ required: true, message: t('common.required') }],
  content: [{ required: true, message: t('common.required') }],
  type: [{ required: true, message: t('common.required') }],
  status: [{ required: true, message: t('common.required') }],
}

// 获取数据
const loadData = async () => {
  loading.value = true
  try {
    const res = await getNoticeList(queryParam.value)
    dataSource.value = res
  } catch (error) {
    message.error(t('common.fetchFailed'))
  }
  loading.value = false
}

// 搜索
const handleSearch = () => {
  pagination.value.current = 1
  loadData()
}

// 重置
const handleReset = () => {
  queryParam.value = { keyword: '' }
  pagination.value.current = 1
  loadData()
}

// 表格变化
const handleTableChange = (pag: TablePaginationConfig) => {
  pagination.value.current = pag.current
  pagination.value.pageSize = pag.pageSize
  loadData()
}

// 创建
const handleCreate = () => {
  formData.value = {}
  modalTitle.value = t('common.create')
  modalVisible.value = true
}

// 编辑
const handleEdit = async (record: any) => {
  const res = await getNotice(record.id)
  formData.value = res
  modalTitle.value = t('common.edit')
  modalVisible.value = true
}

// 删除
const handleDelete = async (record: any) => {
  try {
    await deleteNotice(record.id)
    message.success(t('common.deleteSuccess'))
    loadData()
  } catch (error) {
    message.error(t('common.deleteFailed'))
  }
}

// 弹窗确认
const handleModalOk = () => {
  formRef.value.validate().then(async () => {
    try {
      if (formData.value.id) {
        await updateNotice(formData.value)
        message.success(t('common.updateSuccess'))
      } else {
        await createNotice(formData.value)
        message.success(t('common.createSuccess'))
      }
      modalVisible.value = false
      loadData()
    } catch (error) {
      message.error(t('common.saveFailed'))
    }
  })
}

// 弹窗取消
const handleModalCancel = () => {
  modalVisible.value = false
  formRef.value?.resetFields()
}

onMounted(() => {
  loadData()
})
</script>

<style lang="less" scoped>
.lean-notice-container {
  padding: 24px;

  .table-page-search-wrapper {
    margin-bottom: 16px;
  }

  .table-operator {
    margin-bottom: 16px;
  }

  .danger-text {
    color: #ff4d4f;
  }
}
</style> 