import {Component, Input, OnInit} from '@angular/core';
import {NgClass} from '@angular/common';
@Component({
  selector: 'avatar',
  standalone: true,
  imports: [
    NgClass,
  ],
  templateUrl: './avatar.component.html',
  styleUrl: './avatar.component.scss'
})
export class AvatarComponent implements OnInit {
  _text: string | undefined;
  @Input() src: string | undefined;
  @Input() size: number = 28;
  @Input() shape: 'square' | 'circle' = 'circle';
  @Input() ngClass = '';
  @Input() set text(value: string | undefined) {
    if (value) {
      this._text = value.slice(0, 1).toUpperCase()
    }
  }

  constructor() {
  }

  ngOnInit(): void {
  }

  style() {
    return {
      'width': `${this.size}px`,
      'height': `${this.size}px`,
      'background-color': this._text ? colors[this._text!.charCodeAt(0) % colors.length] : 'none'
    }
  }
}

const colors = [
  '#E9F3FC',
  '#ECF4EE',
  '#F0F0F0',
  '#F4F0FF',
  '#FDF1DD',
  '#FCF1EF',
];
