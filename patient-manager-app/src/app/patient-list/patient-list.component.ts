import { ChangeDetectorRef, Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterLink } from '@angular/router';
import { PatientListItemComponent } from '../patient-list-item/patient-list-item.component';
import { PatientService } from '../../service/patient-service';
import { Patient } from '../../model/patient';
import { HttpErrorResponse } from '@angular/common/http';


@Component({
  selector: 'app-patient-list',
  imports: [RouterLink, PatientListItemComponent],
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.css'
})


export class PatientListComponent
{
  private fetchData(): void
  {
    const search = (this.route.snapshot.paramMap.get("search") ?? "").trim();
    const type = window.location.pathname.split("/")[3];
    this.page = this.obtainPage(this.route.snapshot.paramMap.get("page"));
    this.searchType = this.obtainSearchType(type);
    this.searchData = search;
    this.isLoading = true;
    this.patientList = [];

    if(search)
    {
      if(type === "name")
      {
        this.service.getByName(search, this.page, 
            data => this.setPatientList(data.list),
            error => this.handleError(error));
      }
      else if(type === "id")
      {
        this.service.getById(search,
            data => this.setPatientList([data]),
            error => this.handleError(error));
      }
    }
    else
    {
      this.service.getAll(this.page,
          data => this.setPatientList(data.list),
          error => this.handleError(error));
    }
  }

  public onPageButtonClick(event: Event): void
  {
    if(event.target instanceof HTMLElement)
    {
      this.updateSearchUrl();
      const e = event.target;
      const page = e.getAttribute("name");
      const url = `${this.searchUrl}/${page}`;
      this.router.navigate([url]);
    }
  }

  public onSearchButtonClick(event: Event): void
  {
    this.updateSearchUrl();
    this.router.navigate([this.searchUrl]);
  }

  public onInput(event: Event): void
  {
    const e = event.target as HTMLInputElement;
    (this as any)[e.name] = e.value;
  }

  public onChange(event: Event): void
  {
    const e = event.target as HTMLSelectElement;
    (this as any)[e.name] = e.value;
  }

  public refresh(patient: Patient): void
  {
    this.fetchData();
  }

  public isSearchTypeSelected(searchType: string): boolean
  {
    return this.searchType === searchType;
  }

  public isCurrentPage(pageName: string)
  {
    return pageName === this.page.toString();
  }

  private setPatientList(patientList: Patient[]): void
  {
    this.updatePageNameList(patientList.length);
    this.patientList = patientList.slice(0, 10);
    this.isLoading = false;
    this.changeDetectorRef.detectChanges();
  }

  private updateSearchUrl(): void
  {
    const sd = this.searchData?.trim();
    const st = this.searchType?.toLowerCase()?.replace("by ", "").replace("all", "");
    const appendUrl = (sd && st ? `/${st}/${sd}` : "");
    this.searchUrl = `/patient/show${appendUrl}`;
    this.changeDetectorRef.detectChanges();
  }

  private updatePageNameList(itemAmount: number): void
  {
    const nextPageAmount = Math.floor((itemAmount - 1) / 10);
    const previousPageAmount = (this.page - 3);
    const minPage = previousPageAmount > 0 ? previousPageAmount : 0;
    const maxPage = this.page + nextPageAmount;
    this.pageNameList = Array.from({length: maxPage - minPage},
        (_, index) => (minPage + index + 1).toString());
  }

  private obtainSearchType(type: string): string
  {
    if(type === "name")
      return "By Name";

    if(type === "id")
      return "By Id";

    return "All";
  }

  private obtainPage(pageString: string | null): number
  {
    let p = parseInt(pageString ?? "1");
    p = isNaN(p) ? 1 : p;
    return p > 0 ? p : 1;
  }

  handleError(error: HttpErrorResponse): void
  {
    this.isLoading = false;
  }

  ngOnInit(): void
  {
    this.fetchData();
  }

  constructor(
    private changeDetectorRef: ChangeDetectorRef,
    private route: ActivatedRoute,
    private router: Router,
    private service: PatientService) {}


  @Input() isLoading: boolean = true;
  @Input() patientList: Patient[] = [];
  @Input() searchData: string = "";
  @Input() searchType: string = "All";
  @Input() searchUrl: string = "/patient/show";
  @Input() searchTypeList: string[] = ["All", "By Id", "By Name"];
  @Input() pageNameList: string[] = [];
  @Input() page: number = 1;
}