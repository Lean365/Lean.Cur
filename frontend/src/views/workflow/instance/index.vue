<template>
  <div class="workflow-instance-container">
    <a-card :bordered="false">
      <template #title>
        <a-space>
          <a-button @click="handleRefresh">
            <template #icon><reload-outlined /></template>
            {{ $t('common.refresh') }}
          </a-button>
        </a-space>
      </template>

      <!-- 搜索表单 -->
      <a-form
        ref="queryFormRef"
        :model="queryParams"
        :label-col="{ style: { width: '100px' } }"
        :wrapper-col="{ flex: 1 }"
      >
        <a-row :gutter="16">
          <a-col :span="8">
            <a-form-item label="流程名称" name="processName">
              <a-input v-model:value="queryParams.processName" placeholder="请输入流程名称" allow-clear />
            </a-form-item>
          </a-col>
          <a-col :span="8">
            <a-form-item label="业务标题" name="title">
              <a-input v-model:value="queryParams.title" placeholder="请输入业务标题" allow-clear />
            </a-form-item>
          </a-col>
          <a-col :span="8">
            <a-form-item label="流程状态" name="status">
              <a-select v-model:value="queryParams.status" placeholder="请选择流程状态" allow-clear>
                <a-select-option value="active">运行中</a-select-option>
                <a-select-option value="suspended">已挂起</a-select-option>
                <a-select-option value="completed">已完成</a-select-option>
                <a-select-option value="terminated">已终止</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col :span="8">
            <a-form-item label="发起时间" name="dateRange">
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

      <!-- 流程实例表格 -->
      <a-table
        :columns="columns"
        :data-source="instanceList"
        :loading="loading"
        :pagination="pagination"
        @change="handleTableChange"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'status'">
            <a-tag :color="getStatusTag(record.status).color">
              {{ getStatusTag(record.status).text }}
            </a-tag>
          </template>
          <template v-if="column.key === 'action'">
            <a-space>
              <a-button type="link" @click="handleView(record)" v-permission="['workflow:instance:query']">
                查看
              </a-button>
              <a-button type="link" @click="handleDiagram(record)" v-permission="['workflow:instance:query']">
                流程图
              </a-button>
              <a-dropdown v-if="record.status === 'active'">
                <template #overlay>
                  <a-menu>
                    <a-menu-item @click="handleSuspend(record)" v-permission="['workflow:instance:update']">
                      {{ record.suspended ? '激活' : '挂起' }}
                    </a-menu-item>
                    <a-menu-item @click="handleTerminate(record)" v-permission="['workflow:instance:update']">
                      终止
                    </a-menu-item>
                    <a-menu-item @click="handleDelete(record)" v-permission="['workflow:instance:delete']">
                      删除
                    </a-menu-item>
                  </a-menu>
                </template>
                <a-button type="link">
                  更多 <down-outlined />
                </a-button>
              </a-dropdown>
            </a-space>
          </template>
        </template>
      </a-table>
    </a-card>

    <!-- 流程实例详情对话框 -->
    <a-modal
      v-model:visible="detailVisible"
      title="流程实例详情"
      :footer="null"
      width="800px"
    >
      <a-descriptions bordered :column="2">
        <a-descriptions-item label="流程名称" :span="2">{{ detailData.processName }}</a-descriptions-item>
        <a-descriptions-item label="业务标题" :span="2">{{ detailData.title }}</a-descriptions-item>
        <a-descriptions-item label="流程状态">
          <a-tag :color="getStatusTag(detailData.status).color">
            {{ getStatusTag(detailData.status).text }}
          </a-tag>
        </a-descriptions-item>
        <a-descriptions-item label="发起人">{{ detailData.startUser }}</a-descriptions-item>
        <a-descriptions-item label="发起时间">{{ detailData.startTime }}</a-descriptions-item>
        <a-descriptions-item label="结束时间">{{ detailData.endTime || '-' }}</a-descriptions-item>
        <a-descriptions-item label="当前节点" :span="2">{{ detailData.currentTask || '-' }}</a-descriptions-item>
        <a-descriptions-item label="备注" :span="2">{{ detailData.remark || '-' }}</a-descriptions-item>
      </a-descriptions>

      <!-- 流程变量 -->
      <a-divider orientation="left">流程变量</a-divider>
      <a-table
        :columns="variableColumns"
        :data-source="detailData.variables || []"
        :pagination="false"
        size="small"
      />

      <!-- 审批历史 -->
      <a-divider orientation="left">审批历史</a-divider>
      <a-timeline>
        <a-timeline-item
          v-for="(item, index) in detailData.history"
          :key="index"
          :color="item.type === 'approve' ? 'green' : (item.type === 'reject' ? 'red' : 'blue')"
        >
          <template #dot>
            <check-circle-outlined v-if="item.type === 'approve'" />
            <close-circle-outlined v-if="item.type === 'reject'" />
            <clock-circle-outlined v-if="item.type === 'submit'" />
          </template>
          <div>{{ item.taskName }} - {{ item.assignee }}</div>
          <div>{{ item.comment || '-' }}</div>
          <div style="color: #999">{{ item.time }}</div>
        </a-timeline-item>
      </a-timeline>
    </a-modal>

    <!-- 流程图对话框 -->
    <a-modal
      v-model:visible="diagramVisible"
      title="流程图"
      :footer="null"
      width="80%"
    >
      <div ref="bpmnViewer" style="height: 500px"></div>
    </a-modal>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import {
  ReloadOutlined,
  SearchOutlined,
  RedoOutlined,
  DownOutlined,
  CheckCircleOutlined,
  CloseCircleOutlined,
  ClockCircleOutlined
} from '@ant-design/icons-vue';
import { message } from 'ant-design-vue';
import BpmnJS from 'bpmn-js';

// 查询参数
const queryFormRef = ref();
const queryParams = ref({
  processName: '',
  title: '',
  status: undefined,
  dateRange: []
});

// 表格数据
const loading = ref(false);
const instanceList = ref([]);
const pagination = ref({
  current: 1,
  pageSize: 10,
  total: 0,
  showTotal: total => `共 ${total} 条`
});

// 详情数据
const detailVisible = ref(false);
const detailData = ref({});

// 流程图相关
const diagramVisible = ref(false);
const bpmnViewer = ref();
let viewer = null;

// 表格列定义
const columns = [
  {
    title: '流程名称',
    dataIndex: 'processName',
    key: 'processName',
    ellipsis: true
  },
  {
    title: '业务标题',
    dataIndex: 'title',
    key: 'title',
    ellipsis: true
  },
  {
    title: '流程状态',
    dataIndex: 'status',
    key: 'status',
    width: 100
  },
  {
    title: '发起人',
    dataIndex: 'startUser',
    key: 'startUser',
    width: 120
  },
  {
    title: '发起时间',
    dataIndex: 'startTime',
    key: 'startTime',
    width: 180
  },
  {
    title: '当前节点',
    dataIndex: 'currentTask',
    key: 'currentTask',
    ellipsis: true
  },
  {
    title: '操作',
    key: 'action',
    width: 200
  }
];

// 流程变量列定义
const variableColumns = [
  {
    title: '变量名',
    dataIndex: 'name',
    key: 'name'
  },
  {
    title: '变量值',
    dataIndex: 'value',
    key: 'value'
  },
  {
    title: '类型',
    dataIndex: 'type',
    key: 'type'
  }
];

// 状态标签配置
const getStatusTag = (status) => {
  const tags = {
    active: { color: 'processing', text: '运行中' },
    suspended: { color: 'warning', text: '已挂起' },
    completed: { color: 'success', text: '已完成' },
    terminated: { color: 'error', text: '已终止' }
  };
  return tags[status] || tags.active;
};

// 加载流程实例列表
const loadInstanceList = async (params = {}) => {
  try {
    loading.value = true;
    // TODO: 调用获取流程实例列表API
    // const response = await getProcessInstanceList({
    //   pageNum: params.pageNum || pagination.value.current,
    //   pageSize: params.pageSize || pagination.value.pageSize,
    //   ...queryParams.value
    // });
    // instanceList.value = response.data.list;
    // pagination.value.total = response.data.total;
  } catch (error) {
    message.error('获取流程实例列表失败');
  } finally {
    loading.value = false;
  }
};

// 表格变化事件处理
const handleTableChange = (pag) => {
  pagination.value.current = pag.current;
  pagination.value.pageSize = pag.pageSize;
  loadInstanceList({
    pageNum: pag.current,
    pageSize: pag.pageSize
  });
};

// 刷新按钮事件处理
const handleRefresh = () => {
  pagination.value.current = 1;
  loadInstanceList({
    pageNum: 1,
    pageSize: pagination.value.pageSize
  });
};

// 查询按钮事件处理
const handleQuery = () => {
  pagination.value.current = 1;
  loadInstanceList({
    pageNum: 1,
    pageSize: pagination.value.pageSize
  });
};

// 重置按钮事件处理
const handleReset = () => {
  queryFormRef.value?.resetFields();
  pagination.value.current = 1;
  loadInstanceList({
    pageNum: 1,
    pageSize: pagination.value.pageSize
  });
};

// 查看详情
const handleView = async (record) => {
  try {
    // TODO: 调用获取流程实例详情API
    // const response = await getProcessInstanceDetail(record.id);
    // detailData.value = response.data;
    detailVisible.value = true;
  } catch (error) {
    message.error('获取流程实例详情失败');
  }
};

// 查看流程图
const handleDiagram = async (record) => {
  try {
    // TODO: 调用获取流程图API
    // const response = await getProcessInstanceDiagram(record.id);
    // const xmlData = response.data;
    diagramVisible.value = true;
    // 在 nextTick 中初始化查看器
    nextTick(() => {
      if (!viewer) {
        viewer = new BpmnJS({
          container: bpmnViewer.value
        });
      }
      // viewer.importXML(xmlData);
    });
  } catch (error) {
    message.error('获取流程图失败');
  }
};

// 挂起/激活流程实例
const handleSuspend = async (record) => {
  try {
    // TODO: 调用挂起/激活流程实例API
    message.success(record.suspended ? '激活成功' : '挂起成功');
    await loadInstanceList({
      pageNum: pagination.value.current,
      pageSize: pagination.value.pageSize
    });
  } catch (error) {
    message.error(record.suspended ? '激活失败' : '挂起失败');
  }
};

// 终止流程实例
const handleTerminate = async (record) => {
  try {
    // TODO: 调用终止流程实例API
    message.success('终止成功');
    await loadInstanceList({
      pageNum: pagination.value.current,
      pageSize: pagination.value.pageSize
    });
  } catch (error) {
    message.error('终止失败');
  }
};

// 删除流程实例
const handleDelete = async (record) => {
  try {
    // TODO: 调用删除流程实例API
    message.success('删除成功');
    await loadInstanceList({
      pageNum: pagination.value.current,
      pageSize: pagination.value.pageSize
    });
  } catch (error) {
    message.error('删除失败');
  }
};

onMounted(() => {
  loadInstanceList({
    pageNum: pagination.value.current,
    pageSize: pagination.value.pageSize
  });
});
</script>

<style scoped>
.workflow-instance-container {
  padding: 24px;
}
</style> 