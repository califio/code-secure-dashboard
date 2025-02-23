import {Component, OnInit} from '@angular/core';
import {FormGroup, FormsModule, ReactiveFormsModule} from '@angular/forms';
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../../../../core/forms';
import {AlertSetting} from '../../../../../../api/models/alert-setting';
import {ProjectService} from '../../../../../../api/services/project.service';
import {ProjectStore} from '../../../project.store';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../../../../shared/services/toastr.service';
import {ToggleSwitch} from 'primeng/toggleswitch';
import {ButtonDirective} from 'primeng/button';

@Component({
  selector: 'mail-integration-project',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    ToggleSwitch,
    ButtonDirective
  ],
  templateUrl: './mail.component.html',
  styleUrl: './mail.component.scss'
})
export class MailComponent implements OnInit {
  formConfig = new FormSection<ConfigOf<AlertSetting>>({
    active: new FormField(true),
    scanCompletedEvent: new FormField(false),
    scanFailedEvent: new FormField(false),
    securityAlertEvent: new FormField(false),
    fixedFindingEvent: new FormField(false),
    newFindingEvent: new FormField(false),
  });
  form: FormGroup<ControlsOf<AlertSetting>>

  constructor(
    private formService: FormService,
    private projectService: ProjectService,
    private projectStore: ProjectStore,
    private toastr: ToastrService
  ) {
    this.form = this.formService.group(this.formConfig);
  }

  ngOnInit(): void {
    this.projectService.getMailIntegrationProject({
      projectId: this.projectStore.projectId()
    }).subscribe(setting => {
      this.form.patchValue(setting);
    });
  }

  saveConfig() {
    this.form.disable();
    this.projectService.updateMailIntegrationProject({
      projectId: this.projectStore.projectId(),
      body: this.form.getRawValue()
    }).pipe(
      finalize(() => this.form.enable())
    ).subscribe(() => {
      this.toastr.success({
        message: 'Update success!'
      });
    })
  }
}
