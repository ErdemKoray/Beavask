import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface Friend {
  id: number;
  firstName: string;
  lastName: string;
  username: string;
  email: string;
  avatarUrl: string | null;
  createdAt: string;
  updatedAt: string | null;
  isActive: boolean;
  teamId: number | null;
  companyId: number | null;
}

export interface FriendsListResponse {
  isSuccess: boolean;
  message: string;
  data: Friend[];
  error: any;
  timestamp: string;
}

@Injectable({
  providedIn: 'root'
})
export class FriendshipService {
  private endpoint = 'http://localhost:5092/api/Friendship';

  constructor(private http: HttpClient) {}

  getFriendsList(): Observable<FriendsListResponse> {
    return this.http.get<FriendsListResponse>(`${this.endpoint}/get-friends-list`);
  }
}