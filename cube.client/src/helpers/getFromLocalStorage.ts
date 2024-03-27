import type { IUser } from "@/api/types";

export default function getUserIdFromLocalStorage(): number {
    const user = JSON.parse(localStorage.getItem('user') ?? '{}') as IUser | null;
    return user ? user.id : -1;
}