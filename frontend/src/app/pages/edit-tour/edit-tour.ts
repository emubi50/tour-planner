import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  FormGroup,
  FormControl,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { TourService } from '../../services/tour';
import { ITour } from '../../interfaces/Tour';

@Component({
  selector: 'app-edit-tour',
  imports: [ReactiveFormsModule],
  templateUrl: './edit-tour.html',
  styleUrl: './edit-tour.css',
})
export class EditTour {
  private activatedRouter = inject(ActivatedRoute);
  private tourService = inject(TourService);

  tourId = -1;

  constructor() {
    this.activatedRouter.params.subscribe((params) => {
      this.tourId = Number.parseInt(params['tourId']);
    });
  }

  transportTypeOptions: string[] = [
    'Bicycle',
    'Walking',
    'Bus',
    'Public transport',
  ];

  tourData: ITour = this.tourService.getTourById(this.tourId)!;

  tourForm = new FormGroup({
    name: new FormControl(this.tourData.name, [
      Validators.required,
      Validators.maxLength(200),
    ]),
    description: new FormControl(this.tourData.description, [
      Validators.required,
      Validators.maxLength(500),
    ]),
    transportType: new FormControl('Bicycle', [Validators.required]),
    startLocation: new FormControl(this.tourData.start, [Validators.required]),
    endLocation: new FormControl(this.tourData.end, [Validators.required]),
    tags: new FormControl(this.tourData.tags),
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

    // TODO: update any changed fields in tourData

    this.tourService.updateTour(this.tourData);
  }
}
