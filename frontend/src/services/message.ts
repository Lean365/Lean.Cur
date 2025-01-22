import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import { message } from 'ant-design-vue';
import { ref, onMounted, onUnmounted } from 'vue';
import type { LeanMessageDto } from '@/types/message';

export class MessageSignalRService {
  private connection: HubConnection;
  private messageCallbacks: ((message: LeanMessageDto) => void)[] = [];
  
  constructor() {
    this.connection = new HubConnectionBuilder()
      .withUrl('/messageHub')
      .withAutomaticReconnect()
      .build();
      
    this.connection.on('ReceiveMessage', (message: LeanMessageDto) => {
      // 收到新消息时显示提示
      message.info(`收到新消息: ${message.content}`);
      
      // 调用所有注册的回调函数
      this.messageCallbacks.forEach(callback => callback(message));
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

  public async sendMessageToUser(message: LeanMessageDto) {
    try {
      await this.connection.invoke('SendMessageToUser', message);
    } catch (err) {
      console.error('Error sending message: ', err);
      throw err;
    }
  }

  public async sendMessageToRole(roleCode: string, message: LeanMessageDto) {
    try {
      await this.connection.invoke('SendMessageToRole', roleCode, message);
    } catch (err) {
      console.error('Error sending message to role: ', err);
      throw err;
    }
  }

  public async sendMessageToAll(message: LeanMessageDto) {
    try {
      await this.connection.invoke('SendMessageToAll', message);
    } catch (err) {
      console.error('Error sending message to all: ', err);
      throw err;
    }
  }

  public onMessage(callback: (message: LeanMessageDto) => void) {
    this.messageCallbacks.push(callback);
  }

  public removeMessageCallback(callback: (message: LeanMessageDto) => void) {
    const index = this.messageCallbacks.indexOf(callback);
    if (index > -1) {
      this.messageCallbacks.splice(index, 1);
    }
  }
}

// Vue Composition API Hook
export const useMessageSignalR = () => {
  const signalR = ref<MessageSignalRService | null>(null);
  
  onMounted(() => {
    signalR.value = new MessageSignalRService();
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