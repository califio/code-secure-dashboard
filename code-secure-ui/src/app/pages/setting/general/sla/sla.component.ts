import {Component, OnInit} from '@angular/core';
import {ButtonDirective} from '../../../../shared/ui/button/button.directive';
import {FormGroup, FormsModule, ReactiveFormsModule} from '@angular/forms';
import {DropdownComponent} from '../../../../shared/ui/dropdown/dropdown.component';
import {ToastrService} from '../../../../shared/components/toastr/toastr.service';
import {finalize} from 'rxjs';
import {SlaSetting} from '../../../../api/models/sla-setting';
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../../core/forms';
import {Sla} from '../../../../api/models/sla';
import {SettingService} from '../../../../api/services/setting.service';

@Component({
  selector: 'app-sla',
  standalone: true,
  imports: [
    ButtonDirective,
    ReactiveFormsModule,
    DropdownComponent,
    FormsModule
  ],
  templateUrl: './sla.component.html',
  styleUrl: './sla.component.scss'
})
export class SlaComponent implements OnInit{

  formConfig = new FormSection<ConfigOf<SlaSetting>>({
    sast: new FormSection<ConfigOf<Sla>>({
      critical: new FormField(0),
      high: new FormField(0),
      medium: new FormField(0),
      low: new FormField(0),
      info: new FormField(0),
    }),
    sca: new FormSection<ConfigOf<Sla>>({
      critical: new FormField(0),
      high: new FormField(0),
      medium: new FormField(0),
      low: new FormField(0),
      info: new FormField(0),
    })
  });
  form: FormGroup<ControlsOf<SlaSetting>>;
  constructor(
    private formService: FormService,
    private settingService: SettingService,
    private toastr: ToastrService
  ) {
    this.form = this.formService.group(this.formConfig);
  }

  ngOnInit(): void {
    this.settingService.getSlaSetting().subscribe(sla => {
      this.form.patchValue(sla);
    })
  }

  saveSla() {
    this.form.disable();
    this.settingService.updateSlaSetting({
      body: this.form.getRawValue()
    }).pipe(
      finalize(() => this.form.enable())
    ).subscribe(() => {
      this.toastr.success('Update success!');
    })
  }
}
