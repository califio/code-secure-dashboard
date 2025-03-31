import {Component, OnInit} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {FormService} from '../../../../../../core/forms';
import {ProjectService} from '../../../../../../api/services/project.service';
import {ProjectStore} from '../../../project.store';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../../../../shared/services/toastr.service';
import {ToggleSwitch} from 'primeng/toggleswitch';
import {ButtonDirective} from 'primeng/button';
import {MailProjectAlertSetting} from '../../../../../../api/models/mail-project-alert-setting';

@Component({
  selector: 'mail-integration-project',
  standalone: true,
  imports: [
    FormsModule,
    ToggleSwitch,
    ButtonDirective
  ],
  templateUrl: './mail.component.html',
  styleUrl: './mail.component.scss'
})
export class MailComponent implements OnInit {
  setting: MailProjectAlertSetting = {
    active: false,
    fixedFindingEvent: false,
    needTriageFindingEvent: false,
    newFindingEvent: false,
    scanCompletedEvent: false,
    scanFailedEvent: false,
    securityAlertEvent: false
  };
  loading = false;

  constructor(
    private formService: FormService,
    private projectService: ProjectService,
    private projectStore: ProjectStore,
    private toastr: ToastrService
  ) {
  }

  ngOnInit(): void {
    this.projectService.getMailIntegrationProject({
      projectId: this.projectStore.projectId()
    }).subscribe(setting => {
      this.setting = setting;

    });
  }

  saveConfig() {
    this.loading = true;
    this.projectService.updateMailIntegrationProject({
      projectId: this.projectStore.projectId(),
      body: this.setting
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(() => {
      this.toastr.success({
        message: 'Update success!'
      });
    })
  }
}
