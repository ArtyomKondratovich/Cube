import { defineStore } from 'pinia'
import {
  type IResponse,
  type INotificationModel,
  type IUserNotifications
} from '@/api/types';
import config from '@/config';
import axios from 'axios';
import 'vue3-toastify/dist/index.css'
import getUserIdFromLocalStorage from '@/helpers/getFromLocalStorage';

interface INotificationState {
  unreadNotifications: INotificationModel[];
  readedNotifications: INotificationModel[];
  userId: number;
}

export interface INotificationStore {
  fecthUserNotifications(): void;
  updateNotificationData(): void;
  readNotifications(readNotificationIds: number[]): void;
  deleteReadedNotifications(): void;
  getChatNotifications(chatId: number): INotificationModel[];
  getMessangerNotifications: INotificationModel[];
  getPostsNotifications: INotificationModel[];
  getFriendNotifications: INotificationModel[];
  $state: INotificationState;
}

export const useNotificationStore = defineStore(
  'notification', {
    state: (): INotificationState => ({
        unreadNotifications: [] as INotificationModel[],
        readedNotifications: [] as INotificationModel[],
        userId: getUserIdFromLocalStorage()
    }),
    actions: {
        async fecthUserNotifications() {
          axios.post(`${config.apiUrl}/Notification/get`, { id: this.userId }, 
          {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + `${localStorage.getItem('token')}`
            }
          }).then(response => {
            const data = response.data as IResponse<IUserNotifications>;
            if (data.responseResult == 'Success' && data.value){
              data.value.notifications.forEach(notification => {
                if (!this.unreadNotifications.find(x => x.id == notification.id) && !this.readedNotifications.find(x => x.id == notification.id)){
                  this.unreadNotifications.push(notification);
                }
              })
            }
          }).catch(error => console.log(error));
        },
        async updateNotificationData() {
          await this.fecthUserNotifications();
          if (this.readedNotifications.length > 0)
          {
            await this.deleteReadedNotifications();
          }
        },
        readNotifications(readNotificationIds: number[]) {
          readNotificationIds.forEach(id => {
            const index = this.unreadNotifications.findIndex(item => item.id == id && item.userId == this.userId);

            if (index != -1)
            {
              const notification = this.unreadNotifications[index];
              this.unreadNotifications.splice(index, 1);
              notification.isReaded = true;
              this.readedNotifications.push(notification);
            }
          });
        },
        async deleteReadedNotifications() {
          const ids = this.readedNotifications.map(item => item.id);
          console.log('Удаляются уведомления:', ids);
          axios.post(`${config.apiUrl}/Notification/delete`, { readedNotificationTds: ids },
          {
              headers: {
                  'Content-Type': 'application/json',
                  'Authorization': 'Bearer ' + `${localStorage.getItem('token')}`
              }
          }).then(response => {
              const data = response.data as IResponse<boolean>;
              if (data.responseResult == 'Success' && data.value)
              {
                  this.readedNotifications = [];
              }
          }).catch(error => console.log(error));
        },
        getChatNotifications(chatId: number): INotificationModel[] {
          return this.unreadNotifications
          .filter(x => x.notificationSenderId == chatId 
            && !x.isReaded
            && x.type=='ChatNotification');
        }
    },
    getters: {
      getMessangerNotifications(): INotificationModel[] {
        return this.unreadNotifications.filter(x => x.type == 'ChatNotification' && !x.isReaded);
      },
      getPostsNotifications(): INotificationModel[] {
        return this.unreadNotifications.filter(x => x.type == 'NewsNotification' && !x.isReaded);
      },
      getFriendNotifications(): INotificationModel[] {
        return this.unreadNotifications.filter(x => (x.type == 'FriendRequest' || x.type=='FriendResponse') && !x.isReaded);
      }
    }
  }
);