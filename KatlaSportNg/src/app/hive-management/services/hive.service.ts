import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { Hive } from '../models/hive';
import { HiveListItem } from '../models/hive-list-item';
import { HiveSectionListItem } from '../models/hive-section-list-item';
import { UpdateRequestHive } from '../models/update-request-hive';
import { HiveUpdateResponse } from '../models/hive-update-response';

@Injectable({
  providedIn: 'root'
})
export class HiveService {
  private url = environment.apiUrl + 'api/hives/';

  constructor(private http: HttpClient) { }

  getHives(): Observable<Array<HiveListItem>> {
    return this.http.get<Array<HiveListItem>>(this.url);
  }

  getHive(hiveId: number): Observable<Hive> {
    return this.http.get<Hive>(`${this.url}${hiveId}`);
  }

  getHiveSections(hiveId: number): Observable<Array<HiveSectionListItem>> {
    return this.http.get<Array<HiveSectionListItem>>(`${this.url}${hiveId}/sections`);
  }

  addHive(hive: Hive): Observable<Hive> {
    return this.http.post<Hive>(`${this.url}addHive`,hive);
  }

  updateHive(updateRequestHive: UpdateRequestHive,hiveId: number): Observable<HiveUpdateResponse> {
    return this.http.put<HiveUpdateResponse>(`${this.url}/update/${hiveId}`,updateRequestHive);
  }

  deleteHive(hiveId: number): Observable<Object> {
    return this.http.delete(`${this.url}${hiveId}`);
  }

  setHiveStatus(hiveId: number, deletedStatus: boolean): Observable<Object> {
    var body = {hiveId,deletedStatus};
    return this.http.put<Object>(`${this.url}/${hiveId}/status/${deletedStatus}`,body);
  }
}
