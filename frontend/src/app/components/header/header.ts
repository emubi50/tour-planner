import { Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Navbar } from './navbar/navbar';
import { AccountDropdown } from './account-dropdown/account-dropdown';
import { LucideRoute } from '@lucide/angular';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, Navbar, AccountDropdown, LucideRoute],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  isMenuOpen = signal<boolean>(false);
  toggleMenu() {
    this.isMenuOpen.update((booleanValue) => !booleanValue);
  }
}
