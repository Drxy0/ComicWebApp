import { Component, DestroyRef, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { LoginUserDto } from '../../models/dtos/login-user.dto';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private destroyRef = inject(DestroyRef);
  private authService = inject(AuthService);
  
  form = new FormGroup({
    email: new FormControl('', {
      validators: [Validators.email, Validators.required]
    }),
    password: new FormControl('', {
      validators: [
        Validators.required
      ]
    })
  })

  onLogin() {
    const payload: LoginUserDto = this.form.value as LoginUserDto;
    this.authService.login(payload).subscribe(
      response => {
        console.log('response', response.body);
      }
    )
  }
}
