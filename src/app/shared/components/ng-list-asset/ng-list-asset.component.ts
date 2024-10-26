import { Component } from '@angular/core';
import {NgIcon} from "@ng-icons/core";
import {PaginationComponent} from "../pagination/pagination.component";

@Component({
  selector: 'ng-list-asset',
  standalone: true,
  imports: [
    NgIcon,
    PaginationComponent
  ],
  templateUrl: './ng-list-asset.component.html',
  styleUrl: './ng-list-asset.component.scss'
})
export class NgListAssetComponent {

}
