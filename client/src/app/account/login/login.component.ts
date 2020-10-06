import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  // 190-3 hold a FormGroup as a property
  loginForm: FormGroup;

  constructor() { }

  ngOnInit(): void {
    // 190-4 initializes formGroup
    this.createLoginForm();
  }

  createLoginForm() {
    this.loginForm = new FormGroup({
      // 190-4 define FormGroup fields
      email: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });
  }

  onSubmit() {
    console.log(this.loginForm.value);
  }
}
