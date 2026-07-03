export interface ITourLog {
  id: number;
  tourId: number;
  date: Date;
  comment: string;
  difficulty: number;
  distance: number;
  duration: number;
  rating: number;
}

// Copy of ITourLog without id
export interface ITourLogCreate {
  tourId: number;
  date: DateTime;
  comment: string;
  difficulty: number;
  totalDistance: number;
  totalTime: number;
  rating: number;
}
