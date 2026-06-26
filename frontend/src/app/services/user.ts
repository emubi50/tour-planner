import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { IUser, IUserCredentials } from '../interfaces/User';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private currentUser = signal<IUser | null>(null);

  get user(): IUser | null {
    return this.currentUser();
  }

  set setUser(user: IUser | null) {
    this.currentUser.set(user);
  }

  private http = inject(HttpClient);
/**
 * Sends the provided credentials to the auth endpoint for authentication.
 *
 * @param {IUserCredentials} user
 * @return {Observable<void>}
 * @memberof UserService
 */
loginUserServer(user: IUserCredentials): Observable<void> {
    return this.http.post<void>('/api/auth/login', user);
  }
/**
 * Sends the provided credentials to the auth endpoint for user registration.
 *
 * @param {IUserCredentials} user
 * @return {Observable<void>}
 * @memberof UserService
 */
registerUserServer(user: IUserCredentials): Observable<void> {
    return this.http.post<void>('/api/auth/register', user);
  }
}
