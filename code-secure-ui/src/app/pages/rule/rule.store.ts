import {computed, Injectable, signal} from '@angular/core';
import {RuleInfo} from '../../api/models/rule-info';
import {RuleFilter} from '../../api/models/rule-filter';
import {Scanners} from '../../api/models/scanners';

@Injectable({
  providedIn: 'root'
})
export class RuleStore {
  rules = signal<RuleInfo[]>([]);
  rule: RuleInfo | null = null;
  scanners = signal<Scanners[]>([]);
  filter: RuleFilter = {
    page: 1,
    size: 20,
    name: undefined,
    status: [],
    confidence: undefined,
    scannerId: []
  };
  isDesktop = signal(true);
  loading = signal<boolean>(false);
  isSync = signal<boolean>(false);
  // paginator
  currentPage = signal(1);
  pageSize = signal(20);
  totalRecords = signal(0);
  firstRecord = computed(() => {
    return (this.currentPage() - 1) * this.pageSize();
  });

  constructor() {
  }
}
