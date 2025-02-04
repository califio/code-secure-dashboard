import {
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild
} from '@angular/core';
import {MarkdownComponent, provideMarkdown} from 'ngx-markdown';
import {ControlValueAccessor, FormControl, ReactiveFormsModule} from '@angular/forms';
import '@github/markdown-toolbar-element'
import {NgIcon, provideIcons} from '@ng-icons/core';
import {NgClass} from '@angular/common';
import {
  bold,
  codeBracket,
  eye,
  italic,
  link,
  listBullet,
  listNumber,
  listTask,
  media,
  quote,
  table
} from '../../../icons';
import {heroPencilSquare} from '@ng-icons/heroicons/outline';

@Component({
  selector: 'markdown-editor',
  standalone: true,
  imports: [
    MarkdownComponent,
    NgIcon,
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './markdown-editor.component.html',
  styleUrl: './markdown-editor.component.scss',
  providers: [
    provideMarkdown()
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class MarkdownEditorComponent implements OnInit, ControlValueAccessor {
  controlId: string = `md-editor-${Math.floor(100000 * Math.random())}`;
  @ViewChild('textarea')
  textarea!: ElementRef;
  control = new FormControl<string>('');

  @Input() previewClass = "";

  @Input() set data(value: string) {
    this.onChange(value);
    this.control.setValue(value);
  }

  @Output() dataChange = new EventEmitter<string | null>();
  @Input() editable = true;
  @Input() row: number = 2;
  @Input() preview = false;
  @Output()
  previewChange = new EventEmitter<boolean>();
  @Input() loading = false;
  focus = false;
  iconSize = "15";

  constructor() {
  }

  private onChange = (value: any) => {
  };
  private onTouched = () => {
  };

  ngOnInit(): void {
  }

  writeValue(value: string): void {
    this.control.setValue(value);
  }

  registerOnChange(onChange: any): void {
    this.onChange = onChange;
  }

  registerOnTouched(onTouched: any): void {
    this.onTouched = onTouched;
  }

  setDisabledState(disabled: boolean) {
  }

  onEdit() {
    this.preview = false;
    this.previewChange.emit(false);
    this.focus = true;
  }

  onBlur() {
    this.onChange(this.control.value);
    this.dataChange.emit(this.control.value);
    this.focus = false;
  }

  onPreview() {
    if (this.control.value) {
      this.preview = true;
      this.previewChange.emit(true);
    }
  }

  insertTable() {
    this.insertText("\n| header | header |\n" +
      "| ------ | ------ |\n" +
      "| cell | cell |\n" +
      "| cell | cell |\n");
  }

  private insertText(text: string) {
    var startPos = this.textarea.nativeElement.selectionStart;
    this.textarea.nativeElement.focus();

    const content = this.textarea.nativeElement.value.substr(0, this.textarea.nativeElement.selectionStart)
      + text + this.textarea.nativeElement.value.substr(
        this.textarea.nativeElement.selectionStart, this.textarea.nativeElement.value.length);
    //this.control.setValue(content);
    this.control.setValue(content);
    this.textarea.nativeElement.selectionStart = startPos + text.length;
    this.textarea.nativeElement.selectionEnd = startPos + text.length;
  }
}

