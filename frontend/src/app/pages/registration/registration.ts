import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormGroup, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../services/user';

@Component({
  selector: 'app-registration',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './registration.html',
  styleUrl: './registration.css',
})
export class Registration {
  private router = inject(Router);
  private userService = inject(UserService);

  registrationForm = new FormGroup({
    username: new FormControl('', [Validators.required, Validators.maxLength(50), Validators.minLength(3)]),
    password: new FormControl('', [Validators.required, Validators.maxLength(30), Validators.minLength(8)]
    )
  });

  get username() {
    return this.registrationForm.get('username');
  }

  get password() {
    return this.registrationForm.get('password');
  }

  onSubmit() {
    console.warn(this.registrationForm.value);

    if (this.registrationForm.invalid) {
      this.registrationForm.markAllAsTouched();   
      return;
    }

    this.userService
      .registerUserServer({ username: this.registrationForm.value.username!, password: this.registrationForm.value.password! })
      .subscribe({
        next: () => {
          console.log('Registration successful');

          this.registrationForm.reset();
          this.router.navigate(['/login']);
        },
        error: (error) => {
          console.error('Registration failed:', error);
        }
      });
  }
}
