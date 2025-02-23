import {Component, OnInit} from '@angular/core';
import {FormGroup, FormsModule, ReactiveFormsModule} from '@angular/forms';
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../../../../core/forms';
import {TeamsSetting} from '../../../../../../api/models/teams-setting';
import {ProjectService} from '../../../../../../api/services/project.service';
import {ProjectStore} from '../../../project.store';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../../../../shared/services/toastr.service';
import {InputText} from 'primeng/inputtext';
import {ToggleSwitch} from 'primeng/toggleswitch';
import {ButtonDirective} from 'primeng/button';

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
  formConfig = new FormSection<ConfigOf<TeamsSetting>>({
    webhook: new FormField(''),
    active: new FormField(false),
    scanCompletedEvent: new FormField(false),
    scanFailedEvent: new FormField(false),
    securityAlertEvent: new FormField(false),
    fixedFindingEvent: new FormField(false),
    newFindingEvent: new FormField(false),
  });
  form: FormGroup<ControlsOf<TeamsSetting>>
  loadingTest = false;

  constructor(
    private projectService: ProjectService,
    private formService: FormService,
    private projectStore: ProjectStore,
    private toastr: ToastrService
  ) {
    this.form = this.formService.group(this.formConfig);
  }

  ngOnInit(): void {
    this.projectService.getTeamsIntegrationProject({
      projectId: this.projectStore.projectId()
    }).subscribe(setting => {
      this.form.patchValue(setting);
    })
  }

  saveConfig() {
    this.form.disable()
    this.projectService.updateTeamsIntegrationProject({
      projectId: this.projectStore.projectId(),
      body: this.form.getRawValue()
    }).pipe(
      finalize(() => this.form.enable())
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
