import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Tour } from './pages/tour/tour';
import { TourShort } from './components/tour-short/tour-short';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'tours', component: Tour },
  { path: 'tourShortTest', component: TourShort },
  { path: '**', redirectTo: '' },
];
