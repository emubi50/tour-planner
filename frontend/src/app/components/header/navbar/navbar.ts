import { Component, signal } from '@angular/core';
import { NavItem } from '../nav-item/nav-item';

@Component({
  selector: 'app-navbar',
  imports: [NavItem],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  navItems = [
    { label: 'Tours', routerLink: 'tours' },
    { label: 'My Tours', routerLink: 'my-tours' },
    { label: 'Create Tour', routerLink: 'tours/new' },
    { label: 'Go to Tour List Test', routerLink: 'tourListTest' },
  ];

  isMenuOpen = signal(false);
  toggleMenu() {
    this.isMenuOpen.update((v) => !v);
  }
}
