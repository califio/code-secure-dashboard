import {inject} from "@angular/core";
import {ActivatedRoute, Params, Router} from "@angular/router";
import {map, switchMap} from "rxjs";

export function getPathParam(key: string) {
  return inject(ActivatedRoute).params.pipe(
    map((params) => {
      return params[key];
    }))
}

export function bindQueryParams(params: Params, obj: any) {
  for (let key in obj) {
    if (params[key]) {
      switch (typeof obj[key]) {
        case "object":
          if (Array.isArray(obj[key])) {
            if (Array.isArray(params[key])) {
              obj[key] = params[key]
            } else {
              obj[key] = [params[key]]
            }
          } else {
            obj[key] = params[key]
          }
          break;
        case "boolean":
          obj[key] = params[key] === "true" || params[key] === true;
          break;
        default:
          obj[key] = params[key];
      }
    }
  }
  return obj
}

export function updateQueryParams(router: Router, params: any) {
  router.navigate([], {
    queryParams: params,
    queryParamsHandling: "merge"
  }).then();
}
