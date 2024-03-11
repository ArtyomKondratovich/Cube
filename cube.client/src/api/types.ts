export interface IUser {
    id: number;
    name: string;
    surname: string;
    dateOfBirth: Date | null;
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
    users: number[];
}

export interface IMesssageInput{
    senderId: number;
    chatId: number;
    message: string; 
}
  