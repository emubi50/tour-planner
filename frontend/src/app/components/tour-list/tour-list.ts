import { Component, input, signal } from '@angular/core';
import { TourService } from '../../services/tour';
import { TourShort } from '../tour-short/tour-short';
import { SearchBar } from '../search-bar/search-bar';
import { LucideMapPinned } from '@lucide/angular';
import { ITour } from '../../interfaces/Tour';

@Component({
  selector: 'app-tour-list',
  standalone: true,
  imports: [TourShort, LucideMapPinned, SearchBar],
  templateUrl: './tour-list.html',
  styleUrl: './tour-list.css',
})
export class TourList {
  tours = signal<ITour[]>([]);

  // Not yet implemented
  width = input<number>(140);

  onClickFn = input<(id: number) => void>();

  get onClick() {
    return this.onClickFn() ?? (() => {});
  }

  constructor(private tourService: TourService) {
    this.tourService.getToursServer().subscribe((tours) => {
      this.tours.set(tours);
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

  onSearchChange(term: string) {
    if (!term.trim()) {
      this.tourService.getToursServer().subscribe((tours) => {
        this.tours.set(tours);
      });
      return;
    }
    this.tourService.searchToursServer(term).subscribe((tours) => {
      this.tours.set(tours);
    });
  }
}
