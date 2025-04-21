import { ChangeDetectorRef, Component, NgZone, OnInit } from '@angular/core';
import { ToastData, ToastService } from './toast.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  imports:[CommonModule],
  styleUrls: ['./toast.component.css'],
  standalone: true
})
export class ToastComponent implements OnInit {
  toasts: ToastData[] = [];

  constructor(
    private toastService: ToastService,
    private ngZone: NgZone,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.toastService.toast$.subscribe(data => {
      this.ngZone.run(() => {
        this.toasts = [...data]; 
        this.cdr.detectChanges(); 
      });
    });
  }


  }

