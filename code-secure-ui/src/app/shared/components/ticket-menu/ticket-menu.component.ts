import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {FormsModule} from '@angular/forms';
import {RouterLink} from '@angular/router';
import {TicketType} from '../../../api/models/ticket-type';
import {Tickets} from '../../../api/models/tickets';
import {IntegrationService} from '../../../api/services/integration.service';
import {ButtonDirective} from 'primeng/button';
import {DropdownModule} from 'primeng/dropdown';
import {ConfirmationService, MenuItem} from 'primeng/api';
import {ConfirmDialog} from 'primeng/confirmdialog';
import {finalize} from 'rxjs';
import {Menu} from 'primeng/menu';

@Component({
  selector: 'ticket-menu',
  standalone: true,
  imports: [
    NgIcon,
    FormsModule,
    RouterLink,
    ButtonDirective,
    DropdownModule,
    ConfirmDialog,
    Menu
  ],
  templateUrl: './ticket-menu.component.html',
  providers: [ConfirmationService]
})
export class TicketMenuComponent implements OnInit {
  @Input()
  ticket: Tickets | undefined = undefined;
  @Input()
  loading = false;
  @Output()
  onTicket = new EventEmitter<TicketType>();
  @Output()
  onDelete = new EventEmitter<Tickets>();
  ticketTrackerOptions: MenuItem[] = [];

  constructor(
    private integrationService: IntegrationService,
    private confirmationService: ConfirmationService
  ) {

  }

  ngOnInit(): void {
    this.loading = true;
    this.integrationService.getTicketTrackers().pipe(
      finalize(() => this.loading = false)
    ).subscribe(response => {
      this.ticketTrackerOptions = response.filter(item => item.active).map(item => {
        return {
          label: item.type!.toUpperCase(),
          icon: this.ticketIcon(item.type!),
          type: item.type!,
        };
      });
    })
  }

  createTicket(type: TicketType) {
    if (!this.loading) {
      this.onTicket.emit(type);
    }
  }

  ticketIcon(ticketType: TicketType) {
    if (ticketType == TicketType.Jira) {
      return 'jira';
    }
    return 'heroFlag';
  }

  deleteTicket() {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete this ticket?',
      header: 'Confirmation',
      closable: true,
      closeOnEscape: true,
      icon: 'pi pi-exclamation-triangle',
      rejectButtonProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptButtonProps: {
        label: 'Delete',
      },
      accept: () => {
        this.onDelete.emit(this.ticket);
      }
    });
  }
}
