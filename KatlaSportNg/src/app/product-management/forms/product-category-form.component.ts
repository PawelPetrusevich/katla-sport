import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductCategory } from '../models/product-category';
import { ProductCategoryService } from '../services/product-category.service';
import { NotificationProviderService } from '../../shared/services/notification-provider.service';

@Component({
  selector: 'app-product-category-form',
  templateUrl: './product-category-form.component.html',
  styleUrls: ['./product-category-form.component.css']
})
export class ProductCategoryFormComponent implements OnInit {

  productCategory = new ProductCategory(0, "My New Category", "CATE1", "Category description", false, "");
  existed = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productCategoryService: ProductCategoryService,
    private notificationService: NotificationProviderService
  ) { }

  ngOnInit() {
    this.route.params.subscribe(p => {
      if (p['id'] === undefined) return;
      this.productCategoryService.getProductCategory(p['id']).subscribe(c => this.productCategory = c);
      this.existed = true;
    });
  }

  navigateToCategories() {
    this.router.navigate(['/categories']);
  }

  onCancel() {
    this.navigateToCategories();
  }

  onSubmit() {
    if (this.existed) {
      this.UpdateProductCategory();
    }
    else {
      this.CreateProductCategory();
    }
  }

  onDelete() {
    this.productCategoryService.setProductCategoryStatus(this.productCategory.id, true).subscribe(c => {
      this.productCategory.isDeleted = true
    },
    error => this.notificationService.errorNatification(error)
  );
  }

  onUndelete() {
    this.productCategoryService.setProductCategoryStatus(this.productCategory.id, false).subscribe(c => {
      this.productCategory.isDeleted = false
    },
    error => this.notificationService.errorNatification(error)
  );
  }

  onPurge() {
    this.productCategoryService.deleteProductCategory(this.productCategory.id).subscribe(c => {
      this.navigateToCategories()
    },
    error => this.notificationService.errorNatification(error)
  );
  }

  private UpdateProductCategory() {
    this.productCategoryService.updateProductCategory(this.productCategory).subscribe(c => {
      this.navigateToCategories();
      this.notificationService.okNatification(`${c.name} update.`)
    },
    error => this.notificationService.errorNatification(error)
  );
  }

  private CreateProductCategory() {
    this.productCategoryService.addProductCategory(this.productCategory).subscribe(c => {
      this.navigateToCategories();
      this.notificationService.okNatification(`${c.name} created.`)
    },
      error => this.notificationService.errorNatification(error)
    );
  }
}
