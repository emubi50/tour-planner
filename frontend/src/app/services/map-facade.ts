import * as L from 'leaflet';
import { Injectable } from '@angular/core';
import { ILocation } from '../interfaces/Location';

@Injectable({
  providedIn: 'root',
})
export class MapFacadeService {
  private map: L.Map | null = null;

  private route?: L.Polyline;
  private markers: L.Marker[] = [];

  private TargetMarkerOpt: L.MarkerOptions = {
    icon: L.icon({
      iconUrl: 'flag-triangle-right.png',
      iconSize: [32, 32],
      iconAnchor: [8, 32],
    }),
  };

  private StartMarkerOpt: L.MarkerOptions = {
    icon: L.icon({
      iconUrl: 'map-pin.png',
      iconSize: [32, 32],
      iconAnchor: [16, 32],
    }),
  };

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

  removeMap(): void {
    if (this.map) {
      this.map.remove();
      this.map = null;
    }
  }

  setCenter(lat: number, lng: number, zoom = 13): void {
    this.map?.setView([lat, lng], zoom);
  }

  setMarker(lat: number, lng: number, options?: L.MarkerOptions): void {
    if (!this.map) return;
    const marker = L.marker([lat, lng], options).addTo(this.map);
    this.markers.push(marker);
  }

  setRoute(routePathStr: string): void {
    if (!this.map) return;

    this.route?.remove();
    this.markers.forEach((marker) => marker.remove());

    const latLngs = routePathStr
      .split(';')
      .filter((p) => p.length > 0)
      .map((point) => {
        const [lon, lat] = point.split(',').map(Number);
        return L.latLng(lat, lon);
      });

    this.route = L.polyline(latLngs, {
      weight: 5,
      opacity: 0.8,
    }).addTo(this.map);

    this.setMarker(latLngs[0].lat, latLngs[0].lng, this.StartMarkerOpt);
    this.setMarker(
      latLngs[latLngs.length - 1].lat,
      latLngs[latLngs.length - 1].lng,
      this.TargetMarkerOpt,
    );

    this.map.fitBounds(this.route.getBounds(), {
      padding: [25, 25],
    });
  }
}
