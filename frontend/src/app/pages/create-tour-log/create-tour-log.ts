import { Component, inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {
  FormGroup,
  FormControl,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { TourLogService } from '../../services/tour-log';
import { ITourLogCreate } from '../../interfaces/TourLog';

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

  tourId!: number;
  tourLogForm!: FormGroup;

  difficultyOptions = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
  ratingOptions = [1, 2, 3, 4, 5];

  ngOnInit() {
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
      distance: this.tourLogForm.value.distance,
      duration: this.tourLogForm.value.duration,
      rating: this.tourLogForm.value.rating,
    };

    this.tourLogService.addTourLog(newTourLog);
    this.tourLogForm.reset();
    this.router.navigate(['/tours']);
  }
}
