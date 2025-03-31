import {Component, OnInit} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {ToastrService} from '../../../../shared/services/toastr.service';
import {finalize} from 'rxjs';
import {SlaSetting} from '../../../../api/models/sla-setting';
import {FormService} from '../../../../core/forms';
import {SettingService} from '../../../../api/services/setting.service';
import {Button} from 'primeng/button';
import {InputNumber} from 'primeng/inputnumber';

@Component({
  selector: 'app-sla',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule,
    InputNumber,
    Button
  ],
  templateUrl: './sla.component.html',
  styleUrl: './sla.component.scss'
})
export class SlaComponent implements OnInit {
  loading = false;
  slaSetting: SlaSetting = {
    sast: {},
    sca: {}
  };

  constructor(
    private formService: FormService,
    private settingService: SettingService,
    private toastr: ToastrService
  ) {
  }

  ngOnInit(): void {
    this.settingService.getSlaSetting().subscribe(setting => {
      this.slaSetting = setting;
    })
  }

  saveSla() {
    this.loading = true;
    this.settingService.updateSlaSetting({
      body: this.slaSetting
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(() => {
      this.toastr.success({
        message: 'Update success!'
      });
    })
  }
}
