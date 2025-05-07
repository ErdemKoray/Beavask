// inline-edit.component.ts
import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'inline-edit',
  standalone: true,
  imports: [FormsModule,CommonModule],
  template: `
    <span *ngIf="!editMode"
          (click)="enableEdit()"
          class="view-text">{{ value }}</span>

    <input *ngIf="editMode"
           [(ngModel)]="value"
           class="view-text-focus"
           (blur)="save()"
           (keydown.enter)="save()" />
  `,
  styles: [`
    .view-text        { cursor:pointer; padding:2px 4px; }
    .view-text-focus  { border:2px solid transparent; outline:none; }
    .view-text-focus:focus {
      border-color: var(--card-xsw);
      background:     var(--card-xsw);
      color:#f1f1ec; border-radius:4px;
    }
  `]
})
export class InlineEditComponent {
  @Input() value  = '';
  @Output() valueChange = new EventEmitter<string>();
  editMode = false;

  enableEdit() {
    this.editMode = true;
    setTimeout(() => document.getElementById('in-edit')?.focus());
  }
  
  save()       { this.editMode = false; this.valueChange.emit(this.value.trim()); }
}
