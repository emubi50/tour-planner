import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ITour, ITourCreate } from '../interfaces/Tour';
import { TourLogService } from './tour-log';
import { TransportType } from '../enums/TransportType';
import { HttpClient, provideHttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class TourService {
  //#region Init Tour data definition
  private readonly _toursInit: ITour[] = [
    {
      id: 0,
      name: 'From Vienna to Salzburg type beat Tour',
      description: 'This is the first tour.',
      estimatedTime: 240 * 60, // in seconds
      distance: 12000, // in meters
      from: 'Vienna, Austria',
      to: 'Salzburg, Austria',
      tags: ['Scenic', 'Austria', 'City'],
      transportType: TransportType.PUBLIC,
      rating: 0,
    },
    {
      id: 1,
      name: 'The Great Wall of China walking tour',
      description: 'This is the second tour.',
      estimatedTime: 360 * 60, // in seconds
      distance: 15000, // in meters
      from: 'Beijing, China',
      to: 'Beijing, China',
      tags: [
        'Historical',
        'Cultural',
        'Adventure',
        'Scenic',
        'Long',
        'Wall',
        'China',
        'Yessir',
      ],
      transportType: TransportType.WALK,
      rating: 0,
    },
    {
      id: 2,
      name: 'The Grand Canyon biking tour',
      description: 'This is the third tour.',
      estimatedTime: 180 * 60, // in seconds
      distance: 8000, // in meters
      from: 'Grand Canyon Village, Arizona, USA',
      to: 'Grand Canyon Village, Arizona, USA',
      tags: ['Nature', 'Adventure', 'Scenic'],
      transportType: TransportType.BIKE,
      rating: 0,
    },
    {
      id: 3,
      name: 'A really boring tour through grass fields in the middle of Austria',
      description:
        'This is the fourth tour. It is really really boring, the title does not lie.',
      estimatedTime: 120 * 60, // in seconds
      distance: 5000, // in meters
      from: 'Grass field somewhere in the middle of Austria',
      to: 'Grass field somewhere in the middle of Austria',
      tags: ['Boring', 'Nature', 'City'],
      transportType: TransportType.WALK,
      rating: 0,
    },
    {
      id: 4,
      name: 'Autofahr Tour ab dafür 🤙🤙',
      description: 'AHHHHHHH TOUR NOCH EINE',
      estimatedTime: 60 * 60, // in seconds
      distance: 10000, // in meters
      from: 'Eine Garage, Vienna, Austria',
      to: 'Eine Garage, Vienna, Austria',
      tags: ['Auto', 'City', 'Auto', 'Auto', 'Auto :)'],
      transportType: TransportType.CAR,
      rating: 0,
    },
  ];
  //#endregion

  private toursSubject = new BehaviorSubject<ITour[]>([...this._toursInit]);
  // Does not have rating aggregation
  // Use getTours() instead pleeeeaaase
  // Would need to make a pipe out of this or smth idk
  tours = this.toursSubject.asObservable();

  private http = inject(HttpClient);

  constructor(private tourLogService: TourLogService) {
    for (let i = 0; i < 10; i++) {
      this.addTour({
        name: 'Tour name field',
        description: 'Tour description field',
        from: 'Tour start field',
        to: 'Tour end field',
        tags: ['Tour tag field'],
        transportType: TransportType.BIKE,
      });
    }
  }

  /**
   * Gets all tours and propagates the average ratings using the TourLogService.
   * @returns An array of all tours
   */
  getTours(): ITour[] {
    return this.toursSubject.value.map((tour) => {
      tour.rating = this.tourLogService.getRatingAvgByTourId(tour.id);
      return tour;
    });
  }

  getToursServer(): Observable<ITour[]> {
    return this.http.get<ITour[]>('/api/tours');
  }

  /**
   * Gets a tour by its ID and propagates the average rating using the TourLogService.
   * @param id The tour's ID
   * @returns The tour object, or undefined if not found
   */
  getTourById(id: number): ITour | undefined {
    const tour = this.toursSubject.value.find((tour) => tour.id === id);
    if (!tour) {
      return undefined;
    }
    tour.rating = this.tourLogService.getRatingAvgByTourId(tour.id);
    return tour;
  }

  /**
   * Adds a tour to the list.
   * @param tour The tour creation data
   */
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
      estimatedTime: 0,
      distance: 0,
    };

    this.toursSubject.next([...this.toursSubject.value, newTour]);
  }

  addTourServer(tour: ITourCreate): Observable<ITour> {
    const newTour = {
      ...tour,
      userId: 0,
      estimatedTime: 0,
      distance: 0,
    };

    return this.http.post<ITour>('/api/tours', newTour);
  }

  /**
   * Updates a tour in the list.
   * @param updatedTour The new data of the tour
   */
  updateTour(updatedTour: ITour): void {
    const tours = this.toursSubject.value;
    const index = tours.findIndex((tour) => tour.id === updatedTour.id);
    if (index !== -1) {
      tours[index] = updatedTour;
      this.toursSubject.next([...tours]);
    }
  }

  /**
   * Removes a tour from the list by its ID.
   * @param id The ID of the tour to remove
   */
  deleteTour(id: number): void {
    const tours = this.toursSubject.value.filter((tour) => tour.id !== id);
    this.toursSubject.next([...tours]);
  }
}
