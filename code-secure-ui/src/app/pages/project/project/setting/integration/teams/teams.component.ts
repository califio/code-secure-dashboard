import {Component, OnInit} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {ProjectService} from '../../../../../../api/services/project.service';
import {ProjectStore} from '../../../project.store';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../../../../shared/services/toastr.service';
import {InputText} from 'primeng/inputtext';
import {ToggleSwitch} from 'primeng/toggleswitch';
import {ButtonDirective} from 'primeng/button';
import {TeamsProjectSetting} from '../../../../../../api/models/teams-project-setting';

@Component({
  selector: 'teams-integration-project',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    InputText,
    ToggleSwitch,
    ButtonDirective
  ],
  templateUrl: './teams.component.html',
  styleUrl: './teams.component.scss'
})
export class TeamsComponent implements OnInit {
  loadingTest = false;
  setting: TeamsProjectSetting = {
    webhook: '',
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
    private projectService: ProjectService,
    private projectStore: ProjectStore,
    private toastr: ToastrService
  ) {
  }

  ngOnInit(): void {
    this.projectService.getTeamsIntegrationProject({
      projectId: this.projectStore.projectId()
    }).subscribe(setting => {
      this.setting = setting;
    })
  }

  saveConfig() {
    this.loading = true;
    console.log(this.setting)
    this.projectService.updateTeamsIntegrationProject({
      projectId: this.projectStore.projectId(),
      body: this.setting
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(() => {
      this.toastr.success({
        message: 'Update success!'
      });
    });
  }

  testAlert() {
    this.loadingTest = true;
    this.projectService.testTeamsIntegrationProject({
      projectId: this.projectStore.projectId()
    }).pipe(
      finalize(() => this.loadingTest = false)
    ).subscribe(() => {
      this.toastr.success({
        message: 'Sent test alert!'
      });
    })
  }
}
