import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../model/apiResponse.model';


@Injectable({
  providedIn: 'root'
})
export class GenericHttpsService<T> {
  private baseUrl = 'http://localhost:5092/api'; // API URL'sini buraya ekleyin
 
  constructor(private http: HttpClient) {}

  getAll(endpoint: string): Observable<ApiResponse<T[]>> {
    return this.http.get<ApiResponse<T[]>>(`${this.baseUrl}/${endpoint}`);
  }

  getById(endpoint: string, id: number): Observable<ApiResponse<T>> {
    return this.http.get<ApiResponse<T>>(`${this.baseUrl}/${endpoint}/${id}`);
  }

  create(endpoint: string, item: T): Observable<ApiResponse<T>> {
    return this.http.post<ApiResponse<T>>(`${this.baseUrl}/${endpoint}`, item);
  } 
  register(endpoint: string, item: T): Observable<ApiResponse<T>> {
    return this.http.post<ApiResponse<T>>(`${this.baseUrl}/${endpoint}`, item);
  }

  update(endpoint: string, id: number, item: T): Observable<ApiResponse<T>> {
    return this.http.put<ApiResponse<T>>(`${this.baseUrl}/${endpoint}/${id}`, item);
  }

  delete(endpoint: string, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${endpoint}/${id}`);
  }
}
