import { Component, forwardRef, signal, input, inject } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Subject, debounceTime, distinctUntilChanged, switchMap, of, catchError } from 'rxjs';
import { ILocation} from '../../interfaces/Location';
import { OpenRouteService } from '../../services/ors';

@Component({
  selector: 'app-location-suggestion',
  standalone: true,
  templateUrl: './location-suggestion.html',
  styleUrl: './location-suggestion.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => LocationSuggestion),
      multi: true,
    },
  ],
})
export class LocationSuggestion implements ControlValueAccessor {
  placeholder = input<string>('Location');
  inputClass = input<string>('border-gray-300');

  private locationService = inject(OpenRouteService);

  query = signal('');
  selectedLocation = signal<ILocation | null>(null);
  suggestions = signal<ILocation[]>([]);
  isOpen = signal(false);
  disabled = signal(false);

  private searchSubject = new Subject<string>();
  private onChange: (value: ILocation | null) => void = () => {};
  private onTouched: () => void = () => {};

  constructor() {
    this.searchSubject
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        switchMap((term) => {
          if (!term || term.trim().length < 2) {
            return of<ILocation[]>([]);
          }
          return this.locationService.searchLocation(term).pipe(catchError(() => of([])));
        })
      )
      .subscribe((results) => {
        this.suggestions.set(results);
        this.isOpen.set(results.length > 0);
      });
  }

  onInput(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.query.set(value);
    this.selectedLocation.set(null);
    this.onChange(null);
    this.searchSubject.next(value);
  }

  selectSuggestion(suggestion: ILocation) {
    this.query.set(suggestion.label);
    this.selectedLocation.set(suggestion);
    this.onChange(suggestion);
    this.isOpen.set(false);
    this.suggestions.set([]);
  }

  onFocus() {
    if (this.suggestions().length > 0) this.isOpen.set(true);
  }

  onBlur() {
    this.isOpen.set(false);
    this.onTouched();
  }

  writeValue(value: ILocation | null): void {
    this.selectedLocation.set(value);
    this.query.set(value?.label ?? '');
  }
  registerOnChange(fn: (value: ILocation | null) => void): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }
  setDisabledState(isDisabled: boolean): void {
    this.disabled.set(isDisabled);
  }
}