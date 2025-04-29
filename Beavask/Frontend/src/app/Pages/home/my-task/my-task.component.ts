import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-my-task',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './my-task.component.html',
  styleUrl: './my-task.component.css'
})
export class MyTaskComponent implements OnInit {
  taskFilterForm!: FormGroup;

  projects = ['Dashboard Redesign', 'Bug Hunt Sprint', 'API Refactor'];
  teams = ['Frontend Team', 'Backend Team', 'QA Team'];

  allTasks = [
    {
      id: 101,
      project: 'Dashboard Redesign',
      subject: 'Design login page',
      priority: 'High',
      status: 'Open',
      dueDate: '2025-04-30',
      team: 'Frontend Team'
    },
    {
      id: 102,
      project: 'API Refactor',
      subject: 'Update auth endpoint',
      priority: 'Medium',
      status: 'Pending',
      dueDate: '2025-05-02',
      team: 'Backend Team'
    },
    {
      id: 103,
      project: 'Bug Hunt Sprint',
      subject: 'Fix modal bug',
      priority: 'Low',
      status: 'Closed',
      dueDate: '2025-04-25',
      team: 'QA Team'
    },
    {
      id: 104,
      project: 'Dashboard Redesign',
      subject: 'Sidebar redesign',
      priority: 'Medium',
      status: 'Open',
      dueDate: '2025-05-03',
      team: 'Frontend Team'
    }
  ];

  filteredTasks = [...this.allTasks];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.taskFilterForm = this.fb.group({
      project: [''],
      team: ['']
    });

    // Query param kontrolü
    this.route.queryParams.subscribe(params => {
      const project = params['project'];
      const team = params['team'];
      const status = params['status'];

      this.filteredTasks = this.allTasks.filter(task => {
        const matchesProject = project ? task.project === project : true;
        const matchesTeam = team ? task.team === team : true;
        const matchesStatus = status ? task.status === status : true;
        return matchesProject && matchesTeam && matchesStatus;
      });

      // Opsiyonel: Formu da güncelle
      this.taskFilterForm.patchValue({
        project: project || '',
        team: team || ''
      });
    });
  }

  filterTasks(): void {
    const { project, team } = this.taskFilterForm.value;

    this.filteredTasks = this.allTasks.filter(task => {
      const matchesProject = project ? task.project === project : true;
      const matchesTeam = team ? task.team === team : true;
      return matchesProject && matchesTeam;
    });
  }
}

