import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ChatRequest } from '../Models/chat-request.model';
import { ChatCompletion } from '../Models/chat-completion.model';
import { ChatMessage, ChatRole } from '../Models/chat-message.model';
import { tap } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private http = inject(HttpClient); 
  private apiUrl = 'http://localhost:5000/api/chat';
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
}
