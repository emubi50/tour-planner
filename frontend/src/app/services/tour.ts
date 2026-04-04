import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TourService {
  //#region Init Tour data definition
  private readonly _toursInit: Tour[] = [
    {
      id: 0,
      name: "From Vienna's beauties to Salzburg's wonders over the boat - a scenic tour through Austria",
      description: 'This is the first tour.',
      duration: 240 * 60, // in seconds
      distance: 12000, // in meters
      start: 'Vienna, Austria',
      end: 'Salzburg, Austria',
      tags: ['Scenic', 'Fast', 'City'],
      rating: 4.3,
    },
    {
      id: 1,
      name: 'The Great Wall of China - a historical tour through the ancient wonders',
      description: 'This is the second tour.',
      duration: 360 * 60, // in seconds
      distance: 15000, // in meters
      start: 'Beijing, China',
      end: 'Beijing, China',
      tags: ['Historical', 'Cultural', 'Adventure', 'Scenic', 'Long', 'Wall'],
      rating: 4.5,
    },
    {
      id: 2,
      name: 'The Grand Canyon - a breathtaking tour through the natural wonders of the world',
      description: 'This is the third tour.',
      duration: 180 * 60, // in seconds
      distance: 8000, // in meters
      start: 'Grand Canyon Village, Arizona, USA',
      end: 'Grand Canyon Village, Arizona, USA',
      tags: ['Nature', 'Adventure', 'Scenic'],
      rating: 4.8,
    },
    {
      id: 3,
      name: 'A really boring tour through grass fields in the middle of Austria',
      description:
        'This is the fourth tour. It is really really boring, the title does not lie.',
      duration: 120 * 60, // in seconds
      distance: 5000, // in meters
      start: 'Vienna, Austria',
      end: 'Salzburg, Austria',
      tags: ['Boring', 'Nature', 'City'],
      rating: 1.7,
    },
  ];
  //#endregion

  private toursSubject = new BehaviorSubject<Tour[]>([...this._toursInit]);
  tours$ = this.toursSubject.asObservable();

  constructor() {
    for (let i = 0; i < 10; i++) {
      this.addTour({
        id: 0,
        name: 'Tour name field',
        description: 'Tour description field',
        duration: 60 * 60, // in seconds
        distance: 10000, // in meters
        start: 'Tour start field',
        end: 'Tour end field',
        tags: ['Tour tag field'],
        rating: 3.0,
      });
    }
  }

  getTourById(id: number): Tour | undefined {
    return this.toursSubject.value.find((t) => t.id === id);
  }

  addTour(tour: Tour): void {
    const maxId = Math.max(...this.toursSubject.value.map((t) => t.id), 0);
    const newTour = { ...tour, id: maxId + 1 };

    this.toursSubject.next([...this.toursSubject.value, newTour]);
  }
}

export interface Tour {
  id: number;
  name: string;
  description: string;
  duration: number; // in seconds
  distance: number; // in meters
  start: string;
  end: string;
  tags: string[];
  rating: number; // from 0 to 5
}
