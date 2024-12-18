import {
  ApplicationRef,
  ComponentFactoryResolver,
  ComponentRef,
  EmbeddedViewRef,
  Injectable,
  Injector
} from '@angular/core';
import {ToastrType} from './toastr.model';
import {ToastrComponent} from './toastr.component';

@Injectable({
  providedIn: 'root'
})
export class ToastrService {

  constructor(
    private appRef: ApplicationRef,
    private componentFactoryResolver: ComponentFactoryResolver,
    private injector: Injector
  ) {
  }

  success(message: string, duration: number = 5000) {
    this.notify(message, ToastrType.Success, 'success', duration);
  }

  warning(message: string, duration: number = 5000) {
    this.notify(message, ToastrType.Warning, 'warning', duration);
  }

  error(message: string, duration: number = 5000) {
    this.notify(message, ToastrType.Error, 'error', duration);
  }

  notify(message: string, type: ToastrType, icon: string, duration: number) {
    const factory = this.componentFactoryResolver.resolveComponentFactory(ToastrComponent);
    // Create the component and attach it to the application
    const componentRef: ComponentRef<ToastrComponent> = factory.create(this.injector);
    // Pass the data to the toast component instance
    componentRef.instance.type = type;
    componentRef.instance.icon = icon;
    componentRef.instance.message = message;
    componentRef.instance.hidden = false;
    // Attach component to the appRef so it's inside the ng component tree
    this.appRef.attachView(componentRef.hostView);
    // Get DOM element from component
    const domElem = (componentRef.hostView as EmbeddedViewRef<any>).rootNodes[0] as HTMLElement;
    // Append to the body or any other global container
    document.body.appendChild(domElem);
    // Set timeout to remove the component after duration
    if (duration > 0) {
      setTimeout(() => {
        componentRef.instance.hidden = false;
        this.appRef.detachView(componentRef.hostView);
        componentRef.destroy();
      }, duration);
    }
  }
}
