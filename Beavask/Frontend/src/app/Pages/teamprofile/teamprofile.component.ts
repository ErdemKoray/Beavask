import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Route, Router, RouterLink } from '@angular/router';
import { TeamService } from '../../common/services/team/team.service';
import { Team } from '../../common/model/team.model';

@Component({
  selector: 'app-teamprofile',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule],
  templateUrl: './teamprofile.component.html',
  styleUrl: './teamprofile.component.css'
})
export class TeamprofileComponent implements OnInit {
  constructor(private router: Router,private route: ActivatedRoute, private http: HttpClient,private teamService:TeamService) {}
  teamId: number = 0; 
  team: Team | null = null;
  isLoading: boolean = true;
  errorMessage: string | null = null;
  headerImagePath= 'https://placehold.co/1200x300?text=Frontend+Team'
  ngOnInit(): void {
    
    this.route.params.subscribe(params => {
      this.teamId = +params['id']; // 'id' parametresi

      // API'den veriyi al
      this.getTeamDetail();
    });
  }
  
  getTeamDetail(): void {
    this.teamService.getById(this.teamId).subscribe({
      next: (response) => {
        this.team = response.data;  // Başarılı veri geldiğinde
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = 'Team not found! Redirecting to the home page...';
    
        setTimeout(() => {
          this.router.navigate(['/']); 
        }, 2000); 
      }
    });
  }

  members = [
    {
      firstName: 'Koray',
      lastName: 'Erdem',
      role: 'Frontend Developer',
      isActive: true,
      profileImagePath: 'https://placehold.co/100x100'
    },
    {
      firstName: 'Muhammed Emin',
      lastName: 'Aldaş',
      role: 'UI Designer',
      isActive: true,
      profileImagePath: 'https://placehold.co/100x100'
    }
  ];

  tasks = [
    {
      title: 'Login Sayfası Tasarımı',
      assignedUserName: 'Koray Erdem',
      status: 'In Progress'
    },
    {
      title: 'Header Bileşeni',
      assignedUserName: 'Muhammed Emin Aldaş',
      status: 'Done'
    }
  ];

  activities = [
    {
      userName: 'Koray Erdem',
      description: 'Yeni görev oluşturdu: Login Sayfası Tasarımı',
      createdAt: new Date()
    },
    {
      userName: 'Muhammed Emin Aldaş',
      description: 'Görev tamamlandı: Header Bileşeni',
      createdAt: new Date()
    }
  ];


  gotoprofile() {
    console.log("Navigating to user profile");
    this.router.navigate(['/userprofile']);
  }
}
