import { Component } from '@angular/core';
import {ButtonDirective} from "../../../shared/ui/button/button.directive";
import {FormsModule} from "@angular/forms";
import {ConfigService} from '../../../api/services/config.service';
import {ToastrService} from '../../../shared/components/toastr/toastr.service';
import {MailConfig} from '../../../api/models/mail-config';

@Component({
  selector: 'app-mail',
  standalone: true,
    imports: [
        ButtonDirective,
        FormsModule
    ],
  templateUrl: './mail.component.html',
  styleUrl: './mail.component.scss'
})
export class MailComponent {
  config: MailConfig = {
    password: "", port: 0, server: "", ssl: false, username: ""
  };
  constructor(
    private configService: ConfigService,
    private toastr: ToastrService
  ) {
    configService.getMailConfig().subscribe(config => {
      this.config = config;
    })
  }

  saveConfig() {
    this.configService.updateMailConfig({
      body: this.config
    }).subscribe(config => {
      this.config = config;
      this.toastr.success('Update config success!');
    })
  }
}
