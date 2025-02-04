import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {ScannerLabelComponent} from '../scanner-label/scanner-label.component';
import {FormsModule} from '@angular/forms';
import {NgIf} from '@angular/common';
import {ClickOutsideDirective} from '../../directives/click-outside.directive';
import {RouterLink} from '@angular/router';
import {TicketDropdownStore} from './ticket-dropdown.store';
import {TicketType} from '../../../api/models/ticket-type';
import {Tickets} from '../../../api/models/tickets';
import {ConfirmPopupComponent} from '../../ui/confirm-popup/confirm-popup.component';
import {IntegrationService} from '../../../api/services/integration.service';

@Component({
  selector: 'ticket-dropdown',
  standalone: true,
  imports: [
    NgIcon,
    ScannerLabelComponent,
    FormsModule,
    NgIf,
    ClickOutsideDirective,
    RouterLink,
    ConfirmPopupComponent
  ],
  templateUrl: './ticket-dropdown.component.html',
  styleUrl: './ticket-dropdown.component.scss'
})
export class TicketDropdownComponent implements OnInit {
  @Input()
  ticket: Tickets | undefined = undefined;
  @Input()
  loading = false;
  @Output()
  onTicket = new EventEmitter<TicketType>();
  @Output()
  onDelete = new EventEmitter<Tickets>();
  hidden = true;
  issueTracker = new Map<TicketType, boolean>();

  constructor(
    public store: TicketDropdownStore,
    private integrationService: IntegrationService
  ) {

  }

  ngOnInit(): void {
    this.store.loading.set(true);
    this.integrationService.getTicketTrackers().subscribe(response => {
      response.forEach(item => {
        this.issueTracker.set(item.type!, item.active!);
      })
    })
  }

  createTicket(type: TicketType) {
    if (!this.loading) {
      this.onTicket.emit(type);
    }
  }

  ticketIcon(ticket: Tickets) {
    if (ticket.type == TicketType.Jira) {
      return 'jira';
    }
    return 'heroFlag';
  }

  protected readonly TicketType = TicketType;
  showConfirmPopup = false;

  deleteTicket() {
    this.showConfirmPopup = true;
  }

  confirmDelete() {
    this.onDelete.emit(this.ticket);
    this.showConfirmPopup = false;
  }
}
