import { CommonModule } from '@angular/common';
import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { AuthprofileService } from '../../common/services/profile/authprofile.service';
import { Profile } from '../../common/services/profile/profile.model';
import { LangService } from '../../common/services/lang/lang.service';
import { TranslateModule } from '@ngx-translate/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { UserDetails, UserService } from '../../common/services/user.service';
import { UserContactService } from '../../common/services/userContact/user-contact.service';
import { UserContact } from '../../common/services/userContact/model/userContact.model';
import { FormBuilder, FormGroup, FormsModule } from '@angular/forms';
import { Task } from '../../common/services/task/taskModel/task.model';
import { TaskService } from '../../common/services/task/task.service';
import { Project } from '../../common/model/project.model';
import { ProjectsService } from '../../common/services/projects/projects.service';

interface UserProfile {
  id: number;
  firstName: string;
  lastName: string;
  userName: string;
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
  imports: [CommonModule,TranslateModule,FormsModule],
  templateUrl: './userprofile.component.html',
  styleUrl: './userprofile.component.css'
})

export class UserprofileComponent implements OnInit,OnDestroy {
  user!: UserDetails;
  taskFilterForm!: FormGroup;
  username="";
  setDropdown=false;
  userContacts:UserContact[]=[];
  profileId:number | null = null;
  isOwner: boolean = false;

  aboutContact: UserContact | null = null;
aboutEditMode = false;
aboutEditValue = '';
  lastProjects: Project[] = [];
  loadingProjects = false;
  userInfo: Profile | null = null;
  avatarUrl: string |null = '';
  userProfileView=false;
private routeSub: Subscription = new Subscription();
  activities: ActivityItem[] = [];

  contactModalOpen = false;
  modalMode: 'add' | 'edit' = 'add';
  modalContact: UserContact = {id:0, contactType: 'E-posta', contactValue: '', userId: 0 , createdAt: new Date()};
  editingContactId: number | null = null;
lastTasks: Task[] = [];
  loadingTasks = false;
contactTypes = [
  { value: 'E-posta', label: 'E-posta', icon: 'fa-envelope' },
  { value: 'Telefon', label: 'Telefon', icon: 'fa-phone' },
  { value: 'LinkedIn', label: 'LinkedIn', icon: 'fa-brands fa-linkedin-in' },
  { value: 'GitHub', label: 'GitHub', icon: 'fa-brands fa-github' },
  { value: 'About', label: 'Hakkında', icon: 'fa-user' }
];

  constructor(
    private profileService:AuthprofileService,
    private langService: LangService,
    private route: ActivatedRoute,
    private userService: UserService,
    private userContact:UserContactService,
    private fb: FormBuilder,
    private taskService:TaskService,
    private projectsService: ProjectsService
     ) {
  
   }

  ngOnInit() {
    this.routeSub = this.route.params.subscribe(params => {
      this.profileId = +params['id'];
    
    const currentLang = this.langService.getCurrentLanguage();
    this.langService.setLanguage(currentLang);
   
    if(this.profileId && this.profileId !==null){
      this.isOwner=false;
        this.getUserProfile();
    }else{
      this.getUserInfo();
      this.isOwner=true;
    }
     this.taskFilterForm = this.fb.group({
        contactType: [''],
      contactValue: [''],
      userId: [this.profileId]
  });
   this.modalContact.userId = this.profileId;
   
 
  }

)
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }


getContactIcon(type: string): string {
  const item = this.contactTypes.find(t => t.value === type);
  return item ? item.icon : 'fa-question-circle';
}


  getUserInfo(){
      this.profileService.whoami().subscribe((response: Profile) => {
        if (response) {
          this.userInfo = response;
          if((response.avatarUrl=='' || response.avatarUrl==null || response.avatarUrl=='default_avatar_url')){
            if( response.firstName!='' || response.firstName!=null ){

              this.avatarUrl= this.generateInitialsAvatar(response.firstName+' '+response.lastName,100);
            }else{

              this.avatarUrl= this.generateInitialsAvatar(response.userName,100);
            }
              }else{

                this.avatarUrl=response.avatarUrl
              }
          this.profileId=response.userId;
          
          this.getContact();
          this.username = response.userName
        }
      });
    }

    getUserProfile() {
      if (this.profileId) {
       this.userService.apiUserById(this.profileId).subscribe({
          next: (response) => {
            if (response.data) {
              this.user = response.data;
                 if((this.user.avatarUrl=='' || this.user.avatarUrl==null)){
            if( this.user.firstName!='' || this.user.firstName!=null){

              this.avatarUrl= this.generateInitialsAvatar(this.user.firstName+' '+this.user.lastName,100);
            }else{

              this.avatarUrl= this.generateInitialsAvatar(this.user.username,100);
            }
              }else{

                this.avatarUrl=this.user.avatarUrl
              }
             
              this.getContact();
              this.username = response.data.username
            
            }
          }
       
      })
    }
    }

    toggleDropdown(){
      this.setDropdown=!this.setDropdown;
    }
    @HostListener("document:keydown.escape",['$event'])
    @HostListener("document:click",['$event'])
    handleClickOutside(event:MouseEvent){
      const target = event.target as HTMLElement;
      this.closeMenu()
      if (
        this.userProfileView &&
        !target.closest('.bv-v-c') &&
        !target.closest('[data-modal="profile"]')
      ) {
        this.userProfileView = false;
      } 
      if (
        this.setDropdown &&
        !target.closest('.setting-dropdown') &&
        !target.closest('[data-modal="setting"]')
      ) {
        this.setDropdown = false;
      } 
    }
    toggleProfileModal(): void {
      this.userProfileView = !this.userProfileView;
    }
  
    
    getContact(){
     
      this.userContact.getContactById(Number(this.profileId)).subscribe({
        next: (response)=>{
          this.userContacts=response.data;
           
          this.aboutContact = this.userContacts.find(c => c.contactType === 'About') || null;
        }
      })
       this.getLastTasks();
       this.getRecentProjects();
    }
    
addAboutField() {
  this.aboutEditMode = true;
  this.aboutEditValue = '';
}
enableAboutEdit() {
  if (!this.isOwner) return;
  this.aboutEditMode = true;
  this.aboutEditValue = this.aboutContact?.contactValue || '';
}

saveAboutEdit() {
  if (this.aboutEditValue.trim().length === 0) return;

  if (this.aboutContact) {
    // Güncelleme
    const updated = { ...this.aboutContact, contactValue: this.aboutEditValue };
    this.userContact.updateContactById(updated, this.aboutContact.id).subscribe(() => {
      this.getContact();
      this.aboutEditMode = false;
    });
  } else {
    // İlk kez ekliyor, tekrar eklememek için About olup olmadığını bir daha kontrol et
    if (!this.userContacts.some(c => c.contactType === 'About')) {
      const newAbout: UserContact = {
        id: 0,
        contactType: 'About',
        contactValue: this.aboutEditValue,
        userId: Number(this.profileId),
        createdAt: new Date()
      };
      this.userContact.postContactById(newAbout).subscribe(() => {
        this.getContact();
        this.aboutEditMode = false;
      });
    } else {
      // Var ise güncelle
      const about = this.userContacts.find(c => c.contactType === 'About');
      if (about) {
        const updated = { ...about, contactValue: this.aboutEditValue };
        this.userContact.updateContactById(updated, about.id).subscribe(() => {
          this.getContact();
          this.aboutEditMode = false;
        });
      }
    }
  }
}

saveContact() {
  const req = {
    contactType: this.modalContact.contactType,
    contactValue: this.modalContact.contactValue,
    userId: Number(this.profileId)
  };

  if (this.modalMode === 'add') {
  
    this.userContact.postContactById(req).subscribe(() => {
      this.getContact();
      this.closeModal();
    });
  } else if (this.modalMode === 'edit' && this.editingContactId !== null) {
    const contact = this.userContacts[this.editingContactId];
    this.userContact.updateContactById(req,contact.id).subscribe(() => {
      this.getContact();
      this.closeModal();
    });
  }
}


    
get filteredContacts(): UserContact[] {
  return this.userContacts.filter(
    c => this.contactTypes.some(t => t.value === c.contactType) && c.contactType !== 'About'
  );
}

  openAddModal() {
    this.modalMode = 'add';
    this.modalContact = { id:0,contactType: 'E-posta', contactValue: '', userId: this.modalContact.userId, createdAt: new Date() };
    this.contactModalOpen = true;
  }

  openEditModal(contact: UserContact, index: number) {
    this.modalMode = 'edit';
    this.modalContact = { ...contact }; 
    this.editingContactId = index;
    this.contactModalOpen = true;
  }

  closeModal() {
    this.contactModalOpen = false;
    this.editingContactId = null;
  }


  openMenuIndex: number | null = null;

toggleOptionsMenu(index: number, event: MouseEvent) {
  event.stopPropagation(); 
  this.openMenuIndex = this.openMenuIndex === index ? null : index;
}
closeMenu = () => {
  this.openMenuIndex = null;
};


getAvailableContactTypes(): { value: string; label: string; icon: string }[] {
  
  const editingType = this.modalMode === 'edit' ? this.modalContact.contactType : null;
  return this.contactTypes.filter(type =>
    !this.userContacts.some(c =>
      c.contactType === type.value && type.value !== editingType
    )
  );
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
  getRecentProjects() {
    this.loadingProjects = true;
    this.projectsService.getAll().subscribe({
      next: res => {
        // Burada son 2 projeyi tarihe göre çekiyoruz
        if (res.data && Array.isArray(res.data)) {
          const sorted = [...res.data].sort(
            (a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
          );
          this.lastProjects = sorted.slice(0, 2);
        }
        this.loadingProjects = false;
      },
      error: () => { this.loadingProjects = false; }
    });
  }
   getLastTasks() {
    this.loadingTasks = true;
    this.taskService.getUserTaskById(Number(this.profileId)).subscribe({
      next: (res) => {
        let list: Task[] = res.data || [];
        list = list.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
        this.lastTasks = list.slice(0, 4);
        this.loadingTasks = false;
      },
      error: () => {
        this.lastTasks = [];
        this.loadingTasks = false;
      }
    });
  }


  confirmModalOpen = false;
pendingDeleteIndex: number | null = null;

openConfirmModal(index: number) {
  this.pendingDeleteIndex = index;
  this.confirmModalOpen = true;
}

closeConfirmModal() {
  this.pendingDeleteIndex = null;
  this.confirmModalOpen = false;
}

confirmDelete() {
  if (this.pendingDeleteIndex !== null) {
    const contact = this.userContacts[this.pendingDeleteIndex];
    this.userContact.deleteContactById(contact.id).subscribe(() => {
      this.userContacts.splice(this.pendingDeleteIndex!, 1);
      this.closeConfirmModal();
    });
  }
}
}
