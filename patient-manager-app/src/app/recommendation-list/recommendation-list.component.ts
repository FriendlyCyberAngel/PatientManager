import { ChangeDetectorRef, Component, input, Input, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterLink } from '@angular/router';
import { RecommendationListItemComponent } from '../recommendation-list-item/recommendation-list-item.component';
import { RecommendationService } from '../../service/recommendation-service';
import { PatientService } from '../../service/patient-service';
import { Recommendation } from '../../model/recommendation';
import { Patient } from '../../model/patient';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-recommendation-list',
  imports: [RouterLink, RecommendationListItemComponent],
  templateUrl: './recommendation-list.component.html',
  styleUrl: './recommendation-list.component.css'
})


export class RecommendationListComponent
{
  private fetchData(): void
  {
    const paramMap = this.route.snapshot.paramMap;
    const id = (paramMap.get("patientId") ?? "").trim();
    const completed = (paramMap.get("completed") ?? "") === "true";
    const type = window.location.pathname.split("/")[4] ?? "";
    const aux = type === "completed"
    this.searchType = aux ? `${completed ? "C" : "Inc"}ompleted` : "All";
    this.page = this.obtainPage(paramMap.get("page"));
    this.loadingMask = 0;
    this.recommendationList = [];

    if(id)
    {
      this.patientService.getById(id,
          data => this.setPatient(data),
          error => this.handleError(1));

      if(type === "completed")
      {
        this.recommendationService.getByPatientIdAndCompleted(
            id, completed, this.page,
            data => this.setRecommendationList(data.list),
            error => this.handleError(2));
      }
      else
      {
        this.recommendationService.getByPatientId(id, this.page,
            data => this.setRecommendationList(data.list),
            error => this.handleError(2));
      }
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

  public onChange(event: Event): void
  {
    const e = event.target as HTMLSelectElement;
    (this as any)[e.name] = e.value;
  }

  public refresh(recommendation: Recommendation): void
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

  private setPatient(patient: Patient): void
  {
    this.patient = patient;
    this.loadingMask |= 1;
    this.changeDetectorRef.detectChanges();
  }

  private setRecommendationList(recommendationList: Recommendation[]): void
  {
    this.updatePageNameList(recommendationList.length);
    this.recommendationList = recommendationList.splice(0, 10);
    this.loadingMask |= 2;
    this.changeDetectorRef.detectChanges();
  }

  private updateSearchUrl(): void
  {
    const id = (this.route.snapshot.paramMap.get("patientId") ?? "").trim();
    const st = this.searchType?.toLowerCase().replace("all", "") ?? "";
    const completed = st === "completed";
    const appendUrl = st ? `completed/${id}/${completed}` : id;
    this.searchUrl = `/patient/recommendation/show/${appendUrl}`;
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

  private obtainPage(pageString: string | null): number
  {
    let p = parseInt(pageString ?? "1");
    return isNaN(p) ? 1 : p;
  }
  
  private initAttributes(): void
  {
    const id = (this.route.snapshot.paramMap.get("patientId") ?? "").trim();
    this.recommendationList = [];
    this.loadingMask = 0;
    this.addUrl = `/patient/recommendation/new/${id}`;
  }

  handleError(loadingBit: number): void
  {
    this.loadingMask |= loadingBit;
  }

  ngOnInit(): void
  {
    this.initAttributes();
    this.fetchData();
  }

  constructor(
    private changeDetectorRef: ChangeDetectorRef,
    private route: ActivatedRoute,
    private router: Router,
    private patientService: PatientService,
    private recommendationService: RecommendationService) {}

  
  @Input() loadingMask: number = 0;
  @Input() patient?: Patient;
  @Input() recommendationList: Recommendation[] = [];
  @Input() searchType: string = "All";
  @Input() searchUrl: string = "/patient/recommendation";
  @Input() addUrl: string = "/patient/recommendation";
  @Input() searchTypeList: string[] = ["All", "Completed", "Incompleted"];
  @Input() pageNameList: string[] = [];
  @Input() page: number = 1;
}
