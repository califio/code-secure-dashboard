import {Component, EventEmitter, model, Output} from '@angular/core';
import {Menu} from 'primeng/menu';
import {FindingStatusLabelComponent} from '../finding-status-label/finding-status-label.component';
import {Button} from 'primeng/button';
import {OverlayBadge} from 'primeng/overlaybadge';
import {MenuItem} from 'primeng/api';
import {getFindingStatusOptions} from '../finding-status';
import {FindingStatus} from "../../../../api/models/finding-status";

@Component({
    selector: 'finding-status-menu',
    imports: [
        Menu,
        FindingStatusLabelComponent,
        Button,
        OverlayBadge
    ],
    templateUrl: './finding-status-menu.component.html',
    standalone: true,
})
export class FindingStatusMenuComponent {
    badge = model(0);
    @Output()
    onSelect = new EventEmitter<FindingStatus>();

    menuItems: MenuItem[] = getFindingStatusOptions().map(item => {
        return {
            status: item.status,
            label: item.label
        }
    });

}
