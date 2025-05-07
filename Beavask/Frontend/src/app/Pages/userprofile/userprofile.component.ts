import { CommonModule } from '@angular/common';
import { Component, HostListener, OnInit } from '@angular/core';
import { AuthprofileService } from '../../common/services/profile/authprofile.service';
import { Profile } from '../../common/services/profile/profile.model';
import { LangService } from '../../common/services/lang/lang.service';
import { TranslateModule } from '@ngx-translate/core';

interface UserProfile {
  id: number;
  fullName: string;
  email: string;
  profileImageUrl: string;
  role: string;
  totalProjects: number;
  totalTasks: number;
}

interface ActivityItem {
  id: number;
  title: string;
  type: 'created' | 'updated' | 'commented';
  target: string;
  timestamp: Date;
  project: string;
}
@Component({
  selector: 'app-userprofile',
  standalone: true,
  imports: [CommonModule,TranslateModule],
  templateUrl: './userprofile.component.html',
  styleUrl: './userprofile.component.css'
})

export class UserprofileComponent implements OnInit {
  user!: UserProfile;


  userProfileView=false;
  activities: ActivityItem[] = [];
  constructor(private profileService:AuthprofileService,private langService: LangService ) {
  
   }

  ngOnInit() {
    const currentLang = this.langService.getCurrentLanguage();
    this.langService.setLanguage(currentLang);
    this.getUserInfo(); 
    this.user = {
      id: 1,
      fullName: 'Muhammed Emin Aldaş',
      email: 'emin@example.com',
      profileImageUrl: 'https://placehold.co/150x150',
      role: 'Frontend Developer',
      totalProjects: 5,
      totalTasks: 42
    };

    this.activities = [
      {
        id: 101,
        title: 'Created task: Login UI',
        type: 'created',
        target: 'WorkItems',
        timestamp: new Date('2024-04-01T10:30:00'),
        project: 'Beavask'
      },
      {
        id: 102,
        title: 'Updated project settings',
        type: 'updated',
        target: 'Projects',
        timestamp: new Date('2024-04-02T14:00:00'),
        project: 'Beavask'
      },
      {
        id: 103,
        title: 'Commented on Epic Overview',
        type: 'commented',
        target: 'Comments',
        timestamp: new Date('2024-04-03T09:15:00'),
        project: 'Beavask'
      }
    ];
  }

  userInfo: Profile | null = null;
  avatarUrl: string = '';


  getUserInfo(){
      this.profileService.whoami().subscribe((response: Profile) => {
        if (response) {
          this.userInfo = response;
          this.avatarUrl = response.avatarUrl ;
        }
      });
    }

    @HostListener("document:keydown.escape",['$event'])
    @HostListener("document:click",['$event'])
    handleClickOutside(event:MouseEvent){
      const target = event.target as HTMLElement;

      if (
        this.userProfileView &&
        !target.closest('.bv-v-c') &&
        !target.closest('[data-modal="profile"]')
      ) {
        this.userProfileView = false;
      }
    }
    toggleProfileModal(): void {
      this.userProfileView = !this.userProfileView;
    }
  
    
}
