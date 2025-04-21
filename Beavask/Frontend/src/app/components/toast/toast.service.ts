// toast.service.ts
import { Injectable } from '@angular/core';
import { BehaviorSubject, distinctUntilChanged } from 'rxjs';

export interface ToastData {
  id: number;
  title: string;
  message: string;
  time?: Date;
}

@Injectable({ providedIn: 'root' })
export class ToastService {
  private toasts: ToastData[] = [];
  private toastSubject = new BehaviorSubject<ToastData[]>([]);

  toast$ = this.toastSubject.asObservable().pipe(
    distinctUntilChanged((a, b) => JSON.stringify(a) === JSON.stringify(b))
  );
  
  show(toast: Omit<ToastData, 'id' | 'time'>) {
    const id = Date.now();
    const newToast: ToastData = {
      ...toast,
      id,
      time: new Date(),
    };
  
    this.toasts.push(newToast);
    this.toastSubject.next([...this.toasts]); 
  
    setTimeout(() => this.dismiss(id), 5000);
  }
  
  dismiss(id: number) {
    this.toasts = this.toasts.filter(t => t.id !== id);
    this.toastSubject.next([...this.toasts]);
  }
}
