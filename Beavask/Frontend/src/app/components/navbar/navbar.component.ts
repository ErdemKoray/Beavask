import { Component, OnInit, HostListener } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, NavigationEnd, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TeamService } from '../../common/services/team/team.service';
import { Team } from '../../common/model/team.model';
import { ThemeService } from '../../common/services/theme/theme.service';
import { Profile } from '../../common/services/profile/profile.model';
import { AuthprofileService } from '../../common/services/profile/authprofile.service';
import { TranslateModule } from '@ngx-translate/core';
import { LangService } from '../../common/services/lang/lang.service';
import { CreatprojectService } from '../../common/services/projects/creatproject.service';
import { cProject } from '../../common/services/projects/creatproject.model';
import { ToastService } from '../toast/toast.service';


@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    TranslateModule
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {


  ngOnInit() {
    this.getTeams(); 
    this.getUserInfo();
    localStorage.getItem(this.themeKey)=== 'dark' ? this.darkMode = true : this.darkMode = false;
    
  }
    constructor(
      private router: Router,
      private fb:FormBuilder,
      private teamService: TeamService,
      private themeService: ThemeService,
      private profileService: AuthprofileService,
      private langService: LangService,
      private createPApi:CreatprojectService,
      private toastService:ToastService
    ) {
      this.currentLang = this.langService.getCurrentLanguage();
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
  projectName: new FormControl('', Validators.required)
});

  
 

  
  isProjectDropdownOpen = false;
  isDropdownOpen = false;
  isModalDropdownOpen = false;
  isCreateProjectOpen = false;
  isCreateProjectLoad = false;

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
    if (
      this.dropdownOpen &&
      !target.closest('.bv-cp-dropdown-content') &&
      !target.closest('[data-dropdown="lang"]')
    ) {
      this.dropdownOpen = false;
    }
  }
  userInfo: Profile | null = null;
  avatarUrl: string = '';


  dropdownOpen = false;
  currentLang: string;


  togglelDropdown(): void {
    this.dropdownOpen = !this.dropdownOpen;
  }

  changeLang(lang: string): void {
    this.langService.setLanguage(lang);
    this.currentLang = lang;
    this.dropdownOpen = false; 
  }
  getUserInfo(){
    this.profileService.whoami().subscribe((response: Profile) => {
      if (response) {
        this.userInfo = response;
        if(response.avatarUrl=="default_avatar_url"){

          this.avatarUrl =  this.generateInitialsAvatar(response.firstName + ' ' + response.lastName, 100);
        }else
        {
          this.avatarUrl=response.avatarUrl
        }
      }
    });
  }
  closeAllDropdowns() {
    this.isProjectDropdownOpen = false;
    this.isDropdownOpen = false;
    this.isModalDropdownOpen = false;
    this.isCreateProjectOpen = false;
    this.isTeamDropdownOpen= false;
    this.isTaskDropdownOpen = false;
    this.isProfileDropdownOpen = false;
    this.dropdownOpen = false;
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

  gotomytask() { 
    this.router.navigate(['/mytasks']);
   }

  toggleCreateProject() {
    this.isCreateProjectOpen = !this.isCreateProjectOpen;
  }
  gotomyactivities() {
    this.router.navigate(['/myactivities']);
  }
  createProjectGithub() {

    if (this.form.valid) {
      const cproject: cProject | any= {
        repoUrl: this.form.value.projectName
      };
      console.log(this.form.value);

      this.createPApi.create(cproject).subscribe({
        next:(response) => {
        console.log(response);    
        this.toastService.show({
          title:'success',
          message:'Project added successfully'
        });
      },error:(err)=>{
        this.toastService.show({
          title:'error',
          message:'The link entered was not found'
        });
      }
    
    });
      this.form.reset();
      this.toggleCreateProject();
      this.isCreateProjectLoad = false;
    }
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

  logout(){
    localStorage.removeItem('jwtToken');
    this.router.navigate(['/login']);
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


  generateInitialsAvatar(name: string, size: number = 100): string {
    const canvas = document.createElement('canvas');
    canvas.width = size;
    canvas.height = size;
  
    const ctx = canvas.getContext('2d');
    if (!ctx) return '';
  
    ctx.fillStyle = '#4a5568'; 
    ctx.fillRect(0, 0, size, size);
  
    ctx.fillStyle = '#ffffff'; 
    ctx.font = `${size * 0.4}px Segoe UI, Roboto, sans-serif`;
    ctx.textAlign = 'center';
    ctx.textBaseline = 'middle';
  
    const initials = name.split(' ')
      .map(word => word.charAt(0))
      .join('')
      .substring(0, 2) // Sadece 2 harf alıyoruz
      .toUpperCase();
  
    ctx.fillText(initials, size / 2, size / 2);
  
    // Base64 URL döner
    return canvas.toDataURL();
  }
}
