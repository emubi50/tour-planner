import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ILocation, ILocationSearchResponse } from '../interfaces/Location';

@Injectable({
    providedIn: 'root',
})
export class OpenRouteService {
    private http = inject(HttpClient);

    searchLocation(query: string): Observable<ILocation[]> {
        return this.http.get<ILocationSearchResponse>(`/api/location/search?query=${encodeURIComponent(query)}`)
            .pipe(map((response) => response.locations));;
    }
}
