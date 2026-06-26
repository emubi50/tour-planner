import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class RegistrationService {
  private http = inject(HttpClient);
/**
 * Makes a registration request to server
 *
 * @param {string} username
 * @param {string} password
 * @return {*} 
 * @memberof Registration
 */
registerUserServer(username: string, password: string) {
    return this.http.post('/api/auth/register', { username, password });
  }
}
