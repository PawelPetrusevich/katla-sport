import { Component, OnInit } from '@angular/core';
import { UserLogin } from '../../models/user-login';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { NotificationProviderService } from '../../../shared/services/notification-provider.service';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent implements OnInit {
  user = new UserLogin("","");

  constructor(
    private userService: UserService,
    private router : Router,
    private natification : NotificationProviderService
  ) { }

  ngOnInit() {
  }

  onSubmit(){
    this.userService.userAuthentication(this.user).subscribe((data:any)=>{
      localStorage.setItem('userToken',data.access_token);
      this.router.navigate(['/main']);
    },
    error=> this.natification.errorNatification(error)
  );

  }

}
