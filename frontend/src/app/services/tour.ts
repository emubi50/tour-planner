import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ITour, ITourCreate } from '../interfaces/Tour';
import { Duration } from '../components/DataDisplay/Tour/duration/duration';

@Injectable({
  providedIn: 'root',
})
export class TourService {
  //#region Init Tour data definition
  private readonly _toursInit: ITour[] = [
    {
      id: 0,
      name: "From Vienna's beauties to Salzburg's wonders over the boat - a scenic tour through Austria",
      description: 'This is the first tour.',
      duration: 240 * 60, // in seconds
      distance: 12000, // in meters
      start: 'Vienna, Austria',
      end: 'Salzburg, Austria',
      tags: ['Scenic', 'Fast', 'City'],
      rating: 0,
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
      rating: 0,
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
      rating: 0,
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
      rating: 0,
    },
  ];
  //#endregion

  private toursSubject = new BehaviorSubject<ITour[]>([...this._toursInit]);
  tours = this.toursSubject.asObservable();

  constructor() {
    for (let i = 0; i < 10; i++) {
      this.addTour({
        name: 'Tour name field',
        description: 'Tour description field',
        start: 'Tour start field',
        end: 'Tour end field',
        tags: ['Tour tag field'],
      });
    }
  }

  getTourById(id: number): ITour | undefined {
    return this.toursSubject.value.find((tour) => tour.id === id);
  }

  addTour(tour: ITourCreate): void {
    const maxId = Math.max(
      ...this.toursSubject.value.map((tour) => tour.id),
      0,
    );

    // Brainrot handling of 'computed' values because... no bi- backend :c
    const newTour = {
      ...tour,
      id: maxId + 1,
      rating: 0,
      duration: 0,
      distance: 0,
    };

    this.toursSubject.next([...this.toursSubject.value, newTour]);
  }

  updateTour(updatedTour: ITour): void {
    const tours = this.toursSubject.value;
    const index = tours.findIndex((tour) => tour.id === updatedTour.id);
    if (index !== -1) {
      tours[index] = updatedTour;
      this.toursSubject.next([...tours]);
    }
  }

  deleteTour(id: number): void {
    const tours = this.toursSubject.value.filter((tour) => tour.id !== id);
    this.toursSubject.next([...tours]);
  }
}
