export enum ChatRole {
    System = 'system',
    Assistant = 'assistant',
    User = 'user'
}

export interface ChatMessage {
    content: string;
    role: ChatRole;
} 