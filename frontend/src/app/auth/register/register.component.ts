import { Component, inject } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { TranslatePipe } from "@ngx-translate/core";
import { RegisterUserDto } from '../../models/dtos/register-user.dto';
import { Router } from '@angular/router';
import { MatTooltipModule } from '@angular/material/tooltip';
import { map, catchError } from 'rxjs';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, MatTooltipModule, TranslatePipe],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  form = new FormGroup({
    username: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.email, Validators.required]),
    password: new FormControl('', [Validators.minLength(8), Validators.required]),
    confirmPassword: new FormControl('', [Validators.minLength(8), Validators.required]),
  });

  onRegister() {
    const formValues = this.form.getRawValue();
    if (!formValues) {
      return;
    }

    if (formValues.password != formValues.confirmPassword) {
      this.form.controls.confirmPassword.reset();
      return;
    }

    const registerDto = {
      username: formValues.username,
      email: formValues.email,
      password: formValues.password,
    } as RegisterUserDto;

     this.authService.register(registerDto).subscribe({
      next: () => {
        // Auto-login after successful registration
        const loginDto = {
          email: registerDto.email,
          password: registerDto.password,
        };
        this.authService.login(loginDto).pipe(
          map((response) => response.body),
          catchError((error) => {
            console.error('Auto-login failed', error);
            return [];
          })
        ).subscribe((response) => {
            if (!response) {
              return;
            }

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
