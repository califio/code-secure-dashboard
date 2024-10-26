import { Component } from '@angular/core';
import {ComingSoonComponent} from "../../shared/components/coming-soon/coming-soon.component";

@Component({
  selector: 'app-dashboard',
  standalone: true,
    imports: [
        ComingSoonComponent
    ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {

}
