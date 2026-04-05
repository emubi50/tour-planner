import { Component, input } from '@angular/core';
import { LabelValueSet } from '../../../label-value-set/label-value-set';
import { LucideMapPin } from '@lucide/angular';

@Component({
  selector: 'app-location-start',
  standalone: true,
  imports: [LabelValueSet, LucideMapPin],
  templateUrl: './location-start.html',
  styleUrl: './location-start.css',
})
export class LocationStart {
  location = input<string>('Ort');

  // Also wrap styling passthrough to base component
  labelClass = input<string>('');
  valueClass = input<string>('');
}
