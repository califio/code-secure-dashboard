import {Injectable} from '@angular/core';
import {MessageService} from 'primeng/api';

export interface ToastrPros {
  message: string
  title?: string
  duration?: number
}

@Injectable({
  providedIn: 'root'
})
export class ToastrService {

  constructor(private messageService: MessageService) {
  }

  success(pros: ToastrPros) {
    if (!pros.title) {
      pros.title = 'Success';
    }
    this.alert(pros, "success");
  }

  warning(pros: ToastrPros) {
    if (!pros.title) {
      pros.title = 'Warning';
    }
    this.alert(pros, "warn");
  }

  error(pros: ToastrPros) {
    if (!pros.title) {
      pros.title = 'Error';
    }
    this.alert(pros, "error");
  }

  alert(pros: ToastrPros, type: 'success' | 'error' | 'warn') {
    if (!pros.duration) {
      pros.duration = 5000;
    }
    this.messageService.add({
      severity: type,
      summary: pros.title,
      detail: pros.message,
      life: pros.duration
    });
  }
}
