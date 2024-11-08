import { Component } from '@angular/core';
import {NgIcon} from '@ng-icons/core';

@Component({
  selector: 'app-internal-server-error',
  standalone: true,
  imports: [
    NgIcon
  ],
  templateUrl: './internal-server-error.component.html',
  styleUrl: './internal-server-error.component.scss'
})
export class InternalServerErrorComponent {

}
