import {Component} from '@angular/core';
import {LoadingTableComponent} from '../../../shared/components/ui/loading-table/loading-table.component';
import {NgIcon} from '@ng-icons/core';
import {PaginationComponent} from '../../../shared/components/ui/pagination/pagination.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {TimeagoModule} from 'ngx-timeago';
import {CiTokens} from '../../../api/models/ci-tokens';
import {CiTokenService} from '../../../api/services/ci-token.service';
import {ToastrService} from '../../../shared/components/toastr/toastr.service';
import {ConfirmPopupComponent} from '../../../shared/components/ui/confirm-popup/confirm-popup.component';
import {finalize} from 'rxjs';
import {TooltipDirective} from '../../../shared/components/ui/tooltip/tooltip.directive';

@Component({
  selector: 'app-ci-token',
  standalone: true,
  imports: [
    LoadingTableComponent,
    NgIcon,
    PaginationComponent,
    ReactiveFormsModule,
    TimeagoModule,
    FormsModule,
    ConfirmPopupComponent,
    TooltipDirective
  ],
  templateUrl: './ci-token.component.html',
  styleUrl: './ci-token.component.scss'
})
export class CiTokenComponent {
  loading = false;
  tokens: CiTokens[] = [];
  enableCreateTokenForm = false;
  tokenName = '';
  showConfirmPopup = false;

  constructor(
    private ciTokenService: CiTokenService,
    private toastr: ToastrService,
  ) {
    this.ciTokenService.getCiTokens().subscribe(tokens => {
      this.tokens = tokens;
    })
  }

  createCIToken() {
    if (this.tokenName.trim()) {
      this.ciTokenService.createCiToken({
        body: {
          name: this.tokenName.trim()
        }
      }).subscribe(token => {
        this.tokens = [token].concat(...this.tokens);
        this.toastr.success('create success');
        this.enableCreateTokenForm = false;
      })
    } else {
      this.toastr.warning('token name is blank');
    }
  }

  copyTokenValue(value: string) {
    navigator.clipboard.writeText(value);
    //this.clipboard.writeText(value);
    this.toastr.success('copy CI token success');
  }

  confirmDelete() {
    if (this.deleteTokenId) {
      this.ciTokenService.deleteCiToken({
        id: this.deleteTokenId
      }).pipe(
        finalize(() => {
          this.showConfirmPopup = false;
        })
      ).subscribe(success => {
        if (success) {
          this.toastr.success('Delete success');
          this.tokens = this.tokens.filter(value => value.id != this.deleteTokenId);
        }
      })
    }
  }

  onDeleteToken(tokenId: string) {
    this.deleteTokenId = tokenId;
    this.showConfirmPopup = true;
  }

  private deleteTokenId: string | null = null;
}
