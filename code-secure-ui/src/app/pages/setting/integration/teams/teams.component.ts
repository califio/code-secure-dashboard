import {Component, OnInit} from '@angular/core';
import {ButtonDirective} from "../../../../shared/ui/button/button.directive";
import {FormGroup, FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../../core/forms';
import {ToastrService} from '../../../../shared/components/toastr/toastr.service';
import {finalize} from 'rxjs';
import {TeamsSetting} from "../../../../api/models/teams-setting";
import {IntegrationService} from '../../../../api/services/integration.service';

@Component({
    selector: 'app-teams',
    standalone: true,
    imports: [
        ButtonDirective,
        FormsModule,
        ReactiveFormsModule
    ],
    templateUrl: './teams.component.html',
    styleUrl: './teams.component.scss'
})
export class TeamsComponent implements OnInit {
    formConfig = new FormSection<ConfigOf<TeamsSetting>>({
        webhook: new FormField(''),
        active: new FormField(true),
        scanCompletedEvent: new FormField(false),
        scanFailedEvent: new FormField(false),
        securityAlertEvent: new FormField(true),
        fixedFindingEvent: new FormField(true),
        newFindingEvent: new FormField(true),
    })
    form: FormGroup<ControlsOf<TeamsSetting>>
    loadingTest = false;

    constructor(
        private formService: FormService,
        private integrationService: IntegrationService,
        private toastr: ToastrService
    ) {
        this.form = this.formService.group(this.formConfig);
    }

    ngOnInit(): void {
        this.integrationService.getTeamsIntegrationSetting().subscribe(setting => {
            this.form.patchValue(setting);
        })
    }

    saveConfig() {
        this.form.disable();
        this.integrationService.updateTeamsIntegrationSetting({
            body: this.form.getRawValue()
        }).pipe(
            finalize(() => this.form.enable())
        ).subscribe(() => {
            this.toastr.success('Update config success!');
        })
    }

    testConnection() {
        this.loadingTest = true;
        this.integrationService.testTeamsIntegrationSetting().pipe(
            finalize(() => this.loadingTest = false)
        ).subscribe(() => {
            this.toastr.success('Send notification success');
        })
    }
}
