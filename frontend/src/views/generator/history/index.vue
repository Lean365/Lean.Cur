<template>
  <div class="generator-history-container">
    <a-card :bordered="false">
      <!-- 搜索表单 -->
      <a-form
        ref="queryFormRef"
        :model="queryParams"
        :label-col="{ style: { width: '100px' } }"
        :wrapper-col="{ flex: 1 }"
      >
        <a-row :gutter="16">
          <a-col :span="8">
            <a-form-item label="表名称" name="tableName">
              <a-input v-model:value="queryParams.tableName" placeholder="请输入表名称" allow-clear />
            </a-form-item>
          </a-col>
          <a-col :span="8">
            <a-form-item label="生成时间" name="dateRange">
              <a-range-picker
                v-model:value="queryParams.dateRange"
                :show-time="{ format: 'HH:mm:ss' }"
                format="YYYY-MM-DD HH:mm:ss"
                value-format="YYYY-MM-DD HH:mm:ss"
                style="width: 100%"
              />
            </a-form-item>
          </a-col>
          <a-col :span="8">
            <a-form-item :wrapper-col="{ offset: 2 }">
              <a-space>
                <a-button type="primary" @click="handleQuery">
                  <template #icon><search-outlined /></template>
                  {{ $t('common.search') }}
                </a-button>
                <a-button @click="handleReset">
                  <template #icon><redo-outlined /></template>
                  {{ $t('common.reset') }}
                </a-button>
              </a-space>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>

      <!-- 数据表格 -->
      <a-table
        :columns="columns"
        :data-source="historyList"
        :loading="loading"
        :pagination="pagination"
        @change="handleTableChange"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'status'">
            <a-tag :color="record.status === 'success' ? 'success' : 'error'">
              {{ record.status === 'success' ? '成功' : '失败' }}
            </a-tag>
          </template>
          <template v-if="column.key === 'action'">
            <a-space>
              <a-button type="link" @click="handlePreview(record)" v-permission="['generator:preview']">
                预览
              </a-button>
              <a-button type="link" @click="handleDownload(record)" v-permission="['generator:download']">
                下载
              </a-button>
              <a-button type="link" @click="handleDelete(record)" v-permission="['generator:remove']" danger>
                删除
              </a-button>
            </a-space>
          </template>
        </template>
      </a-table>
    </a-card>

    <!-- 预览对话框 -->
    <a-modal
      v-model:visible="previewVisible"
      title="代码预览"
      width="80%"
      :footer="null"
    >
      <a-tabs v-model:activeKey="activeTab">
        <a-tab-pane key="entity" tab="实体类">
          <pre><code>{{ previewData.entity }}</code></pre>
        </a-tab-pane>
        <a-tab-pane key="mapper" tab="Mapper">
          <pre><code>{{ previewData.mapper }}</code></pre>
        </a-tab-pane>
        <a-tab-pane key="service" tab="Service">
          <pre><code>{{ previewData.service }}</code></pre>
        </a-tab-pane>
        <a-tab-pane key="controller" tab="Controller">
          <pre><code>{{ previewData.controller }}</code></pre>
        </a-tab-pane>
        <a-tab-pane key="vue" tab="Vue">
          <pre><code>{{ previewData.vue }}</code></pre>
        </a-tab-pane>
      </a-tabs>
    </a-modal>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import {
  SearchOutlined,
  RedoOutlined
} from '@ant-design/icons-vue';
import { message } from 'ant-design-vue';

// 查询参数
const queryFormRef = ref();
const queryParams = ref({
  tableName: '',
  dateRange: []
});

// 表格数据
const loading = ref(false);
const historyList = ref([]);
const pagination = ref({
  current: 1,
  pageSize: 10,
  total: 0,
  showTotal: total => `共 ${total} 条`
});

// 表格列定义
const columns = [
  {
    title: '表名称',
    dataIndex: 'tableName',
    key: 'tableName',
    ellipsis: true
  },
  {
    title: '实体类名称',
    dataIndex: 'className',
    key: 'className',
    ellipsis: true
  },
  {
    title: '包名',
    dataIndex: 'packageName',
    key: 'packageName',
    ellipsis: true
  },
  {
    title: '生成状态',
    dataIndex: 'status',
    key: 'status',
    width: 100
  },
  {
    title: '生成时间',
    dataIndex: 'createTime',
    key: 'createTime',
    width: 180
  },
  {
    title: '操作',
    key: 'action',
    width: 200
  }
];

// 预览相关
const previewVisible = ref(false);
const activeTab = ref('entity');
const previewData = ref({
  entity: '',
  mapper: '',
  service: '',
  controller: '',
  vue: ''
});

// 加载历史记录列表
const loadHistoryList = async (params = {}) => {
  try {
    loading.value = true;
    // TODO: 调用获取历史记录列表API
    // const response = await getGenerateHistoryList({
    //   pageNum: params.pageNum || pagination.value.current,
    //   pageSize: params.pageSize || pagination.value.pageSize,
    //   ...queryParams.value
    // });
    // historyList.value = response.data.list;
    // pagination.value.total = response.data.total;
  } catch (error) {
    message.error('获取历史记录列表失败');
  } finally {
    loading.value = false;
  }
};

// 表格变化事件处理
const handleTableChange = (pag) => {
  pagination.value.current = pag.current;
  pagination.value.pageSize = pag.pageSize;
  loadHistoryList({
    pageNum: pag.current,
    pageSize: pag.pageSize
  });
};

// 查询按钮事件处理
const handleQuery = () => {
  pagination.value.current = 1;
  loadHistoryList({
    pageNum: 1,
    pageSize: pagination.value.pageSize
  });
};

// 重置按钮事件处理
const handleReset = () => {
  queryFormRef.value?.resetFields();
  pagination.value.current = 1;
  loadHistoryList({
    pageNum: 1,
    pageSize: pagination.value.pageSize
  });
};

// 预览按钮事件处理
const handlePreview = async (record) => {
  try {
    // TODO: 调用预览代码API
    // const response = await previewGeneratedCode(record.id);
    // previewData.value = response.data;
    previewVisible.value = true;
  } catch (error) {
    message.error('预览失败');
  }
};

// 下载按钮事件处理
const handleDownload = async (record) => {
  try {
    // TODO: 调用下载代码API
    // await downloadGeneratedCode(record.id);
    message.success('下载成功');
  } catch (error) {
    message.error('下载失败');
  }
};

// 删除按钮事件处理
const handleDelete = async (record) => {
  try {
    // TODO: 调用删除历史记录API
    // await deleteGenerateHistory(record.id);
    message.success('删除成功');
    loadHistoryList({
      pageNum: pagination.value.current,
      pageSize: pagination.value.pageSize
    });
  } catch (error) {
    message.error('删除失败');
  }
};

onMounted(() => {
  loadHistoryList({
    pageNum: pagination.value.current,
    pageSize: pagination.value.pageSize
  });
});
</script>

<style scoped>
.generator-history-container {
  padding: 24px;
}

:deep(.ant-card-body) {
  padding-top: 0;
}
</style> 