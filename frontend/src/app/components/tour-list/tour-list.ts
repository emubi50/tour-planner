import {
  Component,
  input,
  Signal,
  signal,
  WritableSignal,
} from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { TourService } from '../../services/tour';
import { TourShort } from '../tour-short/tour-short';

@Component({
  selector: 'app-tour-list',
  standalone: true,
  imports: [TourShort],
  templateUrl: './tour-list.html',
  styleUrl: './tour-list.css',
})
export class TourList {
  readonly tours;

  // Not yet implemented
  width = input<number>(140);

  onClickFn = input<(id: number) => void>();

  get onClick() {
    return this.onClickFn() ?? (() => {});
  }

  constructor(private tourService: TourService) {
    this.tours = this.tourService.getTours();
  }

  isListOpen = signal<boolean>(true);
  stateCallback = input<(val: boolean) => void>();

  toggleList() {
    this.isListOpen.update((isOpen) => !isOpen);
    let cb;
    if ((cb = this.stateCallback())) {
      cb(this.isListOpen());
    }
  }
}
