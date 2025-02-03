import {Component, OnInit, signal} from '@angular/core';
import {ButtonDirective} from '../../../../../../shared/ui/button/button.directive';
import {FormGroup, ReactiveFormsModule} from '@angular/forms';
import {ComingSoonComponent} from '../../../../../../shared/ui/coming-soon/coming-soon.component';
import {DropdownComponent} from '../../../../../../shared/ui/dropdown/dropdown.component';
import {NgIcon} from '@ng-icons/core';
import {DropdownItem} from '../../../../../../shared/ui/dropdown/dropdown.model';
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../../../../core/forms';
import {JiraProjectSetting} from '../../../../../../api/models/jira-project-setting';
import {RouterLink} from '@angular/router';
import {ProjectService} from '../../../../../../api/services/project.service';
import {ProjectStore} from '../../../project.store';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../../../../shared/components/toastr/toastr.service';
import {map} from 'rxjs/operators';
import {IntegrationService} from '../../../../../../api/services/integration.service';

@Component({
  selector: 'jira-integration',
  standalone: true,
  imports: [
    ButtonDirective,
    ReactiveFormsModule,
    ComingSoonComponent,
    DropdownComponent,
    NgIcon,
    RouterLink
  ],
  templateUrl: './jira.component.html',
  styleUrl: './jira.component.scss'
})
export class JiraComponent implements OnInit {
  jiraProjects = signal<DropdownItem[]>([]);
  issueTypes = signal<DropdownItem[]>([]);
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
      this.jiraProjects.set(setting.jiraProjects!.map(item => {
        return {value: item.key, label: item.name}
      }));
      this.loadIssueType(setting.projectKey!).subscribe(issueTypes => {
        this.issueTypes.set(issueTypes);
        this.form.patchValue(setting)
      })
    });

  }

  onReload() {
    this.getJiraSetting(true).subscribe(setting => {
      this.jiraProjects.set(setting.jiraProjects!.map(item => {
        return {value: item.key, label: item.name}
      }));
    })
  }

  loadIssueType(projectKey: string) {
    this.loadingIssueType = true;
    return this.integrationService.getJiraIssueTypes({
      projectKey: projectKey
    }).pipe(
      finalize(() => this.loadingIssueType = false),
      map(result => {
        return result.map(item => {
          return <DropdownItem>{value: item, label: item};
        });
      })
    );
  }

  saveJiraSetting() {
    this.form.disable();
    this.projectService.updateJiraProjectSetting({
      projectId: this.projectStore.projectId(),
      body: this.form.getRawValue()
    }).pipe(
      finalize(() => this.form.enable())
    ).subscribe(() => {
      this.toastr.success('Update success!');
    })
  }

  private getJiraSetting(reload = false) {
    this.loading = true;
    return this.projectService.getJiraProjectSetting({
      projectId: this.projectStore.projectId(),
      reload: reload
    }).pipe(
      finalize(() => {
        this.loading = false;
      })
    );
  }

  onChangeProject(projectKey: any) {
    this.form.controls.projectKey!.setValue(projectKey);
    this.loadIssueType(projectKey).subscribe(issueTypes => {
      this.issueTypes.set(issueTypes);
    });
  }

  onChangeIssueType(issueType: any) {
    this.form.controls.issueType!.setValue(issueType);
  }
}
