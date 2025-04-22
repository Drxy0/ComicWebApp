import { Component } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
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
        validators: [this.equalValues('password', 'confirmPassword')]
      }
    ),
  });

  onRegister() {}

  equalValues(controlName1: string, controlName2: string) {
    return (control: AbstractControl) => {
      const val1 = control.get(controlName1)?.value;
      const val2 = control.get(controlName2)?.value;

      if (val1 === val2) {
        return null;
      }
      return { valuesNotEqual: true};
    }
  }
}
