import { Component, input, signal } from '@angular/core';
import { TourService } from '../../services/tour';
import { TourShort } from '../tour-short/tour-short';
import { LucideMapPinned } from '@lucide/angular';

@Component({
  selector: 'app-tour-list',
  standalone: true,
  imports: [TourShort, LucideMapPinned],
  templateUrl: './tour-list.html',
  styleUrl: './tour-list.css',
})
export class TourList {
  tours: any;

  // Not yet implemented
  width = input<number>(140);

  onClickFn = input<(id: number) => void>();

  get onClick() {
    return this.onClickFn() ?? (() => {});
  }

  constructor(private tourService: TourService) {
    this.tourService.getToursServer().subscribe((tours) => {
      this.tours = signal(tours);
    });
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
