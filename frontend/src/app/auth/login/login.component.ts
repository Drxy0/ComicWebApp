import { Component, DestroyRef, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { LoginUserDto } from '../../models/auth/login-user.dto';
import { TranslatePipe } from '@ngx-translate/core';
@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, TranslatePipe],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
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
    const formValues = this.form.getRawValue();
    const payload:LoginUserDto = { email: formValues.email ?? '', password: formValues.password ?? ''}
    this.authService.login(payload).subscribe(
      response => {
        console.log('response', response.body);
      }
    )
  }
}
