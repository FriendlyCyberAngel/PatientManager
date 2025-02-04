import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { PatientService } from '../../service/patient-service';
import { Patient } from '../../model/patient';
import { DefaultUtil } from '../../util/default-util';

@Component({
  selector: 'app-patient-list-item',
  imports: [RouterLink],
  templateUrl: './patient-list-item.component.html',
  styleUrl: './patient-list-item.component.css'
})


export class PatientListItemComponent
{
  public onRemovePatientButtonClick(event: Event): void
  {
    if(this.patient && confirm("Are you sure you want to delete this patient?"))
    {
      const id = this.patient.id ?? DefaultUtil.getEmptyUUID();
      this.service.remove(id,
          data => this.onAction.emit(data),
          error => alert("An error happened trying to remove the patient!"));
    }
  }

  private initUrls(): void
  {
    if(this.patient)
    {
      const id = this.patient.id ??  DefaultUtil.getEmptyUUID();
      this.searchUrl = `/patient/recommendation/show/${id}`;
      this.editUrl = `/patient/edit/${id}`;
    }
    else
    {
      this.searchUrl = "/patient/show";
      this.editUrl = "/patient";
    }
  }

  ngOnInit(): void
  {
    this.initUrls();
  }

  constructor(
    private router: Router,
    private service: PatientService) {}


  @Input() searchUrl: string = "/patient/recommendation/show";
  @Input() editUrl: string = "/patient/edit";
  @Input() patient?: Patient;
  @Output() onAction: EventEmitter<Patient> = new EventEmitter();
}
