import { Component, inject, signal } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { UserService } from '../../../services/user';
import { LucideLogIn, LucideUser} from '@lucide/angular';

@Component({
  selector: 'app-account-dropdown',
  standalone: true,
  imports: [RouterLink, LucideLogIn, LucideUser],
  templateUrl: './account-dropdown.html',
  styleUrl: './account-dropdown.css',
})
export class AccountDropdown {
  private userService = inject(UserService);
  private router = inject(Router);

  currentUser = this.userService.user;
  isOpen = signal(false);

  toggle() {
    this.isOpen.update((booleanValue) => !booleanValue);
  }

  logout() {
    this.userService.logoutUserServer().subscribe({
      next: () => {
        console.log('Logout successful');
        this.userService.setUser(null);
        this.router.navigate(['/login']);
        this.isOpen.set(false);
      },
      error: (error) => {
        console.error('Logout failed:', error);
      }
    });
  }
}
