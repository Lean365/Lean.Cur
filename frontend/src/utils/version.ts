// 获取当前版本号
export const getVersion = () => {
  return import.meta.env.VITE_APP_VERSION || '0.0.1'
}

// 显示版本号
export const showVersion = () => {
  console.log(`Version: ${getVersion()}`)
} 