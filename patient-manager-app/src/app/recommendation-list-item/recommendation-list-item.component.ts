import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { RouterLink } from '@angular/router';
import { RecommendationService } from '../../service/recommendation-service';
import { Recommendation } from '../../model/recommendation';
import { DefaultUtil } from '../../util/default-util';


@Component({
  selector: 'app-recommendation-list-item',
  imports: [RouterLink],
  templateUrl: './recommendation-list-item.component.html',
  styleUrl: './recommendation-list-item.component.css'
})

export class RecommendationListItemComponent
{
  public onCompletedButtonClick(): void
  {
    const aux = this.recommendation?.completed ? "incompleted" : "completed";
    const q = `Are you sure you want to mark this recommendation as ${aux}?`;

    if(this.recommendation && confirm(q))
    {
      this.recommendation.completed = !this.recommendation.completed;
      this.service.updateCompleted(this.recommendation,
          data => this.onAction.emit(data),
          error => alert("An error happened trying to update the recommendation"));
    }
  }

  public onRemoveButtonClick(): void
  {
    const q = "Are you sure you want to delete this recommendation?";

    if(this.recommendation && confirm(q))
    {
      const id = this.recommendation.id ??  DefaultUtil.getEmptyUUID();
      this.service.remove(id,
          data => this.onAction.emit(data),
          error => alert("An error happened trying to remove the recommendation!"));
    }
  }

  private initAttributes(): void
  {
    if(this.recommendation)
    {
      const id = this.recommendation.id;
      const pId = this.recommendation.patientId;
      const aux = (this.recommendation.completed) ? "Incompleted" : "Completed"
      this.completedSpan = (this.recommendation.completed) ? "Completed" : "Incompleted";
      this.completedButtonLabel = `Mark as ${aux}`;
      this.editUrl = `/patient/recommendation/edit/${pId}/${id}`;
    }
  }

  ngOnInit(): void
  {
    this.initAttributes();
  }

  constructor(
    private router: Router,
    private service: RecommendationService) {}


  @Input() recommendation?: Recommendation;
  @Input() editUrl: string = "/patient/recommendation/edit";
  @Input() completedButtonLabel: string = "";
  @Input() completedSpan: string = "";
  @Output() onAction: EventEmitter<Recommendation> = new EventEmitter();
}
