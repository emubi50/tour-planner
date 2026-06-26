import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  private http = inject(HttpClient);
/**
 * Makes a login request to server
 *
 * @param {string} username
 * @param {string} password
 * @return {*} 
 * @memberof LoginService
 */
loginServer(username: string, password: string) {
    return this.http.post('/api/auth/login', { username, password });
  }
}
