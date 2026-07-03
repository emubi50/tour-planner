interface IRoute {
    distance: number;
    estimatedTime: number;
    coordinates: number[][]; // [[lon, lat], [lon, lat], ...]
}