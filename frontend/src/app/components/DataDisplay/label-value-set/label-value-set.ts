import { NgClass } from '@angular/common';
import { Component, computed, input } from '@angular/core';
import { twMerge } from 'tailwind-merge';

@Component({
  selector: 'app-label-value-set',
  standalone: true,
  imports: [NgClass],
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

  // Styling passthrough
  labelClass = input<string>('');
  valueClass = input<string>('');

  // Styling

  labelBase = 'text-gray-500';
  valueBase = 'text-nowrap overflow-hidden text-ellipsis';

  labelClasses = computed(() => {
    return twMerge(this.labelBase, this.labelClass());
  });

  valueClasses = computed(() => {
    return twMerge(this.valueBase, this.valueClass());
  });
}
