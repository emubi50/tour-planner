import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Tour } from './pages/tour/tour';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'tours', component: Tour },
  { path: '**', redirectTo: '' },
];
