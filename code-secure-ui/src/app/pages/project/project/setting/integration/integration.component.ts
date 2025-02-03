import {Component} from '@angular/core';
import {AvatarComponent} from '../../../../../shared/ui/avatar/avatar.component';
import {ButtonDirective} from '../../../../../shared/ui/button/button.directive';
import {NgIcon} from '@ng-icons/core';
import {TeamsComponent} from '../../../../setting/integration/teams/teams.component';
import {JiraComponent} from './jira/jira.component';
import {ProjectStore} from '../../project.store';

@Component({
  selector: 'app-integration',
  standalone: true,
  imports: [
    AvatarComponent,
    ButtonDirective,
    NgIcon,
    TeamsComponent,
    JiraComponent
  ],
  templateUrl: './integration.component.html',
  styleUrl: './integration.component.scss'
})
export class IntegrationComponent {
  config = {
    jira: false
  }

  constructor(public projectStore: ProjectStore) {
  }
}
