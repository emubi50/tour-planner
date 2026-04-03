import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Login } from './pages/login/login';
import { Registration } from './pages/registration/registration';
import { TourPage } from './pages/tour/tour';
import { TourList } from './components/tour-list/tour-list';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'login', component: Login },
  { path: 'registration', component: Registration },
  { path: 'tours', component: TourPage },
  { path: 'tourListTest', component: TourList },
  { path: '**', redirectTo: '' },
];
