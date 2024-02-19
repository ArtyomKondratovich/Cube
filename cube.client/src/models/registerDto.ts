export interface RegisterDto {
    name: string;

    surname: string;

    dateOfBirth: Date | null;

    email: string;

    password: string;
}