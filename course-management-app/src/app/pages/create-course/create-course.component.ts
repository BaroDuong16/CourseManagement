import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { CourseService } from '../../services/course.service';
import { Router } from '@angular/router';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../auth/auth.service'; 
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
@Component({
  selector: 'app-create-course',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    NzCardModule,
    NzTagModule,
    NzButtonModule,
    NzDividerModule,
    NzGridModule,
    RouterModule,
    NzLayoutModule,
    NzCheckboxModule,
  ],
  templateUrl: './create-course.component.html',
  styleUrl: './create-course.component.css'
})
export class CreateCourseComponent {
  courseForm: FormGroup;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private courseService: CourseService,
    private router: Router,
    private auth: AuthService
  ) {
    this.courseForm = this.fb.group({
      courseName: ['', Validators.required],
      description: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
      maxStudentQuantity: [1, [Validators.required, Validators.min(1)]],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
    });
  }

  isTeacher(): boolean {
  return this.auth.isTeacher();  // Gọi trực tiếp AuthService logic
}

  onSubmit() {
    if (this.courseForm.invalid) return;

    this.loading = true;

    // Convert date strings to ISO format (UTC)
    const rawFormValue = this.courseForm.value;
    const courseData = {
      ...rawFormValue,
      startDate: new Date(rawFormValue.startDate).toISOString(),
      endDate: new Date(rawFormValue.endDate).toISOString(),
    };

    this.courseService.createCourse(courseData).subscribe({
      next: (res) => {
        alert('Course created successfully!');
        this.router.navigate(['/course-table']);
      },
      error: (err) => {
        console.error(err);
        alert('Failed to create course.');
        this.loading = false;
      }
    });
  }
}