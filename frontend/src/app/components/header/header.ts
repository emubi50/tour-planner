import { Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Navbar } from './navbar/navbar';
import { AccountDropdown } from './account-dropdown/account-dropdown';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, Navbar, AccountDropdown],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  currentUser = signal<string>('');
}
