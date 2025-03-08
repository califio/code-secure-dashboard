import {Component, computed, input} from '@angular/core';
import {PackageStatus} from '../../../../api/models/package-status';
import {transformValueNotNull} from '../../../../core/transform';
import {NgClass} from '@angular/common';
import {Message} from 'primeng/message';
import {Tag} from 'primeng/tag';

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
  status = input(PackageStatus.Open, {
    transform: (value: PackageStatus | null | undefined) => transformValueNotNull<PackageStatus>(value, PackageStatus.Open)
  });
  label = computed(() => {
    return this.statusLabel(this.status());
  });

  severity(): "success" | "secondary" | "info" | "warn" | undefined {
    if (this.status() == PackageStatus.Open) {
      return 'info';
    }
    if (this.status() == PackageStatus.Ignore) {
      return 'warn';
    }
    if (this.status() == PackageStatus.Fixed) {
      return 'success';
    }
    return 'secondary';
  }

  private statusLabel(status: PackageStatus): string {
    if (status == PackageStatus.Open) {
      return 'Open';
    }
    if (status == PackageStatus.Ignore) {
      return 'Accepted Risk'
    }
    if (status == PackageStatus.Fixed) {
      return 'Fixed'
    }
    return 'Unknown'
  }
}
