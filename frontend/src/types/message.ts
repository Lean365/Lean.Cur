export interface LeanMessageDto {
  id: number;
  senderId: number;
  senderName: string;
  receiverId: number;
  receiverName: string;
  content: string;
  type: number;
  isRead: boolean;
  readTime?: Date;
  sendTime: Date;
  deviceType?: string;
  deviceName?: string;
  ipAddress: string;
  browser?: string;
  os?: string;
  location: string;
  createTime: Date;
  updateTime?: Date;
}

export interface LeanMessageCreateDto {
  receiverId: number;
  content: string;
  type: number;
  deviceType?: string;
  deviceName?: string;
  ipAddress: string;
  browser?: string;
  os?: string;
  location: string;
}

export interface LeanMessageQueryDto {
  targetUserId: number;
  type?: number;
  deviceType?: string;
  startTime?: Date;
  endTime?: Date;
  pageIndex: number;
  pageSize: number;
}

export interface LeanMessageContactDto {
  userId: number;
  userName: string;
  lastMessage: string;
  lastTime: Date;
  unreadCount: number;
  deviceType: string;
  location: string;
} 