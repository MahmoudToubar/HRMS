import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { DepartmentsService } from '../../../core/services/departments.service';
import { EmployeesService } from '../../../core/services/employees.service';
import { JobsService } from '../../../core/services/jobs.service';
import { Department } from '../../../shared/models/department';
import { Job } from '../../../shared/models/job';
import { CommonModule } from '@angular/common';
import { LookupSpecParams } from '../../../shared/models/lookupSpecParams';


@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './employee-form.component.html',
  styleUrl: './employee-form.component.css'
})

export class EmployeeFormComponent implements OnInit {

  employeeForm!: FormGroup;
  departments: Department[] = [];
  jobs: Job[] = [];

  isEditMode = false;
  employeeId?: number;
  loading = false;

  constructor(
    private fb: FormBuilder, private employeesService: EmployeesService,
    private departmentsService: DepartmentsService, private jobsService: JobsService,
    private route: ActivatedRoute, private router: Router) { }


  ngOnInit(): void {
    this.buildForm();
    this.loadLookups();
    this.checkEditMode();
  }


  private buildForm(): void {
    this.employeeForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.maxLength(150)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(100)]],
      mobile: ['', [Validators.required, Validators.pattern(/^01[0-2,5][0-9]{8}$/),
      Validators.maxLength(11)]],
      dateOfBirth: ['', Validators.required],
      departmentId: ['', Validators.required],
      jobId: ['', Validators.required]
    });
  }



  private loadLookups(): void {
    const lookupParams: LookupSpecParams = { pageIndex: 1, pageSize: 10, sort: 'name' };

    this.departmentsService.getDepartments(lookupParams).subscribe({
      next: res => (this.departments = res.data)
    });

    this.jobsService.getJobs(lookupParams).subscribe({
      next: res => (this.jobs = res.data)
    });
  }


  private checkEditMode(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;

    this.isEditMode = true;
    this.employeeId = +id;

    this.employeesService.getEmployeeById(this.employeeId).subscribe({
      next: employee => {
        this.employeeForm.patchValue({
          fullName: employee.fullName,
          email: employee.email,
          mobile: employee.mobile,
          dateOfBirth: employee.dateOfBirth.substring(0, 10),
          departmentId: employee.departmentId,
          jobId: employee.jobId
        });
      }
    });
  }

  submit(): void {
    if (this.employeeForm.invalid) {
      this.employeeForm.markAllAsTouched();
      return;
    }

    this.loading = true;

    const dto = {
      ...this.employeeForm.value,
      fullName: this.employeeForm.value.fullName.trim(),
      email: this.employeeForm.value.email.trim(),
      mobile: this.employeeForm.value.mobile.trim()
    };


    if (this.isEditMode) {
      this.employeesService
        .updateEmployee(this.employeeId!, dto)
        .subscribe({
          next: () => this.navigateBack(),
          error: () => (this.loading = false)
        });
    } else {
      this.employeesService.createEmployee(dto).subscribe({
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
    this.router.navigate(['/employees']);
  }
}
