import {
  Injectable,
  ComponentFactoryResolver,
  ApplicationRef,
  Injector,
  EmbeddedViewRef,
  Type
} from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  constructor(
    private componentFactoryResolver: ComponentFactoryResolver,
    private appRef: ApplicationRef,
    private injector: Injector
  ) {}

  /**
   * Hiển thị một component như một modal.
   * @param component Thành phần cần hiển thị.
   * @param data Dữ liệu truyền vào component.
   * @returns Đối tượng có `close` để đóng modal và `afterClosed` để bắt sự kiện.
   */
  open<T>(component: Type<T>, data?: Partial<T>): { close: () => void; afterClosed: Subject<any> } {
    // const componentRef = this.viewContainerRef.createComponent(component, {
    //   injector: this.injector
    // })

    // Tạo factory cho component
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(component);
    // Tạo một instance của component
    const componentRef = componentFactory.create(this.injector);
    // Truyền dữ liệu vào component (nếu có)
    /*
    if (data) {
      Object.assign(componentRef.instance, data);
    }
    */
    // Đưa component vào ứng dụng
    this.appRef.attachView(componentRef.hostView);
    const domElem = (componentRef.hostView as EmbeddedViewRef<any>)
      .rootNodes[0] as HTMLElement;
    document.body.appendChild(domElem);
    // Subject để bắt sự kiện sau khi modal đóng
    const afterClosed = new Subject<any>();
    // Hàm đóng modal
    const close = (result?: any) => {
      this.appRef.detachView(componentRef.hostView);
      componentRef.destroy();
      afterClosed.next(result); // Trả kết quả về sau khi modal đóng
      afterClosed.complete();
    };
    // Truyền hàm `close` vào component (nếu có)
    return { close, afterClosed };
  }
}
