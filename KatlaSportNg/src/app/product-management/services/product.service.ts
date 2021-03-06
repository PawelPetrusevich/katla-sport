import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { Product } from '../models/product';
import { ProductCategoryProductListItem } from '../models/product-category-product-list-item';
import { ProductListItem } from '../models/product-list-item';
import { ProductUpdateResponseDTO } from '../models/product-update-response-dto';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private url = environment.apiUrl + 'api/products/';
  private categoryUrl = environment.apiUrl + 'api/categories/';

  constructor(private http: HttpClient) { }

  getProducts(): Observable<Array<ProductListItem>> {
    return this.http.get<Array<ProductListItem>>(this.url);
  }

  getProduct(productId: number): Observable<Product> {
    return this.http.get<Product>(`${this.url}${productId}`);
  }

  getCategoryProducts(productCategoryId: number): Observable<Array<ProductCategoryProductListItem>> {
    return this.http.get<Array<ProductCategoryProductListItem>>(`${this.categoryUrl}${productCategoryId}/products`);
  }

  addProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(`${this.url}addProduct`, product);
  }

  updateProduct(product: Product): Observable<ProductUpdateResponseDTO> {
    return this.http.put<ProductUpdateResponseDTO>(`${this.url}update/${product.id}`, product);
  }

  deleteProduct(productId: number): Observable<Object> {
    return this.http.delete<Object>(`${this.url}delete/${productId}`);
  }

  setStatus(productId: number,status: boolean): Observable<Object> {
    return this.http.put<Object>(`${this.url}${productId}/status/${status}`,null);
  }
}
