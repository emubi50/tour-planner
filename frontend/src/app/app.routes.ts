import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Login } from './pages/login/login';
import { Tour } from './pages/tour/tour';
import { TourShort } from './components/tour-short/tour-short';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'login', component: Login },
  { path: 'tours', component: Tour },
  { path: 'tourShortTest', component: TourShort },
  { path: '**', redirectTo: '' },
];
