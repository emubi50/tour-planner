import { Component, computed, Signal, signal } from '@angular/core';
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
import { RouterLink } from '@angular/router';
import { TransportIcon } from '../../components/transport-icon/transport-icon';

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
  selectedTour = signal<number | null>(null);
  tour: Signal<ITour | null | undefined> = computed(() => {
    const id = this.selectedTour();
    return id !== null ? this.tourService.getTourById(id) : null;
  });

  get setTourFn() {
    return (id: number) => {
      this.selectedTour.set(id);
    };
  }

  isListOpen = signal<boolean>(true);

  stateCallback = (val: boolean) => {
    this.isListOpen.set(val);
  };

  durationInHours = computed(() => {
    const tour = this.tour();
    if (!tour) return '00:00';
    return (tour.duration / 3600).toFixed(2).replace('.', ':');
  });

  distanceInKm = computed(() => {
    const tour = this.tour();
    if (!tour) return '0.00';
    return (tour.distance / 1000).toFixed(2);
  });

  // TourLog stuff

  tourLogs = computed(() => {
    const tour = this.tour();
    if (!tour) return [];
    return this.tourLogService.getTourLogsByTourId(tour.id);
  });

  constructor(
    private tourService: TourService,
    private tourLogService: TourLogService,
  ) {}
}
