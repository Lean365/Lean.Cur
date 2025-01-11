import { createRouter, createWebHistory } from 'vue-router';
import type { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    path: '/login',
    name: 'Login',
    component: () => import('@/views/login/index.vue'),
    meta: {
      title: 'menu.login',
      hidden: true,
    },
  },
  {
    path: '/',
    name: 'Layout',
    component: () => import('@/layouts/default/index.vue'),
    redirect: '/dashboard',
    children: [
      {
        path: 'dashboard',
        name: 'Dashboard',
        component: () => import('@/views/dashboard/index.vue'),
        meta: {
          title: 'menu.dashboard',
          icon: 'dashboard',
        },
      },
      // 系统管理
      {
        path: 'system',
        name: 'System',
        component: () => import('@/views/system/index.vue'),
        meta: {
          title: 'menu.system.title',
          icon: 'setting',
        },
        children: [
          {
            path: 'user',
            name: 'User',
            component: () => import('@/views/system/user/index.vue'),
            meta: {
              title: 'menu.system.user',
              icon: 'user',
              permissions: ['system:user:list']
            }
          },
          {
            path: 'role',
            name: 'Role',
            component: () => import('@/views/system/role/index.vue'),
            meta: {
              title: 'menu.system.role',
              icon: 'team',
              permissions: ['system:role:list']
            }
          },
          {
            path: 'menu',
            name: 'Menu',
            component: () => import('@/views/system/menu/index.vue'),
            meta: {
              title: 'menu.system.menu',
              icon: 'menu',
              permissions: ['system:menu:list']
            }
          },
          {
            path: 'dept',
            name: 'Department',
            component: () => import('@/views/system/dept/index.vue'),
            meta: {
              title: 'menu.system.dept',
              icon: 'apartment',
              permissions: ['system:dept:list']
            }
          },
          {
            path: 'dict',
            name: 'Dictionary',
            component: () => import('@/views/system/dict/index.vue'),
            meta: {
              title: 'menu.system.dict',
              icon: 'book',
              permissions: ['system:dict:list']
            }
          }
        ]
      },
      // 日志管理
      {
        path: 'log',
        name: 'Log',
        component: () => import('@/views/log/index.vue'),
        meta: {
          title: 'menu.log.title',
          icon: 'file-text',
        },
        children: [
          {
            path: 'operation',
            name: 'OperationLog',
            component: () => import('@/views/log/operation/index.vue'),
            meta: {
              title: 'menu.log.operation',
              icon: 'profile',
              permissions: ['log:operation:list']
            }
          },
          {
            path: 'login',
            name: 'LoginLog',
            component: () => import('@/views/log/login/index.vue'),
            meta: {
              title: 'menu.log.login',
              icon: 'login',
              permissions: ['log:login:list']
            }
          },
          {
            path: 'system',
            name: 'SystemLog',
            component: () => import('@/views/log/system/index.vue'),
            meta: {
              title: 'menu.log.system',
              icon: 'bug',
              permissions: ['log:system:list']
            }
          }
        ]
      },
      // 工作流程
      {
        path: 'workflow',
        name: 'Workflow',
        component: () => import('@/views/workflow/index.vue'),
        meta: {
          title: 'menu.workflow.title',
          icon: 'deployment-unit',
        },
        children: [
          {
            path: 'definition',
            name: 'ProcessDefinition',
            component: () => import('@/views/workflow/definition/index.vue'),
            meta: {
              title: 'menu.workflow.definition',
              icon: 'appstore',
              permissions: ['workflow:definition:list']
            }
          },
          {
            path: 'instance',
            name: 'ProcessInstance',
            component: () => import('@/views/workflow/instance/index.vue'),
            meta: {
              title: 'menu.workflow.instance',
              icon: 'apartment',
              permissions: ['workflow:instance:list']
            }
          },
          {
            path: 'task',
            name: 'WorkflowTask',
            component: () => import('@/views/workflow/task/index.vue'),
            meta: {
              title: 'menu.workflow.task',
              icon: 'check-square',
              permissions: ['workflow:task:list']
            }
          }
        ]
      },
      // 代码生成
      {
        path: 'generator',
        name: 'Generator',
        component: () => import('@/views/generator/index.vue'),
        meta: {
          title: 'menu.generator.title',
          icon: 'code',
        },
        children: [
          {
            path: 'list',
            name: 'GeneratorList',
            component: () => import('@/views/generator/list/index.vue'),
            meta: {
              title: 'menu.generator.list',
              icon: 'table',
              permissions: ['generator:list']
            }
          },
          {
            path: 'history',
            name: 'GeneratorHistory',
            component: () => import('@/views/generator/history/index.vue'),
            meta: {
              title: 'menu.generator.history',
              icon: 'history',
              permissions: ['generator:history:list']
            }
          }
        ]
      },
      // 关于系统
      {
        path: 'about',
        name: 'About',
        component: () => import('@/views/about/index.vue'),
        meta: {
          title: 'menu.about.title',
          icon: 'info-circle',
        },
        children: [
          {
            path: 'version',
            name: 'Version',
            component: () => import('@/views/about/version/index.vue'),
            meta: {
              title: 'menu.about.version',
              icon: 'tag'
            }
          },
          {
            path: 'document',
            name: 'Document',
            component: () => import('@/views/about/document/index.vue'),
            meta: {
              title: 'menu.about.document',
              icon: 'file-text'
            }
          },
          {
            path: 'changelog',
            name: 'Changelog',
            component: () => import('@/views/about/changelog/index.vue'),
            meta: {
              title: 'menu.about.changelog',
              icon: 'history'
            }
          }
        ]
      }
    ],
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

// 路由守卫
router.beforeEach((to, from, next) => {
  // 设置标题
  document.title = `${to.meta.title} - ${import.meta.env.VITE_APP_TITLE}`;
  next();
});

export default router; 