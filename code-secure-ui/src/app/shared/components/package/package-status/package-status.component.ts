import {Component, computed, input} from '@angular/core';
import {PackageStatus} from '../../../../api/models/package-status';
import {valueNotNull} from '../../../../core/transform';
import {NgClass} from '@angular/common';
import {Message} from 'primeng/message';

@Component({
  selector: 'package-status',
  imports: [
    NgClass,
    Message
  ],
  templateUrl: './package-status.component.html',
  standalone: true,
  styleUrl: './package-status.component.scss'
})
export class PackageStatusComponent {
  status = input(PackageStatus.Open, {
    transform: (value: PackageStatus | null | undefined) => valueNotNull<PackageStatus>(value, PackageStatus.Open)
  });
  label = computed(() => {
    return this.statusLabel(this.status());
  });

  severity(): string {
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
      return 'Need to Fix';
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
