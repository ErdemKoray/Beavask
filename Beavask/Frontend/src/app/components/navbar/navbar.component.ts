import { Component, ElementRef, Renderer2, ViewChild, OnInit, HostListener } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, NavigationEnd, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TeamService } from '../../common/services/team/team.service';
import { Team } from '../../common/model/team.model';
import { ThemeService } from '../../common/services/theme/theme.service';
@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule,RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {


  ngOnInit() {
    this.getTeams(); 

    localStorage.getItem(this.themeKey)=== 'dark' ? this.darkMode = true : this.darkMode = false;
  }
    constructor(
      private router: Router,
      private fb:FormBuilder,
      private teamService: TeamService,
      private themeService: ThemeService,
    ) {
      this.router.events.subscribe(event => {
        if (event instanceof NavigationEnd) {
          this.closeAllDropdowns();
        }
      });
    }
  person = [
    {id:1, name: 'Profile', icon: 'fa fa-user' },
    {id:2, name: 'Settings', icon: 'fa fa-cog' },
    {id:3, name: 'Logout', icon: 'fa fa-sign-out' }
  ];

  projects = [
    { name: 'Project 1', icon: 'fa fa-folder' },
    { name: 'Project 2', icon: 'fa fa-folder' },
    { name: 'Project 3', icon: 'fa fa-folder' }
  ];

  darkMode = false;
  teams: Team[] = []; 
  private themeKey = 'theme';


  toggleTheme(){
    this.themeService.toggleTheme();
    this.darkMode = !this.darkMode;
  }


form = new FormGroup({
  projectName: new FormControl('', Validators.required),
  projectKey: new FormControl('', [Validators.required, Validators.maxLength(5)]),
  projectDescription: new FormControl(''),
  startDate: new FormControl('', Validators.required),
  endDate: new FormControl('', Validators.required),
  lead: new FormControl('', Validators.required),
  type: new FormControl('Scrum', Validators.required),
  priority: new FormControl('Medium', Validators.required),
  status: new FormControl('Draft', Validators.required),
  projectFile: new FormControl(null)
});

  
 

  
  isProjectDropdownOpen = false;
  isDropdownOpen = false;
  isModalDropdownOpen = false;
  isCreateProjectOpen = false;
  isTeamDropdownOpen = false;
  isTaskDropdownOpen = false;
  isProfileDropdownOpen= false;

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
    if (
      this.isTeamDropdownOpen &&
      !target.closest('.bv-cp-dropdown-content') &&
      !target.closest('[data-dropdown="teamdropdown"]')
    ) {
      this.isTeamDropdownOpen = false;
    }
    if (
      this.isTaskDropdownOpen &&
      !target.closest('.bv-cp-dropdown-content') &&
      !target.closest('[data-dropdown="taskdropdown"]')
    ) {
      this.isTaskDropdownOpen = false;
    } if (
      this.isProfileDropdownOpen &&
      !target.closest('.bv-cp-dropdown-content') &&
      !target.closest('[data-dropdown="profiledropdown"]')
    ) {
      this.isProfileDropdownOpen = false;
    }
  }
  
  

  closeAllDropdowns() {
    this.isProjectDropdownOpen = false;
    this.isDropdownOpen = false;
    this.isModalDropdownOpen = false;
    this.isCreateProjectOpen = false;
    this.isTeamDropdownOpen= false;
    this.isTaskDropdownOpen = false;
    this.isProfileDropdownOpen = false;
  }

  toggleDropdown(type: 'project' | 'dropdown' | 'modal' | 'create' | 'teamdropdown' | 'taskdropdown' | 'profile') {
    if (type === 'project') {
      this.isProjectDropdownOpen = !this.isProjectDropdownOpen;
    } else if (type === 'dropdown') {
      this.isDropdownOpen = !this.isDropdownOpen;
    } else if (type === 'modal') {
      this.isModalDropdownOpen = !this.isModalDropdownOpen;
    }
     else if (type === 'teamdropdown') {
      this.isTeamDropdownOpen = !this.isTeamDropdownOpen;
    }
    else if (type === 'taskdropdown') {
        this.isTaskDropdownOpen = !this.isTaskDropdownOpen;
    }  else if (type === 'profile') {
      this.isProfileDropdownOpen = !this.isProfileDropdownOpen;
  }
  }

  gotomytask() {  }

  toggleCreateProject() {
    this.isCreateProjectOpen = !this.isCreateProjectOpen;
  }
  gotomyactivities() {
    this.router.navigate(['/myactivities']);
  }
 
  choiseoption(option: any) {
    const optionsElement = document.querySelector('#options');
    if (optionsElement) {
      optionsElement.innerHTML = option;
    }
    this.toggleDropdown('modal');
  }

  gotohome() {
    this.router.navigate(['/']);
  }

  gotoaccount() {
    this.router.navigate(['/login']);
  }

  gotomyproject() {
    this.router.navigate(['/projects']);
  }

  gototeam(id: number) {
    this.router.navigate(['/teams', id]);  
  }

  

  createnewproject() {
    if (this.form.valid) {
      console.log(this.form.value);
      this.form.reset();
      this.toggleCreateProject();
    }
  }



  getTeams() {
    this.teamService.getAll().subscribe(response => {
      if (response?.data) {
        this.teams = response.data.slice(0,3).map((team: any) => ({
          id: team.id,
          name: team.title 
        }));
      }
    });
  }
}
