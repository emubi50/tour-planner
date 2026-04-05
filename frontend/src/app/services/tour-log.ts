import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ITourLog, ITourLogCreate } from '../interfaces/TourLog';

@Injectable({
  providedIn: 'root',
})
export class TourLogService {
  //#region Init TourLog data definition
  private readonly _tourLogsInit: ITourLog[] = [
    {
      id: 0,
      tourId: 0,
      date: new Date(),
      comment: 'Initial tour log entry',
      difficulty: 3,
      distance: 1000,
      duration: 60 * 60,
      rating: 3.0,
    },
    {
      id: 1,
      tourId: 1,
      date: new Date(),
      comment: 'Initial tour log entry',
      difficulty: 4,
      distance: 4500,
      duration: 120 * 60,
      rating: 4.0,
    },
    {
      id: 2,
      tourId: 2,
      date: new Date(),
      comment: 'Initial tour log entry',
      difficulty: 5,
      distance: 800,
      duration: 30 * 60,
      rating: 5.0,
    },
    {
      id: 3,
      tourId: 0,
      date: new Date(),
      comment: 'Second tour log entry for tour 0',
      difficulty: 2,
      distance: 1000,
      duration: 60 * 60,
      rating: 2.0,
    },
    {
      id: 4,
      tourId: 0,
      date: new Date(),
      comment: 'Another TourLog yessir',
      difficulty: 4,
      distance: 1000,
      duration: 60 * 60,
      rating: 4.0,
    },
    {
      id: 5,
      tourId: 1,
      date: new Date(),
      comment: 'Second tour log entry for tour 1',
      difficulty: 3,
      distance: 4500,
      duration: 120 * 60,
      rating: 3.5,
    },
  ];
  //#endregion

  private tourLogsSubject = new BehaviorSubject<ITourLog[]>([
    ...this._tourLogsInit,
  ]);
  tourLogs = this.tourLogsSubject.asObservable();

  getTourLogsByTourId(tourId: number): ITourLog[] {
    return this.tourLogsSubject.value.filter(
      (tourLog) => tourLog.tourId === tourId,
    );
  }

  getRatingAvgByTourId(tourId: number): number {
    const tourLogs = this.getTourLogsByTourId(tourId);
    if (tourLogs.length === 0) return 0;
    const totalRating = tourLogs.reduce((sum, log) => sum + log.rating, 0);
    return totalRating / tourLogs.length;
  }

  addTourLog(tourLog: ITourLogCreate): void {
    const newTourLog: ITourLog = {
      id: this.tourLogsSubject.value.length
        ? Math.max(...this.tourLogsSubject.value.map((tourLog) => tourLog.id)) +
          1
        : 0,
      ...tourLog,
    };
    this.tourLogsSubject.next([...this.tourLogsSubject.value, newTourLog]);
  }

  updateTourLog(updatedTourLog: ITourLog): void {
    const tourLogs = this.tourLogsSubject.value;
    const index = tourLogs.findIndex(
      (tourLog) => tourLog.id === updatedTourLog.id,
    );
    if (index !== -1) {
      tourLogs[index] = updatedTourLog;
      this.tourLogsSubject.next([...tourLogs]);
    }
  }

  deleteTourLog(id: number): void {
    const tourLogs = this.tourLogsSubject.value.filter(
      (tourLog) => tourLog.id !== id,
    );
    this.tourLogsSubject.next([...tourLogs]);
  }
}
