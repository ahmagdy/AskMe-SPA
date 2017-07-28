export interface IMessage {
    id: number;
    content: string;
    submissionDate: Date;
    isVisible: boolean;
    reply: string;
}