import { Photo } from './photo';

export interface User {
    id: number;
    username: string;
    knownAs: string;
    age: number;
    gender: string;
    createDate: Date;
    lastActive: Date;
    photoURL: string;
    city: string;
    lookingFor?: string;
    introduction?: string;
    interests?: string;
    photos?: Photo[];
}
