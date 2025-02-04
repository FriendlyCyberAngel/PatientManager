import { ApplicationConfig, ErrorHandler, provideZoneChangeDetection } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { ActivatedRouteSnapshot, BaseRouteReuseStrategy, provideRouter, RouteReuseStrategy } from '@angular/router';

import { routes } from './root.routes';
import { ForceReloadRouteReuseStrategy } from '../../util/force-reload-route-reuse-strategy';
import { GlobalErrorHandlerService } from '../../service/global-error-handler-service';


export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    // {provide: ErrorHandler, useClass: GlobalErrorHandlerService},
    {
      // To allow navigation to the same
      provide: RouteReuseStrategy,
      useClass: ForceReloadRouteReuseStrategy
    }
  ]
};
