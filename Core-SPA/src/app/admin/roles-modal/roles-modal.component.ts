import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { User } from 'src/app/_models/user';
import { UserRoles } from 'src/app/_models/constants';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent implements OnInit {
  @Output() updateSelectedRoles = new EventEmitter();
  user: User;
  roles: any[];
  UserRoles = UserRoles;
  userId: number;

  constructor(public bsModalRef: BsModalRef, private authService: AuthService) {}

  ngOnInit() {
    this.userId = +this.authService.decodedToken.nameid;
  }

  updateRoles() {
    this.updateSelectedRoles.emit(this.roles);
    this.bsModalRef.hide();
  }
}
