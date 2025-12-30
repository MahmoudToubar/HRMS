import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { JobsService } from '../../../core/services/jobs.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-job-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './job-form.component.html',
  styleUrl: './job-form.component.css'
})
export class JobFormComponent implements OnInit {

  jobForm!: FormGroup;
  isEditMode = false;
  jobId?: number;
  loading = false;

  constructor(
    private fb: FormBuilder, private jobsService: JobsService,
    private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.buildForm();
    this.checkEditMode();
  }

  private buildForm(): void {
    this.jobForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]]
    });
  }

  private checkEditMode(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;

    this.isEditMode = true;
    this.jobId = +id;

    this.jobsService.getJobById(this.jobId).subscribe({
      next: job => {
        this.jobForm.patchValue({
          name: job.name
        });
      }
    });
  }

  submit(): void {
    if (this.jobForm.invalid) {
      this.jobForm.markAllAsTouched();
      return;
    }

    this.loading = true;
    
    const dto = {
      ...this.jobForm.value,
      name: this.jobForm.value.name.trim()
    };

    if (this.isEditMode) {
      this.jobsService
        .updateJob(this.jobId!, dto)
        .subscribe({
          next: () => this.navigateBack(),
          error: () => (this.loading = false)
        });
    } else {
      this.jobsService.createJob(dto).subscribe({
        next: () => this.navigateBack(),
        error: () => (this.loading = false)
      });
    }
  }

  cancel(): void {
    this.navigateBack();
  }

  private navigateBack(): void {
    this.loading = false;
    this.router.navigate(['/jobs']);
  }

}
