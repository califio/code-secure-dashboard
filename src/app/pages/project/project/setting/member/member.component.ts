import {Component, OnDestroy, OnInit} from '@angular/core';
import {ComingSoonComponent} from '../../../../../shared/components/ui/coming-soon/coming-soon.component';
import {AvatarComponent} from '../../../../../shared/components/ui/avatar/avatar.component';
import {FormsModule} from '@angular/forms';
import {LoadingTableComponent} from '../../../../../shared/components/ui/loading-table/loading-table.component';
import {NgIcon} from '@ng-icons/core';
import {PaginationComponent} from '../../../../../shared/components/ui/pagination/pagination.component';
import {TimeagoModule} from 'ngx-timeago';
import {MemberStore} from './member.store';
import {bindQueryParams, updateQueryParams} from '../../../../../core/router';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectStore} from '../../project-store';
import {ProjectService} from '../../../../../api/services/project.service';
import {finalize, Subject, switchMap, takeUntil, tap} from 'rxjs';
import {ConfirmPopupComponent} from '../../../../../shared/components/ui/confirm-popup/confirm-popup.component';
import {ProjectUser} from '../../../../../api/models/project-user';
import {ToastrService} from '../../../../../shared/components/toastr/toastr.service';
import {UpdateMemberPopupComponent} from './update-member-popup/update-member-popup.component';
import {ButtonDirective} from '../../../../../shared/directives/button.directive';
import {ModalService} from '../../../../../core/modal/modal.service';
import {AddMemberPopupComponent} from './add-member-popup/add-member-popup.component';
import {TooltipDirective} from '../../../../../shared/components/ui/tooltip/tooltip.directive';

@Component({
  selector: 'app-member',
  standalone: true,
  imports: [
    ComingSoonComponent,
    AvatarComponent,
    FormsModule,
    LoadingTableComponent,
    NgIcon,
    PaginationComponent,
    TimeagoModule,
    ConfirmPopupComponent,
    UpdateMemberPopupComponent,
    ButtonDirective,
    AddMemberPopupComponent,
    TooltipDirective
  ],
  templateUrl: './member.component.html',
  styleUrl: './member.component.scss'
})
export class MemberComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject();
  constructor(
    public store: MemberStore,
    private projectStore: ProjectStore,
    private projectService: ProjectService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
    private modalService: ModalService
  ) {
  }

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  ngOnInit(): void {
    this.route.queryParams.pipe(
      switchMap(params => {
        bindQueryParams(params, this.store.filter);
        return this.getProjectUsers();
      }),
      takeUntil(this.destroy$)
    ).subscribe();
  }

  onSearchChange() {
    updateQueryParams(this.router, this.store.filter);
  }

  onChangePage(page: number) {
    this.store.filter.page = page;
    updateQueryParams(this.router, this.store.filter);
  }

  private getProjectUsers() {
    this.store.loading.set(true)
    return this.projectService.getProjectUsers({
      slug: this.projectStore.slug(),
      body: this.store.filter
    }).pipe(
      finalize(() => this.store.loading.set(false)),
      tap(response => {
        this.store.members.set(response.items!);
        this.store.currentPage.set(response.currentPage!);
        this.store.totalPage.set(response.pageCount!);
        this.store.count.set(response.count!);
      })
    )
  }

  deleteMember() {
    this.projectService.deleteProjectMember({
      slug: this.projectStore.slug(),
      userId: this.member.userId ?? ''
    }).subscribe(() => {
      const members = this.store.members().filter(value => value.userId != this.member?.userId);
      this.store.members.set(members);
      this.toastr.success('Delete member success!');
      this.store.showConfirmDeleteMemberPopup.set(false);
    })
  }

  showConfirmDeleteMemberPopup(member: ProjectUser) {
    this.member = member;
    this.store.showConfirmDeleteMemberPopup.set(true);
  }

  showUpdateMemberPopup(member: ProjectUser) {
    this.member = member;
    this.store.showUpdateMemberPopup.set(true);
  }
  member: ProjectUser = {};

  showAddMemberPopup() {
    this.store.showAddMemberPopup.set(true);
  }
}
