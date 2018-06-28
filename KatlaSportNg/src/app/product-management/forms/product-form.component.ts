import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from '../models/product';
import { ProductCategoryListItem } from '../models/product-category-list-item';
import { ProductCategoryService } from '../services/product-category.service';
import { ProductService } from '../services/product.service';
import { NotificationsService } from 'angular2-notifications';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.css']
})
export class ProductFormComponent implements OnInit {

  product = new Product(0, "", "", 0, "", "", 0, false, "");
  existed = false;
  categoryId: number;
  productCategories: ProductCategoryListItem[];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService,
    private productCategoryService: ProductCategoryService,
    private notificationService: NotificationsService,
  ) { }

  ngOnInit() {
    this.productCategoryService.getProductCategories().subscribe(c => this.productCategories = c);
    this.route.params.subscribe(p => {
      this.categoryId = p['categoryId'];
      if (p['id'] === undefined) return;
      this.productService.getProduct(p['id']).subscribe(c => this.product = c);
      this.existed = true;
    });
  }

  navigateTo() {
    if (this.categoryId === undefined) {
      this.router.navigate(['/products']);
    } else {
      this.router.navigate([`/category/${this.categoryId}/products`]);
    }
  }

  onCancel() {
    this.navigateTo();
  }

  onSubmit() {
    if (this.existed) {
      this.productService.updateProduct(this.product).subscribe(p => this.navigateTo());
    } else {
      this.product.сategoryId = this.categoryId;
      this.productService.addProduct(this.product).subscribe(p => {this.navigateTo()},error => this.errorNotification(error as HttpErrorResponse));
    }
  }

  onDelete() {
    this.productService.setStatus(this.product.id,true).subscribe(p => this.product.isDeleted = true);
  }

  onUndelete() {
    this.productService.setStatus(this.product.id,false).subscribe(p => this.product.isDeleted = false);
  }

  onPurge() {
    this.productService.deleteProduct(this.product.id).subscribe(p => this.navigateTo());
  }

  okNotification(){

  }

  errorNotification(error: HttpErrorResponse){
    this.notificationService.error(
      error.status,
      error.error.Message
    )
  }
}
