import { Router, ActivatedRoute } from '@angular/router';
import { AccountService } from './../account.service';
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
  // 203-3
  returnUrl: string;

  constructor(
    private accountService: AccountService,
    // 203-4 Inject ActivatedRoute
    private activatedRoute: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    // 203-5 initialize returnUrl w/ activated route
    this.returnUrl = this.activatedRoute.snapshot.queryParams.returnUrl || '/shop';

    // 190-4 initializes formGroup
    this.createLoginForm();
  }

  createLoginForm() {
    this.loginForm = new FormGroup({
      // 190-4 define FormGroup fields
      email: new FormControl('', [ Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$') ]),
      password: new FormControl('', Validators.required)
    });
  }

  onSubmit() {
    this.accountService.login(this.loginForm.value).subscribe(() => {
      // 203-6 set to the url to be routed after submit.
      this.router.navigateByUrl(this.returnUrl);
    }, error => {
      console.log(error);
    });
  }
}
