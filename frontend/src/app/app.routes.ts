import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Login } from './pages/login/login';
import { Registration } from './pages/registration/registration';
import { TourPage } from './pages/tour/tour';
import { CreateTour } from './pages/create-tour/create-tour';
import { EditTour } from './pages/edit-tour/edit-tour';
import { TourList } from './components/tour-list/tour-list';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'login', component: Login },
  { path: 'registration', component: Registration },
  { path: 'tours', component: TourPage },
  { path: 'tours/new', component: CreateTour },
  { path: 'tours/:id', component: EditTour },
  { path: 'tourListTest', component: TourList },
  { path: '**', redirectTo: '' },
];
