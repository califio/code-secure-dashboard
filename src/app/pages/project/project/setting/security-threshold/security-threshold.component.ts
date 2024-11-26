import {Component, OnInit} from '@angular/core';
import {ComingSoonComponent} from '../../../../../shared/ui/coming-soon/coming-soon.component';
import {ButtonDirective} from "../../../../../shared/ui/button/button.directive";
import {FormGroup, FormsModule, ReactiveFormsModule} from "@angular/forms";
import {AvatarComponent} from '../../../../../shared/ui/avatar/avatar.component';
import {LoadingTableComponent} from '../../../../../shared/ui/loading-table/loading-table.component';
import {NgIcon} from '@ng-icons/core';
import {PaginationComponent} from '../../../../../shared/ui/pagination/pagination.component';
import {TimeagoModule} from 'ngx-timeago';
import {DropdownComponent} from '../../../../../shared/ui/dropdown/dropdown.component';
import {DropdownItem} from '../../../../../shared/ui/dropdown/dropdown.model';
import {
  AuthRequest,
  ProjectSettingMetadata,
  SastSetting,
  ScaSetting,
  SeverityThreshold,
  ThresholdMode
} from '../../../../../api/models';
import {ProjectStore} from '../../project.store';
import {ProjectService} from '../../../../../api/services/project.service';
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../../../core/forms';
import {ToastrService} from '../../../../../shared/components/toastr/toastr.service';

@Component({
  selector: 'app-security-threshold',
  standalone: true,
  imports: [
    ComingSoonComponent,
    ButtonDirective,
    ReactiveFormsModule,
    AvatarComponent,
    LoadingTableComponent,
    NgIcon,
    PaginationComponent,
    TimeagoModule,
    DropdownComponent,
    FormsModule
  ],
  templateUrl: './security-threshold.component.html',
  styleUrl: './security-threshold.component.scss'
})
export class SecurityThresholdComponent implements OnInit {
  thresholdModeOptions: DropdownItem[] = [
    {
      value: ThresholdMode.MonitorOnly,
      label: 'Monitor Only'
    },
    {
      value: ThresholdMode.BlockOnConfirmation,
      label: 'Block On Confirmation'
    },
    {
      value: ThresholdMode.BlockOnDetection,
      label: 'Block On Detection'
    },
  ];
  formConfig = new FormSection<ConfigOf<ProjectSettingMetadata>>({
    sastSetting: new FormSection<ConfigOf<SastSetting>>({
      mode: new FormField(ThresholdMode.MonitorOnly),
      severityThreshold: new FormSection<ConfigOf<SeverityThreshold>>({
        critical: new FormField(0),
        high: new FormField(0),
        low: new FormField(0),
        medium: new FormField(0),
      })
    }),
    scaSetting: new FormSection<ConfigOf<ScaSetting>>({
      mode: new FormField(ThresholdMode.MonitorOnly),
      severityThreshold: new FormSection<ConfigOf<SeverityThreshold>>({
        critical: new FormField(0),
        high: new FormField(0),
        low: new FormField(0),
        medium: new FormField(0),
      })
    })
  });
  form: FormGroup<ControlsOf<ProjectSettingMetadata>>;
  constructor(
    private projectService: ProjectService,
    private store: ProjectStore,
    private formService: FormService,
    private toastr: ToastrService
  ) {
    this.form = this.formService.group(this.formConfig);
  }

  ngOnInit(): void {
    this.projectService.getProjectSetting({
      slug: this.store.slug()
    }).subscribe(setting => {
      this.form.patchValue(setting);
    });
  }

  saveSecurityThreshold() {

  }

  updateSecurityThreshold() {
    this.projectService.updateProjectSetting({
      slug: this.store.slug(),
      body: this.form.getRawValue()
    }).subscribe(data => {
      this.form.patchValue(data);
      this.toastr.success('Update success!');
    })
  }

  changeSastMode($event: any) {
    this.form.controls.sastSetting.controls.mode.setValue($event);
  }
}
