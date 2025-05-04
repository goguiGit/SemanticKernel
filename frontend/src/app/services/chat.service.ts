import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ChatRequest } from '../Models/chat-request.model';
import { ChatCompletion } from '../Models/chat-completion.model';
import { ChatMessage, ChatRole } from '../Models/chat-message.model';
import { ChatHistory } from '../Models/chat-history.model';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private http = inject(HttpClient); 
  private apiUrl = 'http://localhost:5000/api/chat';
  private chatHistoryUrl = 'http://localhost:5000/api/chathistory';
  private sessionState?: string;

  sendMessage(message: string, history: { user: string; assistant: string }[]) {
    // Convert history to ChatMessage array
    const messages: ChatMessage[] = history.flatMap(entry => [
      { content: entry.user, role: ChatRole.User },
      { content: entry.assistant, role: ChatRole.Assistant }
    ]);
    
    // Add the new user message
    messages.push({ content: message, role: ChatRole.User });

    const request: ChatRequest = {
      messages,
      sessionState: this.sessionState
    };

    return this.http.post<ChatCompletion>(this.apiUrl, request).pipe(
      tap(response => {
        // Store the session state for future requests
        if (response.sessionState) {
          this.sessionState = response.sessionState;
        }
      })
    );
  }

  saveChatHistory(history: { user: string; assistant: string }[], title: string): Observable<any> {
    const chatHistory: ChatHistory = {
      id: crypto.randomUUID(),
      title: title,
      createdAt: new Date().toISOString(),
      messages: history.flatMap(entry => [
        { content: entry.user, role: ChatRole.User },
        { content: entry.assistant, role: ChatRole.Assistant }
      ]),
      sessionState: this.sessionState
    };

    return this.http.post(`${this.chatHistoryUrl}/save`, chatHistory);
  }

  loadChatHistory(fileName: string): Observable<ChatHistory> {
    return this.http.get<ChatHistory>(`${this.chatHistoryUrl}/load/${fileName}`).pipe(
      tap(chatHistory => {
        // Restaurar el estado de la sesión al cargar una conversación
        if (chatHistory.sessionState) {
          this.sessionState = chatHistory.sessionState;
        }
      })
    );
  }

  getAvailableChatHistories(): Observable<string[]> {
    return this.http.get<string[]>(`${this.chatHistoryUrl}/list`);
  }

  // Método para obtener el estado actual de la sesión
  getCurrentSessionState(): string | undefined {
    return this.sessionState;
  }
}
