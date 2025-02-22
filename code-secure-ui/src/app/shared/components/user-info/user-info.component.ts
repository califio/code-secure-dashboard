import {Component, Input} from '@angular/core';
import {Avatar} from 'primeng/avatar';
import {FirstCharPipe} from '../../pipes/firstchar.pipe';
import {UpperCasePipe} from '@angular/common';

@Component({
  selector: 'user-info',
  standalone: true,
  imports: [
    Avatar,
    FirstCharPipe,
    UpperCasePipe
  ],
  templateUrl: './user-info.component.html',
})
export class UserInfoComponent {
  @Input()
  avatarShape: 'square' | 'circle' = 'square';
  @Input()
  avatarSize: 'normal' | 'large' | 'xlarge' = "large";
  @Input()
  avatar: string | undefined | null;
  @Input()
  username: string | undefined | null = '';
  @Input()
  fullName: string | undefined | null;
  @Input()
  email: string | undefined | null;
}
