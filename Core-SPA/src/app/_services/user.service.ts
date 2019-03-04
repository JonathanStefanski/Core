import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/Pagination';
import { map } from 'rxjs/operators';
import { Message } from '../_models/message';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = `${environment.locationApi}users/`;

  constructor(private http: HttpClient) { }

  getUsers(page?: number, itemsPerPage?: number, userParams?, likesParam?): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', `${page}`);
      params = params.append('pageSize', `${itemsPerPage}`);
    }

    if (userParams != null) {
      params = params.append('minAge', `${userParams.minAge}`);
      params = params.append('maxAge', `${userParams.maxAge}`);
      params = params.append('gender', `${userParams.gender}`);
      params = params.append('orderBy', `${userParams.orderBy}`);
    }

    if (likesParam === 'Likers') {
      params = params.append('likers', 'true');
    }
    if (likesParam === 'Likees') {
      params = params.append('likees', 'true');
    }
    if (likesParam === 'Both') {
      params = params.append('likers', 'true');
      params = params.append('likees', 'true');
    }

    return this.http.get<User[]>(`${this.baseUrl}`, { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${this.baseUrl}${id}`);
  }

  updateUser(id: number, user: User) {
    return this.http.put(`${this.baseUrl}${id}`, user);
  }

  setMainPhoto(userId: number, id: number) {
    return this.http.post(`${this.baseUrl}${userId}/photos/${id}/setMain`, {});
  }

  deletePhoto(userId: number, id: number) {
    return this.http.delete(`${this.baseUrl}${userId}/photos/${id}`);
  }

  sendLike(id: number, recipientId: number) {
    return this.http.post(`${this.baseUrl}${id}/like/${recipientId}`, {});
  }

  removeLike(id: number, recipientId: number) {
    return this.http.delete(`${this.baseUrl}${id}/like/${recipientId}`);
  }

  getMessages(id: number, page?, itemsPerPage?, messageContainer?) {
    const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', `${page}`);
      params = params.append('pageSize', `${itemsPerPage}`);
    }

    if (messageContainer != null) {
      params = params.append('messageContainer', messageContainer);
    }

    return this.http.get<Message[]>(`${this.baseUrl}${id}/messages`, {observe: 'response', params})
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }

  getMessageThread(userId: number, recipientId: number) {
    return this.http.get<Message[]>(`${this.baseUrl}${userId}/messages/thread/${recipientId}`);
  }

  sendMessage(userId: number, message: Message) {
    return this.http.post(`${this.baseUrl}${userId}/messages`, message);
  }

  deleteMessage(id: number, userId: number) {
    return this.http.post(`${this.baseUrl}${userId}/messages/${id}`, {});
  }

  markAsRead(userId: number, id: number) {
    return this.http.post(`${this.baseUrl}${userId}/messages/${id}/read`, {}).subscribe();
  }
}
