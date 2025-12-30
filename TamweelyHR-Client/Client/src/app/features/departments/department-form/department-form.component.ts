import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { DepartmentsService } from '../../../core/services/departments.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-department-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './department-form.component.html',
  styleUrl: './department-form.component.css'
})
export class DepartmentFormComponent implements OnInit {

  departmentForm!: FormGroup;
  isEditMode = false;
  departmentId?: number;
  loading = false;

  constructor(
    private fb: FormBuilder, private departmentsService: DepartmentsService,
    private route: ActivatedRoute, private router: Router) { }


  ngOnInit(): void {
    this.buildForm();
    this.checkEditMode();
  }

  private buildForm(): void {
    this.departmentForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]]
    });
  }

  private checkEditMode(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;

    this.isEditMode = true;
    this.departmentId = +id;

    this.departmentsService.getDepartmentById(this.departmentId).subscribe({
      next: department => {
        this.departmentForm.patchValue({
          name: department.name
        });
      }
    });
  }

  submit(): void {
    if (this.departmentForm.invalid) {
      this.departmentForm.markAllAsTouched();
      return;
    }

    this.loading = true;

    const dto = {
      ...this.departmentForm.value,
      name: this.departmentForm.value.name.trim()
    };

    if (this.isEditMode) {
      this.departmentsService
        .updateDepartment(this.departmentId!, dto)
        .subscribe({
          next: () => this.navigateBack(),
          error: () => (this.loading = false)
        });
    } else {
      this.departmentsService.createDepartment(dto).subscribe({
        next: () => this.navigateBack(),
        error: err => {
          this.loading = false;

          if (err.error?.errors?.Name) {
            this.departmentForm
              .get('name')
              ?.setErrors({ server: err.error.errors.Name[0] });
          }
        }
      });
    }
  }

  cancel(): void {
    this.navigateBack();
  }

  private navigateBack(): void {
    this.loading = false;
    this.router.navigate(['/departments']);
  }

}
