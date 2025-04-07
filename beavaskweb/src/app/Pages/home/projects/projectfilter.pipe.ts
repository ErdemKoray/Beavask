import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'projectfilter',
  standalone: true
})
export class ProjectfilterPipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    return null;
  }

}
