import 'vue';

declare module '@vue/runtime-dom' {
  export interface GlobalComponents {
    ADropdown: typeof import('ant-design-vue')['Dropdown'];
    AMenu: typeof import('ant-design-vue')['Menu'];
    AMenuItem: typeof import('ant-design-vue')['MenuItem'];
    BulbOutlined: typeof import('@ant-design/icons-vue')['BulbOutlined'];
    DownOutlined: typeof import('@ant-design/icons-vue')['DownOutlined'];
  }

  export interface ComponentCustomProperties {
    $t: (key: string) => string;
  }
}

declare module '@vue/runtime-core' {
  export interface ComponentCustomProperties {
    $t: (key: string) => string;
  }
}

export {}; 