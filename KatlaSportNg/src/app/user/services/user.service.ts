import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from '../models/user.model';
import { Observable } from 'rxjs';
import { UserLogin } from 'app/user/models/user-login';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private url = environment.apiUrl + 'api/users/'

  constructor(
    private http: HttpClient
  ) { }

  registerUser(user: User): Observable<Object>{
    return this.http.post<Object>(`${this.url}registration`,user);
  }

  userAuthentication(user: UserLogin): Observable<Object>{
    var data = "username="+user.login+"&password="+user.password+"&grant_type=password";
    var header = new HttpHeaders({'Content-Type': 'application/x-www-urlencoded'});
    return this.http.post<Object>(` http://localhost:56952/token`,data,{headers: header,withCredentials: true});
  }
}
