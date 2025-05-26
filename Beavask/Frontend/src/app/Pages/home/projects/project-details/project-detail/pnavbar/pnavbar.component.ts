import { NgClass } from '@angular/common';
import { Component, EventEmitter, OnInit, Output, output } from '@angular/core';
import {  RouterModule  } from '@angular/router';

@Component({
  selector: 'app-pnavbar',
  standalone: true,
  imports: [RouterModule,NgClass ],
  templateUrl: './pnavbar.component.html',
  styleUrl: './pnavbar.component.css'
})
export class PnavbarComponent implements OnInit {
@Output() activeBoard= new EventEmitter<boolean>(); 
activeClass=true;

ngOnInit(): void {

}

toggleChange(active:boolean){
  this.activeClass=active
  console.log(active)
  this.activeBoard.emit(active)
}

}
