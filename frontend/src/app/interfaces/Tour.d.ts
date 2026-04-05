/* Needs to be changed!!!
 * Either create a new interface (ITourOut) wich is then returned by the TourService;
 * Or create a new interface (ITourStore),
 * and only change the TourService's internal storage type;
 * Values like rating shouldn't be stored but computed by the service at runtime;
 * UNLESS we add triggers to the DB later on,
 * that update's the rating on the moment a tour log is added/edited/deleted.
 */

import { TransportType } from '../enums/TransportType';

export interface ITour {
  id: number;
  name: string;
  description: string;
  duration: number; // in seconds
  distance: number; // in meters
  start: string;
  end: string;
  tags: string[];
  transportType: TransportType;
  rating: number; // from 0 to 5
}

export interface ITourCreate {
  name: string;
  description: string;
  start: string;
  end: string;
  tags: string[];
  transportType: TransportType;
}
