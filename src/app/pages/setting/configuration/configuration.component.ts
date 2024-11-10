import { Component } from '@angular/core';
import {ComingSoonComponent} from '../../../shared/components/ui/coming-soon/coming-soon.component';
import {ConfirmPopupComponent} from '../../../shared/components/ui/confirm-popup/confirm-popup.component';
import {LoadingTableComponent} from '../../../shared/components/ui/loading-table/loading-table.component';
import {NgIcon} from '@ng-icons/core';
import {ReactiveFormsModule} from '@angular/forms';
import {TimeagoModule} from 'ngx-timeago';

@Component({
  selector: 'app-configuration',
  standalone: true,
  imports: [
    ComingSoonComponent,
    ConfirmPopupComponent,
    LoadingTableComponent,
    NgIcon,
    ReactiveFormsModule,
    TimeagoModule
  ],
  templateUrl: './configuration.component.html',
  styleUrl: './configuration.component.scss'
})
export class ConfigurationComponent {

}
