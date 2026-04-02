import { Component } from '@angular/core';
import { StarRating } from '../star-rating/star-rating';
import { Duration } from '../DataDisplay/Tour/duration/duration';
import { Distance } from '../DataDisplay/Tour/distance/distance';
import { LocationStart } from '../DataDisplay/Tour/Location/location-start/location-start';
import { LocationEnd } from '../DataDisplay/Tour/Location/location-end/location-end';

@Component({
  selector: 'app-tour-short',
  standalone: true,
  imports: [StarRating, Duration, Distance, LocationStart, LocationEnd],
  templateUrl: './tour-short.html',
  styleUrl: './tour-short.css',
})
export class TourShort {
  tour = {
    id: 0,
    name: "From Vienna's beauties to Salzburg's wonders over the boat - a scenic tour through Austria",
    description: 'This is the first tour.',
    duration: 240 * 60, // in seconds
    distance: 12000, // in meters
    start: 'Vienna, Austria',
    end: 'Salzburg, Austria',
    tags: ['Scenic', 'Fast', 'City'],
    rating: 4.3,
  };

  get durationInHours() {
    return (this.tour.duration / 3600).toFixed(2).replace('.', ':');
  }

  get distanceInKm() {
    return (this.tour.distance / 1000).toFixed(2);
  }
}
