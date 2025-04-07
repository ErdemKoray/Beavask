import {  Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FooterComponent } from "../../components/footer/footer.component";
import { ToastComponent } from "../../components/toast/toast.component";
import {  RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [NavbarComponent, FooterComponent, ToastComponent,RouterOutlet],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent{
  @ViewChild('hambcontainer') hamb!:ElementRef;
  constructor(private rendered:Renderer2
  ) {}

  tooglemenu(){
    if(this.hamb){
   const hasClass = this.hamb.nativeElement.classList.contains('bv-hamb-container-o');
   if(hasClass){
    console.log("fdjl")
     this.rendered.removeClass(this.hamb.nativeElement, 'bv-hamb-container-o');
     this.rendered.addClass(this.hamb.nativeElement, '.bv-hamb-container');
    }else{
      this.rendered.addClass(this.hamb.nativeElement, 'bv-hamb-container-o');
      this.rendered.removeClass(this.hamb.nativeElement, '.bv-hamb-container');
    }
  }
}
}
