import { Component, input } from '@angular/core';
import { LabelMetricSet } from '../../label-metric-set/label-metric-set';
import { LucideRoute } from '@lucide/angular';

@Component({
  selector: 'app-distance',
  standalone: true,
  imports: [LabelMetricSet, LucideRoute],
  templateUrl: './distance.html',
  styleUrl: './distance.css',
})
export class Distance {
  // Same as duration ->
  // IMPLEMENT TOGGLING ICON AND/OR TEXT ON OFF
  distance = input<string | number>(0);

  // Also wrap styling passthrough to base component
  labelClass = input<string>('');
  valueClass = input<string>('');
}
