<template>
  <div class="workflow-definition-container">
    <a-card :bordered="false">
      <template #title>
        <a-space>
          <a-button type="primary" @click="handleDeploy" v-permission="['workflow:definition:create']">
            <template #icon><upload-outlined /></template>
            部署流程
          </a-button>
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
            <a-form-item label="流程名称" name="name">
              <a-input v-model:value="queryParams.name" placeholder="请输入流程名称" allow-clear />
            </a-form-item>
          </a-col>
          <a-col :span="8">
            <a-form-item label="流程标识" name="key">
              <a-input v-model:value="queryParams.key" placeholder="请输入流程标识" allow-clear />
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

      <!-- 流程定义表格 -->
      <a-table
        :columns="columns"
        :data-source="definitionList"
        :loading="loading"
        :pagination="pagination"
        @change="handleTableChange"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'suspended'">
            <a-tag :color="record.suspended ? 'error' : 'success'">
              {{ record.suspended ? '已挂起' : '已激活' }}
            </a-tag>
          </template>
          <template v-if="column.key === 'action'">
            <a-space>
              <a-button type="link" @click="handleView(record)" v-permission="['workflow:definition:query']">
                查看
              </a-button>
              <a-button type="link" @click="handleStartForm(record)" v-permission="['workflow:definition:start']">
                启动
              </a-button>
              <a-dropdown>
                <template #overlay>
                  <a-menu>
                    <a-menu-item @click="handleState(record)" v-permission="['workflow:definition:update']">
                      {{ record.suspended ? '激活' : '挂起' }}
                    </a-menu-item>
                    <a-menu-item @click="handleDelete(record)" v-permission="['workflow:definition:delete']">
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

    <!-- 部署流程对话框 -->
    <a-modal
      v-model:visible="deployVisible"
      title="部署流程"
      :confirm-loading="deployLoading"
      @ok="handleDeployOk"
    >
      <a-form
        ref="deployFormRef"
        :model="deployForm"
        :rules="deployRules"
        :label-col="{ span: 4 }"
        :wrapper-col="{ span: 18 }"
      >
        <a-form-item label="流程文件" name="file" help="支持 .bpmn、.bpmn20.xml 格式">
          <a-upload
            v-model:file-list="fileList"
            :before-upload="beforeUpload"
            :max-count="1"
          >
            <a-button>
              <template #icon><upload-outlined /></template>
              选择文件
            </a-button>
          </a-upload>
        </a-form-item>
      </a-form>
    </a-modal>

    <!-- 启动流程对话框 -->
    <a-modal
      v-model:visible="startFormVisible"
      title="启动流程"
      :confirm-loading="startFormLoading"
      @ok="handleStartFormOk"
    >
      <a-form
        ref="startFormRef"
        :model="startForm"
        :rules="startFormRules"
        :label-col="{ span: 4 }"
        :wrapper-col="{ span: 18 }"
      >
        <a-form-item label="业务标题" name="title">
          <a-input v-model:value="startForm.title" placeholder="请输入业务标题" />
        </a-form-item>
        <a-form-item label="备注" name="remark">
          <a-textarea v-model:value="startForm.remark" placeholder="请输入备注" :rows="4" />
        </a-form-item>
      </a-form>
    </a-modal>

    <!-- 查看流程图对话框 -->
    <a-modal
      v-model:visible="viewVisible"
      title="查看流程图"
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
  UploadOutlined,
  ReloadOutlined,
  SearchOutlined,
  RedoOutlined,
  DownOutlined
} from '@ant-design/icons-vue';
import { message } from 'ant-design-vue';
import BpmnJS from 'bpmn-js';

// 查询参数
const queryFormRef = ref();
const queryParams = ref({
  name: '',
  key: ''
});

// 表格数据
const loading = ref(false);
const definitionList = ref([]);
const pagination = ref({
  current: 1,
  pageSize: 10,
  total: 0,
  showTotal: total => `共 ${total} 条`
});

// 部署相关
const deployVisible = ref(false);
const deployLoading = ref(false);
const deployFormRef = ref();
const deployForm = ref({});
const fileList = ref([]);
const deployRules = {
  file: [{ required: true, message: '请选择流程文件', trigger: 'change' }]
};

// 启动表单相关
const startFormVisible = ref(false);
const startFormLoading = ref(false);
const startFormRef = ref();
const startForm = ref({
  processDefinitionId: '',
  title: '',
  remark: ''
});
const startFormRules = {
  title: [{ required: true, message: '请输入业务标题', trigger: 'blur' }]
};

// 查看流程图相关
const viewVisible = ref(false);
const bpmnViewer = ref();
let viewer = null;

// 表格列定义
const columns = [
  {
    title: '流程名称',
    dataIndex: 'name',
    key: 'name',
    ellipsis: true
  },
  {
    title: '流程标识',
    dataIndex: 'key',
    key: 'key',
    width: 120
  },
  {
    title: '流程版本',
    dataIndex: 'version',
    key: 'version',
    width: 100
  },
  {
    title: '部署时间',
    dataIndex: 'deploymentTime',
    key: 'deploymentTime',
    width: 180
  },
  {
    title: '状态',
    dataIndex: 'suspended',
    key: 'suspended',
    width: 100
  },
  {
    title: '操作',
    key: 'action',
    width: 200
  }
];

// 加载流程定义列表
const loadDefinitionList = async (params = {}) => {
  try {
    loading.value = true;
    // TODO: 调用获取流程定义列表API
    // const response = await getProcessDefinitionList({
    //   pageNum: params.pageNum || pagination.value.current,
    //   pageSize: params.pageSize || pagination.value.pageSize,
    //   ...queryParams.value
    // });
    // definitionList.value = response.data.list;
    // pagination.value.total = response.data.total;
  } catch (error) {
    message.error('获取流程定义列表失败');
  } finally {
    loading.value = false;
  }
};

// 表格变化事件处理
const handleTableChange = (pag) => {
  pagination.value.current = pag.current;
  pagination.value.pageSize = pag.pageSize;
  loadDefinitionList({
    pageNum: pag.current,
    pageSize: pag.pageSize
  });
};

// 刷新按钮事件处理
const handleRefresh = () => {
  pagination.value.current = 1;
  loadDefinitionList({
    pageNum: 1,
    pageSize: pagination.value.pageSize
  });
};

// 查询按钮事件处理
const handleQuery = () => {
  pagination.value.current = 1;
  loadDefinitionList({
    pageNum: 1,
    pageSize: pagination.value.pageSize
  });
};

// 重置按钮事件处理
const handleReset = () => {
  queryFormRef.value?.resetFields();
  pagination.value.current = 1;
  loadDefinitionList({
    pageNum: 1,
    pageSize: pagination.value.pageSize
  });
};

// 部署流程相关方法
const handleDeploy = () => {
  deployForm.value = {};
  fileList.value = [];
  deployVisible.value = true;
};

const beforeUpload = (file) => {
  const isXML = file.type === 'text/xml' || file.name.endsWith('.bpmn') || file.name.endsWith('.bpmn20.xml');
  if (!isXML) {
    message.error('只能上传 .bpmn、.bpmn20.xml 格式的文件！');
  }
  return isXML || Upload.LIST_IGNORE;
};

const handleDeployOk = () => {
  deployFormRef.value?.validate().then(async () => {
    try {
      deployLoading.value = true;
      // TODO: 调用部署流程API
      message.success('部署成功');
      deployVisible.value = false;
      await loadDefinitionList({
        pageNum: 1,
        pageSize: pagination.value.pageSize
      });
    } catch (error) {
      message.error('部署失败');
    } finally {
      deployLoading.value = false;
    }
  });
};

// 启动流程相关方法
const handleStartForm = (record) => {
  startForm.value = {
    processDefinitionId: record.id,
    title: '',
    remark: ''
  };
  startFormVisible.value = true;
};

const handleStartFormOk = () => {
  startFormRef.value?.validate().then(async () => {
    try {
      startFormLoading.value = true;
      // TODO: 调用启动流程API
      message.success('启动成功');
      startFormVisible.value = false;
    } catch (error) {
      message.error('启动失败');
    } finally {
      startFormLoading.value = false;
    }
  });
};

// 查看流程图相关方法
const handleView = async (record) => {
  try {
    // TODO: 调用获取流程图API
    // const response = await getProcessDefinitionXML(record.id);
    // const xmlData = response.data;
    viewVisible.value = true;
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

// 激活/挂起流程
const handleState = async (record) => {
  try {
    // TODO: 调用激活/挂起流程API
    message.success(record.suspended ? '激活成功' : '挂起成功');
    await loadDefinitionList({
      pageNum: pagination.value.current,
      pageSize: pagination.value.pageSize
    });
  } catch (error) {
    message.error(record.suspended ? '激活失败' : '挂起失败');
  }
};

// 删除流程定义
const handleDelete = async (record) => {
  try {
    // TODO: 调用删除流程定义API
    message.success('删除成功');
    await loadDefinitionList({
      pageNum: pagination.value.current,
      pageSize: pagination.value.pageSize
    });
  } catch (error) {
    message.error('删除失败');
  }
};

onMounted(() => {
  loadDefinitionList({
    pageNum: pagination.value.current,
    pageSize: pagination.value.pageSize
  });
});
</script>

<style scoped>
.workflow-definition-container {
  padding: 24px;
}
</style> 