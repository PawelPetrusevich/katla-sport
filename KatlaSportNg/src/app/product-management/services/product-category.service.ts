import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { ProductCategory } from '../models/product-category';
import { ProductCategoryListItem } from '../models/product-category-list-item';
import { ProductCategoryUpdateResponse } from '../models/product-category-update-response';

@Injectable({
  providedIn: 'root'
})
export class ProductCategoryService {
  private url = environment.apiUrl + 'api/categories/';

  constructor(private http: HttpClient) { }

  getProductCategories(): Observable<Array<ProductCategoryListItem>> {
    return this.http.get<Array<ProductCategoryListItem>>(this.url);
  }

  getProductCategory(productCategoryId: number): Observable<ProductCategory> {
    return this.http.get<ProductCategory>(`${this.url}${productCategoryId}`);
  }

  addProductCategory(productCategory: ProductCategory): Observable<ProductCategory> {
    return this.http.post<ProductCategory>(`${this.url}addProductCategory`, productCategory);
  }

  updateProductCategory(productCategory: ProductCategory): Observable<ProductCategoryUpdateResponse> {
    return this.http.put<ProductCategoryUpdateResponse>(`${this.url}update/${productCategory.id}`, productCategory);
  }

  deleteProductCategory(productCategoryId: number): Observable<Object> {
    return this.http.delete<Object>(`${this.url}delete/${productCategoryId}`);
  }

  setProductCategoryStatus(productCategoryId: number, deletedStatus: boolean): Observable<Object> {
    return this.http.put<Object>(`${this.url}${productCategoryId}/status/${deletedStatus}`, null);
  }
}
