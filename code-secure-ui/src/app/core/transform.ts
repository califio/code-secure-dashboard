export function arrayNotNull<T>(input: T[] | undefined | null): T[] {
  if (!input) {
    return [] as T[];
  }
  return input;
}

export function toArray<T>(input: T[] | T | undefined | null): T[] {
  if (!input) {
    return [];
  }
  if (Array.isArray(input)) {
    return input;
  } else {
    return [input];
  }
}


export function valueNotNull<T>(input: T | undefined | null, defaultValue: T): T {
  if (!input) {
    return defaultValue;
  }
  return input;
}

export function stringNotNull(input: string | undefined | null): string {
  if (!input) {
    return '';
  }
  return input;
}
