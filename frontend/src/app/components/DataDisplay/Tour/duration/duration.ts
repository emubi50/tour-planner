import { Component, input } from '@angular/core';
import { LabelMetricSet } from '../../label-metric-set/label-metric-set';
import { LucideTimer } from '@lucide/angular';

@Component({
  selector: 'app-duration',
  standalone: true,
  imports: [LabelMetricSet, LucideTimer],
  templateUrl: './duration.html',
  styleUrl: './duration.css',
})
export class Duration {
  // !!FOR LATER!! Implement toggling icon and/or text on off
  duration = input<string | number>(0);

  // Also wrap styling passthrough to base component
  labelClass = input<string>('');
  valueClass = input<string>('');
}
