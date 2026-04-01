import { Component, input } from '@angular/core';

@Component({
  selector: 'app-account-dropdown',
  imports: [],
  templateUrl: './account-dropdown.html',
  styleUrl: './account-dropdown.css',
})
export class AccountDropdown {
  currentUser = input();
}
