import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-timeline',
  standalone: true,
  imports: [CommonModule,],
  templateUrl: './timeline.component.html',
  styleUrl: './timeline.component.css'
})
export class TimelineComponent implements OnInit {
  timelineItems = [
    {
      code: 'BEAV-8',
      title: 'ldkşsgsş',
      startDate: new Date('2024-04-01'),
      endDate: new Date('2024-04-06')
    },
    {
      code: 'BEAV-9',
      title: 'hkykjhg',
      startDate: new Date('2024-04-05'),
      endDate: new Date('2024-04-20')
    }
  ];

  dateRange: Date[] = [];

  ngOnInit(): void {
    this.generateDateRange();
  }

  generateDateRange(): void {
    const start = new Date('2024-03-27');
    const end = new Date('2024-04-30');
    let current = new Date(start);
    while (current <= end) {
      this.dateRange.push(new Date(current));
      current.setDate(current.getDate() + 1);
    }
  }

  getStartOffset(start: Date): number {
    const firstDate = this.dateRange[0];
    const diff = (new Date(start).getTime() - firstDate.getTime()) / (1000 * 60 * 60 * 24);
    return (diff / this.dateRange.length) * 100;
  }

  getDurationWidth(start: Date, end: Date): number {
    const diff = (new Date(end).getTime() - new Date(start).getTime()) / (1000 * 60 * 60 * 24);
    return (diff / this.dateRange.length) * 100;
  }
}
