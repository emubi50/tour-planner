import { Component, input } from '@angular/core';
import { LabelMetricSet } from '../../label-metric-set/label-metric-set';

@Component({
  selector: 'app-distance',
  standalone: true,
  imports: [LabelMetricSet],
  templateUrl: './distance.html',
  styleUrl: './distance.css',
})
export class Distance {
  distance = input<string | number>(0);
}
