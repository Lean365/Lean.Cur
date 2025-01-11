<template>
  <div class="settings-container">
    <a-card :bordered="false">
      <a-tabs v-model:activeKey="activeTab">
        <!-- 基本设置 -->
        <a-tab-pane key="general" :tab="$t('settings.tabs.general')">
          <a-form :label-col="{ span: 4 }" :wrapper-col="{ span: 14 }">
            <!-- 语言设置 -->
            <a-form-item :label="$t('settings.general.language')">
              <a-select v-model:value="settings.language">
                <a-select-option value="zh-CN">简体中文</a-select-option>
                <a-select-option value="en-US">English</a-select-option>
              </a-select>
            </a-form-item>
            <!-- 时区设置 -->
            <a-form-item :label="$t('settings.general.timezone')">
              <a-select v-model:value="settings.timezone">
                <a-select-option value="UTC+8">UTC+8 (北京时间)</a-select-option>
                <a-select-option value="UTC+0">UTC+0 (格林威治时间)</a-select-option>
              </a-select>
            </a-form-item>
            <!-- 日期格式 -->
            <a-form-item :label="$t('settings.general.dateFormat')">
              <a-select v-model:value="settings.dateFormat">
                <a-select-option value="YYYY-MM-DD">YYYY-MM-DD</a-select-option>
                <a-select-option value="DD/MM/YYYY">DD/MM/YYYY</a-select-option>
                <a-select-option value="MM/DD/YYYY">MM/DD/YYYY</a-select-option>
              </a-select>
            </a-form-item>
            <!-- 时间格式 -->
            <a-form-item :label="$t('settings.general.timeFormat')">
              <a-select v-model:value="settings.timeFormat">
                <a-select-option value="HH:mm:ss">24小时制 (HH:mm:ss)</a-select-option>
                <a-select-option value="hh:mm:ss a">12小时制 (hh:mm:ss a)</a-select-option>
              </a-select>
            </a-form-item>
          </a-form>
        </a-tab-pane>

        <!-- 安全设置 -->
        <a-tab-pane key="security" :tab="$t('settings.tabs.security')">
          <a-list item-layout="horizontal">
            <!-- 修改密码 -->
            <a-list-item>
              <a-list-item-meta :title="$t('settings.security.changePassword')">
                <template #description>
                  {{ $t('settings.security.changePasswordDesc') }}
                </template>
              </a-list-item-meta>
              <template #extra>
                <a-button type="primary" @click="showChangePasswordModal">
                  {{ $t('settings.actions.change') }}
                </a-button>
              </template>
            </a-list-item>
            <!-- 两步验证 -->
            <a-list-item>
              <a-list-item-meta :title="$t('settings.security.twoFactor')">
                <template #description>
                  {{ $t('settings.security.twoFactorDesc') }}
                </template>
              </a-list-item-meta>
              <template #extra>
                <a-switch v-model:checked="settings.twoFactorEnabled" />
              </template>
            </a-list-item>
            <!-- 登录历史 -->
            <a-list-item>
              <a-list-item-meta :title="$t('settings.security.loginHistory')">
                <template #description>
                  {{ $t('settings.security.loginHistoryDesc') }}
                </template>
              </a-list-item-meta>
              <template #extra>
                <a-button @click="showLoginHistory">
                  {{ $t('settings.actions.view') }}
                </a-button>
              </template>
            </a-list-item>
          </a-list>
        </a-tab-pane>

        <!-- 通知设置 -->
        <a-tab-pane key="notification" :tab="$t('settings.tabs.notification')">
          <a-list item-layout="horizontal">
            <!-- 邮件通知 -->
            <a-list-item>
              <a-list-item-meta :title="$t('settings.notification.email')">
                <template #description>
                  {{ $t('settings.notification.emailDesc') }}
                </template>
              </a-list-item-meta>
              <template #extra>
                <a-switch v-model:checked="settings.emailNotification" />
              </template>
            </a-list-item>
            <!-- 推送通知 -->
            <a-list-item>
              <a-list-item-meta :title="$t('settings.notification.push')">
                <template #description>
                  {{ $t('settings.notification.pushDesc') }}
                </template>
              </a-list-item-meta>
              <template #extra>
                <a-switch v-model:checked="settings.pushNotification" />
              </template>
            </a-list-item>
            <!-- 通知频率 -->
            <a-list-item>
              <a-list-item-meta :title="$t('settings.notification.frequency')">
                <template #description>
                  {{ $t('settings.notification.frequencyDesc') }}
                </template>
              </a-list-item-meta>
              <template #extra>
                <a-select v-model:value="settings.notificationFrequency" style="width: 200px">
                  <a-select-option value="realtime">实时</a-select-option>
                  <a-select-option value="daily">每日汇总</a-select-option>
                  <a-select-option value="weekly">每周汇总</a-select-option>
                </a-select>
              </template>
            </a-list-item>
          </a-list>
        </a-tab-pane>

        <!-- 外观设置 -->
        <a-tab-pane key="appearance" :tab="$t('settings.tabs.appearance')">
          <a-form :label-col="{ span: 4 }" :wrapper-col="{ span: 14 }">
            <!-- 主题设置 -->
            <a-form-item :label="$t('settings.appearance.theme')">
              <a-radio-group v-model:value="settings.theme">
                <a-radio value="light">{{ $t('settings.appearance.light') }}</a-radio>
                <a-radio value="dark">{{ $t('settings.appearance.dark') }}</a-radio>
                <a-radio value="system">{{ $t('settings.appearance.system') }}</a-radio>
              </a-radio-group>
            </a-form-item>
            <!-- 紧凑模式 -->
            <a-form-item :label="$t('settings.appearance.compactMode')">
              <a-switch v-model:checked="settings.compactMode" />
            </a-form-item>
            <!-- 字体大小 -->
            <a-form-item :label="$t('settings.appearance.fontSize')">
              <a-slider
                v-model:value="settings.fontSize"
                :min="12"
                :max="20"
                :step="1"
                :marks="{
                  12: '12px',
                  14: '14px',
                  16: '16px',
                  18: '18px',
                  20: '20px'
                }"
              />
            </a-form-item>
          </a-form>
        </a-tab-pane>
      </a-tabs>

      <!-- 底部操作按钮 -->
      <div class="settings-footer">
        <a-space>
          <a-button @click="handleResetSettings">{{ t('settings.actions.reset') }}</a-button>
          <a-button type="primary" @click="handleSaveSettings">{{ t('settings.actions.save') }}</a-button>
        </a-space>
      </div>
    </a-card>

    <!-- 修改密码弹窗 -->
    <a-modal
      v-model:visible="changePasswordVisible"
      :title="$t('settings.security.changePassword')"
      @ok="handleChangePassword"
    >
      <a-form :model="passwordForm" :rules="passwordRules">
        <a-form-item name="oldPassword" :label="$t('settings.security.oldPassword')">
          <a-input-password v-model:value="passwordForm.oldPassword" />
        </a-form-item>
        <a-form-item name="newPassword" :label="$t('settings.security.newPassword')">
          <a-input-password v-model:value="passwordForm.newPassword" />
        </a-form-item>
        <a-form-item name="confirmPassword" :label="$t('settings.security.confirmPassword')">
          <a-input-password v-model:value="passwordForm.confirmPassword" />
        </a-form-item>
      </a-form>
    </a-modal>

    <!-- 登录历史弹窗 -->
    <a-modal
      v-model:visible="loginHistoryVisible"
      :title="$t('settings.security.loginHistory')"
      width="800px"
    >
      <a-table :columns="loginHistoryColumns" :data-source="loginHistoryData" :pagination="false">
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'status'">
            <a-tag :color="record.status === 'success' ? 'success' : 'error'">
              {{ record.status }}
            </a-tag>
          </template>
        </template>
      </a-table>
    </a-modal>
  </div>
</template>

<script setup>
import { ref, reactive, computed } from 'vue';
import { message } from 'ant-design-vue';
import { useI18n } from 'vue-i18n';
import { useUserStore } from '@/stores/user';
import { useAppStore } from '@/stores/app';
import { useSettingsStore } from '@/stores/settings';

const { t } = useI18n();
const userStore = useUserStore();
const appStore = useAppStore();
const settingsStore = useSettingsStore();

// 当前激活的标签页
const activeTab = ref('general');

// 设置数据
const settings = computed(() => settingsStore.settings);

// 修改密码相关
const changePasswordVisible = ref(false);
const passwordForm = reactive({
  oldPassword: '',
  newPassword: '',
  confirmPassword: '',
});

const passwordRules = {
  oldPassword: [
    { required: true, message: t('settings.security.oldPasswordRequired') },
  ],
  newPassword: [
    { required: true, message: t('settings.security.newPasswordRequired') },
    { min: 6, message: t('settings.security.passwordMinLength') },
  ],
  confirmPassword: [
    { required: true, message: t('settings.security.confirmPasswordRequired') },
    {
      validator: (rule, value) => {
        if (value !== passwordForm.newPassword) {
          return Promise.reject(t('settings.security.passwordNotMatch'));
        }
        return Promise.resolve();
      },
    },
  ],
};

// 登录历史相关
const loginHistoryVisible = ref(false);
const loginHistoryColumns = [
  {
    title: '登录时间',
    dataIndex: 'loginTime',
    key: 'loginTime'
  },
  {
    title: '登录IP',
    dataIndex: 'ip',
    key: 'ip'
  },
  {
    title: '登录设备',
    dataIndex: 'device',
    key: 'device'
  },
  {
    title: '登录状态',
    dataIndex: 'status',
    key: 'status'
  }
];
const loginHistoryData = ref([
  {
    key: '1',
    loginTime: '2024-01-20 10:00:00',
    ip: '192.168.1.1',
    device: 'Chrome on Windows',
    status: 'success'
  }
]);

// 显示修改密码弹窗
const showChangePasswordModal = () => {
  passwordForm.oldPassword = '';
  passwordForm.newPassword = '';
  passwordForm.confirmPassword = '';
  changePasswordVisible.value = true;
};

// 显示登录历史弹窗
const showLoginHistory = () => {
  loginHistoryVisible.value = true;
};

// 处理修改密码
const handleChangePassword = async () => {
  try {
    await userStore.changePassword(
      passwordForm.oldPassword,
      passwordForm.newPassword
    );
    message.success(t('settings.messages.passwordChanged'));
    changePasswordVisible.value = false;
  } catch (error) {
    message.error(t('settings.messages.passwordChangeFailed'));
  }
};

// 重置设置
const handleResetSettings = async () => {
  try {
    await settingsStore.resetSettings();
    message.success(t('settings.messages.resetSuccess'));
  } catch (error) {
    message.error(t('settings.messages.resetError'));
  }
};

// 保存设置
const handleSaveSettings = async () => {
  try {
    await settingsStore.saveSettings(settings.value);
    message.success(t('settings.messages.saveSuccess'));
  } catch (error) {
    message.error(t('settings.messages.saveError'));
  }
};
</script>

<style lang="less" scoped>
.settings-container {
  padding: 24px;
  background-color: var(--ant-layout-body-background);

  .settings-footer {
    margin-top: 24px;
    text-align: right;
  }
}
</style> 