import { Component } from '@angular/core';
import { NavItem } from './nav-item/nav-item';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [NavItem],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  navItems = [
    { label: 'Tours', routerLink: '/tours' },
    { label: 'My Tours', routerLink: '/my' },
    { label: 'Create Tours', routerLink: '/create' },
  ]
}
