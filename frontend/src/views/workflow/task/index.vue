<template>
  <div class="workflow-task-container">
    <a-card :bordered="false">
      <a-tabs v-model:activeKey="activeTabKey">
        <!-- 待办任务 -->
        <a-tab-pane key="todo" tab="待办任务">
          <!-- 搜索表单 -->
          <a-form
            ref="todoQueryFormRef"
            :model="todoQueryParams"
            :label-col="{ style: { width: '100px' } }"
            :wrapper-col="{ flex: 1 }"
          >
            <a-row :gutter="16">
              <a-col :span="8">
                <a-form-item label="任务名称" name="taskName">
                  <a-input v-model:value="todoQueryParams.taskName" placeholder="请输入任务名称" allow-clear />
                </a-form-item>
              </a-col>
              <a-col :span="8">
                <a-form-item label="流程名称" name="processName">
                  <a-input v-model:value="todoQueryParams.processName" placeholder="请输入流程名称" allow-clear />
                </a-form-item>
              </a-col>
              <a-col :span="8">
                <a-form-item label="业务标题" name="title">
                  <a-input v-model:value="todoQueryParams.title" placeholder="请输入业务标题" allow-clear />
                </a-form-item>
              </a-col>
              <a-col :span="8">
                <a-form-item :wrapper-col="{ offset: 2 }">
                  <a-space>
                    <a-button type="primary" @click="handleTodoQuery">
                      <template #icon><search-outlined /></template>
                      {{ $t('common.search') }}
                    </a-button>
                    <a-button @click="handleTodoReset">
                      <template #icon><redo-outlined /></template>
                      {{ $t('common.reset') }}
                    </a-button>
                  </a-space>
                </a-form-item>
              </a-col>
            </a-row>
          </a-form>

          <!-- 待办任务表格 -->
          <a-table
            :columns="todoColumns"
            :data-source="todoList"
            :loading="todoLoading"
            :pagination="todoPagination"
            @change="handleTodoTableChange"
          >
            <template #bodyCell="{ column, record }">
              <template v-if="column.key === 'action'">
                <a-space>
                  <a-button type="primary" @click="handleApprove(record)" v-permission="['workflow:task:approve']">
                    审批
                  </a-button>
                  <a-button @click="handleDiagram(record)" v-permission="['workflow:task:query']">
                    流程图
                  </a-button>
                </a-space>
              </template>
            </template>
          </a-table>
        </a-tab-pane>

        <!-- 已办任务 -->
        <a-tab-pane key="done" tab="已办任务">
          <!-- 搜索表单 -->
          <a-form
            ref="doneQueryFormRef"
            :model="doneQueryParams"
            :label-col="{ style: { width: '100px' } }"
            :wrapper-col="{ flex: 1 }"
          >
            <a-row :gutter="16">
              <a-col :span="8">
                <a-form-item label="任务名称" name="taskName">
                  <a-input v-model:value="doneQueryParams.taskName" placeholder="请输入任务名称" allow-clear />
                </a-form-item>
              </a-col>
              <a-col :span="8">
                <a-form-item label="流程名称" name="processName">
                  <a-input v-model:value="doneQueryParams.processName" placeholder="请输入流程名称" allow-clear />
                </a-form-item>
              </a-col>
              <a-col :span="8">
                <a-form-item label="业务标题" name="title">
                  <a-input v-model:value="doneQueryParams.title" placeholder="请输入业务标题" allow-clear />
                </a-form-item>
              </a-col>
              <a-col :span="8">
                <a-form-item :wrapper-col="{ offset: 2 }">
                  <a-space>
                    <a-button type="primary" @click="handleDoneQuery">
                      <template #icon><search-outlined /></template>
                      {{ $t('common.search') }}
                    </a-button>
                    <a-button @click="handleDoneReset">
                      <template #icon><redo-outlined /></template>
                      {{ $t('common.reset') }}
                    </a-button>
                  </a-space>
                </a-form-item>
              </a-col>
            </a-row>
          </a-form>

          <!-- 已办任务表格 -->
          <a-table
            :columns="doneColumns"
            :data-source="doneList"
            :loading="doneLoading"
            :pagination="donePagination"
            @change="handleDoneTableChange"
          >
            <template #bodyCell="{ column, record }">
              <template v-if="column.key === 'action'">
                <a-space>
                  <a-button @click="handleView(record)" v-permission="['workflow:task:query']">
                    查看
                  </a-button>
                  <a-button @click="handleDiagram(record)" v-permission="['workflow:task:query']">
                    流程图
                  </a-button>
                </a-space>
              </template>
            </template>
          </a-table>
        </a-tab-pane>
      </a-tabs>
    </a-card>

    <!-- 审批对话框 -->
    <a-modal
      v-model:visible="approveVisible"
      title="任务审批"
      @ok="handleApproveSubmit"
      :confirmLoading="approveLoading"
    >
      <a-form
        ref="approveFormRef"
        :model="approveForm"
        :rules="approveRules"
        :label-col="{ span: 4 }"
        :wrapper-col="{ span: 20 }"
      >
        <a-form-item label="审批结果" name="approved">
          <a-radio-group v-model:value="approveForm.approved">
            <a-radio :value="true">同意</a-radio>
            <a-radio :value="false">拒绝</a-radio>
          </a-radio-group>
        </a-form-item>
        <a-form-item label="审批意见" name="comment">
          <a-textarea
            v-model:value="approveForm.comment"
            :rows="4"
            placeholder="请输入审批意见"
          />
        </a-form-item>
      </a-form>
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
  SearchOutlined,
  RedoOutlined
} from '@ant-design/icons-vue';
import { message } from 'ant-design-vue';
import BpmnJS from 'bpmn-js';

// 当前激活的标签页
const activeTabKey = ref('todo');

// 待办任务相关
const todoQueryFormRef = ref();
const todoQueryParams = ref({
  taskName: '',
  processName: '',
  title: ''
});

const todoLoading = ref(false);
const todoList = ref([]);
const todoPagination = ref({
  current: 1,
  pageSize: 10,
  total: 0,
  showTotal: total => `共 ${total} 条`
});

// 已办任务相关
const doneQueryFormRef = ref();
const doneQueryParams = ref({
  taskName: '',
  processName: '',
  title: ''
});

const doneLoading = ref(false);
const doneList = ref([]);
const donePagination = ref({
  current: 1,
  pageSize: 10,
  total: 0,
  showTotal: total => `共 ${total} 条`
});

// 审批相关
const approveVisible = ref(false);
const approveLoading = ref(false);
const approveFormRef = ref();
const approveForm = ref({
  taskId: '',
  approved: true,
  comment: ''
});

const approveRules = {
  approved: [{ required: true, message: '请选择审批结果' }],
  comment: [{ required: true, message: '请输入审批意见' }]
};

// 流程图相关
const diagramVisible = ref(false);
const bpmnViewer = ref();
let viewer = null;

// 待办任务表格列定义
const todoColumns = [
  {
    title: '任务名称',
    dataIndex: 'taskName',
    key: 'taskName',
    ellipsis: true
  },
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
    title: '到达时间',
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

// 已办任务表格列定义
const doneColumns = [
  {
    title: '任务名称',
    dataIndex: 'taskName',
    key: 'taskName',
    ellipsis: true
  },
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
    title: '发起人',
    dataIndex: 'startUser',
    key: 'startUser',
    width: 120
  },
  {
    title: '完成时间',
    dataIndex: 'completeTime',
    key: 'completeTime',
    width: 180
  },
  {
    title: '审批结果',
    dataIndex: 'approved',
    key: 'approved',
    width: 100,
    customRender: ({ text }) => (
      <a-tag color={text ? 'success' : 'error'}>
        {text ? '同意' : '拒绝'}
      </a-tag>
    )
  },
  {
    title: '操作',
    key: 'action',
    width: 200
  }
];

// 加载待办任务列表
const loadTodoList = async (params = {}) => {
  try {
    todoLoading.value = true;
    // TODO: 调用获取待办任务列表API
    // const response = await getTodoTaskList({
    //   pageNum: params.pageNum || todoPagination.value.current,
    //   pageSize: params.pageSize || todoPagination.value.pageSize,
    //   ...todoQueryParams.value
    // });
    // todoList.value = response.data.list;
    // todoPagination.value.total = response.data.total;
  } catch (error) {
    message.error('获取待办任务列表失败');
  } finally {
    todoLoading.value = false;
  }
};

// 加载已办任务列表
const loadDoneList = async (params = {}) => {
  try {
    doneLoading.value = true;
    // TODO: 调用获取已办任务列表API
    // const response = await getDoneTaskList({
    //   pageNum: params.pageNum || donePagination.value.current,
    //   pageSize: params.pageSize || donePagination.value.pageSize,
    //   ...doneQueryParams.value
    // });
    // doneList.value = response.data.list;
    // donePagination.value.total = response.data.total;
  } catch (error) {
    message.error('获取已办任务列表失败');
  } finally {
    doneLoading.value = false;
  }
};

// 待办任务表格变化事件处理
const handleTodoTableChange = (pag) => {
  todoPagination.value.current = pag.current;
  todoPagination.value.pageSize = pag.pageSize;
  loadTodoList({
    pageNum: pag.current,
    pageSize: pag.pageSize
  });
};

// 已办任务表格变化事件处理
const handleDoneTableChange = (pag) => {
  donePagination.value.current = pag.current;
  donePagination.value.pageSize = pag.pageSize;
  loadDoneList({
    pageNum: pag.current,
    pageSize: pag.pageSize
  });
};

// 待办任务查询按钮事件处理
const handleTodoQuery = () => {
  todoPagination.value.current = 1;
  loadTodoList({
    pageNum: 1,
    pageSize: todoPagination.value.pageSize
  });
};

// 已办任务查询按钮事件处理
const handleDoneQuery = () => {
  donePagination.value.current = 1;
  loadDoneList({
    pageNum: 1,
    pageSize: donePagination.value.pageSize
  });
};

// 待办任务重置按钮事件处理
const handleTodoReset = () => {
  todoQueryFormRef.value?.resetFields();
  todoPagination.value.current = 1;
  loadTodoList({
    pageNum: 1,
    pageSize: todoPagination.value.pageSize
  });
};

// 已办任务重置按钮事件处理
const handleDoneReset = () => {
  doneQueryFormRef.value?.resetFields();
  donePagination.value.current = 1;
  loadDoneList({
    pageNum: 1,
    pageSize: donePagination.value.pageSize
  });
};

// 审批按钮事件处理
const handleApprove = (record) => {
  approveForm.value = {
    taskId: record.id,
    approved: true,
    comment: ''
  };
  approveVisible.value = true;
};

// 提交审批
const handleApproveSubmit = async () => {
  try {
    await approveFormRef.value?.validate();
    approveLoading.value = true;
    // TODO: 调用提交审批API
    // await approveTask(approveForm.value);
    message.success('审批成功');
    approveVisible.value = false;
    loadTodoList({
      pageNum: todoPagination.value.current,
      pageSize: todoPagination.value.pageSize
    });
  } catch (error) {
    if (error?.errorFields) {
      return;
    }
    message.error('审批失败');
  } finally {
    approveLoading.value = false;
  }
};

// 查看流程图
const handleDiagram = async (record) => {
  try {
    // TODO: 调用获取流程图API
    // const response = await getProcessDiagram(record.processInstanceId);
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

onMounted(() => {
  loadTodoList({
    pageNum: todoPagination.value.current,
    pageSize: todoPagination.value.pageSize
  });
});
</script>

<style scoped>
.workflow-task-container {
  padding: 24px;
}
</style> 