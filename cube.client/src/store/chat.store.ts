import type { IChat, IResponse, IUser } from '@/api/types'
import { defineStore } from 'pinia'
import axios from 'axios'
import config from '@/config'
import { toast } from 'vue3-toastify'

interface IChatState {
    chats: IChat[]
}
export const useChatStore = defineStore(
    'chat', {
        state: (): IChatState => ({
            chats: []
        }),
        actions: {
            async loadChats() {
                const user = JSON.parse(localStorage.getItem('user') ?? '') as IUser;
                const id = user.id;

                axios.post(`${config.apiUrl}/Chat/GetAll`, { id }, {
                    headers: {
                        'Authorization': `Bearer ${localStorage.getItem('token')}`,
                        'Content-Type': 'application/json'
                    }
                }).then(async (response) => {
                    const data = response.data as IResponse<IChat[]>;

                    if (data.responseResult == 'Success' && data.value){
                        this.chats = data.value;
                    }
                    else{
                        toast.error(data.responseResult);
                        await new Promise(resolve => setTimeout(resolve, 2000));
                    }
                }).catch(async error => {
                    toast.error(error);
                    await new Promise(resolve => setTimeout(resolve, 2000));
                })
            }
        },
        getters: {
            getChats(): IChat[] {
                return this.chats;
            }
        }
    },
    
)