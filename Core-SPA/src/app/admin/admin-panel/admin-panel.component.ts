import { Component, OnInit } from '@angular/core';
import { UserRoles } from 'src/app/_models/constants';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
  UserRoles = UserRoles;

  constructor() { }

  ngOnInit() {
  }

}
