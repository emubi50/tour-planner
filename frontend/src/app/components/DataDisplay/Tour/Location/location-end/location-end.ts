import { Component, input } from '@angular/core';
import { LabelValueSet } from '../../../label-value-set/label-value-set';

@Component({
  selector: 'app-location-end',
  standalone: true,
  imports: [LabelValueSet],
  templateUrl: './location-end.html',
  styleUrl: './location-end.css',
})
export class LocationEnd {
  location = input<string>('Ort');

  // Also wrap styling passthrough to base component
  labelClass = input<string>('');
  valueClass = input<string>('');
}
