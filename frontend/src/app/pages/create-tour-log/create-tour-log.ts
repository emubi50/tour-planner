import { Component, inject, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {
  FormGroup,
  FormControl,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { TourLogService } from '../../services/tour-log';
import { ITourLogCreate } from '../../interfaces/TourLog';
import { UserService } from '../../services/user';

@Component({
  selector: 'app-create-tour-log',
  imports: [ReactiveFormsModule],
  templateUrl: './create-tour-log.html',
  styleUrl: './create-tour-log.css',
})
export class CreateTourLog {
  private router = inject(Router);
  private activatedRoute = inject(ActivatedRoute);
  private tourLogService = inject(TourLogService);

private userService = inject(UserService);

  tourId!: number;
  tourLogForm!: FormGroup;

  difficultyOptions = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
  ratingOptions = [1, 2, 3, 4, 5];
  isSubmitting = false;
  submitError = false;

  ngOnInit() {
    if (!this.userService.user()) {
      this.router.navigate(['/login']);
    }

    this.activatedRoute.params.subscribe((params) => {
      this.tourId = Number.parseInt(params['tourId']);

      this.tourLogForm = new FormGroup({
        date: new FormControl('', [Validators.required]),
        comment: new FormControl('', [
          Validators.required,
          Validators.maxLength(500),
        ]),
        difficulty: new FormControl('', [
          Validators.required,
          Validators.min(1),
          Validators.max(10),
        ]),
        distance: new FormControl('', [Validators.required, Validators.min(0)]),
        duration: new FormControl('', [Validators.required, Validators.min(0)]),
        rating: new FormControl('', [
          Validators.required,
          Validators.min(0),
          Validators.max(5),
        ]),
      });
    });
  }

  get date() {
    return this.tourLogForm.get('date');
  }

  get comment() {
    return this.tourLogForm.get('comment');
  }

  get difficulty() {
    return this.tourLogForm.get('difficulty');
  }

  get distance() {
    return this.tourLogForm.get('distance');
  }

  get duration() {
    return this.tourLogForm.get('duration');
  }

  get rating() {
    return this.tourLogForm.get('rating');
  }

  onSubmit() {
    console.warn(this.tourLogForm.value);

    if (this.tourLogForm.invalid) {
      this.tourLogForm.markAllAsTouched();
      return;
    }

    const newTourLog: ITourLogCreate = {
      tourId: this.tourId,
      date: this.tourLogForm.value.date,
      comment: this.tourLogForm.value.comment,
      difficulty: this.tourLogForm.value.difficulty,
      totalDistance: this.tourLogForm.value.distance,
      totalTime: this.tourLogForm.value.duration,
      rating: this.tourLogForm.value.rating,
    };
    this.isSubmitting = true;
    this.submitError = false;
    this.tourLogService.addTourLogServer(this.tourId, newTourLog).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.tourLogForm.reset();
        this.router.navigate(['/tours']);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.submitError = true;
        console.error('Error adding tour log:', err);
      }
    });
  }
}
