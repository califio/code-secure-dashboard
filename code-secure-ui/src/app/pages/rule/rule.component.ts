import {Component, OnDestroy, OnInit} from '@angular/core';
import {ComingSoonComponent} from '../../shared/ui/coming-soon/coming-soon.component';
import {Panel} from 'primeng/panel';
import {IconField} from 'primeng/iconfield';
import {SortByComponent} from '../../shared/ui/sort-by/sort-by.component';
import {InputIcon} from 'primeng/inputicon';
import {InputText} from 'primeng/inputtext';
import {Button, ButtonDirective} from 'primeng/button';
import {TableModule} from 'primeng/table';
import {RuleStore} from './rule.store';
import {RuleService} from '../../api/services/rule.service';
import {Paginator, PaginatorState} from 'primeng/paginator';
import {LayoutService} from '../../layout/layout.service';
import {finalize, Subject, switchMap, takeUntil} from 'rxjs';
import {bindQueryParams, updateQueryParams} from '../../core/router';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {Checkbox} from 'primeng/checkbox';
import {MeterGroup} from 'primeng/metergroup';
import {RuleInfo} from '../../api/models/rule-info';
import {ScannerLabelComponent} from '../../shared/components/scanner-label/scanner-label.component';
import {RuleConfidence, RuleStatus} from '../../api/models';
import {Chip} from 'primeng/chip';
import {ToggleButton, ToggleButtonChangeEvent} from 'primeng/togglebutton';
import {FormsModule} from '@angular/forms';
import {ToastrService} from '../../shared/services/toastr.service';
import {
  FindingScannerFilterComponent
} from '../../shared/components/finding/finding-scanner-filter/finding-scanner-filter.component';
import {FloatLabel} from 'primeng/floatlabel';
import {MultiSelect, MultiSelectChangeEvent} from 'primeng/multiselect';
import {Dialog} from 'primeng/dialog';
import {Select} from 'primeng/select';
import {ToggleSwitch} from 'primeng/toggleswitch';
import {SelectButton} from 'primeng/selectbutton';
import {rule} from 'postcss';

@Component({
  selector: 'app-rule',
  standalone: true,
  imports: [
    ComingSoonComponent,
    Panel,
    IconField,
    SortByComponent,
    InputIcon,
    InputText,
    ButtonDirective,
    TableModule,
    Paginator,
    Checkbox,
    MeterGroup,
    ScannerLabelComponent,
    Chip,
    ToggleButton,
    FormsModule,
    FindingScannerFilterComponent,
    FloatLabel,
    MultiSelect,
    RouterLink,
    Dialog,
    Button,
    Select,
    ToggleSwitch,
    SelectButton,
  ],
  templateUrl: './rule.component.html',
  styleUrl: './rule.component.scss'
})
export class RuleComponent implements OnInit, OnDestroy {
  showUpdateRuleDialog = false;
  loadingUpdateRule = false;
  statusOptions = [RuleStatus.Enable, RuleStatus.Disable];
  confidenceOptions = [RuleConfidence.High, RuleConfidence.Medium, RuleConfidence.Low, RuleConfidence.Unknown];
  private destroy$ = new Subject();
  constructor(
    public store: RuleStore,
    private ruleService: RuleService,
    private layoutService: LayoutService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.store.isDesktop.set(this.layoutService.isDesktop());
  }

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  ngOnInit(): void {
    this.ruleService.getRuleScanners().subscribe(scanners => {
      this.store.scanners.set(scanners);
    })
    this.route.queryParams.pipe(
      switchMap(params => {
        this.store.loading.set(true);
        bindQueryParams(params, this.store.filter);
        this.store.currentPage.set(this.store.filter.page!);
        this.store.pageSize.set(this.store.filter.size!);
        return this.ruleService.getRuleInfo({
          body: this.store.filter
        }).pipe(
          finalize(() => this.store.loading.set(false))
        )
      }),
      takeUntil(this.destroy$)
    ).subscribe(response => {
      this.store.rules.set(response.items!);
      this.store.totalRecords.set(response.count!);
    });
  }

  onSync() {
    this.store.isSync.set(true);
    this.ruleService.syncRules().pipe(
      finalize(() => this.store.isSync.set(false))
    ).subscribe();
  }

  onPageChange($event: PaginatorState) {
    this.store.filter.page = $event.page! + 1;
    this.store.filter.size = $event.rows;
    updateQueryParams(this.router, this.store.filter);
  }

  ruleConfidence(rule: RuleInfo) {
    return [
      { label: 'Incorrect', color: '#F44336', value: rule.incorrectFinding },
      { label: 'Correct', color: '#4CAF50', value: rule.correctFinding },
      { label: 'Uncertain', color: '#fbbf24', value: rule.uncertainFinding },
    ];
  }
  ruleConfidenceTotal(rule: RuleInfo) {
    return rule.uncertainFinding! + rule.incorrectFinding! + rule.correctFinding!;
  }

  protected readonly RuleStatus = RuleStatus;

  onSearch() {
    updateQueryParams(this.router, this.store.filter);
  }

  onChangeScanners($event: string[]) {
    this.store.filter.scannerId = $event;
    updateQueryParams(this.router, this.store.filter);
  }

  onFilterStatus($event: MultiSelectChangeEvent) {
    this.store.filter.status = $event.value;
    updateQueryParams(this.router, this.store.filter);
  }

  onFilterConfidence($event: MultiSelectChangeEvent) {
    this.store.filter.confidence = $event.value;
    updateQueryParams(this.router, this.store.filter);
  }

  protected readonly rule = rule;

  closeDialog() {
    this.store.rule = null;
    this.showUpdateRuleDialog = false;
  }

  onUpdateRule() {
    this.loadingUpdateRule = true;
    this.ruleService.updateRule({
      body: {
        ruleId: this.store.rule!.id!,
        scannerId: this.store.rule!.scannerId!,
        status: this.store.rule?.status,
        confidence: this.store.rule?.confidence
      }
    }).pipe(
      finalize(() => this.loadingUpdateRule = false)
    ).subscribe(() => {
      this.toastr.success({
        message: 'Update rule success!'
      });
    });
    this.showUpdateRuleDialog = false;
  }

  showRuleDialog(rule: RuleInfo) {
    this.store.rule = rule;
    this.showUpdateRuleDialog = true;
  }

  onChangeConfidence($event: any) {
    this.store.rule!.confidence = $event;
  }

  onChangeRuleStatus($event: any) {
    this.store.rule!.status = $event;
  }
}
