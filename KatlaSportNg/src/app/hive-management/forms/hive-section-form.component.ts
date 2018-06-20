import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HiveSection } from '../models/hive-section';
import { UpdateRequestHiveSection } from '../models/update-request-hive-section';
import { HiveSectionService } from '../services/hive-section.service';

@Component({
  selector: 'app-hive-section-form',
  templateUrl: './hive-section-form.component.html',
  styleUrls: ['./hive-section-form.component.css']
})
export class HiveSectionFormComponent implements OnInit {

  hiveSection = new HiveSection(0,"","",false,"");
  hiveId = 0;
  existed = false;
  updateHiveSectionRequest = new UpdateRequestHiveSection("","");

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: HiveSectionService
  ) { }

  navigateToHiveSection(){
    this.router.navigate([`/hive/${this.hiveId}/sections`]);
  }

  ngOnInit() {
    this.route.params.subscribe(p=>{
      this.hiveId = p['hiveId'];
      if(p['id'] === undefined) return;
      this.existed = true;
      this.service.getHiveSection(p['id']).subscribe(h=>this.hiveSection = h);
    })
  }

  onDelete(){
    this.service.setHiveSectionStatus(this.hiveSection.id,true).subscribe(x=> this.hiveSection.isDeleted = true);
  }

  onUnDelete(){
    this.service.setHiveSectionStatus(this.hiveSection.id,false).subscribe(x=> this.hiveSection.isDeleted = false);
  }

  onPurge(){
    this.service.deleteHiveSection(this.hiveSection.id).subscribe(x=>this.navigateToHiveSection());
  }

  onSubmit() {
    if(this.hiveSection.id == 0){
      this.service.createHiveSection(this.hiveSection,this.hiveId).subscribe(x=>this.navigateToHiveSection());
    }
    else{
      this.updateHiveSectionRequest.code = this.hiveSection.code;
      this.updateHiveSectionRequest.name = this.hiveSection.name;
      this.service.updateHiveSection(this.updateHiveSectionRequest,this.hiveSection.id).subscribe(x=>this.navigateToHiveSection());
    }
  }
}
