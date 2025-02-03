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
import {ProjectStore} from '../../project.store';
import {ProjectService} from '../../../../../api/services/project.service';
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../../../core/forms';
import {ToastrService} from '../../../../../shared/components/toastr/toastr.service';
import {ThresholdMode, ThresholdSetting} from '../../../../../api/models';

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
    FormsModule,
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
  sastFormConfig = new FormSection<ConfigOf<ThresholdSetting>>({
    mode: new FormField(ThresholdMode.MonitorOnly),
    critical: new FormField(0),
    high: new FormField(0),
    low: new FormField(0),
    medium: new FormField(0),
  });
  scaFormConfig = new FormSection<ConfigOf<ThresholdSetting>>({
    mode: new FormField(ThresholdMode.MonitorOnly),
    critical: new FormField(0),
    high: new FormField(0),
    low: new FormField(0),
    medium: new FormField(0),
  });
  sastForm: FormGroup<ControlsOf<ThresholdSetting>>;
  scaForm: FormGroup<ControlsOf<ThresholdSetting>>;

  constructor(
    private projectService: ProjectService,
    private store: ProjectStore,
    private formService: FormService,
    private toastr: ToastrService
  ) {
    this.sastForm = this.formService.group(this.sastFormConfig);
    this.scaForm = this.formService.group(this.scaFormConfig);
  }

  ngOnInit(): void {
    this.projectService.getProjectSetting({
      projectId: this.store.projectId()
    }).subscribe(setting => {
      this.sastForm.patchValue(setting.sastSetting!);
      this.scaForm.patchValue(setting.scaSetting!);
    });
  }

  updateSastSetting() {
    this.projectService.updateSastSettingProject({
      projectId: this.store.projectId(),
      body: this.sastForm.getRawValue()
    }).subscribe(() => {
      this.toastr.success('Update success!');
    })
  }

  updateScaSetting() {
    this.projectService.updateScaSettingProject({
      projectId: this.store.projectId(),
      body: this.scaForm.getRawValue()
    }).subscribe(() => {
      this.toastr.success('Update success!');
    })
  }

  changeSastMode($event: any) {
    this.sastForm.controls.mode!.setValue($event);
  }

  changeScaMode($event: any) {
    this.scaForm.controls.mode!.setValue($event);
  }
}
