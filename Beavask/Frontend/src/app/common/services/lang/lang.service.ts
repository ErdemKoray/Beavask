import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class LangService {

  private langKey = 'language'; 
  private currentLanguage: string = 'tr'; // Varsayılan Türkçe

  constructor(private translate: TranslateService) {
    this.translate.setDefaultLang(this.currentLanguage);
    this.loadInitialLanguage();
  }

  setLanguage(lang: string): void {
    this.currentLanguage = lang;
    this.translate.use(lang);
    localStorage.setItem(this.langKey, lang);
  }

  loadInitialLanguage(): void {
    const savedLang = localStorage.getItem(this.langKey);
    if (savedLang) {
      this.currentLanguage = savedLang;
      this.translate.use(this.currentLanguage);
    } else {
      this.translate.use(this.currentLanguage); // Eğer localStorage'da yoksa default kullan
    }
  }

  getCurrentLanguage(): string {
    return this.currentLanguage;
  }
}
