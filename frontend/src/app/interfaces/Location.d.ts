export interface ILocation {
    label: string;
    coordinates: [number, number]; // [longitude, latitude]
}

export interface ILocationSearchResponse {
    timestamp: number;
    locations: ILocation[];
}