import { Component, input } from '@angular/core';
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
}
