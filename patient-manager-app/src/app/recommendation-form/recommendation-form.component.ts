import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RecommendationService } from '../../service/recommendation-service';
import { Recommendation } from '../../model/recommendation';

@Component({
  selector: 'app-recommendation-form',
  imports: [],
  templateUrl: './recommendation-form.component.html',
  styleUrl: './recommendation-form.component.css'
})


export class RecommendationFormComponent
{
  public fetchData(): void
  {
    const paramMap = this.route.snapshot.paramMap;
    const patientId = (paramMap.get("patientId") ?? "").trim();
    const id = (paramMap.get("id") ?? "").trim();
    const type = window.location.pathname.split("/")[3];
    this.editing = type === "edit";

    if(this.editing)
    {
      this.formTitle = "Edit Recommendation";
      this.service.getById(id,
          data => this.recommendation = data,
          error => this.inactiveMessage =
              "An error happened trying to get the patient data! Please restart the form.");
    }
    else
      this.recommendation.patientId = patientId;
  }

  public onConfirmButtonClick(event: Event): void
  {
    event.preventDefault();

    if(this.inactiveMessage)
      return;

    const action = this.editing ? "edit the" : "add the new";
    const baseUrl = "/patient/recommendation/show";
    const question = `Are you sure you want to ${action} recommendation?`;
    this.errorMessage = this.service.validate(this.recommendation);
    setTimeout(() => this.errorMessage = "", 4000);

    if(!this.errorMessage && confirm(question))
    {
      if(this.editing)
      {
        this.service.update(this.recommendation,
            data => this.router.navigate([`${baseUrl}/${data.patientId}`]),
            error => alert("An error happened trying to change the recommendation data!"));
      }
      else
      {
        this.service.add(this.recommendation,
            data => this.router.navigate([`${baseUrl}/${data.patientId}`]),
            error => alert("An error happened trying to add the new recommendation!"));
      }
    }
  }

  public onCancelButtonClick(event: Event): void
  {
    event.preventDefault();
    const action = this.editing ? "recommendation changes" : "new recommendation";
    const paramMap = this.route.snapshot.paramMap;
    let pId = this.recommendation.patientId;
    pId = pId ? pId : (paramMap.get("patientId") ?? "").trim();
    const url = `/patient/recommendation/show/${pId}`;
    const question = `Are you sure you want to discard the ${action}?`;

    if(this.inactiveMessage || confirm(question))
      this.router.navigate([url]);
  }

  public onInput(event: Event): void
  {
    const e = event.target as HTMLInputElement;
    (this.recommendation as any)[e.name] = e.value;
  }

  public onCheckboxInput(event: Event): void
  {
    const e = event.target as HTMLInputElement;
    (this.recommendation as any)[e.name] = e.checked;
  }

  ngOnInit(): void
  {
    this.fetchData();
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private service: RecommendationService) {}


  @Input() recommendation: Recommendation = new Recommendation("", "", "", false, new Date());
  @Input() formTitle: string = "New Recommendation";
  @Input() editing: boolean = false;
  @Input() errorMessage: string = "";
  @Input() inactiveMessage: string = "";
}
