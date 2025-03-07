import {Component, input} from '@angular/core';
import {TableModule} from 'primeng/table';
import {Packages} from '../../../../api/models/packages';
import {PackageTypeComponent} from '../package-type/package-type.component';
import {RiskLevelIconComponent} from '../../risk-level-icon/risk-level-icon.component';

@Component({
  selector: 'list-package',
  imports: [
    TableModule,
    PackageTypeComponent,
    RiskLevelIconComponent
  ],
  templateUrl: './list-package.component.html',
  standalone: true,
})
export class ListPackageComponent {
  packages = input([], {transform: transformPackages});

  packageName(pkg: Packages): string {
    if (pkg.group) {
      return `${pkg.group}.${pkg.name}@${pkg.version}`;
    }
    return `${pkg.name}@${pkg.version}`;
  }

  typedPackage(pkg: Packages): Packages {
    return pkg;
  }
}

function transformPackages(input: Packages[] | undefined | null): Packages[] {
  if (!input) {
    return [];
  }
  return input;
}
