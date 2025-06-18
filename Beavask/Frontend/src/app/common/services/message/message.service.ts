import { Injectable } from '@angular/core';
import { Message } from './model/message.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { createMessage } from './model/messageCreate.model';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl="http://localhost:5092/api/Message"

  constructor(private _http:HttpClient) { }

  sendMessage(model:createMessage):Observable<string>{
    return this._http.post<string>(`${this.baseUrl}`,model)
  }

  senderMessage(id:number):Observable<Message[]>{
    return this._http.get<Message[]>(`${this.baseUrl}/sender/${id}`)
  }
  
  receiverMessage(id:number):Observable<Message[]>{
    return this._http.get<Message[]>(`${this.baseUrl}/receiver/${id}`)
  }

   senderFriendMessage(id:number):Observable<Message[]>{
    return this._http.get<Message[]>(`${this.baseUrl}/friend/${id}`)
  }
}
