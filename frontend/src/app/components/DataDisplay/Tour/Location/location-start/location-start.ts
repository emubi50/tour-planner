import { Component, input } from '@angular/core';
import { LabelValueSet } from '../../../label-value-set/label-value-set';

@Component({
  selector: 'app-location-start',
  standalone: true,
  imports: [LabelValueSet],
  templateUrl: './location-start.html',
  styleUrl: './location-start.css',
})
export class LocationStart {
  location = input<string>('Ort');
}
