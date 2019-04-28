import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { Message } from '../_models/message';


// const httpOptions = {
//   headers : new HttpHeaders({
//     'Authorization': 'Bearer ' + localStorage.getItem('token')
//   })
// }

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.apiUrl;
constructor(private http: HttpClient) { }

getUsers(page? , itemsperpage? , userparams? , likesParam?): Observable<PaginatedResult<User[]>> {

  const paginatedresult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

  let params = new HttpParams();

  if (page != null && itemsperpage != null) {

    params = params.append('pageNumber' , page) ;
    params = params.append('pageSize' , itemsperpage) ;

  }

  if (userparams != null) {

    params  = params.append('minAge' , userparams.minAge) ;
    params  = params.append('maxAge' , userparams.maxAge) ;
    params  = params.append('gender' , userparams.gender) ;
    params  = params.append('orderBy' , userparams.orderBy) ;
  }

  if (likesParam === 'Likers') {
params = params.append('likers' , 'true');

  }

  if (likesParam === 'Likees') {
    params = params.append('likees' , 'true');

      }

  return this.http.get<User[]>(this.baseUrl + 'users' , {observe: 'response' , params}).pipe(
 map(response => {

  paginatedresult.result = response.body ;
  if (response.headers.get('Pagination') != null) {

paginatedresult.pagination = JSON.parse(response.headers.get('Pagination'));
  }
  return paginatedresult ;
 })

  );
}

getUser( id: number): Observable<User> {

  return this.http.get<User>(this.baseUrl + 'users/' + id ) ;
}

updateUser(id: number, user: User) {
return this.http.put(this.baseUrl + 'users/' + id , user);
}

setMainPhoto(userId: number , photoId: number) {

  return this.http.post(this.baseUrl + 'users/' + userId + '/photos/' + photoId + '/setMain' , {});

}

deletePhoto(userId: number , photoid: number) {
return this.http.delete(this.baseUrl + 'users/' + userId + '/photos/' + photoid);

}

sendLike( id: number,  recipientID: number) {

return this.http.post(this.baseUrl + 'users/' + id + '/like/' + recipientID, {}) ;
}

getMessages(id: number , page? , itemsPerPage?, messageContainer?) {

const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();


let params = new HttpParams() ;
params = params.append('MessageContainer' , messageContainer) ;
if (page != null && itemsPerPage != null) {

  params = params.append('pageNumber' , page) ;
  params = params.append('pageSize' , itemsPerPage) ;

}

return this.http.get<Message[]>(this.baseUrl + 'users/' + id + '/messages/', {observe: 'response' , params} )
.pipe( map(
response => {
  paginatedResult.result = response.body ;
if (response.headers.get('Pagination') !== null) {

  paginatedResult.pagination = JSON.parse(response.headers.get('Pagination')) ;
}
return paginatedResult;
}
)) ;

}

getMessageThread(id: number , recipientId: number) {

  return this.http.get<Message[]>(this.baseUrl + 'users/' + id + '/messages/thread/' + recipientId);
}

sendMessage(id: number , message: Message) {

return this.http.post(this.baseUrl + 'users/' + id + '/messages/', message ) ;
}

deleteMessage(id: number , userId: number) {

return this.http.post(this.baseUrl + 'users/' + userId + '/messages/' + id , {}) ;

}

markAsread(userId: number , messageId: number) {
  this.http.post(this.baseUrl + 'users/' + userId + '/messages/' + messageId + '/read/', {}).subscribe();

}

}
