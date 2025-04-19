import {Component} from '@angular/core';
import {TableModule} from "primeng/table";
import {TimeagoModule} from "ngx-timeago";
import {StatsComponent} from './stats/stats.component';
import {CommitComponent} from './commit/commit.component';

@Component({
  selector: 'app-commit',
  imports: [
    TableModule,
    TimeagoModule,
    StatsComponent,
    CommitComponent
  ],
  templateUrl: './overview.component.html',
})
export class OverviewComponent {
}
