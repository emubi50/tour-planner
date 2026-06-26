import { Component, signal, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Navbar } from './navbar/navbar';
import { AccountDropdown } from './account-dropdown/account-dropdown';
import { UserService } from '../../services/user';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, Navbar, AccountDropdown],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  private userService = inject(UserService);
  currentUser = this.userService.user;
  isMenuOpen = signal<boolean>(false);
  toggleMenu() {
    this.isMenuOpen.update((booleanValue) => !booleanValue);
  }
}
