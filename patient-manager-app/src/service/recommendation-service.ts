import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { ApiUrlUtil } from "../util/api-url-util";
import { ArrayResponse } from "../model/array-response";
import { Recommendation } from "../model/recommendation";
import { DefaultUtil } from "../util/default-util";


@Injectable({providedIn: 'root'})
export class RecommendationService
{
  public getById(id: string, callback?: (data: Recommendation) => void,
      errorCallBack?: (error: HttpErrorResponse) => void): void
  {
    const baseUrl = `api/recommendation/${(id ?? "")}`;
    const url =  ApiUrlUtil.getUrl(baseUrl);
    const request = this.http.get<Recommendation>(url);
    request.subscribe({next: callback, error: errorCallBack});
  }

  public getByPatientId(patientId: string, page: number,
      callback?: (data: ArrayResponse<Recommendation[]>) => void,
      errorCallBack?: (error: HttpErrorResponse) => void): void
  {
    const skip = (page - 1) * 10;
    const pId = patientId ?? "";
    const baseUrl = `api/recommendation/by-patient-id/${pId}/${skip}/30`;
    const url = ApiUrlUtil.getUrl(baseUrl);
    const request = this.http.get<ArrayResponse<Recommendation[]>>(url);
    request.subscribe({next: callback, error: errorCallBack});
  }

  public getByPatientIdAndCompleted(patientId: string, completed: boolean,
      page: number, callback?: (data: ArrayResponse<Recommendation[]>) => void,
      errorCallBack?: (error: HttpErrorResponse) => void): void
  {
    const skip = (page - 1) * 10;
    const pId = patientId ??  DefaultUtil.getEmptyUUID();
    const noParamUrl = "api/recommendation/by-patient-id-and-completed";
    const baseUrl = `${noParamUrl}/${pId}/${completed}/${skip}/30`;
    const url = ApiUrlUtil.getUrl(baseUrl);
    const request = this.http.get<ArrayResponse<Recommendation[]>>(url);
    request.subscribe({next: callback, error: errorCallBack});
  }

  public updateCompleted(recommendation: Recommendation,
      callback?: (data: Recommendation) => void,
      errorCallBack?: (error: HttpErrorResponse) => void): void
  {
    const baseUrl = `api/recommendation/completed/${(recommendation.id ?? "")}`;
    const url = ApiUrlUtil.getUrl(baseUrl);
    const headers = {"content-type": "application/json"};
    const body = JSON.stringify(recommendation);
    const request = this.http.put<Recommendation>(url, body, {"headers": headers});
    request.subscribe({next: callback, error: errorCallBack});
  }

  public add(recommendation: Recommendation,
      callback?: (data: Recommendation) => void,
      errorCallBack?: (error: HttpErrorResponse) => void): void
  {
    const baseUrl = "api/recommendation";
    const url = ApiUrlUtil.getUrl(baseUrl);
    const headers = {"content-type": "application/json"};
    const body = JSON.stringify(recommendation);
    const request = this.http.post<Recommendation>(url, body, {"headers": headers});
    request.subscribe({next: callback, error: errorCallBack});
  }

  public update(recommendation: Recommendation,
      callback?: (data: Recommendation) => void,
      errorCallBack?: (error: HttpErrorResponse) => void): void
  {
    const baseUrl = `api/recommendation/${recommendation.id ?? ""}`;
    const url = ApiUrlUtil.getUrl(baseUrl);
    const headers = {"content-type": "application/json"};
    const body = JSON.stringify(recommendation);
    const request = this.http.put<Recommendation>(url, body, {"headers": headers});
    request.subscribe({next: callback, error: errorCallBack});
  }

  public remove(id: string, callback?: (data: Recommendation) => void,
      errorCallBack?: (error: HttpErrorResponse) => void): void
  {
    const baseUrl = `api/recommendation/${(id ?? "")}`;
    const url =  ApiUrlUtil.getUrl(baseUrl);
    const request = this.http.delete<Recommendation>(url);
    request.subscribe({next: callback, error: errorCallBack});
  }

  public validate(recommendation: Recommendation): string
  {
    if(!recommendation.description || !recommendation.description.trim())
      return "The field 'Description' can't be empty!";

    return "";
  }

  constructor(private http: HttpClient) {}
}