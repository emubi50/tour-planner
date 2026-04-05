export interface ITour {
  id: number;
  name: string;
  description: string;
  duration: number; // in seconds
  distance: number; // in meters
  start: string;
  end: string;
  tags: string[];
  rating: number; // from 0 to 5
}

// Copy from interface Tour but without id
export interface ITourCreate {
  name: string;
  description: string;
  duration: number; // in seconds
  distance: number; // in meters
  start: string;
  end: string;
  tags: string[];
  rating: number; // from 0 to 5
}
