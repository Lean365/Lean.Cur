import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import { message } from 'ant-design-vue';
import { ref, onMounted, onUnmounted } from 'vue';
import type { LeanNoticeDto } from '@/types/notice';

export class NoticeSignalRService {
  private connection: HubConnection;
  
  constructor() {
    this.connection = new HubConnectionBuilder()
      .withUrl('/noticeHub')
      .withAutomaticReconnect()
      .build();
      
    this.connection.on('ReceiveNotice', (notice: LeanNoticeDto) => {
      // 收到新通知时显示提示
      message.info(`收到新通知: ${notice.noticeTitle}`);
      // TODO: 更新通知列表、未读数量等
    });
  }
  
  public async start() {
    try {
      await this.connection.start();
      console.log('SignalR Connected');
    } catch (err) {
      console.error('SignalR Connection Error: ', err);
      setTimeout(() => this.start(), 5000);
    }
  }
  
  public async stop() {
    try {
      await this.connection.stop();
      console.log('SignalR Disconnected');
    } catch (err) {
      console.error('SignalR Disconnection Error: ', err);
    }
  }
}

// Vue Composition API Hook
export const useNoticeSignalR = () => {
  const signalR = ref<NoticeSignalRService | null>(null);
  
  onMounted(() => {
    signalR.value = new NoticeSignalRService();
    signalR.value.start();
  });
  
  onUnmounted(() => {
    if (signalR.value) {
      signalR.value.stop();
    }
  });
  
  return {
    signalR
  };
}; 