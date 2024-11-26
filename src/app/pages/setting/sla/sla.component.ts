import {Component, OnInit} from '@angular/core';
import {ButtonDirective} from '../../../shared/ui/button/button.directive';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {SlaConfig} from '../../../api/models/sla-config';
import {ConfigService} from '../../../api/services/config.service';
import {DropdownComponent} from '../../../shared/ui/dropdown/dropdown.component';
import {ToastrService} from '../../../shared/components/toastr/toastr.service';
import {finalize} from 'rxjs';

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

  sla: SlaConfig = {};
  loading = false;
  constructor(
    private configService: ConfigService,
    private toastr: ToastrService
  ) {
  }

  ngOnInit(): void {
    this.configService.getSlaConfig().subscribe(sla => {
      this.sla = sla;
    })
  }

  saveSla() {
    if (this.loading) {
      return;
    }
    this.loading = true;
    this.configService.updateSlaConfig({
      body: this.sla
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(sla => {
      this.sla = sla;
      this.toastr.success('Update success!');
    })
  }
}
