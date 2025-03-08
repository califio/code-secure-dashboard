import {Component, effect, input, signal} from '@angular/core';
import {PackageStatus} from '../../../../api/models/package-status';
import {transformValueNotNull} from '../../../../core/transform';
import {NgClass} from '@angular/common';
import {Message} from 'primeng/message';
import {Tag} from 'primeng/tag';
import {FindingStatus} from '../../../../api/models/finding-status';

@Component({
  selector: 'package-status',
  imports: [
    NgClass,
    Message,
    Tag
  ],
  templateUrl: './package-status.component.html',
  standalone: true,
  styleUrl: './package-status.component.scss'
})
export class PackageStatusComponent {
  styleClass = input<string>();
  status = input(PackageStatus.Open, {
    transform: (value: PackageStatus | null | undefined) => transformValueNotNull<PackageStatus>(value, PackageStatus.Open)
  });
  severity = signal<"success" | "secondary" | "info" | "warn" | undefined>('secondary');
  icon = signal('pi pi-circle');
  label = signal('');

  constructor() {
    effect(() => {
      const statusOption = this.mStatus.get(this.status());
      if (statusOption) {
        this.severity.set(statusOption.severity);
        this.icon.set(statusOption.icon);
        this.label.set(statusOption.label);
      }
    })
  }

  private mStatus = new Map<PackageStatus, any>([
    [
      PackageStatus.Open,
      {
        label: 'Open',
        status: PackageStatus.Open,
        icon: 'pi pi-circle',
        severity: 'info'
      }
    ],
    [
      PackageStatus.Ignore,
      {
        label: 'Accepted Risk',
        status: PackageStatus.Ignore,
        icon: 'pi pi-exclamation-triangle',
        severity: 'warn'
      }
    ],
    [
      PackageStatus.Fixed,
      {
        label: 'Fixed',
        status: FindingStatus.Fixed,
        icon: 'pi pi-verified',
        severity: 'success'
      }
    ],
  ]);
}
