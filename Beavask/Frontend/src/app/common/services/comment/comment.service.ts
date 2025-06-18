import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../model/apiResponse.model';
import { Comment } from '../../model/comment/comment.model';
import { CreateComment } from '../../model/comment/createcomment.model';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(private _http:HttpClient) { }

   private endpoint = "http://localhost:5092/api/Comment";

   getCommentByTaskId(id:number): Observable<ApiResponse<Comment[]>>{
    return this._http.get<ApiResponse<Comment[]>>(`${this.endpoint}/task/${id}`)
   }
getCommentByUserId(): Observable<ApiResponse<Comment[]>> {
  return this._http.get<ApiResponse<Comment[]>>(`${this.endpoint}`);
}

   createComment(model:CreateComment): Observable<ApiResponse<CreateComment>>{
    return this._http.post<ApiResponse<CreateComment>>(`${this.endpoint}`,model)
   }
   
    deleteComment(CommentId:number){
    return this._http.delete(`${this.endpoint}/${CommentId}`)
   }


}
