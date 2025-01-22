/** 通知公告DTO */
export interface LeanNoticeDto {
  /** ID */
  id: number;
  
  /** 标题 */
  noticeTitle: string;
  
  /** 内容 */
  noticeContent: string;
  
  /** 类型 */
  type: number;
  
  /** 状态 */
  status: boolean;
  
  /** 发布时间 */
  publishTime?: Date;
  
  /** 发布人 */
  publisher?: string;
  
  /** 附件名称 */
  fileName?: string;
  
  /** 附件路径 */
  filePath?: string;
  
  /** 附件大小 */
  fileSize?: number;
  
  /** 附件类型 */
  fileType?: string;
  
  /** 上传时间 */
  uploadTime?: Date;
  
  /** 备注 */
  remark?: string;
  
  /** 创建时间 */
  createTime: Date;
  
  /** 更新时间 */
  updateTime: Date;
} 