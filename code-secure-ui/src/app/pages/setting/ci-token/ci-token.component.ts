import {Component} from '@angular/core';
import {LoadingTableComponent} from '../../../shared/ui/loading-table/loading-table.component';
import {NgIcon} from '@ng-icons/core';
import {PaginationComponent} from '../../../shared/ui/pagination/pagination.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {TimeagoModule} from 'ngx-timeago';
import {CiTokens} from '../../../api/models/ci-tokens';
import {ToastrService} from '../../../shared/components/toastr/toastr.service';
import {ConfirmPopupComponent} from '../../../shared/ui/confirm-popup/confirm-popup.component';
import {finalize} from 'rxjs';
import {TooltipDirective} from '../../../shared/ui/tooltip/tooltip.directive';
import {ButtonDirective} from '../../../shared/ui/button/button.directive';
import {TokenService} from '../../../api/services/token.service';

interface CiTokenView {
  token: CiTokens
  hidden: boolean
}

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
    TooltipDirective,
    ButtonDirective
  ],
  templateUrl: './ci-token.component.html',
  styleUrl: './ci-token.component.scss'
})
export class CiTokenComponent {
  loading = false;
  tokens: CiTokenView[] = [];
  enableCreateTokenForm = false;
  tokenName = '';
  showConfirmPopup = false;

  constructor(
    private tokenService: TokenService,
    private toastr: ToastrService,
  ) {

    this.tokenService.getCiTokens().subscribe(tokens => {
      this.tokens = tokens.map(value => <CiTokenView>{
        hidden: true,
        token: value
      });
    })
  }

  createCIToken() {
    if (this.tokenName.trim()) {
      this.tokenService.createCiToken({
        body: {
          name: this.tokenName.trim()
        }
      }).subscribe(token => {
        this.tokens = [<CiTokenView>{hidden: true, token: token}].concat(...this.tokens);
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
      this.tokenService.deleteCiToken({
        id: this.deleteTokenId
      }).pipe(
        finalize(() => {
          this.showConfirmPopup = false;
        })
      ).subscribe(success => {
        if (success) {
          this.toastr.success('Delete success');
          this.tokens = this.tokens.filter(value => value.token.id != this.deleteTokenId);
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
