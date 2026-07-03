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
import { UserService } from '../../services/user';

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

private userService = inject(UserService);

  private tourId: number = -1;
  private tourLogId: number = -1;

  tourLogData!: ITourLog;
  tourLogForm!: FormGroup;

  isLoading = true;
  loadError = false;

  ngOnInit() {
    if (!this.userService.user()) {
      this.router.navigate(['/login']);
    }

    this.activatedRoute.params.subscribe((params) => {
      this.tourId = Number.parseInt(params['tourId']);
      this.tourLogId = Number.parseInt(params['tourLogId']);
    });
    this.loadTourLog();
  }

  private loadTourLog(): void {
    this.isLoading = true;
    this.loadError = false;
    this.tourLogService.getTourLogsByTourIdServer(this.tourId).subscribe({
      next: (tourLogs) => {
        const tourLog = tourLogs.find((log) => log.id === this.tourLogId);
        if (!tourLog) {
          this.loadError = true;
          this.isLoading = false;
          return;
        }
        this.tourLogData = tourLog;
        this.initializeForm();
        this.isLoading = false;
      },
      error: () => {
        this.loadError = true;
        this.isLoading = false;
      }
    });
  }

  private initializeForm(): void {
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

    this.tourLogService.updateTourLogServer(this.tourId, updatedTourLog).subscribe({
      next: () => {
        this.tourLogForm.markAsPristine();
        this.tourLogForm.markAsUntouched();
      },
      error: (err) => {
        console.error('Error updating tour log:', err);
      }
    });
  }
  deleteTourLog(): void {
    this.tourLogService.deleteTourLogServer(this.tourId, this.tourLogId).subscribe({
      next: () => {
        this.router.navigate(['/tours', this.tourId]);
      },
      error: (err) => {
        console.error('Error deleting tour log:', err);
      }
    });
  }
}
