import { ChatMessage } from './chat-message.model';

export interface ChatHistory {
    id: string;
    title: string;
    createdAt: string;
    messages: ChatMessage[];
    sessionState?: string;
} 