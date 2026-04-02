import { Component, input, signal } from '@angular/core';

@Component({
  selector: 'app-account-dropdown',
  standalone: true,
  imports: [],
  templateUrl: './account-dropdown.html',
  styleUrl: './account-dropdown.css',
})
export class AccountDropdown {
  currentUser = input<string>();

  isOpen = signal(false);
  toggle() {
    this.isOpen.update((booleanValue) => !booleanValue);
  }
}
