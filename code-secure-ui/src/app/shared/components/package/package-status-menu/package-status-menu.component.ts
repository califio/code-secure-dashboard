import {Component, effect, EventEmitter, input, Output, signal} from '@angular/core';
import {Menu} from 'primeng/menu';
import {PackageStatus} from '../../../../api/models';
import {Tag} from 'primeng/tag';
import {transformValueNotNull} from '../../../../core/transform';
import {NgClass} from '@angular/common';
import {ButtonDirective} from 'primeng/button';

@Component({
  selector: 'package-status-menu',
  imports: [
    Menu,
    Tag,
    NgClass,
    ButtonDirective
  ],
  templateUrl: './package-status-menu.component.html',
  standalone: true,
})
export class PackageStatusMenuComponent {
  styleClass = input('px-4 py-2')
  status = input(PackageStatus.Open, {
    transform: (value: PackageStatus | null | undefined) => transformValueNotNull(value, PackageStatus.Open)
  });
  @Output()
  onChange = new EventEmitter<PackageStatus>();
  selectedOption = signal<any>(null);

  constructor() {
    effect(() => {
      this.selectedOption.set(this.menuItems.find(item => item.status == this.status()));
    });
  }

  onChangeStatus(option: any) {
    this.selectedOption.set(option);
    this.onChange.emit(option.status);
  }

  menuItems: any[] = [
    {
      label: 'Open',
      status: PackageStatus.Open,
      severity: 'info'
    },
    {
      label: 'Accepted Risk',
      status: PackageStatus.Ignore,
      severity: 'warn'
    },
    {
      label: 'Fixed',
      status: PackageStatus.Fixed,
      severity: 'success'
    }
  ];
}
