import { Component, inject } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { RegisterUserDto } from '../../models/dtos/register-user.dto';
import { Router } from '@angular/router';
import { MatTooltipModule } from '@angular/material/tooltip';
import { map, catchError } from 'rxjs';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, MatTooltipModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  tooltipEmailRequired = 'Email is required';
  tooltipEmailNotValid = 'Email is not valid';
  tooltipUsernameRequired = 'Username is required';
  tooltipPasswordRequired = 'Password is required';
  tooltipPasswordMinLength = 'Password must be at least 8 characters long';
  tooltipPasswordDoNotMatch = 'Password do not match';

  form = new FormGroup({
    username: new FormControl('', { validators: Validators.required }),
    email: new FormControl('', {
      validators: [Validators.email, Validators.required],
    }),
    passwords: new FormGroup(
      {
        password: new FormControl('', {
          validators: [Validators.minLength(8), Validators.required],
        }),
        confirmPassword: new FormControl('', {
          validators: [Validators.minLength(8), Validators.required],
        }),
      },
      {
        validators: [this.equalValues('password', 'confirmPassword')],
      }
    ),
  });

  onRegister() {
    const { username, email, passwords } = this.form.value;
    const registerDto: RegisterUserDto = {
      username: username!,
      email: email!,
      password: passwords!.password!,
    };

    this.authService.register(registerDto).subscribe({
      next: () => {
        // Auto-login after successful registration
        const loginDto = {
          email: registerDto.email,
          password: registerDto.password,
        };
        this.authService.login(loginDto)
          .pipe(
            map((response) => response.body),
            catchError((error) => {
              console.error('Auto-login failed', error);
              return [];
            })
          )
          .subscribe(
            (response) => {
              if (!response) return;

              const accessToken = response.accessToken;
              const refreshToken = response.refreshToken;

              if (accessToken && refreshToken) {
                localStorage.setItem('access_token', accessToken);
                localStorage.setItem('refresh_token', refreshToken);
              }

              this.router.navigate(['/profile']);
            },
            (loginErr) => {
              console.error('Auto-login failed', loginErr);
            }
          );
      },
      error: (regErr) => {
        console.error('Registration failed', regErr);
      },
    });
  }

  equalValues(controlName1: string, controlName2: string) {
    return (control: AbstractControl) => {
      const val1 = control.get(controlName1)?.value;
      const val2 = control.get(controlName2)?.value;

      if (val1 === val2) {
        return null;
      }
      return { valuesNotEqual: true };
    };
  }
}
