import {Component, Inject, OnInit} from '@angular/core';
import {NavigationEnd, Router, RouterOutlet} from '@angular/router';
import {DOCUMENT} from "@angular/common";
import {filter, take} from "rxjs";
import {provideIcons} from '@ng-icons/core';
import {
  arrowPath,
  arrowRightOnRectangle,
  arrowsPointingOut,
  arrowTopRightOnSquare,
  asset,
  bars3,
  bold,
  bug,
  chartPie,
  checkBadge,
  chevronDoubleLeft,
  chevronDoubleRight,
  chevronRight,
  codeBracket,
  commandLine,
  cursorArrowRays,
  dotDashCircle,
  eye,
  eyeSlash,
  gitBranch,
  gitlab,
  gitMerge,
  handThumbDown,
  inventory,
  italic,
  jira,
  key,
  link,
  listBullet,
  listNumber,
  listTask,
  media,
  moon,
  npm,
  pencilSquare,
  pip,
  plugin,
  plusCircle,
  pom,
  quote,
  rectangleGroup,
  scale,
  scan,
  setting,
  share,
  shieldCheck,
  shieldExclamation,
  spin,
  success,
  sun,
  table,
  trash,
  userCircle,
  users,
  warning,
  x,
  xCircle,
} from './icons';
import {
  heroAdjustmentsHorizontal,
  heroCheck,
  heroCheckCircle,
  heroChevronDown,
  heroClock,
  heroEnvelope,
  heroFlag,
  heroKey,
  heroMagnifyingGlass,
  heroMinusCircle,
  heroPaperAirplane,
  heroPauseCircle,
  heroPencilSquare,
  heroScale
} from '@ng-icons/heroicons/outline';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  viewProviders: [provideIcons({
    users,
    asset,
    chartPie,
    bug,
    bold,
    quote,
    italic,
    link,
    media,
    listBullet,
    listNumber,
    listTask,
    table,
    eye,
    eyeSlash,
    heroFlag,
    cursorArrowRays,
    codeBracket,
    commandLine,
    arrowsPointingOut,
    handThumbDown,
    chevronRight,
    chevronDoubleRight,
    chevronDoubleLeft,
    arrowTopRightOnSquare,
    arrowRightOnRectangle,
    dotDashCircle,
    pencilSquare,
    key,
    inventory,
    gitBranch,
    gitMerge,
    gitlab,
    jira,
    pip,
    npm,
    pom,
    plugin,
    plusCircle,
    heroMagnifyingGlass,
    bars3,
    moon,
    arrowPath,
    rectangleGroup,
    scale,
    scan,
    setting,
    shieldExclamation,
    shieldCheck,
    share,
    spin,
    sun,
    checkBadge,
    warning,
    x,
    xCircle,
    trash,
    userCircle,
    success,
    heroCheckCircle, heroCheck, heroChevronDown, heroClock, heroMinusCircle, heroPauseCircle,
    heroPencilSquare, heroEnvelope, heroScale, heroKey, heroAdjustmentsHorizontal, heroPaperAirplane
  })]
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
