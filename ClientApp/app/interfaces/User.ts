import { IUser } from './IUser';
export class User implements IUser {
    email: string;
    name: string;
    username: string;
    password: string;
    imageUrl: string;
    description: string;

}