import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { ApiUrlUtil } from "../util/api-url-util";
import { ArrayResponse } from "../model/array-response";
import { Patient } from "../model/patient";


@Injectable({providedIn: 'root'})
export class PatientService
{
  public getAll(page: number,
      callback?: (data: ArrayResponse<Patient[]>) => void,
      errorCallBack?: (error: HttpErrorResponse) => void): void
  {
    const skip = (page - 1) * 10;
    const baseUrl = `api/patient/${skip}/30`;
    const url = ApiUrlUtil.getUrl(baseUrl);
    const request = this.http.get<ArrayResponse<Patient[]>>(url);
    request.subscribe({next: callback, error: errorCallBack});
  }

  public getByName(name: string, page: number,
      callback?: (data: ArrayResponse<Patient[]>) => void,
      errorCallBack?: (error: HttpErrorResponse) => void): void
  {
    const skip = (page - 1) * 10;
    const baseUrl = `api/patient/by-name/${(name ?? "")}/${skip}/30`;
    const url = ApiUrlUtil.getUrl(baseUrl); 
    const request = this.http.get<ArrayResponse<Patient[]>>(url);
    request.subscribe({next: callback, error: errorCallBack});
  }

  public getById(id: string, callback?: (data: Patient) => void,
      errorCallBack?: (error: HttpErrorResponse) => void): void
  {
    const baseUrl = `api/patient/${(id ?? "")}`;
    const url =  ApiUrlUtil.getUrl(baseUrl);
    const request = this.http.get<Patient>(url);
    request.subscribe({next: callback, error: errorCallBack});
  }

  public add(patient: Patient, callback?: (data: Patient) => void,
      errorCallBack?: (error: HttpErrorResponse) => void): void
  {
    const baseUrl = "api/patient";
    const url = ApiUrlUtil.getUrl(baseUrl);
    const headers = {"content-type": "application/json"};
    const body = JSON.stringify(patient);
    const request = this.http.post<Patient>(url, body, {"headers": headers});
    request.subscribe({next: callback, error: errorCallBack});
  }

  public update(patient: Patient, callback?: (data: Patient) => void,
      errorCallBack?: (error: HttpErrorResponse) => void): void
  {
    const baseUrl = `api/patient/${patient.id ?? ""}`;
    const url = ApiUrlUtil.getUrl(baseUrl);
    const headers = {"content-type": "application/json"};
    const body = JSON.stringify(patient);
    const request = this.http.put<Patient>(url, body, {"headers": headers});
    request.subscribe({next: callback, error: errorCallBack});
  }

  public remove(id: string, callback?: (data: Patient) => void,
      errorCallBack?: (error: HttpErrorResponse) => void): void
  {
    const baseUrl = `api/patient/${(id ?? "")}`;
    const url =  ApiUrlUtil.getUrl(baseUrl);
    const request = this.http.delete<Patient>(url);
    request.subscribe({next: callback, error: errorCallBack});
  }

  public validate(patient: Patient): string
  {
    if(!patient.name || !patient.name.trim())
      return "The field 'Name' can't be empty!";

    if(!patient.birthdate || !patient.birthdate.trim())
      return "The field 'Birthdate' can't be empty!";

    if(!patient.phoneNumber || !patient.phoneNumber.trim())
      return "The field 'Phone Number' can't be empty!";

    if(!patient.email || !patient.email.trim())
      return "The field 'Email' can't be empty!";

    const regex = new RegExp('^[0-9]{4}-[0-9]{2}-[0-9]{2}$');

    if(!regex.test(patient.birthdate))
      return "The field 'Birthdate' must follow the pattern YYYY-MM-DD!";

    const birthdate = new Date(patient.birthdate);

    if(isNaN(birthdate.getTime()))
      return "The field 'birthdate' has an invalid date!";

    if(birthdate > new Date())
      return "The field 'birthdate' can't be a date in the future!";

    const regex2 = new RegExp("^[0-9]{3}-[0-9]{3}-[0-9]{4}$");

    if(!regex2.test(patient.phoneNumber))
      return "The field 'Phone Number' has to match the format '123-456-7890'!";

    const regex3 = new RegExp("^([\\w\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$");

    if(!regex3.test(patient.email))
      return "The field 'email' has to match the format 'useremail@service.com'!";

    return "";
  }


  constructor(private http: HttpClient) {}
}