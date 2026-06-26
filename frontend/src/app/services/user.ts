import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { IUser, IUserCredentials } from '../interfaces/User';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private currentUser = signal<IUser | null>(null);

  readonly user = this.currentUser.asReadonly();

  setUser(user: IUser | null): void {
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
 * Sends a request to the server to remove the token cookie.
 *
 * @return {Observable<void>}
 * @memberof UserService
 */
logoutUserServer(): Observable<void> {
    return this.http.post<void>(
      '/api/auth/logout',
      {}
    );
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
/**
 * Fetches the user's information based on the token stored.
 * NOT YET IMPLEMENTED: Backend support is not yet available for this.
 *
 * @return {Observable<IUser>}
 * @memberof UserService
 */
loadCurrentUser(): Observable<IUser> {
    return this.http.get<IUser>('/api/auth/me');
  }
}
