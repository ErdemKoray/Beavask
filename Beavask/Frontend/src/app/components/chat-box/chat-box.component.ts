import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { User } from '../../common/model/user.model';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Friend, FriendshipService } from '../../common/services/friendship/friendship.service';
import { MessageService } from '../../common/services/message/message.service';
import { AuthService } from '../../common/services/auth/auth.service';
import { AuthprofileService } from '../../common/services/profile/authprofile.service';
import { Message } from '../../common/services/message/model/message.model';
interface ChatUser {
  id: number;
  name: string;
  avatarUrl?: string;
  online?: boolean;
  lastMessage?: string;
  unreadCount?: number;
}

interface ChatMessage {
  text: string;
  time: Date;
  own: boolean;
  user: string; // 'Support' veya kullanıcının adı
}
@Component({
  selector: 'app-chat-box',
  standalone: true,
  imports: [CommonModule,FormsModule,TranslateModule],
  templateUrl: './chat-box.component.html',
  styleUrl: './chat-box.component.css'
})
export class ChatBoxComponent {
   chatbox = false;
  showEmojiPicker = false;
  messageInput = '';
  emojis: string[] = ['😀', '😂', '😎', '😍', '👍', '🙏', '🎉', '🔥', '❤️', '😭'];
  userList: ChatUser[] = [];
  selectedUser: ChatUser | null = null;
  messages: ChatMessage[] = [];
  userListModel = true;
  loadingUsers = false;
  loadingMessages = false;
  defaultAvatar: string = 'iconbeavask.png'; 


  myUserId: number = 0;

  constructor(
    private friendshipService: FriendshipService,
    private messageService: MessageService,
    private authService: AuthprofileService, 
    private translate: TranslateService
  ) {
    document.addEventListener('click', () => {
      this.showEmojiPicker = false;
    });
  }

  ngOnInit() {
    // Kullanıcı kimliğini çek
    this.authService.whoami().subscribe({
      next:(res)=>{
        this.myUserId = res.userId
      }
    })
    this.fetchFriendsWithLastMessage();
  }

  
  fetchFriendsWithLastMessage() {
    this.loadingUsers = true;
    this.friendshipService.getFriendsList().subscribe({
      next: (response) => {
        const friends: Friend[] = response.data || [];
        if (friends.length === 0) {
          this.userList = [];
          this.loadingUsers = false;
          return;
        }
        Promise.all(
          friends.map(async (friend) => {
            const [sentMessages, receivedMessages] = await Promise.all([
              this.messageService.senderFriendMessage(friend.id).toPromise().catch(() => []),
              this.messageService.receiverMessage(friend.id).toPromise().catch(() => [])
            ]);
            const allMessages: Message[] = [
              ...(sentMessages || []), ...(receivedMessages || [])
            ].sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
            const lastMsg = allMessages[0]?.content || '';
            
            return {
              id: friend.id,
              name: friend.username,
              avatarUrl: friend.avatarUrl || this.defaultAvatar,
              online: true, 
              lastMessage: lastMsg,
              unreadCount: 0
            } as ChatUser;
          })
        ).then(users => {
          this.userList = users.sort((a, b) => {
            if (!a.lastMessage) return 1;
            if (!b.lastMessage) return -1;
            return 0;
          });
          this.loadingUsers = false;
        });
      },
      error: () => {
        this.userList = [];
        this.loadingUsers = false;
      }
    });
  }


selectUser(user: ChatUser) {
  this.selectedUser = user;
  this.userListModel = false;
  this.loadingMessages = true;

  Promise.all([
    this.messageService.senderFriendMessage(user.id).toPromise().catch(() => []),
    this.messageService.receiverMessage(user.id).toPromise().catch(() => [])
  ]).then(([sent, received]) => {
    const all: Message[] = [
      ...(sent || []),
      ...(received || [])
    ];

    const seen = new Set<number>();
    const uniqueMessages = all.filter(msg => {
      if (seen.has(msg.id)) return false;
      seen.add(msg.id);
      return true;
    }).sort((a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime());

    this.messages = uniqueMessages.map(msg => ({
      text: msg.content,
      time: new Date(msg.createdAt),
      own: msg.senderId === this.myUserId,
      user: msg.senderId === this.myUserId ? this.translate.instant('chat.you') : user.name
    }));

    user.unreadCount = 0;
    this.loadingMessages = false;
  });
}

  changeList() {
    this.userListModel = true;
    this.selectedUser = null;
  }


  toggleCloseModal() {
    this.userListModel = false;
    this.chatbox = false;
    this.selectedUser = null;
  }

  toggleOPenModal() {
    this.userListModel = true;
    this.chatbox = true;
  }


  sendMessage() {
    if (!this.messageInput.trim() || !this.selectedUser) return;
    const msgText = this.messageInput;
    const friendId = this.selectedUser.id;
    this.messageService.sendMessage({
      content: msgText,
      receiverId: friendId
    }).subscribe(() => {
      this.messages.push({
        text: msgText,
        time: new Date(),
        own: true,
        user: this.translate.instant('chat.you')
      });
      this.messageInput = '';
    });
  }

  addEmoji(emoji: string) {
    this.messageInput += emoji;
    this.showEmojiPicker = false;
  }

  toggleEmojiPicker(event: MouseEvent) {
    event.stopPropagation();
    this.showEmojiPicker = !this.showEmojiPicker;
  }

  closeModal() {
    this.chatbox = false;
    this.selectedUser = null;
  }

}
