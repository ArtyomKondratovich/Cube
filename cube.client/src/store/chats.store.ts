import type { IChat, IChatLoad, IResponse, IUser } from '@/api/types'
import { defineStore } from 'pinia'
import axios from 'axios'
import config from '@/config'
import { toast } from 'vue3-toastify'

interface IChatsState {
    chats: IChatLoad[]
}
export const useChatStore = defineStore(
    'chat', {
        state: (): IChatsState => ({
            chats: []
        }),
        actions: {
            loadChats() {
                const user = JSON.parse(localStorage.getItem('user') ?? '') as IUser;
                const id = user.id;

                return axios.post(`${config.apiUrl}/Chat/GetAll`, { id }, {
                    headers: {
                        'Authorization': `Bearer ${localStorage.getItem('token')}`,
                        'Content-Type': 'application/json'
                    }
                });
            }
        },
        getters: {
            getChats(): IChatLoad[] {
                return this.chats;
            }
        }
    },
    
)