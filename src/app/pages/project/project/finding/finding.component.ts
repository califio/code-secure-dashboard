import {Component, HostListener, OnInit} from '@angular/core';
import {ListFindingComponent} from '../../../../shared/components/finding/list-finding/list-finding.component';
import {NgIcon} from '@ng-icons/core';
import {DropdownComponent} from '../../../../shared/components/dropdown/dropdown.component';
import {StatusFindingComponent} from '../../../../shared/components/status-finding/status-finding.component';
import {Finding} from '../../../../shared/components/finding/finding.model';
import {FindingDetailComponent} from '../../../../shared/components/finding/finding-detail/finding-detail.component';
import {Router} from '@angular/router';

@Component({
  selector: 'app-finding',
  standalone: true,
  imports: [
    ListFindingComponent,
    NgIcon,
    DropdownComponent,
    StatusFindingComponent,
    FindingDetailComponent,
  ],
  templateUrl: './finding.component.html',
  styleUrl: './finding.component.scss'
})
export class FindingComponent implements OnInit{
  loading = true;
  finding: Finding | null = null;
  loadingFinding = false;
  showDetailFinding = false;
  constructor(
    private router: Router
  ) {
  }
  ngOnInit(): void {
    setTimeout(() => this.getFindings(), 500);
  }
  findings: Finding[] = [];
  private getFindings() {
    this.loading = false;
    this.findings = [
      {
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
      },
      {
        id: 2,
        name: 'Path Traversal at src/main/java/vn/com/ivnd/leads/controller/FileIOController.java',
        description: "Detected user input controlling a file path. An attacker could control the location of this file, to\n" +
          "          include going backwards in the directory with '../'. To address this, ensure that user-controlled\n" +
          "          variables in file paths are sanitized. You may also consider using a utility method such as\n" +
          "          org.apache.commons.io.FilenameUtils.getName(...) to only retrieve the file name from the path.\n" +
          "          Details: https://sg.run/x9o0",
        location: 'src/main/java/vn/com/ivnd/leads/controller/FileIOController.java',
        severity: 'critical',
        status: 'confirmed',
        branch: 'master'
      },
      {
        id: 3,
        name: 'Path Traversal at src/main/java/vn/com/ivnd/leads/controller/FileIOController.java',
        description: "Detected user input controlling a file path. An attacker could control the location of this file, to\n" +
          "          include going backwards in the directory with '../'. To address this, ensure that user-controlled\n" +
          "          variables in file paths are sanitized. You may also consider using a utility method such as\n" +
          "          org.apache.commons.io.FilenameUtils.getName(...) to only retrieve the file name from the path.\n" +
          "          Details: https://sg.run/x9o0",
        location: 'src/main/java/vn/com/ivnd/leads/controller/FileIOController.java',
        severity: 'critical',
        status: 'ignore',
        branch: 'master'
      },
      {
        id: 3,
        name: 'Path Traversal at src/main/java/vn/com/ivnd/leads/controller/FileIOController.java',
        description: "Detected user input controlling a file path. An attacker could control the location of this file, to\n" +
          "          include going backwards in the directory with '../'. To address this, ensure that user-controlled\n" +
          "          variables in file paths are sanitized. You may also consider using a utility method such as\n" +
          "          org.apache.commons.io.FilenameUtils.getName(...) to only retrieve the file name from the path.\n" +
          "          Details: https://sg.run/x9o0",
        location: 'src/main/java/vn/com/ivnd/leads/controller/FileIOController.java',
        severity: 'critical',
        status: 'fixed',
        branch: 'master'
      },
      {
        id: 3,
        name: 'Path Traversal at src/main/java/vn/com/ivnd/leads/controller/FileIOController.java',
        description: "Detected user input controlling a file path. An attacker could control the location of this file, to\n" +
          "          include going backwards in the directory with '../'. To address this, ensure that user-controlled\n" +
          "          variables in file paths are sanitized. You may also consider using a utility method such as\n" +
          "          org.apache.commons.io.FilenameUtils.getName(...) to only retrieve the file name from the path.\n" +
          "          Details: https://sg.run/x9o0",
        location: 'src/main/java/vn/com/ivnd/leads/controller/FileIOController.java',
        severity: 'critical',
        status: 'false_positive',
        branch: 'master'
      }
    ]
  }

  onOpenFinding(finding: Finding) {
    console.log(this.showDetailFinding);
    if (this.showDetailFinding) {
      this.router.navigate(['/finding', finding.id]).then();
    } else {
      this.loadingFinding = true;
      setTimeout(() => this.getFindingDetail(finding), 400);
      this.finding = null;
    }
  }

  closeFinding() {
    this.finding = null;
  }

  private getFindingDetail(finding: Finding) {
    this.loadingFinding = false;
    this.finding = finding;
  }

  @HostListener('window:resize', ['$event'])
  getScreenSize() {
    this.showDetailFinding = window.innerWidth < 1024;
  }
}
