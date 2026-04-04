import { Component, computed, Signal, signal } from '@angular/core';
import { TourService } from '../../services/tour';
import { TourList } from '../../components/tour-list/tour-list';
import { ITour } from '../../services/tour';
import { TagList } from '../../components/tag-list/tag-list';
import { Distance } from '../../components/DataDisplay/Tour/distance/distance';
import { Duration } from '../../components/DataDisplay/Tour/duration/duration';
import { LocationEnd } from '../../components/DataDisplay/Tour/Location/location-end/location-end';
import { LocationStart } from '../../components/DataDisplay/Tour/Location/location-start/location-start';
import { StarRating } from '../../components/star-rating/star-rating';

@Component({
  selector: 'app-tour',
  standalone: true,
  imports: [
    TourList,
    TagList,
    Distance,
    Duration,
    LocationStart,
    LocationEnd,
    StarRating,
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

  constructor(private tourService: TourService) {}
}
