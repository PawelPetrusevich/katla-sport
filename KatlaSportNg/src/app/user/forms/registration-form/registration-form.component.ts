import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user.model';
import { NgForm } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { NotificationProviderService } from '../../../shared/services/notification-provider.service';

@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html',
  styleUrls: ['./registration-form.component.css']
})
export class RegistrationFormComponent implements OnInit {

  user = new User("","","");
  confirmPassword = "";


  constructor(
    private userService: UserService,
    private notificationServise: NotificationProviderService
  ) { }

  ngOnInit() {
    this.resetForm();
  }

  resetForm(form? : NgForm){
    if (form != null) {
      form.reset();
      this.user = new User("","","");
    }
  }

  onSubmit(){
    this.userService.registerUser(this.user).subscribe(x=> {
      this.notificationServise.okNatification("User was created.")
    },
    error=> this.notificationServise.errorNatification(error)
  );
  }

}
