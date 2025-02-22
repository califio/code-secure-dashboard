import {
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  ElementRef,
  EventEmitter,
  forwardRef,
  Input,
  Output,
  ViewChild
} from '@angular/core';
import {MarkdownComponent, provideMarkdown} from 'ngx-markdown';
import {ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR, ReactiveFormsModule} from '@angular/forms';
import '@github/markdown-toolbar-element'
import {NgIcon} from '@ng-icons/core';
import {NgClass} from '@angular/common';
import {Textarea} from 'primeng/textarea';

export const CUSTOM_CONROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => MarkdownEditorComponent),
  multi: true,
};

@Component({
  selector: 'markdown-editor',
  standalone: true,
  imports: [
    MarkdownComponent,
    NgIcon,
    ReactiveFormsModule,
    NgClass,
    Textarea,
    FormsModule,
  ],
  templateUrl: './markdown-editor.component.html',
  styleUrl: './markdown-editor.component.scss',
  providers: [
    provideMarkdown(),
    CUSTOM_CONROL_VALUE_ACCESSOR
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class MarkdownEditorComponent implements ControlValueAccessor {
  controlId: string = `md-editor-${Math.floor(100000 * Math.random())}`;
  @ViewChild('textarea')
  textarea!: ElementRef;
  data = '';
  @Input() previewClass = "";
  @Input() editable = true;
  @Input() row: number = 2;
  @Input() preview = false;
  @Output()
  previewChange = new EventEmitter<boolean>();
  @Input() loading = false;
  iconSize = "15";
  disabled = false;
  private onChange = (value: any) => {
  };
  private onTouched = () => {
  };

  writeValue(value: string): void {
    this.data = value;
  }

  registerOnChange(onChange: any): void {
    this.onChange = onChange;
  }

  registerOnTouched(onTouched: any): void {
    this.onTouched = onTouched;
  }

  setDisabledState(disabled: boolean) {
    this.disabled = disabled;
  }

  onEdit() {
    this.preview = false;
    this.previewChange.emit(false);
  }

  onPreview() {
    if (this.data) {
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
    this.data = content;
    this.textarea.nativeElement.selectionStart = startPos + text.length;
    this.textarea.nativeElement.selectionEnd = startPos + text.length;
    this.onChange(this.data);
  }

  changeValue($event: any) {
    this.onChange($event);
  }
}

