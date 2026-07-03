import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { ITour, ITourCreate } from '../interfaces/Tour';
import { TourLogService } from './tour-log';
import { TransportType } from '../enums/TransportType';
import { HttpClient, provideHttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class TourService {
  private toursServerSubject = new BehaviorSubject<ITour[]>([]);
  toursServer = this.toursServerSubject.asObservable();

  private http = inject(HttpClient);

  /**
   * Gets all tours and propagates the average ratings using the TourLogService.
   * @returns An array of all tours
   */
  getToursServer(): Observable<ITour[]> {
    return this.http.get<ITour[]>('/api/tours').pipe(tap((tours) => this.toursServerSubject.next(tours)));
  }

  /**
   * Gets a tour by its ID and propagates the average rating using the TourLogService.
   * @param id The tour's ID
   * @returns The tour object, or undefined if not found
   */

  getTourByIdServer(id: number): Observable<ITour> {
    return this.http.get<ITour>(`/api/tours/${id}`);
  }

  /**
   * Adds a tour to the list.
   * @param tour The tour creation data
   */
  addTourServer(tour: ITourCreate): Observable<ITour> {
    const newTour = {
      ...tour,
      userId: 0,
      estimatedTime: 0,
      distance: 0,
    };

    return this.http.post<ITour>('/api/tours', newTour).pipe(tap(() => this.getToursServer().subscribe()));
  }

  /**
   * Updates a tour in the list.
   * @param updatedTour The new data of the tour
   */
  updateTourServer(updatedTour: ITour): Observable<void> {
    let updatedTourCreate: ITourCreate = {
      name: updatedTour.name,
      description: updatedTour.description,
      from: updatedTour.from,
      to: updatedTour.to,
      tags: updatedTour.tags,
      transportType: updatedTour.transportType,
    };
    return this.http.put<void>(`/api/tours/${updatedTour.id}`, updatedTourCreate).pipe(tap(() => this.getToursServer().subscribe()));
  }

  /**
   * Removes a tour from the list by its ID.
   * @param id The ID of the tour to remove
   */
  deleteTourServer(id: number): Observable<void> {
    return this.http.delete<void>(`/api/tours/${id}`).pipe(tap(() => this.getToursServer().subscribe()));
  }

  searchToursServer(searchTerm: string): Observable<ITour[]> {
    return this.http.get<ITour[]>(`/api/tours/search?searchTerm=${encodeURIComponent(searchTerm)}`).pipe(
      tap((tours) => this.toursServerSubject.next(tours)),
    );
  }
}
