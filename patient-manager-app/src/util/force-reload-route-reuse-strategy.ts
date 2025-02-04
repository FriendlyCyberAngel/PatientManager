import { ActivatedRouteSnapshot, BaseRouteReuseStrategy } from "@angular/router";


export class ForceReloadRouteReuseStrategy extends BaseRouteReuseStrategy
{
  override shouldReuseRoute(future: ActivatedRouteSnapshot,
      curr: ActivatedRouteSnapshot): boolean
  {
    // Returning false to always reload, even if it is the same URL.
    return false;
  }
}