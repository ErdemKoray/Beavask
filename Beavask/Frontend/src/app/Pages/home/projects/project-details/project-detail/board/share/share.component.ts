import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Clipboard } from '@angular/cdk/clipboard';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Friend, FriendshipService } from '../../../../../../../common/services/friendship/friendship.service';
import { InvitationService } from '../../../../../../../common/services/invitation/invitation.service';
import { ProjectUserService } from '../../../../../../../common/services/projects/project-user.service';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-share-modal',
  standalone: true,
  imports: [CommonModule, FormsModule,TranslateModule],
  templateUrl: './share.component.html',
  styleUrl: './share.component.css'
})
export class ShareModalComponent {
   @Input() projectId!: number;  // Proje id'sini parent verir
  @Output() invited = new EventEmitter<number>(); // Başarılı davette parent'ı bilgilendir
  friends: Friend[] = [];
  inviteSent: { [userId: number]: boolean } = {};
  loading = false;

  constructor(private invitationService: InvitationService,private friendshipService:FriendshipService,private projectUserService:ProjectUserService) {}

  ngOnInit() {
    this.getFriends();
  }

getFriends() {
  this.loading = true;
  this.friendshipService.getFriendsList().subscribe({
    next: (res) => {
      if (res.isSuccess && res.data) {
        // Önce projede kimler var onu çek
        this.projectUserService.getProjectUser(this.projectId).subscribe({
          next: (response) => {
            const projectUsers = response.data || [];
            const projectUserIds = projectUsers.map((u: any) => u.id);

            // Arkadaşlardan projede olmayanları filtrele
            this.friends = res.data.filter(friend => !projectUserIds.includes(friend.id));

            this.loading = false;
          },
          error: () => { 
            this.friends = res.data; // Hata olursa herkesi göster
            this.loading = false;
          }
        });
      } else {
        this.friends = [];
        this.loading = false;
      }
    },
    error: () => { 
      this.friends = [];
      this.loading = false;
    }
  });
}


  invite(user: Friend) {
    if (this.inviteSent[user.id]) return; // Yeniden gönderme
    this.invitationService.inviteToProject(this.projectId, user.id).subscribe({
      next: () => {
        
        this.inviteSent[user.id] = true;
        this.invited.emit(user.id);
      }
    });
  }

  defaultAvatar(username: string): string {
    return `https://ui-avatars.com/api/?name=${username}`;
  }
}
