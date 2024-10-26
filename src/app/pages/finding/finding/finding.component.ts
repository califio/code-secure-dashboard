import { Component } from '@angular/core';
import {Finding} from '../../../shared/components/finding/finding.model';
import {FindingDetailComponent} from '../../../shared/components/finding/finding-detail/finding-detail.component';

@Component({
  selector: 'app-finding',
  standalone: true,
  imports: [
    FindingDetailComponent
  ],
  templateUrl: './finding.component.html',
  styleUrl: './finding.component.scss'
})
export class FindingComponent {
  finding: Finding = {
    id: 1,
    name: 'Path Traversal at src/main/java/vn/com/ivnd/leads/controller/FileIOController.java',
    description: "Detected user input controlling a file path. An attacker could control the location of this file, to\n" +
      "          include going backwards in the directory with '../'. To address this, ensure that user-controlled\n" +
      "          variables in file paths are sanitized. You may also consider using a utility method such as\n" +
      "          org.apache.commons.io.FilenameUtils.getName(...) to only retrieve the file name from the path.\n" +
      "          Details: https://sg.run/x9o0",
    location: 'src/main/java/vn/com/ivnd/leads/controller/FileIOController.java',
    severity: 'critical',
    status: 'open',
    branch: 'master'
  }
}
