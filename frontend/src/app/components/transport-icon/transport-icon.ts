import { Component, input } from '@angular/core';
import {
  LucideBike,
  LucideBus,
  LucideCar,
  LucideFootprints,
} from '@lucide/angular';
import { TransportType } from '../../enums/TransportType';

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
    switch (this.transportType()) {
      case TransportType.BIKE:
        return 'Bike';
      case TransportType.WALK:
        return 'Walk';
      case TransportType.CAR:
        return 'Car';
      case TransportType.PUBLIC:
        // Change to 'Public Transport' once I figure out,
        // how I make the Tour Page not cooked if it is
        return 'Public';
    }
  }

  // expose as property to be accessible in template
  TransportType = TransportType;
}
