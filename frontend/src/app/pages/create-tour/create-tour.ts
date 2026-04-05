import { Component } from '@angular/core';
import {
  FormGroup,
  FormControl,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-create-tour',
  imports: [ReactiveFormsModule],
  templateUrl: './create-tour.html',
  styleUrl: './create-tour.css',
})
export class CreateTour {
  transportTypeOptions: string[] = [
    'Bicycle',
    'Walking',
    'Bus',
    'Public transport',
  ];

  tour = {
    name: 'Tour name',
    description: 'Tour description',
    transportType: this.transportTypeOptions[0],
    startLocation: 'Tour start location',
    endLocation: 'Tour end location',
    tags: 'tag 1,tag 2',
  };

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
  }
}
