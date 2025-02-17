import {Pipe, PipeTransform} from '@angular/core';

@Pipe({standalone: true, name: 'firstChar'})
export class FirstCharPipe implements PipeTransform {
  transform(value: string | null | undefined, defaultValue: string = ''): string {
    if (!value || value.trim().length === 0) {
      return defaultValue;
    }
    return value.charAt(0);
  }
}
