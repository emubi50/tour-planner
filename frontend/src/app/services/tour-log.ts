import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

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
      distance: 0,
      duration: 0,
      rating: 3.0,
    },
    {
      id: 1,
      tourId: 1,
      date: new Date(),
      comment: 'Initial tour log entry',
      difficulty: 4,
      distance: 0,
      duration: 0,
      rating: 4.0,
    },
    {
      id: 2,
      tourId: 2,
      date: new Date(),
      comment: 'Initial tour log entry',
      difficulty: 5,
      distance: 0,
      duration: 0,
      rating: 5.0,
    },
    {
      id: 3,
      tourId: 0,
      date: new Date(),
      comment: 'Second tour log entry for tour 0',
      difficulty: 2,
      distance: 0,
      duration: 0,
      rating: 2.0,
    },
    {
      id: 4,
      tourId: 0,
      date: new Date(),
      comment: 'Another TourLog yessir',
      difficulty: 4,
      distance: 0,
      duration: 0,
      rating: 4.0,
    },
    {
      id: 5,
      tourId: 1,
      date: new Date(),
      comment: 'Second tour log entry for tour 1',
      difficulty: 3,
      distance: 0,
      duration: 0,
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

export interface ITourLog {
  id: number;
  tourId: number;
  date: Date;
  comment: string;
  difficulty: number;
  distance: number;
  duration: number;
  rating: number;
}

// Copy of ITourLog without id
export interface ITourLogCreate {
  tourId: number;
  date: Date;
  comment: string;
  difficulty: number;
  distance: number;
  duration: number;
  rating: number;
}
