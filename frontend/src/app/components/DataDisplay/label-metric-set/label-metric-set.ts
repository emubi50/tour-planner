import { NgClass } from '@angular/common';
import { Component, input, computed } from '@angular/core';
import { twMerge } from 'tailwind-merge';

@Component({
  selector: 'app-label-metric-set',
  standalone: true,
  imports: [NgClass],
  templateUrl: './label-metric-set.html',
  styleUrl: './label-metric-set.css',
})
export class LabelMetricSet {
  label = input<string>('Label');
  value = input<string | number>('Value');
  metric = input.required<string>();

  omitColon = input<boolean>(false);

  get Label() {
    return this.omitColon() ? this.label() : `${this.label()}:`;
  }

  get ValueWithMetric() {
    return `${this.value()} ${this.metric()}`.trim();
  }

  // Styling passthrough
  labelClass = input<string>('');
  valueClass = input<string>('');

  // Styling

  labelBase = 'text-gray-500';
  valueBase = 'text-nowrap';

  labelClasses = computed(() => {
    return twMerge(this.labelBase, this.labelClass());
  });

  valueClasses = computed(() => {
    return twMerge(this.valueBase, this.valueClass());
  });
}
