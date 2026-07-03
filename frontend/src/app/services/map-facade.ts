import * as L from 'leaflet';
import { Injectable } from '@angular/core';
import { ILocation } from '../interfaces/Location';

@Injectable({
  providedIn: 'root',
})
export class MapFacadeService {
  private map: L.Map | null = null;

  private route?: L.Polyline;

  initMap(container: HTMLElement): void {
    if (this.map) return;

    this.map = L.map(container, {
      zoomControl: true,
      attributionControl: true,
    });

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution:
        '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
    }).addTo(this.map);

    this.map.setView([48.2082, 16.3738], 12); // Vienna
  }

  setCenter(lat: number, lng: number, zoom = 13): void {
    this.map?.setView([lat, lng], zoom);
  }

  setMarker(lat: number, lng: number): void {
    if (!this.map) return;
    L.marker([lat, lng]).addTo(this.map);
  }

  setRoute(routePathStr: string): void {
    if (!this.map) return;

    this.route?.remove();

    const latLngs = routePathStr.split(";").filter(p => p.length > 0).map(point => {
    const [lon, lat] = point.split(",").map(Number);
    return L.latLng(lat, lon);
  });

  this.route = L.polyline(latLngs, {
        weight: 5,
        opacity: 0.8
    }).addTo(this.map);

    this.map.fitBounds(this.route.getBounds(), {
        padding: [25, 25]
    });
  }
}
