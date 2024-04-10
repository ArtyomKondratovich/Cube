export interface IUser {
    id: number;
    name: string;
    surname: string;
    email: string;
    dateOfBirth: Date | null;
    roleId: number;
    avatarBytes: [];
}

export interface ILoginInput {
    email: string;
    password: string;
}

export interface IRegisterInput {
    name: string;
    surname: string;
    dateOfBirth: Date | null;
    email: string;
    password: string;
    file: File | null;
}

export interface IResponse<TResult> {
    value: TResult | null;
    messages: string[] | null;
    responseResult: string;
}

export interface IAuth {
    user: IUser;
    token: string;
}

export interface IMessage {
    id: number;
    message: string;
    createdDate: Date;
    updatedDate: Date | null;
    formatedCreatedDate: string;
    formatedUpdatedDate: string;
    userId: number;
    chatId: number;
}
export interface IChatLoad {
    id: number;
    title: string;
    type: string;
}

export interface IChat{
    id: number;
    title: string;
    type: string;
    users: IUser[];
}

export interface IChatInput{
    title: string;
    type: ChatType;
    patricipantsIds: number[];
}

export interface IMesssageInput{
    senderId: number;
    chatId: number;
    message: string; 
}

export interface IImageModel{
    id: number;
    ownerId: number;
    imgBytes: [];
    type: string;
}

export interface IImageInput{
    ownerId: number;
    type: string;
}

export interface INotificationModel{
    id: number;
    userId: number;
    notificationSenderId: number;
    isReaded: boolean;
    type: string;
    accepted: boolean;
}

export interface IUserNotifications{
    notifications: INotificationModel[];
}

export enum ChatType {
    Private,
    Group,
    SavedMessages
}