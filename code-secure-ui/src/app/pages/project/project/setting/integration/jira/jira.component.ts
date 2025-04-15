import {Component, OnInit, signal} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {RouterLink} from '@angular/router';
import {ProjectService} from '../../../../../../api/services/project.service';
import {ProjectStore} from '../../../project.store';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../../../../shared/services/toastr.service';
import {IntegrationService} from '../../../../../../api/services/integration.service';
import {Button, ButtonDirective} from 'primeng/button';
import {Select, SelectChangeEvent} from 'primeng/select';
import {JiraProject} from '../../../../../../api/models/jira-project';
import {JiraProjectSetting} from '../../../../../../api/models/jira-project-setting';

@Component({
  selector: 'jira-integration-project',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink,
    Button,
    Select,
    ButtonDirective,
    FormsModule
  ],
  templateUrl: './jira.component.html',
  styleUrl: './jira.component.scss'
})
export class JiraComponent implements OnInit {
  jiraProjects = signal<JiraProject[]>([]);
  issueTypes = signal<{ label: string, value: string }[]>([]);
  setting: JiraProjectSetting = {active: false, issueType: '', projectKey: ''};
  loading = false;
  loadingIssueType = false;

  constructor(
    public projectStore: ProjectStore,
    private projectService: ProjectService,
    private integrationService: IntegrationService,
    private toastr: ToastrService
  ) {
  }

  ngOnInit(): void {
    this.getJiraSetting().subscribe(setting => {
      this.setting = {
        active: setting.active, issueType: setting.issueType, projectKey: setting.projectKey
      };
      this.jiraProjects.set(setting.jiraProjects!);
      if (setting.projectKey) {
        this.loadIssueType(setting.projectKey!).subscribe(issueTypes => {
          this.issueTypes.set(issueTypes.map(item => {
            return {value: item, label: item}
          }));
        })
      }
    });

  }

  onReload() {
    this.getJiraSetting(true).subscribe(setting => {
      this.jiraProjects.set(setting.jiraProjects!);
    });
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
    this.loading = true;
    this.projectService.updateJiraIntegrationProject({
      projectId: this.projectStore.projectId(),
      body: this.setting
    }).pipe(
      finalize(() => this.loading = false)
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

  onChangeProject($event: SelectChangeEvent) {
    this.loadIssueType($event.value).subscribe(issueTypes => {
      this.issueTypes.set(issueTypes.map(item => {
        return {value: item, label: item}
      }));
    });
  }
}
