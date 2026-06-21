import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import {
  FormGroup,
  FormControl,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { TourService } from '../../services/tour';
import { TransportType, TransportTypeText } from '../../enums/TransportType';
import { TransportIcon } from '../../components/transport-icon/transport-icon';

@Component({
  selector: 'app-create-tour',
  imports: [ReactiveFormsModule, TransportIcon],
  templateUrl: './create-tour.html',
  styleUrl: './create-tour.css',
})
export class CreateTour {
  private router = inject(Router);
  private tourService = inject(TourService);

  // Expose enum to template
  TransportType = TransportType;
  TransportTypeText = TransportTypeText;

  transportTypes = Object.values(TransportType).filter(
    (v) => typeof v === 'number',
  );

  tourForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.maxLength(200)]),
    description: new FormControl('', [
      Validators.required,
      Validators.maxLength(500),
    ]),
    transportType: new FormControl(0, {
      validators: [Validators.required],
      nonNullable: true,
    }),
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

    this.tourService
      .addTourServer({
        name: this.tourForm.value.name!,
        description: this.tourForm.value.description!,
        from: this.tourForm.value.startLocation!,
        to: this.tourForm.value.endLocation!,
        tags:
          this.tourForm.value.tags
            ?.split(',')
            .map((t) => t.trim())
            .filter((t) => t.length > 0) ?? [],
        transportType: this.tourForm.value.transportType!,
      })
      .subscribe(() => {
        this.tourForm.reset();
        this.router.navigate(['/tours']);
      });
  }
}
