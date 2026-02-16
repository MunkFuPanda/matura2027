import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthenticationService } from '../service/authentication.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private authService = inject(AuthenticationService);

  loginForm: FormGroup;
  submitted = false;
  errorMessage = '';

  constructor() {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  get f() {
    return this.loginForm.controls;
  }

  onSubmit(): void {
    this.submitted = true;
    this.errorMessage = '';

    console.log('Login: Form submitted');

    if (this.loginForm.invalid) {
      console.log('Login: Form is invalid');
      return;
    }

    const username = this.loginForm.value.username;
    const password = this.loginForm.value.password;

    console.log('Login: Attempting to login user', username);

    this.authService.login(username, password).subscribe({
      next: (response) => {
        console.log('Login: Login successful', response);
        this.router.navigate(['/employee']);
      },
      error: (error) => {
        console.error('Login: Login failed', error);
        this.errorMessage = 'Benutzername oder Passwort falsch.';
      }
    });
  }
}
