import { Component, input, computed } from '@angular/core';
import { StarRating } from '../star-rating/star-rating';
import { Duration } from '../DataDisplay/Tour/duration/duration';
import { Distance } from '../DataDisplay/Tour/distance/distance';
import { LocationStart } from '../DataDisplay/Tour/Location/location-start/location-start';
import { LocationEnd } from '../DataDisplay/Tour/Location/location-end/location-end';
import { ITour } from '../../services/tour';
import { TagList } from '../tag-list/tag-list';

@Component({
  selector: 'app-tour-short',
  standalone: true,
  imports: [
    StarRating,
    Duration,
    Distance,
    LocationStart,
    LocationEnd,
    TagList,
  ],
  templateUrl: './tour-short.html',
  styleUrl: './tour-short.css',
})
export class TourShort {
  tour = input.required<ITour>();

  durationInHours = computed(() => {
    const tour = this.tour();
    return (tour.duration / 3600).toFixed(2).replace('.', ':');
  });

  distanceInKm = computed(() => {
    const tour = this.tour();
    return (tour.distance / 1000).toFixed(2);
  });
}
