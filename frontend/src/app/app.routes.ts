import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Tour } from './pages/tour/tour';
import { TourList } from './components/tour-list/tour-list';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'tours', component: Tour },
  { path: 'tourListTest', component: TourList },
  { path: '**', redirectTo: '' },
];
