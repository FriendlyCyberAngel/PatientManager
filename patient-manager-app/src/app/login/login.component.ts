import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-login',
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})


export class LoginComponent
{
  public onLoginButtonClick(event: Event): void
  {
    
  }

  public onInput(event: Event): void
  {
    const e = event.target as HTMLInputElement;
    (this as any)[e.name] = e.value;
  }


  @Input() login: string = "";
  @Input() password: string = "";
}
