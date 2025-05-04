import { ChatMessage } from "./chat-message.model";

export interface ChatCompletion {
    message: ChatMessage;
    sessionState?: string;
} 