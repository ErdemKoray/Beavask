import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  constructor() { }

  errorhandle(error:HttpErrorResponse){
    if(error.status==404){
      console.log("api adresine ulaşılamıyor")
      
    }
    console.log(error)
  }
}
