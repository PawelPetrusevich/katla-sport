import { Injectable } from '@angular/core';
import { NotificationsService } from 'angular2-notifications';
import { HttpResponse, HttpErrorResponse, HttpResponseBase } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class NotificationProviderService {

  constructor(
    private notificationService: NotificationsService
  ) { }

  okNatification(message: string){
    this.notificationService.success("success",message);
  }

  errorNatification(error: HttpErrorResponse){
    this.notificationService.error(error.statusText,error.error.message);

  }
}
