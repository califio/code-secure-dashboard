import {Component, OnInit, signal} from '@angular/core';
import {FormGroup, ReactiveFormsModule} from '@angular/forms';
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../../../../core/forms';
import {JiraProjectSetting} from '../../../../../../api/models/jira-project-setting';
import {RouterLink} from '@angular/router';
import {ProjectService} from '../../../../../../api/services/project.service';
import {ProjectStore} from '../../../project.store';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../../../../shared/services/toastr.service';
import {IntegrationService} from '../../../../../../api/services/integration.service';
import {Button, ButtonDirective} from 'primeng/button';
import {Select, SelectChangeEvent} from 'primeng/select';
import {JiraProject} from '../../../../../../api/models/jira-project';

@Component({
  selector: 'jira-integration-project',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink,
    Button,
    Select,
    ButtonDirective
  ],
  templateUrl: './jira.component.html',
  styleUrl: './jira.component.scss'
})
export class JiraComponent implements OnInit {
  jiraProjects = signal<JiraProject[]>([]);
  issueTypes = signal<{ label: string, value: string }[]>([]);
  formConfig = new FormSection<ConfigOf<JiraProjectSetting>>({
    active: new FormField(false),
    projectKey: new FormField(''),
    issueType: new FormField('')
  })
  form: FormGroup<ControlsOf<JiraProjectSetting>>;
  loading = false;
  loadingIssueType = false;

  constructor(
    public projectStore: ProjectStore,
    private projectService: ProjectService,
    private integrationService: IntegrationService,
    private formService: FormService,
    private toastr: ToastrService
  ) {
    this.form = this.formService.group(this.formConfig);
    this.form.controls.active!.setValue(this.projectStore.projectSetting().jiraSetting?.active);
  }

  ngOnInit(): void {
    this.getJiraSetting().subscribe(setting => {
      this.jiraProjects.set(setting.jiraProjects!);
      this.loadIssueType(setting.projectKey!).subscribe(issueTypes => {
        this.issueTypes.set(issueTypes.map(item => {
          return {value: item, label: item}
        }));
        this.form.patchValue(setting)
      })
    });

  }

  onReload() {
    this.getJiraSetting(true).subscribe(setting => {
      this.jiraProjects.set(setting.jiraProjects!);
    })
  }

  loadIssueType(projectKey: string) {
    this.loadingIssueType = true;
    return this.integrationService.getJiraIssueTypes({
      projectKey: projectKey
    }).pipe(
      finalize(() => this.loadingIssueType = false),
    );
  }

  saveJiraSetting() {
    this.form.disable();
    this.projectService.updateJiraIntegrationProject({
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

  private getJiraSetting(reload = false) {
    this.loading = true;
    return this.projectService.getJiraIntegrationProject({
      projectId: this.projectStore.projectId(),
      reload: reload
    }).pipe(
      finalize(() => {
        this.loading = false;
      })
    );
  }

  onChangeIssueType(issueType: any) {
    this.form.controls.issueType!.setValue(issueType);
  }

  onChangeProject($event: SelectChangeEvent) {
    this.loadIssueType($event.value).subscribe(issueTypes => {
      this.issueTypes.set(issueTypes.map(item => {
        return {value: item, label: item}
      }));
    });
  }
}
