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
  