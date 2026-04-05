import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Login } from './pages/login/login';
import { Registration } from './pages/registration/registration';
import { TourPage } from './pages/tour/tour';
import { CreateTour } from './pages/create-tour/create-tour';
import { EditTour } from './pages/edit-tour/edit-tour';
import { CreateTourLog } from './pages/create-tour-log/create-tour-log';
import { EditTourLog } from './pages/edit-tour-log/edit-tour-log';
import { TourList } from './components/tour-list/tour-list';

export const routes: Routes = [
  { path: '', component: Home, title: 'Home Page' },
  { path: 'login', component: Login, title: 'Login' },
  { path: 'registration', component: Registration, title: 'Registration' },
  { path: 'tours', component: TourPage, title: 'Tours' },
  { path: 'tours/new', component: CreateTour, title: 'Create Tour' },
  { path: 'tours/:tourId', component: EditTour, title: 'Edit Tour' },
  {
    path: 'tours/:tourId/logs/new',
    component: CreateTourLog,
    title: 'Create Tour Log',
  },
  {
    path: 'tours/:tourId/logs/:tourLogId',
    component: EditTourLog,
    title: 'Edit Tour Log',
  },
  { path: 'tourListTest', component: TourList, title: 'Tour List Test' },
  { path: '**', redirectTo: '' },
];
