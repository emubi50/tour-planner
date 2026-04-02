import { Component } from '@angular/core';

@Component({
  selector: 'app-tour',
  standalone: true,
  imports: [],
  templateUrl: './tour.html',
  styleUrl: './tour.css',
})
export class Tour {
  tours = [
    {
      id: 0,
      name: 'Tour 1',
      description: 'This is the first tour.',
      time: 240 * 60, // in seconds
      distance: 12000, // in meters
      start: 'Vienna, Austria',
      end: 'Salzburg, Austria',
    },
    {
      id: 1,
      name: 'Tour 2',
      description: 'This is the second tour.',
      time: 180 * 60, // in seconds
      distance: 7000, // in meters
      start: 'Nuremberg, Germany',
      end: 'Vienna, Austria',
    },
  ];
}
