import { Component, ElementRef, ViewChild } from '@angular/core';
import { ChatMessagePair } from '../../Models/chat-message-pair.model';
import { ChatService } from '../../services/chat.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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

  constructor(private chatService: ChatService) {}

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