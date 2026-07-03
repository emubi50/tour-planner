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

  isLoading = true;
  loadError = false;
  isSubmitting = false;
  submitError = false;

  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      this.tourId = Number.parseInt(params['tourId']);
      this.loadTour();
    });
  }

  private loadTour(): void {
    this.isLoading = true;
    this.loadError = false;
    this.tourService.getTourByIdServer(this.tourId).subscribe({
      next: (tours) => {
        this.tourData = tours;
        this.initializeForm();
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        this.loadError = true;
        console.error('Error loading tour:', err);
      }
    });
  }

  private initializeForm(): void {
    this.tourForm = new FormGroup({
      name: new FormControl(this.tourData.name, [
        Validators.required,
        Validators.maxLength(200),
      ]),
      description: new FormControl(this.tourData.description, [
        Validators.required,
        Validators.maxLength(500),
      ]),
      transportType: new FormControl(this.tourData.transportType, [Validators.required]),
      startLocation: new FormControl(this.tourData.from, [Validators.required]),
      endLocation: new FormControl(this.tourData.to, [Validators.required]),
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
      from: this.tourForm.value.startLocation ?? this.tourData.from,
      to: this.tourForm.value.endLocation ?? this.tourData.to,
      tags: this.tourForm.value.tags ?? this.tourData.tags,
      transportType: this.transportTypeFormValue,
      estimatedTime: this.tourData.estimatedTime,
      distance: this.tourData.distance,
      rating: this.tourData.rating,
      popularity: this.tourData.popularity,
      childFriendliness: this.tourData.childFriendliness,
    };

    this.isSubmitting = true;
    this.submitError = false;
    this.tourService.updateTourServer(updatedTour).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.tourData = updatedTour;
        this.tourForm.markAsPristine();
        this.tourForm.markAsUntouched();
      },
      error: (err) => {
        this.isSubmitting = false;
        this.submitError = true;
        console.error('Error updating tour:', err);
      }
    });
  }

  deleteTour(): void {
    this.tourService.deleteTourServer(this.tourId).subscribe({
      next: () => {
        this.router.navigate(['/tours']);
      },
      error: (err) => {
        console.error('Error deleting tour:', err);
      }
    });
  }
}
