import { Component } from '@angular/core';
import { NavItem } from './nav-item/nav-item';
import { AccountDropdown } from './account-dropdown/account-dropdown';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [NavItem, AccountDropdown],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  navItems = [
    { label: 'Tours', routerLink: '/tours' },
    { label: 'My Tours', routerLink: '/my' },
    { label: 'Create Tours', routerLink: '/create' },
  ]
  currentUser = "GigaChad420";
}
