import { Component, input, signal } from '@angular/core';

@Component({
  selector: 'app-account-dropdown',
  imports: [],
  templateUrl: './account-dropdown.html',
  styleUrl: './account-dropdown.css',
})
export class AccountDropdown {
  currentUser = input();

  isDropdownOpen = signal(false);
  toggleDropdown() { this.isDropdownOpen.update(v => !v); }
}
