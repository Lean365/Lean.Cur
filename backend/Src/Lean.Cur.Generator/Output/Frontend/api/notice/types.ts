export interface LeanNotice {
  id: number
  title: string
  content: string
  type: number
  status: boolean
  publishtime: string| null
  publisher: string| null
  createTime: string
  createBy: number
  updateTime: string | null
  updateBy: number | null
  remark: string | null
} 