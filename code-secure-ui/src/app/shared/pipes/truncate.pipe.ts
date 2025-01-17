import {Pipe, PipeTransform} from '@angular/core';

@Pipe({standalone: true, name: 'truncate'})
export class TruncatePipe implements PipeTransform {
  transform(value: string, limit: number = 20): string {
    return value.length < limit
      ? value
      : value.slice(0, limit) + '...';
  }
}
