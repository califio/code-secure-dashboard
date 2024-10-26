import {Injectable} from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormControl,
  FormGroup,
} from '@angular/forms';
import {ConfigOf, ControlsOf, FormField, FormSection} from "./forms.model";


@Injectable({
  providedIn: 'root',
})
export class FormService {
  public group<T extends Record<string, any>>(
    section: FormSection<ConfigOf<T>>
  ): FormGroup<ControlsOf<T>> {
    // we need to create an empty FormGroup first, so we can add FormControls recursively
    const group = new FormGroup({});

    Object.keys(section.fields).forEach((key: any) => {
      const field = section.fields[key];
      if (Array.isArray(field)) {
        group.addControl(key, this.array(field));
      } else {
        if (field instanceof FormSection) {
          group.addControl(key, this.group(field));
        } else {
          group.addControl(key, new FormControl(field.value, field.validators));
        }
      }
    });

    // and we need to cast the group to the correct type before returning
    return group as unknown as FormGroup<ControlsOf<T>>;
  }

  public array<T extends Record<string, any>>(
    fields: unknown[]
  ): FormArray<AbstractControl<T>> {
    const array: FormArray<AbstractControl<any>> = new FormArray(
      []
    ) as unknown as FormArray<AbstractControl<T>>;

    fields.forEach((field) => {
      if (field instanceof FormSection) {
        array.push(this.group(field));
      } else {
        const {value, validators} = field as FormField<T>;
        array.push(new FormControl(value, validators));
      }
    });

    return array as unknown as FormArray<AbstractControl<T>>;
  }
}
