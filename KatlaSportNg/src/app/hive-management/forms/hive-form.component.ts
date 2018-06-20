import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HiveService } from '../services/hive.service';
import { Hive } from '../models/hive';
import { UpdateRequestHive } from "../models/update-request-hive";

@Component({
  selector: 'app-hive-form',
  templateUrl: './hive-form.component.html',
  styleUrls: ['./hive-form.component.css']
})
export class HiveFormComponent implements OnInit {

  hive = new Hive(0, "", "", "", false, "");
  existed = false;
  updateRequestHive = new UpdateRequestHive("","","");

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private hiveService: HiveService
  ) { }

  ngOnInit() {
    this.route.params.subscribe(p => {
      if (p['id'] === undefined) return;
      this.hiveService.getHive(p['id']).subscribe(h => this.hive = h);
      this.existed = true;
    });
  }

  navigateToHives() {
    this.router.navigate(['/hives']);
  }

  onCancel() {
    this.navigateToHives();
  }

  onSubmit() {
    if(this.hive.id == 0){
      this.hiveService.addHive(this.hive).subscribe(x=>this.navigateToHives());
    }
    else{
      this.updateRequestHive.address = this.hive.address;
      this.updateRequestHive.code = this.hive.code;
      this.updateRequestHive.name = this.hive.name;
      this.hiveService.updateHive(this.updateRequestHive,this.hive.id).subscribe(x=>this.navigateToHives());
    }
  }

  onDelete() {
    this.hiveService.setHiveStatus(this.hive.id,true).subscribe(x=>this.hive.isDeleted = true);
  }

  onUndelete() {
    this.hiveService.setHiveStatus(this.hive.id,false).subscribe(x=>this.hive.isDeleted = false);
  }

  onPurge() {
    this.hiveService.deleteHive(this.hive.id).subscribe(x=>this.navigateToHives());
  }
}
