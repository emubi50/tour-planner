import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { ITourLog, ITourLogCreate } from '../interfaces/TourLog';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class TourLogService {
  private tourLogsServerSubject = new BehaviorSubject<ITourLog[]>([]); 
  tourLogsServer = this.tourLogsServerSubject.asObservable();

  private http = inject(HttpClient);

  /**
   * Gets all tour logs for the specified tour ID.
   * @param tourId The ID of the tour to get logs for
   * @returns An array of all tour logs belonging to the specified tour
   */
  getTourLogsByTourIdServer(tourId: number): Observable<ITourLog[]> {
    return this.http.get<ITourLog[]>(`/api/tours/${tourId}/logs`).pipe(
      tap((tourLogs) => this.tourLogsServerSubject.next(tourLogs)),
    );
  }

  /**
   * Gets the average rating for a tour by its ID.
   * @param tourId The ID of the tour to get the average rating for
   * @returns The average value of all ratings for the specified tour
   * @returns 0 if there are no tour logs for the specified tour ID
   */
  getRatingAvgByTourIdServer(tourId: number): number {
    const tourLogs = this.tourLogsServerSubject.value.filter(
      (tourLog) => tourLog.tourId === tourId,
    );
    if (tourLogs.length === 0) return 0;
    const totalRating = tourLogs.reduce((sum, log) => sum + log.rating, 0);
    return totalRating / tourLogs.length;
  }

  addTourLogServer(tourId: number, tourLog: ITourLogCreate): Observable<void> {
    return this.http.post<void>(`/api/tours/${tourId}/logs`, tourLog).pipe(
      tap(() => {
        this.getTourLogsByTourIdServer(tourId).subscribe();
      }),
    );
  }

  /**
   * Updates a tour log in the list.
   * @param updatedTourLog The updated tour log data
   */
  updateTourLogServer(tourId: number, updatedTourLog: ITourLog): Observable<void> {
    return this.http
      .put<void>(`/api/tours/${tourId}/logs/${updatedTourLog.id}`, updatedTourLog)
      .pipe(
        tap(() => {
          this.getTourLogsByTourIdServer(tourId).subscribe();
        }),
      );
  }

  /**
   * Removes a tour log from the list by its ID.
   * @param id The ID of the tour log to remove
   */
  deleteTourLogServer(tourId: number, id: number): Observable<void> {
    return this.http.delete<void>(`/api/tours/${tourId}/logs/${id}`).pipe(
      tap(() => {
        this.getTourLogsByTourIdServer(tourId).subscribe();
      }),
    );
  }
}
