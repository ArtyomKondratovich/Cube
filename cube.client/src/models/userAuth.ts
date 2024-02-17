import { UserEntity } from "./userEntity";

export class UserAuth {
    token: string = '';
    user: UserEntity = new UserEntity();
}