import { Component, inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {
  FormGroup,
  FormControl,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { TourLogService } from '../../services/tour-log';
import { ITourLog } from '../../interfaces/TourLog';

@Component({
  selector: 'app-edit-tour-log',
  imports: [ReactiveFormsModule],
  templateUrl: './edit-tour-log.html',
  styleUrl: './edit-tour-log.css',
})
export class EditTourLog {
  private router = inject(Router);
  private activatedRoute = inject(ActivatedRoute);
  private tourLogService = inject(TourLogService);

  private tourId: number = -1;
  private tourLogId: number = -1;

  tourLogData!: ITourLog;
  tourLogForm!: FormGroup;

  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      this.tourId = Number.parseInt(params['tourId']);
      this.tourLogId = Number.parseInt(params['tourLogId']);
    });

    this.tourLogData = this.tourLogService
      .getTourLogsByTourId(this.tourId)
      .find((tourLog) => tourLog.id === this.tourLogId)!;

    this.tourLogForm = new FormGroup({
      date: new FormControl(this.tourLogData.date.toISOString().split('T')[0], [
        Validators.required,
      ]),
      comment: new FormControl(this.tourLogData.comment, [
        Validators.required,
        Validators.maxLength(500),
      ]),
      difficulty: new FormControl(this.tourLogData.difficulty, [
        Validators.required,
        Validators.min(1),
        Validators.max(10),
      ]),
      distance: new FormControl(this.tourLogData.distance, [
        Validators.required,
        Validators.min(0),
      ]),
      duration: new FormControl(this.tourLogData.duration, [
        Validators.required,
        Validators.min(0),
      ]),
      rating: new FormControl(this.tourLogData.rating, [
        Validators.required,
        Validators.min(0),
        Validators.max(5),
      ]),
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

    const updatedTourLog: ITourLog = {
      id: this.tourLogId,
      tourId: this.tourId,
      date: new Date(this.tourLogForm.value.date),
      comment: this.tourLogForm.value.comment,
      difficulty: this.tourLogForm.value.difficulty,
      distance: this.tourLogForm.value.distance,
      duration: this.tourLogForm.value.duration,
      rating: this.tourLogForm.value.rating,
    };

    this.tourLogService.updateTourLog(updatedTourLog);
    this.tourLogForm.markAsPristine();
    this.tourLogForm.markAsUntouched();
  }
  deleteTourLog(): void {
    this.tourLogService.deleteTourLog(this.tourLogId);
    this.router.navigate(['/tours', this.tourId]);
  }
}
