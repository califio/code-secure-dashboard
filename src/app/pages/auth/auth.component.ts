import { Component } from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {NgIcon} from '@ng-icons/core';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [
    RouterOutlet,
    NgIcon
  ],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.scss'
})
export class AuthComponent {

}
