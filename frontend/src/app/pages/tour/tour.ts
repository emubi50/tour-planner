import { Component, computed, Signal, signal } from '@angular/core';
import { TourService } from '../../services/tour';
import { TourList } from '../../components/tour-list/tour-list';
import { Tour } from '../../services/tour';

@Component({
  selector: 'app-tour',
  standalone: true,
  imports: [TourList],
  templateUrl: './tour.html',
  styleUrl: './tour.css',
})
export class TourPage {
  selectedTour = signal<number | null>(null);
  tour: Signal<Tour | null | undefined> = computed(() => {
    const id = this.selectedTour();
    return id !== null ? this.tourService.getTourById(id) : null;
  });

  get setTourFn() {
    return (id: number) => {
      this.selectedTour.set(id);
    };
  }

  constructor(private tourService: TourService) {}
}
