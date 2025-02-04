import { Routes } from "@angular/router";
import { LoginComponent } from "../login/login.component";
import { HomeComponent } from "../home/home.component";
import { AboutComponent } from "../about/about.component";
import { PatientListComponent } from "../patient-list/patient-list.component";
import { RecommendationListComponent } from "../recommendation-list/recommendation-list.component";
import { PatientFormComponent } from "../patient-form/patient-form.component";
import { RecommendationFormComponent } from "../recommendation-form/recommendation-form.component";


export const routes: Routes =
[
  {path: "home", component: HomeComponent},
  {path: "user", component: AboutComponent},
  {path: "patient/new", component: PatientFormComponent},
  {path: "patient/edit/:id", component: PatientFormComponent},
  {path: "patient/show", component: PatientListComponent},
  {path: "patient/show/:page", component: PatientListComponent},
  {path: "patient/show/id/:search", component: PatientListComponent},
  {path: "patient/show/name/:search", component: PatientListComponent},
  {path: "patient/show/name/:search/:page", component: PatientListComponent},
  {path: "patient/recommendation/new/:patientId", component: RecommendationFormComponent},
  {path: "patient/recommendation/edit/:patientId/:id", component: RecommendationFormComponent},
  {path: "patient/recommendation/show/:patientId", component: RecommendationListComponent},
  {path: "patient/recommendation/show/:patientId/:page", component: RecommendationListComponent},
  {path: "patient/recommendation/show/completed/:patientId/:completed", component: RecommendationListComponent},
  {path: "patient/recommendation/show/completed/:patientId/:completed/:page", component: RecommendationListComponent},
  {path: "about", component: AboutComponent},
  {path: "login", component: LoginComponent},
  {path: "", redirectTo: "home", pathMatch: "full"}
];
