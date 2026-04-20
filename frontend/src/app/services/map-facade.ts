import * as L from 'leaflet';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MapFacadeService {
  private map: L.Map | null = null;

  initMap(containerId: string): void {
    if (this.map) return;

    this.map = L.map(containerId, {
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
}
