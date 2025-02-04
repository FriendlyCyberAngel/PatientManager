import { isDevMode } from "@angular/core";


export class ApiUrlUtil
{
  public static getUrl(path: string): string
  {
    if(isDevMode())
      return `http://localhost:5156/${path}`;
    else
      return `http://localhost:5156/${path}`;   // Change this to production URL
  }
}