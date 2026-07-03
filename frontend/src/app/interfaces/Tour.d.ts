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
  estimatedTime: number; // in seconds
  distance: number; // in meters
  from: ILocation;
  to: ILocation;
  tags: string[];
  transportType: TransportType;
  rating: number; // from 0 to 5
  popularity: number; // derive from number of logs
  childFriendliness: number; // derived from recorded difficulty values, total time and distance
  routeInformation: string; // "lon,lat;lon,lat;lon,lat;..."
}

export interface ITourCreate {
  name: string;
  description: string;
  from: ILocation;
  to: ILocation;
  tags: string[];
  transportType: TransportType;
}
