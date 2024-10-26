import {AbstractControl, FormArray, FormControl, FormGroup, Validators} from "@angular/forms";

export class FormSection<
  T extends {
    [K in keyof T]:
    | FormSection<any>
    | FormField<any>
    | (FormSection<any> | FormField<any>)[];
  } = any
> {
  public fields: T;

  constructor(fields: T) {
    this.fields = fields;
  }
}

// And this will be a ViewModel for our FormControls
export class FormField<T> {
  public value: T;
  public validators?: Validators;

  constructor(value: T, validators?: Validators) {
    this.value = value;
    this.validators = validators;
  }
}

// We will use this type mapping to properly declare our form group
export type ControlsOf<T extends Record<string, any>> = {
  [K in keyof T]: T[K] extends (infer U)[]
    ? U extends Record<any, any>
      ? FormArray<AbstractControl<T[K]>>
      : FormControl<T[K]>
    : T[K] extends Record<any, any>
      ? FormGroup<ControlsOf<T[K]>>
      : FormControl<T[K]>;
};

// We will use this type mapping to type our form config
export type ConfigOf<T> = {
  [K in keyof T]: T[K] extends (infer U)[]
    ? U extends Record<any, any>
      ? FormSection<ConfigOf<U>>[]
      : FormField<T[K]>
    : T[K] extends Record<any, any>
      ? FormSection<ConfigOf<T[K]>>
      : FormField<T[K]>;
};
