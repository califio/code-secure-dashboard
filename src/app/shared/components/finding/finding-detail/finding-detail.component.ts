import {Component, EventEmitter, HostListener, Input, input, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {StatusFindingComponent} from '../../status-finding/status-finding.component';
import {Finding} from '../finding.model';
import {RouterLink} from '@angular/router';
import {NgClass} from '@angular/common';

@Component({
  selector: 'finding-detail',
  standalone: true,
  imports: [
    NgIcon,
    StatusFindingComponent,
    RouterLink,
    NgClass
  ],
  templateUrl: './finding-detail.component.html',
  styleUrl: './finding-detail.component.scss'
})
export class FindingDetailComponent {
  @Output()
  onClose = new EventEmitter();
  finding = input<Finding>();
  @Input()
  minimal = true;
  closeFinding() {
    this.onClose.emit();
  }
  @HostListener('window:resize', ['$event'])
  getScreenSize() {
    this.minimal = window.innerWidth < 1024;
  }
}
