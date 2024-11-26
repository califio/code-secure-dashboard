import {Component, EventEmitter, input, OnInit, Output} from '@angular/core';
import {NgIcon} from "@ng-icons/core";

@Component({
  selector: 'pagination',
  standalone: true,
  imports: [
    NgIcon
  ],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.scss'
})
export class PaginationComponent implements OnInit {
  currentPage = input<number>(0);
  totalPage = input<number>(0);
  @Output()
  pageChange = new EventEmitter<number>();

  ngOnInit(): void {
    this.updateDisableState();
  }
  disable = {
    prev: true,
    next: false
  }

  endPage() {
    if (this.totalPage() > 1 && this.currentPage() < this.totalPage()) {
      this.pageChange.emit(this.totalPage())
      this.disable.next = true;
      this.disable.prev = false;
    }
    this.updateDisableState();
  }

  nextPage() {
    if (this.currentPage() < this.totalPage()) {
      this.pageChange.emit(this.currentPage() + 1);
    }
    this.updateDisableState();
  }

  firstPage() {
    if (this.totalPage() > 1 && this.currentPage() > 1) {
      this.pageChange.emit(1);
    }
    this.updateDisableState();
  }

  prevPage() {
    if (this.currentPage() > 1) {
      this.pageChange.emit(this.currentPage() - 1);
    }
    this.updateDisableState();
  }

  private updateDisableState() {
    this.disable.prev = this.currentPage() == 1;
    this.disable.next = this.currentPage() == this.totalPage();
  }
}
