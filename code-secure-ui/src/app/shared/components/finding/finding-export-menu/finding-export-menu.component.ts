import {Component, computed, EventEmitter, input, Input, Output} from '@angular/core';
import {Button} from 'primeng/button';
import {Menu} from 'primeng/menu';
import {MenuItem, PrimeIcons} from 'primeng/api';
import {ExportType} from '../../../../api/models/export-type';

@Component({
  selector: 'finding-export-menu',
  imports: [
    Button,
    Menu
  ],
  templateUrl: './finding-export-menu.component.html',
  standalone: true,
})
export class FindingExportMenuComponent {
  exportTypes = input([ExportType.Pdf, ExportType.Excel, ExportType.Json]);
  @Input()
  loading = false;
  @Output()
  onSelect = new EventEmitter<ExportType>();
  items = computed(() => {
    return this.exportTypes().map(type => this.mMenuItems.get(type)!);
  });
  mMenuItems = new Map<ExportType, MenuItem>([
    [
      ExportType.Pdf, {
        label: 'PDF',
        icon: PrimeIcons.FILE_PDF,
        command: () => {
          this.onSelect.emit(ExportType.Pdf);
        }
      }
    ],
    [
      ExportType.Excel, {
        label: 'EXCEL',
        icon: PrimeIcons.FILE_EXCEL,
        command: () => {
          this.onSelect.emit(ExportType.Excel);
        }
      }
    ],
    [
      ExportType.Json, {
        label: 'JSON',
        icon: PrimeIcons.FILE,
        command: () => {
          this.onSelect.emit(ExportType.Json);
        }
      }
    ]
  ]);

}
