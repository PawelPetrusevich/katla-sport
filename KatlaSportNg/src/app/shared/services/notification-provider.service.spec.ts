import { TestBed, inject } from '@angular/core/testing';

import { NotificationProviderService } from './notification-provider.service';

describe('NotificationProviderService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [NotificationProviderService]
    });
  });

  it('should be created', inject([NotificationProviderService], (service: NotificationProviderService) => {
    expect(service).toBeTruthy();
  }));
});
