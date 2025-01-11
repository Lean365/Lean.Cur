export default {
  title: '用户管理',
  list: {
    search: '搜索用户',
    add: '添加用户',
    import: '导入用户',
    export: '导出用户',
  },
  table: {
    id: '用户ID',
    username: '用户名',
    nickname: '昵称',
    email: '邮箱',
    phone: '手机号',
    role: '角色',
    status: '状态',
    createTime: '创建时间',
    lastLogin: '最后登录',
    actions: '操作',
  },
  status: {
    active: '正常',
    inactive: '禁用',
    locked: '锁定',
  },
  form: {
    basicInfo: '基本信息',
    accountInfo: '账号信息',
    roleInfo: '角色信息',
    required: '此项为必填项',
    username: {
      label: '用户名',
      placeholder: '请输入用户名',
      rules: '用户名只能包含字母、数字和下划线',
    },
    password: {
      label: '密码',
      placeholder: '请输入密码',
      rules: '密码长度必须在6-20个字符之间',
    },
    email: {
      label: '邮箱',
      placeholder: '请输入邮箱',
      rules: '请输入有效的邮箱地址',
    },
    phone: {
      label: '手机号',
      placeholder: '请输入手机号',
      rules: '请输入有效的手机号',
    },
  },
  messages: {
    createSuccess: '用户创建成功',
    updateSuccess: '用户更新成功',
    deleteSuccess: '用户删除成功',
    deleteConfirm: '确定要删除该用户吗？',
  },
}; 