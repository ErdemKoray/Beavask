import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../model/apiResponse.model';


  export interface pendingFriend{
  friendshipId: number
      userId: number
      username: string
      firstName: string
      lastName: string
  }

  export interface pendingProjects{
    id: number
      status: number
      createdAt: string
      senderId: number
      senderName: string
      projectId: number
      projectName: string
  }
 
@Injectable({
  providedIn: 'root'
})
export class InvitationService {
  private endpoint = "http://localhost:5092/api/Invitation";
  constructor(
    private _http:HttpClient
  ) { }

  sendFriendshipReq(receiverId:number):Observable<string>{
    return this._http.post<string>(`${this.endpoint}/send-friendship-request`,{ReceiverId:receiverId});
  }

   acceptFriendshipReq(friendshipId:number):Observable<string>{
    return this._http.post<string>(`${this.endpoint}/accept-friendship-request`,{friendshipId});
  }

  rejectFriendshipReq(friendshipId:number):Observable<string>{
    return this._http.post<string>(`${this.endpoint}/reject-friendship-request`,{friendshipId});
  }

  getPendingReq():Observable<ApiResponse<pendingFriend>>{
    return this._http.get<ApiResponse<pendingFriend>>(`${this.endpoint}/get-pending-friend-requests`);
  }




  inviteToProject(projectId:number,userId: number):Observable<string>{
    return this._http.post<string>(`${this.endpoint}/invite-friend-to-personal-project`,{projectId,userId});
  }
    acceptProject(projectInvitationId:number):Observable<string>{
    return this._http.post<string>(`${this.endpoint}/accept-project-invitation`,{projectInvitationId});
  }
    rejectProject(projectInvitationId:number):Observable<string>{
    return this._http.post<string>(`${this.endpoint}/reject-project-invitation`,{projectInvitationId});
  }

   getPrpjectReq():Observable<ApiResponse<pendingProjects>>{
    return this._http.get<ApiResponse<pendingProjects>>(`${this.endpoint}/get-project-invitations`);
  }
}
