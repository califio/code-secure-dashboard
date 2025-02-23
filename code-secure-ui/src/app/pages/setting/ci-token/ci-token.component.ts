import {Component} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {TimeagoModule} from 'ngx-timeago';
import {CiTokens} from '../../../api/models/ci-tokens';
import {ToastrService} from '../../../shared/services/toastr.service';
import {TokenService} from '../../../api/services/token.service';
import {ConfirmDialog} from 'primeng/confirmdialog';
import {ButtonDirective} from 'primeng/button';
import {ConfirmationService} from 'primeng/api';
import {IconField} from "primeng/iconfield";
import {InputIcon} from "primeng/inputicon";
import {InputText} from "primeng/inputtext";
import {TableModule} from 'primeng/table';
import {Checkbox} from 'primeng/checkbox';
import {Password} from 'primeng/password';
import {Tooltip} from 'primeng/tooltip';
import {Dialog} from 'primeng/dialog';

@Component({
  selector: 'app-ci-token',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    TimeagoModule,
    FormsModule,
    ConfirmDialog,
    ButtonDirective,
    IconField,
    InputIcon,
    InputText,
    TableModule,
    Checkbox,
    Password,
    Tooltip,
    Dialog,
  ],
  templateUrl: './ci-token.component.html',
  styleUrl: './ci-token.component.scss',
  providers: [ConfirmationService]
})
export class CiTokenComponent {
  loading = false;
  tokens: CiTokens[] = [];
  showCreateTokenDialog = false;
  tokenName = '';

  constructor(
    private tokenService: TokenService,
    private toastr: ToastrService,
    private confirmationService: ConfirmationService
  ) {
    this.tokenService.getCiTokens().subscribe(tokens => {
      this.tokens = tokens;
    })
  }

  createCIToken() {
    if (this.tokenName.trim()) {
      this.tokenService.createCiToken({
        body: {
          name: this.tokenName.trim()
        }
      }).subscribe(token => {
        this.tokens = [token].concat(...this.tokens);
        this.toastr.success({
          message: 'Create token success'
        });
        this.showCreateTokenDialog = false;
      })
    } else {
      this.toastr.warning({
        message: 'Token name is blank'
      });
    }
  }

  onDeleteToken(tokenId: string) {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete this token?',
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
        severity: 'danger',
        label: 'Delete',
      },
      accept: () => {
        this.tokenService.deleteCiToken({
          id: tokenId
        }).subscribe(success => {
          if (success) {
            this.toastr.success({
              message: 'Delete success'
            });
            this.tokens = this.tokens.filter(token => token.id != tokenId);
          }
        })
      }
    });
  }

  private deleteTokenId: string | null = null;

  onSearch() {

  }

  closeDialog() {
    this.showCreateTokenDialog = false;
    this.tokenName = '';
  }
}
