import { Component, input } from '@angular/core';
import { NavItem } from '../nav-item/nav-item';

@Component({
  selector: 'app-navbar',
  imports: [NavItem],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  items = [
    { label: 'Tours', routerLink: 'tours' },
    { label: 'My Tours', routerLink: 'my-tours' },
    { label: 'Create Tour', routerLink: 'create' },
  ];
  vertical = input<boolean>(false);
}
