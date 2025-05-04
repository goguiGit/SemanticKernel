import { Component, ElementRef, ViewChild } from '@angular/core';
import { ChatMessagePair } from '../../Models/chat-message-pair.model';
import { ChatService } from '../../services/chat.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatRole } from '../../Models/chat-role.model';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule], 
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent {
  userInput = '';
  history: ChatMessagePair[] = [];
  showSaveDialog = false;
  chatTitle = '';
  availableHistories: string[] = [];
  showLoadDialog = false;
  chatHistory: { user: string; assistant: string }[] = [];
  currentMessage = '';
  isLoading = false;

  constructor(private chatService: ChatService) {
    this.loadAvailableHistories();
  }

  sendMessage() {
    const message = this.userInput.trim();
    if (!message) return;

    this.chatService.sendMessage(message, this.history).subscribe(response => {
      this.history.push({ 
        user: message, 
        assistant: response.message.content 
      });
      this.userInput = '';
    });
  }

  openSaveDialog() {
    this.showSaveDialog = true;
    this.chatTitle = `Conversación ${new Date().toLocaleString()}`;
  }

  saveChatHistory() {
    if (!this.chatTitle.trim()) return;

    this.chatService.saveChatHistory(this.history, this.chatTitle).subscribe({
      next: () => {
        this.showSaveDialog = false;
        this.chatTitle = '';
        this.loadAvailableHistories();
      },
      error: (error) => {
        console.error('Error saving chat history:', error);
      }
    });
  }

  openLoadDialog() {
    this.showLoadDialog = true;
    this.loadAvailableHistories();
  }

  loadAvailableHistories() {
    this.chatService.getAvailableChatHistories().subscribe(histories => {
      this.availableHistories = histories;
    });
  }

  loadConversation(fileName: string) {
    this.chatService.loadChatHistory(fileName).subscribe({
      next: (chatHistory) => {
        this.chatHistory = chatHistory.messages.reduce((acc: { user: string; assistant: string }[], msg, index, array) => {
          if (msg.role === ChatRole.User && index + 1 < array.length && array[index + 1].role === ChatRole.Assistant) {
            acc.push({
              user: msg.content,
              assistant: array[index + 1].content
            });
          }
          return acc;
        }, []);
        this.currentMessage = '';
        this.isLoading = false;
        this.showLoadDialog = false;
        // Asegurarse de que el estado de la sesión se ha restaurado
        console.log('Session state after loading:', this.chatService.getCurrentSessionState());
      },
      error: (error) => {
        console.error('Error loading conversation:', error);
        this.isLoading = false;
      }
    });
  }

  @ViewChild('chatHistory') chatHistoryRef!: ElementRef;

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  private scrollToBottom(): void {
    try {
      this.chatHistoryRef.nativeElement.scrollTop = this.chatHistoryRef.nativeElement.scrollHeight;
    } catch (err) {}
  }
}