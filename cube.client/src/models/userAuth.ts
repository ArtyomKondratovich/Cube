import { UserEntity } from "./userEntity";

export interface UserAuth {
    token: string;
    user: UserEntity;
}