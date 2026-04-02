import { Component, input } from '@angular/core';

@Component({
  selector: 'app-label-value-set',
  standalone: true,
  imports: [],
  templateUrl: './label-value-set.html',
  styleUrl: './label-value-set.css',
})
export class LabelValueSet {
  label = input<string>('Label');
  value = input<string | number>('Value');

  omitColon = input<boolean>(false);

  get Label() {
    return this.omitColon() ? this.label() : `${this.label()}:`;
  }
}
