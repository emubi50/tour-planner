import { NgClass } from '@angular/common';
import { Component, input } from '@angular/core';

@Component({
  selector: 'app-star-rating',
  standalone: true,
  imports: [NgClass],
  templateUrl: './star-rating.html',
  styleUrl: './star-rating.css',
})
export class StarRating {
  rating = input<number>(0);
  reverse = input<boolean>(false);

  hideStars = input<boolean>(false);
  hideText = input<boolean>(false);

  get normalizedRating() {
    return Math.max(0, Math.min(5, this.rating()));
  }

  get normalizedRatingString() {
    const rating = this.normalizedRating;
    return rating.toFixed(1);
  }
}
