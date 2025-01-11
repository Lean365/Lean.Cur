export default {
  title: 'User Management',
  list: {
    search: 'Search Users',
    add: 'Add User',
    import: 'Import Users',
    export: 'Export Users',
  },
  table: {
    id: 'User ID',
    username: 'Username',
    nickname: 'Nickname',
    email: 'Email',
    phone: 'Phone',
    role: 'Role',
    status: 'Status',
    createTime: 'Created At',
    lastLogin: 'Last Login',
    actions: 'Actions',
  },
  status: {
    active: 'Active',
    inactive: 'Inactive',
    locked: 'Locked',
  },
  form: {
    basicInfo: 'Basic Information',
    accountInfo: 'Account Information',
    roleInfo: 'Role Information',
    required: 'This field is required',
    username: {
      label: 'Username',
      placeholder: 'Enter username',
      rules: 'Username can only contain letters, numbers and underscores',
    },
    password: {
      label: 'Password',
      placeholder: 'Enter password',
      rules: 'Password must be between 6-20 characters',
    },
    email: {
      label: 'Email',
      placeholder: 'Enter email',
      rules: 'Please enter a valid email address',
    },
    phone: {
      label: 'Phone',
      placeholder: 'Enter phone number',
      rules: 'Please enter a valid phone number',
    },
  },
  messages: {
    createSuccess: 'User created successfully',
    updateSuccess: 'User updated successfully',
    deleteSuccess: 'User deleted successfully',
    deleteConfirm: 'Are you sure you want to delete this user?',
  },
}; 