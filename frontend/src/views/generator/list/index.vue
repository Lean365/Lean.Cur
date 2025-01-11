<template>
  <div class="generator-container">
    <a-card :bordered="false">
      <template #title>
        <a-space>
          <a-button type="primary" @click="handleImport">
            <template #icon><upload-outlined /></template>
            导入表结构
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
            <a-form-item label="表名称" name="tableName">
              <a-input v-model:value="queryParams.tableName" placeholder="请输入表名称" allow-clear />
            </a-form-item>
          </a-col>
          <a-col :span="8">
            <a-form-item label="表描述" name="tableComment">
              <a-input v-model:value="queryParams.tableComment" placeholder="请输入表描述" allow-clear />
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
        :data-source="tableList"
        :loading="loading"
        :pagination="pagination"
        @change="handleTableChange"
        row-key="tableName"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'action'">
            <a-space>
              <a-button type="link" @click="handlePreview(record)" v-permission="['generator:preview']">
                预览
              </a-button>
              <a-button type="link" @click="handleEdit(record)" v-permission="['generator:edit']">
                编辑
              </a-button>
              <a-button type="link" @click="handleGenerate(record)" v-permission="['generator:code']">
                生成代码
              </a-button>
              <a-button type="link" @click="handleDelete(record)" v-permission="['generator:remove']" danger>
                删除
              </a-button>
            </a-space>
          </template>
        </template>
      </a-table>
    </a-card>

    <!-- 导入表结构对话框 -->
    <a-modal
      v-model:visible="importVisible"
      title="导入表结构"
      @ok="handleImportSubmit"
      :confirmLoading="importLoading"
    >
      <a-form
        ref="importFormRef"
        :model="importForm"
        :rules="importRules"
        :label-col="{ span: 4 }"
        :wrapper-col="{ span: 20 }"
      >
        <a-form-item label="表名称" name="tables">
          <a-select
            v-model:value="importForm.tables"
            mode="multiple"
            placeholder="请选择要导入的表"
            :loading="tablesLoading"
            :options="tableOptions"
          />
        </a-form-item>
      </a-form>
    </a-modal>

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

    <!-- 编辑对话框 -->
    <a-modal
      v-model:visible="editVisible"
      title="编辑生成配置"
      width="80%"
      @ok="handleEditSubmit"
      :confirmLoading="editLoading"
    >
      <a-form
        ref="editFormRef"
        :model="editForm"
        :rules="editRules"
        :label-col="{ span: 4 }"
        :wrapper-col="{ span: 20 }"
      >
        <a-form-item label="表名称" name="tableName">
          <a-input v-model:value="editForm.tableName" disabled />
        </a-form-item>
        <a-form-item label="表描述" name="tableComment">
          <a-input v-model:value="editForm.tableComment" placeholder="请输入表描述" />
        </a-form-item>
        <a-form-item label="实体类名称" name="className">
          <a-input v-model:value="editForm.className" placeholder="请输入实体类名称" />
        </a-form-item>
        <a-form-item label="包名" name="packageName">
          <a-input v-model:value="editForm.packageName" placeholder="请输入包名" />
        </a-form-item>
        <a-form-item label="模块名" name="moduleName">
          <a-input v-model:value="editForm.moduleName" placeholder="请输入模块名" />
        </a-form-item>
        <a-form-item label="作者" name="author">
          <a-input v-model:value="editForm.author" placeholder="请输入作者" />
        </a-form-item>

        <!-- 字段配置 -->
        <a-divider>字段配置</a-divider>
        <a-table
          :columns="columnConfigColumns"
          :data-source="editForm.columns"
          :pagination="false"
          bordered
        >
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'columnComment'">
              <a-input v-model:value="record.columnComment" placeholder="请输入字段描述" />
            </template>
            <template v-if="column.key === 'javaType'">
              <a-select v-model:value="record.javaType" style="width: 120px">
                <a-select-option value="String">String</a-select-option>
                <a-select-option value="Long">Long</a-select-option>
                <a-select-option value="Integer">Integer</a-select-option>
                <a-select-option value="Double">Double</a-select-option>
                <a-select-option value="BigDecimal">BigDecimal</a-select-option>
                <a-select-option value="Date">Date</a-select-option>
                <a-select-option value="Boolean">Boolean</a-select-option>
              </a-select>
            </template>
            <template v-if="column.key === 'javaField'">
              <a-input v-model:value="record.javaField" placeholder="请输入Java属性名" />
            </template>
            <template v-if="column.key === 'queryType'">
              <a-select v-model:value="record.queryType" style="width: 120px">
                <a-select-option value="EQ">=</a-select-option>
                <a-select-option value="NE">!=</a-select-option>
                <a-select-option value="GT">></a-select-option>
                <a-select-option value="GTE">>=</a-select-option>
                <a-select-option value="LT"><</a-select-option>
                <a-select-option value="LTE"><=</a-select-option>
                <a-select-option value="LIKE">LIKE</a-select-option>
                <a-select-option value="BETWEEN">BETWEEN</a-select-option>
              </a-select>
            </template>
            <template v-if="column.key === 'htmlType'">
              <a-select v-model:value="record.htmlType" style="width: 120px">
                <a-select-option value="input">文本框</a-select-option>
                <a-select-option value="textarea">文本域</a-select-option>
                <a-select-option value="select">下拉框</a-select-option>
                <a-select-option value="radio">单选框</a-select-option>
                <a-select-option value="checkbox">复选框</a-select-option>
                <a-select-option value="datetime">日期控件</a-select-option>
                <a-select-option value="imageUpload">图片上传</a-select-option>
                <a-select-option value="fileUpload">文件上传</a-select-option>
                <a-select-option value="editor">富文本控件</a-select-option>
              </a-select>
            </template>
            <template v-if="column.key === 'dictType'">
              <a-select
                v-model:value="record.dictType"
                style="width: 120px"
                placeholder="请选择字典类型"
                allow-clear
              >
                <a-select-option v-for="dict in dictTypeOptions" :key="dict.type" :value="dict.type">
                  {{ dict.name }}
                </a-select-option>
              </a-select>
            </template>
          </template>
        </a-table>
      </a-form>
    </a-modal>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import {
  SearchOutlined,
  RedoOutlined,
  UploadOutlined,
  ReloadOutlined
} from '@ant-design/icons-vue';
import { message } from 'ant-design-vue';

// 查询参数
const queryFormRef = ref();
const queryParams = ref({
  tableName: '',
  tableComment: ''
});

// 表格数据
const loading = ref(false);
const tableList = ref([]);
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
    title: '表描述',
    dataIndex: 'tableComment',
    key: 'tableComment',
    ellipsis: true
  },
  {
    title: '实体类名称',
    dataIndex: 'className',
    key: 'className',
    ellipsis: true
  },
  {
    title: '创建时间',
    dataIndex: 'createTime',
    key: 'createTime',
    width: 180
  },
  {
    title: '更新时间',
    dataIndex: 'updateTime',
    key: 'updateTime',
    width: 180
  },
  {
    title: '操作',
    key: 'action',
    width: 280
  }
];

// 导入相关
const importVisible = ref(false);
const importLoading = ref(false);
const tablesLoading = ref(false);
const importFormRef = ref();
const importForm = ref({
  tables: []
});
const tableOptions = ref([]);

const importRules = {
  tables: [{ required: true, message: '请选择要导入的表' }]
};

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

// 编辑相关
const editVisible = ref(false);
const editLoading = ref(false);
const editFormRef = ref();
const editForm = ref({
  tableName: '',
  tableComment: '',
  className: '',
  packageName: '',
  moduleName: '',
  author: '',
  columns: []
});

const editRules = {
  tableComment: [{ required: true, message: '请输入表描述' }],
  className: [{ required: true, message: '请输入实体类名称' }],
  packageName: [{ required: true, message: '请输入包名' }],
  moduleName: [{ required: true, message: '请输入模块名' }],
  author: [{ required: true, message: '请输入作者' }]
};

// 字段配置列定义
const columnConfigColumns = [
  {
    title: '字段名称',
    dataIndex: 'columnName',
    key: 'columnName',
    width: 180
  },
  {
    title: '字段描述',
    dataIndex: 'columnComment',
    key: 'columnComment',
    width: 180
  },
  {
    title: '物理类型',
    dataIndex: 'columnType',
    key: 'columnType',
    width: 120
  },
  {
    title: 'Java类型',
    dataIndex: 'javaType',
    key: 'javaType',
    width: 120
  },
  {
    title: 'Java属性名',
    dataIndex: 'javaField',
    key: 'javaField',
    width: 120
  },
  {
    title: '查询方式',
    dataIndex: 'queryType',
    key: 'queryType',
    width: 120
  },
  {
    title: '显示类型',
    dataIndex: 'htmlType',
    key: 'htmlType',
    width: 120
  },
  {
    title: '字典类型',
    dataIndex: 'dictType',
    key: 'dictType',
    width: 120
  }
];

// 字典类型选项
const dictTypeOptions = ref([]);

// 加载表格数据
const loadTableList = async (params = {}) => {
  try {
    loading.value = true;
    // TODO: 调用获取表列表API
    // const response = await getTableList({
    //   pageNum: params.pageNum || pagination.value.current,
    //   pageSize: params.pageSize || pagination.value.pageSize,
    //   ...queryParams.value
    // });
    // tableList.value = response.data.list;
    // pagination.value.total = response.data.total;
  } catch (error) {
    message.error('获取表列表失败');
  } finally {
    loading.value = false;
  }
};

// 加载数据库中的表
const loadDatabaseTables = async () => {
  try {
    tablesLoading.value = true;
    // TODO: 调用获取数据库表API
    // const response = await getDatabaseTables();
    // tableOptions.value = response.data.map(table => ({
    //   label: `${table.tableName}（${table.tableComment}）`,
    //   value: table.tableName
    // }));
  } catch (error) {
    message.error('获取数据库表失败');
  } finally {
    tablesLoading.value = false;
  }
};

// 加载字典类型
const loadDictTypes = async () => {
  try {
    // TODO: 调用获取字典类型API
    // const response = await getDictTypeList();
    // dictTypeOptions.value = response.data;
  } catch (error) {
    message.error('获取字典类型失败');
  }
};

// 表格变化事件处理
const handleTableChange = (pag) => {
  pagination.value.current = pag.current;
  pagination.value.pageSize = pag.pageSize;
  loadTableList({
    pageNum: pag.current,
    pageSize: pag.pageSize
  });
};

// 刷新按钮事件处理
const handleRefresh = () => {
  pagination.value.current = 1;
  loadTableList({
    pageNum: 1,
    pageSize: pagination.value.pageSize
  });
};

// 查询按钮事件处理
const handleQuery = () => {
  pagination.value.current = 1;
  loadTableList({
    pageNum: 1,
    pageSize: pagination.value.pageSize
  });
};

// 重置按钮事件处理
const handleReset = () => {
  queryFormRef.value?.resetFields();
  pagination.value.current = 1;
  loadTableList({
    pageNum: 1,
    pageSize: pagination.value.pageSize
  });
};

// 导入按钮事件处理
const handleImport = async () => {
  await loadDatabaseTables();
  importForm.value.tables = [];
  importVisible.value = true;
};

// 提交导入
const handleImportSubmit = async () => {
  try {
    await importFormRef.value?.validate();
    importLoading.value = true;
    // TODO: 调用导入表API
    // await importTables(importForm.value);
    message.success('导入成功');
    importVisible.value = false;
    handleRefresh();
  } catch (error) {
    if (error?.errorFields) {
      return;
    }
    message.error('导入失败');
  } finally {
    importLoading.value = false;
  }
};

// 预览按钮事件处理
const handlePreview = async (record) => {
  try {
    // TODO: 调用预览代码API
    // const response = await previewCode(record.tableName);
    // previewData.value = response.data;
    previewVisible.value = true;
  } catch (error) {
    message.error('预览失败');
  }
};

// 编辑按钮事件处理
const handleEdit = async (record) => {
  try {
    // TODO: 调用获取表详情API
    // const response = await getTableInfo(record.tableName);
    // editForm.value = response.data;
    editVisible.value = true;
  } catch (error) {
    message.error('获取表详情失败');
  }
};

// 提交编辑
const handleEditSubmit = async () => {
  try {
    await editFormRef.value?.validate();
    editLoading.value = true;
    // TODO: 调用更新生成配置API
    // await updateGenConfig(editForm.value);
    message.success('保存成功');
    editVisible.value = false;
    handleRefresh();
  } catch (error) {
    if (error?.errorFields) {
      return;
    }
    message.error('保存失败');
  } finally {
    editLoading.value = false;
  }
};

// 生成代码按钮事件处理
const handleGenerate = async (record) => {
  try {
    // TODO: 调用生成代码API
    // await generateCode(record.tableName);
    message.success('生成成功');
  } catch (error) {
    message.error('生成失败');
  }
};

// 删除按钮事件处理
const handleDelete = async (record) => {
  try {
    // TODO: 调用删除表API
    // await deleteTable(record.tableName);
    message.success('删除成功');
    handleRefresh();
  } catch (error) {
    message.error('删除失败');
  }
};

onMounted(() => {
  loadTableList({
    pageNum: pagination.value.current,
    pageSize: pagination.value.pageSize
  });
  loadDictTypes();
});
</script>

<style scoped>
.generator-container {
  padding: 24px;
}

:deep(.ant-card-body) {
  padding-top: 0;
}
</style> 