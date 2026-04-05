import { Component, inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {
  FormGroup,
  FormControl,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { TourService } from '../../services/tour';
import { ITour } from '../../interfaces/Tour';
import { TransportType } from '../../enums/TransportType';

@Component({
  selector: 'app-edit-tour',
  imports: [ReactiveFormsModule],
  templateUrl: './edit-tour.html',
  styleUrl: './edit-tour.css',
})
export class EditTour {
  private router = inject(Router);
  private activatedRoute = inject(ActivatedRoute);
  private tourService = inject(TourService);

  private tourId: number = -1;

  tourData!: ITour;
  tourForm!: FormGroup;

  transportTypeOptions: string[] = [
    'Bicycle',
    'Walking',
    'Bus',
    'Public transport',
  ];

  transportTypeFormValue = TransportType.BIKE;

  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      this.tourId = Number.parseInt(params['tourId']);
    });

    this.tourData = this.tourService.getTourById(this.tourId)!;
    this.transportTypeFormValue = this.tourData.transportType;

    this.tourForm = new FormGroup({
      name: new FormControl(this.tourData.name, [
        Validators.required,
        Validators.maxLength(200),
      ]),
      description: new FormControl(this.tourData.description, [
        Validators.required,
        Validators.maxLength(500),
      ]),
      transportType: new FormControl('Bicycle', [Validators.required]),
      startLocation: new FormControl(this.tourData.start, [
        Validators.required,
      ]),
      endLocation: new FormControl(this.tourData.end, [Validators.required]),
      tags: new FormControl(this.tourData.tags),
    });
  }

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

    switch (this.tourForm.value.TransportType) {
      case 'Bicycle':
        this.transportTypeFormValue = TransportType.BIKE;
        break;
      case 'Walking':
        this.transportTypeFormValue = TransportType.WALK;
        break;
      case 'Bus':
        this.transportTypeFormValue = TransportType.CAR;
        break;
      case 'Public transport':
        this.transportTypeFormValue = TransportType.PUBLIC;
        break;
      default:
        break;
    }

    const updatedTour: ITour = {
      id: this.tourData.id,
      name: this.tourForm.value.name ?? this.tourData.name,
      description: this.tourForm.value.description ?? this.tourData.description,
      start: this.tourForm.value.startLocation ?? this.tourData.start,
      end: this.tourForm.value.endLocation ?? this.tourData.end,
      tags: this.tourForm.value.tags ?? this.tourData.tags,
      transportType: this.transportTypeFormValue,
      duration: this.tourData.duration,
      distance: this.tourData.distance,
      rating: this.tourData.rating,
    };

    this.tourService.updateTour(updatedTour);
    this.tourForm.markAsPristine();
    this.tourForm.markAsUntouched();
  }

  deleteTour(): void {
    this.tourService.deleteTour(this.tourId);
    this.router.navigate(['/tours']);
  }
}
