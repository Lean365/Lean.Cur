import type { LeanMessageDto, LeanMessageCreateDto, LeanMessageQueryDto, LeanMessageContactDto } from '@/types/message';
import { request } from '@/utils/request';

const baseUrl = '/api/routine/message';

export const MessageApi = {
  /**
   * 获取消息分页列表
   * @param params 查询参数
   */
  getPagedList: (params: LeanMessageQueryDto) => {
    return request.get(`${baseUrl}/page`, { params });
  },

  /**
   * 获取消息列表
   * @param params 查询参数
   */
  getList: (params: LeanMessageQueryDto) => {
    return request.get(`${baseUrl}/list`, { params });
  },

  /**
   * 获取消息详情
   * @param id 消息ID
   */
  getById: (id: number) => {
    return request.get(`${baseUrl}/${id}`);
  },

  /**
   * 发送消息
   * @param data 消息内容
   */
  sendMessage: (data: LeanMessageCreateDto) => {
    return request.post(baseUrl, data);
  },

  /**
   * 删除消息
   * @param id 消息ID
   */
  delete: (id: number) => {
    return request.delete(`${baseUrl}/${id}`);
  },

  /**
   * 批量删除消息
   * @param ids 消息ID列表
   */
  batchDelete: (ids: number[]) => {
    return request.delete(`${baseUrl}/batch`, { data: ids });
  },

  /**
   * 获取聊天历史记录
   * @param params 查询参数
   */
  getChatHistory: (params: LeanMessageQueryDto) => {
    return request.get(`${baseUrl}/chat-history`, { params });
  },

  /**
   * 获取最近联系人列表
   * @param userId 用户ID
   * @param count 获取数量
   */
  getRecentContacts: (userId: number, count: number = 20) => {
    return request.get(`${baseUrl}/recent-contacts/${userId}`, { params: { count } });
  },

  /**
   * 获取未读消息列表
   * @param userId 用户ID
   */
  getUnreadMessages: (userId: number) => {
    return request.get(`${baseUrl}/unread/${userId}`);
  },

  /**
   * 获取未读消息数量
   * @param userId 用户ID
   */
  getUnreadCount: (userId: number) => {
    return request.get(`${baseUrl}/unread-count/${userId}`);
  },

  /**
   * 标记消息为已读
   * @param messageId 消息ID
   */
  markAsRead: (messageId: number) => {
    return request.post(`${baseUrl}/mark-read/${messageId}`);
  },

  /**
   * 批量标记消息为已读
   * @param messageIds 消息ID列表
   */
  batchMarkAsRead: (messageIds: number[]) => {
    return request.post(`${baseUrl}/batch-mark-read`, messageIds);
  }
}; 