import { Component, input } from '@angular/core';
import { LabelMetricSet } from '../../label-metric-set/label-metric-set';

@Component({
  selector: 'app-duration',
  standalone: true,
  imports: [LabelMetricSet],
  templateUrl: './duration.html',
  styleUrl: './duration.css',
})
export class Duration {
  duration = input<string | number>(0);

  // Also wrap styling passthrough to base component
  labelClass = input<string>('');
  valueClass = input<string>('');
}
