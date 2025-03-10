import {Component, effect, EventEmitter, input, Output, signal} from '@angular/core';
import {Menu} from 'primeng/menu';
import {PackageStatus} from '../../../../api/models';
import {transformValueNotNull} from '../../../../core/transform';
import {PackageStatusComponent} from '../package-status/package-status.component';

@Component({
  selector: 'package-status-menu',
  imports: [
    Menu,
    PackageStatusComponent
  ],
  templateUrl: './package-status-menu.component.html',
  standalone: true,
})
export class PackageStatusMenuComponent {
  styleClass = input('')
  status = input(PackageStatus.Open, {
    transform: (value: PackageStatus | null | undefined) => transformValueNotNull(value, PackageStatus.Open)
  });
  @Output()
  onChange = new EventEmitter<PackageStatus>();
  selectedOption = signal<PackageStatus>(PackageStatus.Open);

  constructor() {
    effect(() => {
      this.selectedOption.set(this.status());
    });
  }

  onChangeStatus(status: PackageStatus) {
    this.selectedOption.set(status);
    this.onChange.emit(status);
  }

  menuItems: any[] = [
    PackageStatus.Open,
    PackageStatus.Ignore,
    PackageStatus.Fixed
  ];
}
