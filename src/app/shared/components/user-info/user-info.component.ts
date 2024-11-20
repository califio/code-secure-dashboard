import {Component, Input} from '@angular/core';
import {AvatarComponent} from "../ui/avatar/avatar.component";

@Component({
  selector: 'user-info',
  standalone: true,
    imports: [
        AvatarComponent
    ],
  templateUrl: './user-info.component.html',
  styleUrl: './user-info.component.scss'
})
export class UserInfoComponent {
  @Input()
  avatarSize = 36;
  @Input()
  avatar: string | undefined | null;
  @Input()
  username: string = '';
  @Input()
  fullName: string | undefined | null;
  @Input()
  email: string | undefined | null;

}
