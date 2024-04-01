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

interface NotificationState {
  unreadNotifications: INotificationModel[];
  readedNotifications: INotificationModel[];
  userId: number;
  unreadMessages: number;
  unreadFriendNotifications: number;
  unreadPosts: number;
}

export const useNotificationStore = defineStore(
  'notification', {
    state: (): NotificationState => ({
        unreadNotifications: [],
        readedNotifications: [],
        userId: getUserIdFromLocalStorage(),
        unreadMessages: 0,
        unreadPosts: 0,
        unreadFriendNotifications: 0
    }),
    actions: {
        fecthUserNotifications(): void {
            axios.post(`${config.apiUrl}/Notification/get`, { id: this.userId }, 
            {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + `${localStorage.getItem('token')}`
                }
            }).then(response => {
                const data = response.data as IResponse<IUserNotifications>;

                if (data.responseResult == 'Success' && data.value){
                    this.unreadNotifications = data.value.notifications;
                }
            }).catch(error => console.log(error));
        },
        updateNotificationData(): void {
            this.fecthUserNotifications();
            this.unreadMessages = this.unreadNotifications.filter(x => x.type == 'ChatNotification' && !x.isReaded).length;
            this.unreadPosts = this.unreadNotifications.filter(x => x.type == 'NewsNotification' && !x.isReaded).length;
            this.unreadFriendNotifications = this.unreadNotifications.filter(x => (x.type == 'FriendRequest' || x.type=='FriendResponse') && !x.isReaded).length;
            if (this.readedNotifications.length > 0)
            {
              this.deleteReadedNotifications();
            }
        },
        readNotification(notifId: number): void {

            const index = this.unreadNotifications.findIndex(item => item.id == notifId && item.userId == this.userId);

            if (index != -1)
            {
              console.log('asdasdasdasd')
              const notification = this.unreadNotifications[index];
              this.unreadNotifications.splice(index, 1);
              notification.isReaded = true;
              this.readedNotifications.push(notification);
            }
        },
        deleteReadedNotifications(): void {
            console.log('hi');
            const ids = this.readedNotifications.map(item => item.id);
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
        getChatNotifications(chatId: number) {
          return this.unreadNotifications
          .filter(x => x.notificationSenderId == chatId 
            && !x.isReaded
            && x.type=='ChatNotification');
        }
    },
    getters: {
      getMessangerNotifications(): number {
        return this.unreadMessages;
      },
      getPostsNotifications(): number {
        return this.unreadPosts;
      },
      getFriendNotifications(): number {
        return this.unreadFriendNotifications;
      }
    }
  }
);