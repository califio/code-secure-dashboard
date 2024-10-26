import { Component } from '@angular/core';
import {PaginationComponent} from "../../../shared/components/pagination/pagination.component";
import {NG_ICON_DIRECTIVES} from "@ng-icons/core";
import {NgListAssetComponent} from "../../../shared/components/ng-list-asset/ng-list-asset.component";

@Component({
  selector: 'app-list-asset',
  standalone: true,
  imports: [
    PaginationComponent,
    NG_ICON_DIRECTIVES,
    NgListAssetComponent,
  ],
  templateUrl: './list-asset.component.html',
  styleUrl: './list-asset.component.scss'
})
export class ListAssetComponent {

}
