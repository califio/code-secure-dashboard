import {Component, OnInit, signal} from '@angular/core';
import {FormGroup, FormsModule, ReactiveFormsModule} from "@angular/forms";
import {JiraSetting} from '../../../../api/models/jira-setting';
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../../core/forms';
import {finalize, forkJoin, tap} from 'rxjs';
import {ToastrService} from '../../../../shared/services/toastr.service';
import {IntegrationService} from '../../../../api/services/integration.service';
import {JiraProject} from '../../../../api/models/jira-project';
import {Select, SelectChangeEvent} from 'primeng/select';
import {Button, ButtonDirective} from 'primeng/button';
import {ToggleSwitch} from 'primeng/toggleswitch';
import {InputText} from 'primeng/inputtext';
import {Password} from 'primeng/password';


@Component({
  selector: 'app-jira',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    Select,
    ButtonDirective,
    ToggleSwitch,
    InputText,
    Password,
    Button
  ],
  templateUrl: './jira.component.html',
})
export class JiraComponent implements OnInit {
  formConfig = new FormSection<ConfigOf<JiraSetting>>({
    active: new FormField(true),
    webUrl: new FormField(''),
    userName: new FormField(''),
    password: new FormField(''),
    projectKey: new FormField(''),
    issueType: new FormField(''),
  })
  form: FormGroup<ControlsOf<JiraSetting>>
  jiraProjects = signal<JiraProject[]>([]);
  issueTypes = signal<{ name: string, value: string }[]>([]);
  loadingJiraProject = false;
  loadingIssueType = false;
  loadingTest = false;

  constructor(
    private integrationService: IntegrationService,
    private formService: FormService,
    private toastr: ToastrService
  ) {
    this.form = this.formService.group(this.formConfig);
  }

  ngOnInit(): void {
    forkJoin([
      this.loadJiraProject(),
      this.integrationService.getJiraIntegrationSetting()
    ]).subscribe(result => {
      const jiraSetting = result[1];
      if (jiraSetting.projectKey) {
        this.loadIssueType(jiraSetting.projectKey!).pipe(
          finalize(() => {
            this.form.patchValue(jiraSetting);
          })
        ).subscribe(issueTypes => {
          this.issueTypes.set(issueTypes.map(item => {
            return {name: item, value: item}
          }));
        });
      }
    });
  }

  onChangeProject(projectKey: string) {
    this.form.controls.projectKey!.setValue(projectKey);
    this.loadIssueType(projectKey).subscribe(issueTypes => {
      this.issueTypes.set(issueTypes.map(item => {
        return {name: item, value: item}
      }));
    });
  }

  onChangeIssueType(issueType: any) {
    this.form.controls.issueType!.setValue(issueType);
  }

  loadIssueType(projectKey: string) {
    this.loadingIssueType = true;
    return this.integrationService.getJiraIssueTypes({
      projectKey: projectKey
    }).pipe(
      finalize(() => this.loadingIssueType = false),
    );
  }

  onReload() {
    if (this.form.controls.password!.value) {
      this.loadJiraProject(true, this.form.getRawValue()).subscribe();
    } else {
      this.loadJiraProject(true).subscribe();
    }
  }

  loadJiraProject(reload: boolean = false, setting: JiraSetting | undefined = undefined) {
    this.loadingJiraProject = true;
    return this.integrationService.getJiraProjects({
      reload: reload,
      body: setting
    }).pipe(
      finalize(() => this.loadingJiraProject = false),
      tap(projects => {
        this.jiraProjects.set(projects);
      })
    )
  }

  saveSetting() {
    this.form.disable()
    this.integrationService.updateJiraIntegrationSetting({
      body: this.form.getRawValue()
    }).pipe(
      finalize(() => this.form.enable())
    ).subscribe(() => {
      this.toastr.success({
        message: 'Update success!'
      });
    })
  }

  testConnection() {
    this.loadingTest = true;
    this.integrationService.testJiraIntegrationSetting().pipe(
      finalize(() => this.loadingTest = false)
    ).subscribe(() => {
      this.toastr.success({
        message: 'Success!'
      });
    })
  }

  onChangeJiraProject($event: SelectChangeEvent) {
    this.loadIssueType($event.value).subscribe(issueTypes => {
      this.issueTypes.set(issueTypes.map(item => {
        return {name: item, value: item}
      }));
    });
  }
}
