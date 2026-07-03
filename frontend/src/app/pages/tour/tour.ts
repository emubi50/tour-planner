import { Component, computed, effect, inject, Signal, signal } from '@angular/core';
import { TourService } from '../../services/tour';
import { TourLogService } from '../../services/tour-log';
import { ITour } from '../../interfaces/Tour';
import { TourList } from '../../components/tour-list/tour-list';
import { TagList } from '../../components/tag-list/tag-list';
import { Distance } from '../../components/DataDisplay/Tour/distance/distance';
import { Duration } from '../../components/DataDisplay/Tour/duration/duration';
import { LocationEnd } from '../../components/DataDisplay/Tour/Location/location-end/location-end';
import { LocationStart } from '../../components/DataDisplay/Tour/Location/location-start/location-start';
import { StarRating } from '../../components/star-rating/star-rating';
import { TourLog } from '../../components/tour-log/tour-log';
import { Router, RouterLink } from '@angular/router';
import { TransportIcon } from '../../components/transport-icon/transport-icon';
import { MapFacadeService } from '../../services/map-facade';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { switchMap } from 'rxjs/internal/operators/switchMap';
import { ITourLog } from '../../interfaces/TourLog';
import { of } from 'rxjs';
import { UserService } from '../../services/user';

@Component({
  selector: 'app-tour',
  standalone: true,
  imports: [
    TourList,
    TourLog,
    TagList,
    Distance,
    Duration,
    LocationStart,
    LocationEnd,
    StarRating,
    RouterLink,
    TransportIcon,
  ],
  templateUrl: './tour.html',
  styleUrl: './tour.css',
})
export class TourPage {
  readonly selectedTour = signal<number | null>(null);
  readonly tour = signal<ITour | null>(null);

  private userService = inject(UserService);
  private router = inject(Router);

  ngOnInit() {
    if (!this.userService.user()) {
      this.router.navigate(['/login']);
    }
  }

  tourEffect = effect(() => {
    const tourId = this.selectedTour();
    if (tourId === null) {
      this.tour.set(null);
      return;
    }

    this.tourService
      .getTourByIdServer(tourId)
      .subscribe((tour) => this.tour.set(tour));
  });

  get setTourFn() {
    return (id: number) => {
      this.selectedTour.set(id);
      this.mapFacadeService.initMap('map');
    };
  }

  isListOpen = signal<boolean>(true);

  stateCallback = (val: boolean) => {
    this.isListOpen.set(val);
  };

  durationInHours = computed(() => {
    const tour = this.tour();
    if (!tour) return '00:00';
    return (tour.estimatedTime / 3600).toFixed(2);
  });

  distanceInKm = computed(() => {
    const tour = this.tour();
    if (!tour) return '0.00';
    return (tour.distance / 1000).toFixed(2);
  });

  // TourLog stuff

  /*tourLogs = computed(() => {
    const tour = this.tour();
    if (!tour) return [];
    return this.tourLogService.getTourLogsByTourId(tour.id);
  });*/

  tourLogs: Signal<ITourLog[]> = toSignal(
    toObservable(this.tour).pipe(
      switchMap((tour) => {
        if (!tour) return of([]);
        return this.tourLogService.getTourLogsByTourIdServer(tour.id);
      })
    ),
    { initialValue: [] }
  );

  avgRating = computed(() => {
    const logs = this.tourLogs();
    if (!logs || logs.length === 0) return 0;
    const totalRating = logs.reduce((sum, log) => sum + log.rating, 0);
    return totalRating / logs.length;
  });

  constructor(
    private tourService: TourService,
    private tourLogService: TourLogService,
    private mapFacadeService: MapFacadeService,
  ) {}
}
