import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { connectableObservableDescriptor } from 'rxjs/internal/observable/ConnectableObservable';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = { };

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  register() {
    this.authService.register(this.model).subscribe(() => {
      console.log('registration succesful');
      this.authService.login(this.model).subscribe(next => {
        console.log('Logged in successfully');
        this.cancel();
      }, error => {
        console.log('Failed to login');
      });
    }, error => {
      console.log(error);
    });
  }

  cancel() {
    this.model = { };
    this.cancelRegister.emit(false);
    console.log('cancelled');
  }

}
