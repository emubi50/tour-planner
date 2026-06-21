import { Component, input } from '@angular/core';
import {
  LucideBike,
  LucideBus,
  LucideCar,
  LucideFootprints,
} from '@lucide/angular';
import { TransportType, TransportTypeText } from '../../enums/TransportType';

@Component({
  selector: 'app-transport-icon',
  standalone: true,
  imports: [LucideBike, LucideFootprints, LucideCar, LucideBus],
  templateUrl: './transport-icon.html',
  styleUrl: './transport-icon.css',
})
export class TransportIcon {
  transportType = input.required<TransportType>();

  hideIcon = input<boolean>(false);
  showText = input<boolean>(false);

  get typeText() {
    return TransportTypeText[this.transportType()];
  }

  // expose as property to be accessible in template
  TransportType = TransportType;
}
