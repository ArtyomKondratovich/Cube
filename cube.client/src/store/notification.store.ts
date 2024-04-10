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
  notifications: INotificationModel[];
  userId: number;
}

export interface INotificationStore {
  fecthUserNotifications(): void;
  updateNotificationData(): void;
  readNotifications(readedIds: number[]): void;
  getChatNotifications(chatId: number): INotificationModel[];
  login(id: number): void;
  logout(): void;
  getMessangerNotifications: INotificationModel[];
  getPostsNotifications: INotificationModel[];
  getFriendNotifications: INotificationModel[];
  $state: INotificationState;
}

export const useNotificationStore = defineStore(
  'notification', {
    state: (): INotificationState => ({
        notifications: [] as INotificationModel[],
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
                if (!this.notifications.find(x => x.id == notification.id)){
                  this.notifications.push(notification);
                }
              })
            }
          }).catch(error => console.log(error));
        },
        async updateNotificationData() {
          await this.fecthUserNotifications();
        },
        readNotifications(readedIds: number[]): void {
          readedIds.forEach(id => {
            const index = this.notifications.findIndex(item => item.id == id && item.userId == this.userId);

            if (index != -1)
            {
              this.notifications[index].isReaded = true;
            }
          });
          
          readedIds = this.notifications.filter(x => x.isReaded).map(x => x.id);

          axios.post(`${config.apiUrl}/Notification/delete`, { readedNotificationTds: readedIds },
          {
              headers: {
                  'Content-Type': 'application/json',
                  'Authorization': 'Bearer ' + `${localStorage.getItem('token')}`
              }
          }).then(response => {
          }).catch(error => console.log(error));

        },
        getChatNotifications(chatId: number): INotificationModel[] {
          return this.notifications
          .filter(x => x.notificationSenderId == chatId 
            && !x.isReaded
            && x.type=='ChatNotification');
        },
        login(id: number): void {
          this.$state.userId = id;
          this.updateNotificationData();
        },
        logout(): void {
          this.notifications = [];
          this.$state.userId = 0;
        }
    },
    getters: {
      getMessangerNotifications(): INotificationModel[] {
        return this.notifications.filter(x => x.type == 'ChatNotification' && !x.isReaded);
      },
      getPostsNotifications(): INotificationModel[] {
        return this.notifications.filter(x => x.type == 'NewsNotification' && !x.isReaded);
      },
      getFriendNotifications(): INotificationModel[] {
        return this.notifications.filter(x => (x.type == 'FriendRequest' || x.type=='FriendResponse') && !x.isReaded);
      }
    }
  }
);