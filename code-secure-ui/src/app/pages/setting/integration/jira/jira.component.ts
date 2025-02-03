import {Component, OnInit, signal} from '@angular/core';
import {ButtonDirective} from "../../../../shared/ui/button/button.directive";
import {FormGroup, FormsModule, ReactiveFormsModule} from "@angular/forms";
import {DropdownItem} from '../../../../shared/ui/dropdown/dropdown.model';
import {DropdownComponent} from '../../../../shared/ui/dropdown/dropdown.component';
import {NgIcon} from '@ng-icons/core';
import {JiraSetting} from '../../../../api/models/jira-setting';
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../../core/forms';
import {finalize, forkJoin, switchMap, tap} from 'rxjs';
import {ToastrService} from '../../../../shared/components/toastr/toastr.service';
import {IntegrationService} from '../../../../api/services/integration.service';
import {map} from 'rxjs/operators';


@Component({
  selector: 'app-jira',
  standalone: true,
  imports: [
    ButtonDirective,
    FormsModule,
    DropdownComponent,
    NgIcon,
    ReactiveFormsModule
  ],
  templateUrl: './jira.component.html',
  styleUrl: './jira.component.scss'
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
  jiraProjects = signal<DropdownItem[]>([]);
  issueTypes = signal<DropdownItem[]>([]);
  loadingJiraProject = false;
  loadingIssueType = false;

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
      this.integrationService.getJiraSetting()
    ]).subscribe(result => {
      const jiraSetting = result[1];
      this.loadIssueType(jiraSetting.projectKey!).pipe(
        finalize(() => {
          this.form.patchValue(jiraSetting);
        })
      ).subscribe(issueTypes => {
        this.issueTypes.set(issueTypes);
      });
    });
  }

  onChangeProject(projectKey: string) {
    this.form.controls.projectKey!.setValue(projectKey);
    this.loadIssueType(projectKey).subscribe(issueTypes => {
      this.issueTypes.set(issueTypes);
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
      map(result => {
        return result.map(item => {
          return <DropdownItem>{value: item, label: item};
        });
      })
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
        const jiraProjects = projects.map(item => {
          return {label: item.name, value: item.key}
        })
        this.jiraProjects.set(jiraProjects);
      })
    )
  }

  saveSetting() {
    this.form.disable()
    this.integrationService.updateJiraSetting({
      body: this.form.getRawValue()
    }).pipe(
      finalize(() => this.form.enable())
    ).subscribe(() => {
      this.toastr.success('Update success!');
    })
  }
}
