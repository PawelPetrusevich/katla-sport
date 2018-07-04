import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user.model';
import { Observable } from 'rxjs';

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
}
