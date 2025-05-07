import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Clipboard } from '@angular/cdk/clipboard';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-share-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './share.component.html',
  styleUrls: ['./share.component.scss']
})
export class ShareModalComponent {
  @Input() url = '';                
  @Output() close = new EventEmitter<void>();

  copied = false;

  constructor(private clip: Clipboard) {}

  copy() {
    this.clip.copy(this.url);
    this.copied = true;
    setTimeout(() => (this.copied = false), 1500);
  }

  
  hide() { this.close.emit(); }
}
