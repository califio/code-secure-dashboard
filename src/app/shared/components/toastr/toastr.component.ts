import {Component} from '@angular/core';
import {ToastrType} from './toastr.model';
import {NgIcon} from '@ng-icons/core';
import {NgClass} from '@angular/common';

@Component({
  selector: 'app-toastr',
  standalone: true,
  imports: [
    NgIcon,
    NgClass
  ],
  templateUrl: './toastr.component.html',
  styleUrl: './toastr.component.scss'
})
export class ToastrComponent {
  hidden = true;
  icon = 'success';
  type: ToastrType = ToastrType.Success;
  message: string = '';
  constructor(
  ) {
  }

  protected mClass: Map<ToastrType, any> = new Map<ToastrType, any>([
    [ToastrType.Success, 'text-green-600'],
    [ToastrType.Warning, 'text-orange-600'],
    [ToastrType.Error, 'text-rose-500'],
  ]);

}
