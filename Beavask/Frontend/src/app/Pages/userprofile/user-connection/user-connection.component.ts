import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InvitationService, pendingFriend } from '../../../common/services/invitation/invitation.service';
import { UserDetails, UserService } from '../../../common/services/user.service';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { Friend, FriendshipService } from '../../../common/services/friendship/friendship.service';
import { RouterLink } from '@angular/router';


@Component({
  selector: 'app-user-connection',
  standalone:true,
  imports:[RouterLink,ReactiveFormsModule,CommonModule,FormsModule,TranslateModule],
  templateUrl: './user-connection.component.html',
  styleUrls: ['./user-connection.component.css']
})
export class UserConnectionComponent implements OnInit {
   modalOpen = false;
  usernameSearch = '';
  searchResults: UserDetails[] = [];
  requestSent: { [key: number]: boolean } = {};
  friends: Friend[] = [];
  pendingRequests: pendingFriend[] = [];

  constructor(
    private userService: UserService,
    private invitationService: InvitationService,
    private friendshipService: FriendshipService // yeni servis
  ) {}

  ngOnInit() {
    this.getPendingRequests();
    this.getFriends();
  }

  openAddConnectionModal() {
    this.modalOpen = true;
    this.usernameSearch = '';
    this.searchResults = [];
  }

  closeModal() {
    this.modalOpen = false;
    this.usernameSearch = '';
    this.searchResults = [];
  }

searchUsers() {
  if (!this.usernameSearch.trim()) {
    this.searchResults = [];
    return;
  }
  this.userService.apiUserByUsername(this.usernameSearch.trim()).subscribe({
    next: (res: any) => {
      console.log(res)
      if (res && res.isSuccess && res.data) {
        this.searchResults = [res.data];
      } else {
        this.searchResults = [];
      }
      
    },
    error: () => {
      this.searchResults = [];
    }
  });
}


  sendFriendRequest(userId: number) {
    this.invitationService.sendFriendshipReq(userId).subscribe({
      next: () => {
        this.requestSent[userId] = true;
        this.getPendingRequests();
      }
    });
  }

  getPendingRequests() {
    this.invitationService.getPendingReq().subscribe({
      next: res => {
        this.pendingRequests = Array.isArray(res.data) ? res.data : [];
      }
    });
  }

  acceptRequest(friendshipId: number) {
    this.invitationService.acceptFriendshipReq(friendshipId).subscribe({
      next: () => this.getPendingRequests()
    });
  }

  rejectRequest(friendshipId: number) {
    this.invitationService.rejectFriendshipReq(friendshipId).subscribe({
      next: () => this.getPendingRequests()
    });
  }

  getFriends() {
    this.friendshipService.getFriendsList().subscribe({
      next: res => {
        if (res.isSuccess) {
          this.friends = res.data;
        }
      }
    });
  }

  defaultAvatar(username: string): string {
    return `https://ui-avatars.com/api/?name=${username}`;
  }
}
