// import { defineStore, type Store } from 'pinia'
// import { 
//   type IUser,
//   type IResponse,
//   type IChat,
//   type IChatInput
// } from '@/api/types';
// import config from '@/config';
// import axios from 'axios';
// import { toast } from 'vue3-toastify';

// interface IUserState {
//   user: IUser;
//   token: string;
//   chats: IChat[];
//   friends: IUser[];
// }

// export interface IUserStore extends Store {
//     addFriend(id: number): void;
//     removeFriend(id: number): void;
//     addChat(newChat: IChatInput): void;
//     updateData(): void;
//     isLoggedIn: boolean;
//     getChats: IChat[];
//     getFriends: IUser[];
//     $state: {
//       user: IUser;
//       token: string;
//     };
// }

// export const useAuthStore = defineStore(
//   'auth', {
//     state: (): IUserState => ({
//       user: (JSON.parse(localStorage.getItem('user') ?? '{}')) as IUser,
//       token: localStorage.getItem('token') ?? '',
//       chats: [],
//       friends: []
//     }),
//     actions: {
//       addFriend(id: number): void {

//         axios.post(`${config.apiUrl}/User/createFriendship`, {
//             userId: this.$state.user.id,
//             friendId: id
//         }).then(async (response) => {
//             const data = response.data as IResponse<any>;
            
//             if (data.responseResult == 'Success' && data){
//                 toast.success(``);
//                 await new Promise(resolve => setTimeout(resolve, 2000));
//             }
//         });
//       }
//     },
//     getters: {
//       isLoggedIn(): boolean {
//         return this.token != '';
//       }
//     }
//   }
// );