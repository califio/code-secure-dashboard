import { Component } from '@angular/core';
import {ComingSoonComponent} from '../../../../shared/components/ui/coming-soon/coming-soon.component';

@Component({
  selector: 'app-dependency',
  standalone: true,
  imports: [
    ComingSoonComponent
  ],
  templateUrl: './dependency.component.html',
  styleUrl: './dependency.component.scss'
})
export class DependencyComponent {

}
