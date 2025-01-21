import {Component, Input} from '@angular/core';
import {FindingStatusLabelComponent} from "../../finding/finding-status-label/finding-status-label.component";
import {LoadingTableComponent} from "../../../ui/loading-table/loading-table.component";
import {NgIcon} from "@ng-icons/core";

@Component({
  selector: 'app-list-dependency',
  standalone: true,
    imports: [
        FindingStatusLabelComponent,
        LoadingTableComponent,
        NgIcon
    ],
  templateUrl: './list-dependency.component.html',
  styleUrl: './list-dependency.component.scss'
})
export class ListDependencyComponent {
  @Input()
  loading = false;
}
