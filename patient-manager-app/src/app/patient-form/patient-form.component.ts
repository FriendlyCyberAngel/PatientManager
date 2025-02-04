import { Component, Input } from '@angular/core';
import { Patient } from '../../model/patient';
import { ActivatedRoute, Router } from '@angular/router';
import { PatientService } from '../../service/patient-service';

@Component({
  selector: 'app-patient-form',
  imports: [],
  templateUrl: './patient-form.component.html',
  styleUrl: './patient-form.component.css'
})


export class PatientFormComponent
{
  public fetchData(): void
  {
    const id = (this.route.snapshot.paramMap.get("id") ?? "").trim();
    const type = window.location.pathname.split("/")[2];
    this.editing = type === "edit";

    if(this.editing)
    {
      this.formTitle = "Edit Patient";
      this.service.getById(id,
          data => this.patient = data,
          error => this.inactiveMessage =
              "An error happened trying to get the patient data! Please restart the form.");
    }
  }

  public onConfirmButtonClick(event: Event): void
  {
    event.preventDefault();

    if(this.inactiveMessage)
      return;

    const action = this.editing ? "edit the" : "add the new";
    const question = `Are you sure you want to ${action} patient?`;
    this.errorMessage = this.service.validate(this.patient);
    setTimeout(() => this.errorMessage = "", 4000);

    if(!this.errorMessage && confirm(question))
    {
      if(this.editing)
      {
        this.service.update(this.patient,
            data => this.router.navigate([`/patient/show/id/${data.id}`]),
            error => alert("An error happened trying to change the patient data!"));
      }
      else
      {
        this.service.add(this.patient, data =>
            this.router.navigate([`/patient/show/id/${data.id}`]),
            error => alert("An error happened trying to add the new patient!"));
      }
    }
  }

  public onCancelButtonClick(event: Event): void
  {
    event.preventDefault();
    const action = this.editing ? "patient changes" : "new patient";
    const url = `/patient/show${(this.editing ? `/id/${this.patient.id}` : "")}`;
    const question = `Are you sure you want to discard the ${action}?`;

    if(this.inactiveMessage || confirm(question))
      this.router.navigate([url]);
  }

  public onInput(event: Event): void
  {
    const e = event.target as HTMLInputElement;
    (this.patient as any)[e.name] = e.value;
  }

  ngOnInit(): void
  {
    this.fetchData();
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private service: PatientService) {}


  @Input() patient: Patient = new Patient("", "", "", "", "", new Date());
  @Input() formTitle: string = "New Patient";
  @Input() editing: boolean = false;
  @Input() errorMessage: string = "";
  @Input() inactiveMessage: string = "";
}
