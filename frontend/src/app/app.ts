import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from './components/header/header';
import { UserService } from './services/user';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, Header],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected title = 'frontend';

  _userService = inject(UserService);

  ngOnInit() {
    this._userService.loadCurrentUser().subscribe({
      next: (user) => {
        this._userService.setUser(user);
      },
      error: (error) => {
        console.error('Error loading current user:', error);
        this._userService.setUser(null);
      },
    });
  }
}
