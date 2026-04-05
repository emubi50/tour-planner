import { Component, computed, input } from '@angular/core';
import { ITourLog } from '../../services/tour-log';
import { StarRating } from '../star-rating/star-rating';
import { DatePipe } from '@angular/common';
import { LabelValueSet } from '../DataDisplay/label-value-set/label-value-set';
import { Duration } from '../DataDisplay/Tour/duration/duration';
import { Distance } from '../DataDisplay/Tour/distance/distance';

@Component({
  selector: 'app-tour-log',
  standalone: true,
  imports: [StarRating, DatePipe, LabelValueSet, Duration, Distance],
  templateUrl: './tour-log.html',
  styleUrl: './tour-log.css',
})
export class TourLog {
  tourLog = input.required<ITourLog>();

  durationInHours = computed(() => {
    const tourLog = this.tourLog();
    if (!tourLog) return '00:00';
    return (tourLog.duration / 3600).toFixed(2).replace('.', ':');
  });

  distanceInKm = computed(() => {
    const tourLog = this.tourLog();
    if (!tourLog) return '0.00';
    return (tourLog.distance / 1000).toFixed(2);
  });
}
