import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  public options = {
    position: ["bottom","right"],
    timeOut : 1000,
    lastOnBottom : true,
    clickToClose : true,
    animate : "scale"
  }
}
