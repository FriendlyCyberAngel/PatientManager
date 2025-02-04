import { Component } from '@angular/core';
import { HeaderComponent } from '../header/header.component'
import { FooterComponent } from '../footer/footer.component'
import { NavBarComponent } from '../nav-bar/nav-bar.component'
import { RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-root',
  imports: [HeaderComponent, FooterComponent, NavBarComponent, RouterOutlet],
  templateUrl: './root.component.html',
  styleUrl: './root.component.css'
})


export class RootComponent {

}
