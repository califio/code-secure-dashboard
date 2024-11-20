import {Component, Inject, OnInit, ViewContainerRef} from '@angular/core';
import {NavigationEnd, Router, RouterOutlet} from '@angular/router';
import {DOCUMENT} from "@angular/common";
import {filter, take} from "rxjs";
import {ModalService} from './core/modal/modal.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  title = 'Code Secure';

  constructor(
    @Inject(DOCUMENT) private _document: any,
    private _router: Router,
  ) {
  }

  ngOnInit(): void {
    this._router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        take(1)
      )
      .subscribe(() => {
        this.hide();
      });
  }

  /**
   * Hide the splash screen
   */
  hide(): void {
    this._document.body.classList.add('splash-screen-hidden');
  }
}
