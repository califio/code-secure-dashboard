import {Component, OnInit} from '@angular/core';
import {FormGroup, FormsModule, ReactiveFormsModule} from "@angular/forms";
import {TimeagoModule} from 'ngx-timeago';
import {ProjectStore} from '../../project.store';
import {ProjectService} from '../../../../../api/services/project.service';
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../../../core/forms';
import {ToastrService} from '../../../../../shared/services/toastr.service';
import {ThresholdMode, ThresholdSetting} from '../../../../../api/models';
import {Panel} from 'primeng/panel';
import {SelectButton} from 'primeng/selectbutton';
import {InputNumber} from 'primeng/inputnumber';
import {Button} from 'primeng/button';

@Component({
  selector: 'app-security-threshold',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    TimeagoModule,
    FormsModule,
    Panel,
    SelectButton,
    InputNumber,
    Button,
  ],
  templateUrl: './security-threshold.component.html',
  styleUrl: './security-threshold.component.scss'
})
export class SecurityThresholdComponent implements OnInit {
  thresholdModeOptions = [
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
    this.projectService.getThresholdProject({
      projectId: this.store.projectId()
    }).subscribe(threshold => {
      this.sastForm.patchValue(threshold.sast!);
      this.scaForm.patchValue(threshold.sca!);
    });
  }

  updateSastSetting() {
    this.projectService.updateThresholdProject({
      projectId: this.store.projectId(),
      body: {
        sast: this.sastForm.getRawValue()
      }
    }).subscribe(() => {
      this.toastr.success({
        message: 'Update success!'
      });
    })
  }

  updateScaSetting() {
    this.projectService.updateThresholdProject({
      projectId: this.store.projectId(),
      body: {
        sca: this.scaForm.getRawValue()
      }
    }).subscribe(() => {
      this.toastr.success({
        message: 'Update success!'
      });
    })
  }
}
