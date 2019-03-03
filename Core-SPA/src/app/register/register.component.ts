import { Component, OnInit, Input, Output, EventEmitter, AfterViewInit, ElementRef, ViewChildren } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormControlName, FormBuilder } from '@angular/forms';
import { Observable, fromEvent, merge } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { GenericValidator } from '../_validators/generic.validator';
import { UserValidators } from '../_validators/user.validator';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];
  @Input() validatonMessages: { [key: string]: { [key: string]: string } };
  @Output() cancelRegister = new EventEmitter();
  user: User;
  registerForm: FormGroup;
  displayMessage: { [key: string]: string } = {};
  genericValidator: GenericValidator;
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(private authService: AuthService, private alertify: AlertifyService,
              private formBuilder: FormBuilder, private router: Router) { }

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red',
      dateInputFormat: 'DD/MM/YYYY'
    },
    this.genericValidator = new GenericValidator(this.validatonMessages);
    this.createForm();
  }

  ngAfterViewInit(): void {
    // Watch for the blur event from any input element on the form.
    const controlBlurs: Observable<any>[] = this.formInputElements
    .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

    // Watch for any statusChanges
    merge(this.registerForm.statusChanges, ...controlBlurs).pipe(debounceTime(800)).subscribe(value => {
      this.displayMessage = this.genericValidator.processMessages(this.registerForm);
    });
  }

  register() {
    if (this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value);
      this.user['password'] = 'password';

      console.log(this.user);

      this.authService.register(this.user).subscribe(() => {
        this.alertify.success('registration succesful');
      }, error => {
        this.alertify.error(error);
      }, () => {
        this.authService.login(this.user).subscribe(() => {
          this.router.navigate(['/members']);
        }, error => {
          this.alertify.error(error);
        });
      });
    }
  }

  cancel() {
    this.user = null;
    this.cancelRegister.emit(false);
  }

  createForm() {
    const passwordGroup = this.formBuilder.group({
      password: ['', [Validators.required, Validators.pattern(/^((?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,20})/)]],
      confirmPassword: ['', Validators.required]
    }, { validator: UserValidators.matchPasswords()});

    this.registerForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      passwords: passwordGroup,
      gender: ['male'],
      knownAs: ['', [Validators.required]],
      dateOfBirth: [null, [Validators.required]],
      city: ['', [Validators.required]],
      country: ['', [Validators.required]]
    });
  }
}
