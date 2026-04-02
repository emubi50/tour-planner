import { Component, input } from '@angular/core';

@Component({
  selector: 'app-label-metric-set',
  standalone: true,
  imports: [],
  templateUrl: './label-metric-set.html',
  styleUrl: './label-metric-set.css',
})
export class LabelMetricSet {
  label = input<string>('Label');
  value = input<string | number>('Value');
  metric = input<string>('');

  omitColon = input<boolean>(false);

  get Label() {
    return this.omitColon() ? this.label() : `${this.label()}:`;
  }

  get ValueWithMetric() {
    return this.metric() ? `${this.value()} ${this.metric()}` : this.value();
  }
}
