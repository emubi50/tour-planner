import { Component, inject } from '@angular/core';
import {
  FormGroup,
  FormControl,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { TourService } from '../../services/tour';
import { TransportType } from '../../enums/TransportType';

@Component({
  selector: 'app-create-tour',
  imports: [ReactiveFormsModule],
  templateUrl: './create-tour.html',
  styleUrl: './create-tour.css',
})
export class CreateTour {
  private tourService = inject(TourService);

  transportTypeOptions: string[] = [
    'Bicycle',
    'Walking',
    'Bus',
    'Public transport',
  ];

  tourForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.maxLength(200)]),
    description: new FormControl('', [
      Validators.required,
      Validators.maxLength(500),
    ]),
    transportType: new FormControl('', [Validators.required]),
    startLocation: new FormControl('', [Validators.required]),
    endLocation: new FormControl('', [Validators.required]),
    tags: new FormControl(''),
  });

  get name() {
    return this.tourForm.get('name');
  }

  get description() {
    return this.tourForm.get('description');
  }

  get transportType() {
    return this.tourForm.get('transportType');
  }

  get startLocation() {
    return this.tourForm.get('startLocation');
  }

  get endLocation() {
    return this.tourForm.get('endLocation');
  }

  onSubmit() {
    console.warn(this.tourForm.value);

    if (this.tourForm.invalid) {
      this.tourForm.markAllAsTouched();
      return;
    }

    this.tourService.addTour({
      name: this.tourForm.value.name!,
      description: this.tourForm.value.description!,
      start: this.tourForm.value.startLocation!,
      end: this.tourForm.value.endLocation!,
      tags:
        this.tourForm.value.tags
          ?.split(',')
          .map((t) => t.trim())
          .filter((t) => t.length > 0) ?? [],
      transportType: TransportType.BIKE, // TODO: map from form value
    });

    this.tourForm.reset();
  }
}
