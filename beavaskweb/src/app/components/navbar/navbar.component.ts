import { Component, ElementRef, Renderer2, ViewChild, OnInit, HostListener } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent   {
  person = [
    { name: 'Profile', icon: 'fa fa-user' },
    { name: 'Settings', icon: 'fa fa-cog' },
    { name: 'Logout', icon: 'fa fa-sign-out' }
  ];

  // Dropdown & Modal flags
  isProjectDropdownOpen = false;
  isDropdownOpen = false;
  isModalDropdownOpen = false;
  isCreateProjectOpen = false;

  constructor(
    private router: Router,
  ) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.closeAllDropdowns();
      }
    });
  }


  @HostListener('document:keydown.escape', ['$event'])
  onEscape(event: KeyboardEvent) {
    this.closeAllDropdowns();
  }

  @HostListener('document:click', ['$event'])
  handleClickOutside(event: MouseEvent) {
    const target = event.target as HTMLElement;
  

    if (
      this.isProjectDropdownOpen &&
      !target.closest('.bv-dropdown-content') &&
      !target.closest('[data-dropdown="project"]')
    ) {
      this.isProjectDropdownOpen = false;
    }
  
    if (
      this.isDropdownOpen &&
      !target.closest('.bv-dropdown-content') &&
      !target.closest('[data-dropdown="dropdown"]')
    ) {
      this.isDropdownOpen = false;
    }
  
    if (
      this.isModalDropdownOpen &&
      !target.closest('.bv-cp-dropdown-content') &&
      !target.closest('[data-dropdown="modal"]')
    ) {
      this.isModalDropdownOpen = false;
    }
    if (
      this.isCreateProjectOpen &&
      !target.closest('.bv-cp-container') &&
      !target.closest('[data-dropdown="create"]')
    ) {
      this.isCreateProjectOpen = false;
    }
  }
  
  

  closeAllDropdowns() {
    this.isProjectDropdownOpen = false;
    this.isDropdownOpen = false;
    this.isModalDropdownOpen = false;
    this.isCreateProjectOpen = false;
  }

  toggleDropdown(type: 'project' | 'dropdown' | 'modal') {
    if (type === 'project') {
      this.isProjectDropdownOpen = !this.isProjectDropdownOpen;
    } else if (type === 'dropdown') {
      this.isDropdownOpen = !this.isDropdownOpen;
    } else if (type === 'modal') {
      this.isModalDropdownOpen = !this.isModalDropdownOpen;
    }
  }

  /** Create project modal toggle */
  toggleCreateProject() {
    this.isCreateProjectOpen = !this.isCreateProjectOpen;
  }

  /** Seçim dropdown içerik güncelleme */
  choiseoption(option: any) {
    const optionsElement = document.querySelector('#options');
    if (optionsElement) {
      optionsElement.innerHTML = option;
    }
    this.toggleDropdown('modal');
  }

  /** Navigation */
  gotohome() {
    this.router.navigate(['/']);
  }

  gotoaccount() {
    this.router.navigate(['/login']);
  }

  gotomyproject() {
    this.router.navigate(['/projects']);
  }

  /** Reactive form tanımı */
  form = new FormGroup({
    projectName: new FormControl(''),
    projectexp: new FormControl(''),
    projecttitle: new FormControl(''),
    projectfile: new FormControl('')
  });

  createnewproject() {
    if (this.form.valid) {
      console.log(this.form.value);
      this.form.reset();
      this.toggleCreateProject();
    }
  }
}
