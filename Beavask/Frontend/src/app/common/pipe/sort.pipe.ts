import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sort',
  standalone: true 
})
export class SortPipe implements PipeTransform {

  transform(items: any[] | null, sortKey: string | null, sortDirection: 'asc' | 'desc' = 'asc'): any[] | null {
   
    if (!items || items.length === 0 || !sortKey) {
      return items;
    }


    const sortedItems = [...items];

    sortedItems.sort((a, b) => {
    
      const valueA = this.getValue(a, sortKey);
      const valueB = this.getValue(b, sortKey);

     
      let comparison = 0;

    
      if (typeof valueA === 'number' && typeof valueB === 'number') {
         comparison = valueA - valueB;
      }
      
      else if (typeof valueA === 'string' && typeof valueB === 'string') {
          comparison = valueA.toLowerCase().localeCompare(valueB.toLowerCase());
      }
     
      else {
      
          const stringA = String(valueA ?? '').toLowerCase();
          const stringB = String(valueB ?? '').toLowerCase();
          comparison = stringA.localeCompare(stringB);
      }


      return sortDirection === 'asc' ? comparison : -comparison;
    });

    return sortedItems;
  }

 
  private getValue(obj: any, key: string): any {
    if (!obj || !key) return undefined;

    const keys = key.split('.');
    let value = obj;

    for (const k of keys) {
      if (value === null || value === undefined) {
        return undefined; 
      }
      value = value[k];
    }

    return value;
  }
}