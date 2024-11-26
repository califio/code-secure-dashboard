import { Component } from '@angular/core';
import {ComingSoonComponent} from '../../shared/ui/coming-soon/coming-soon.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    ComingSoonComponent
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {

}
