import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import {
  FormGroup,
  FormControl,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UserService } from '../../services/user';

@Component({
  selector: 'app-login',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private router = inject(Router);
  private userService = inject(UserService);

  loginForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  }); 

  get username() {
    return this.loginForm.get('username');
  }

  get password() {
    return this.loginForm.get('password');
  }

  onSubmit() {
    console.warn(this.loginForm.value);

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.userService
      .loginUserServer({ username: this.loginForm.value.username!, password: this.loginForm.value.password! })
      .subscribe({
        next: () => {
          console.log('Login successful');

          this.userService.setUser = { username: this.loginForm.value.username! };
          this.loginForm.reset();
          this.router.navigate(['/tours']);
        },
        error: (error) => {
          console.error('Login failed:', error);
        }
      });
  }
}
