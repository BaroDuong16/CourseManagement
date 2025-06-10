import { Component, OnInit } from '@angular/core';
import { CourseService, CourseDetailRes } from '../services/course.service';
import { CommonModule } from '@angular/common';
import { AuthService } from '../auth/auth.service';
import { RouterModule } from '@angular/router';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzGridModule } from 'ng-zorro-antd/grid';

@Component({
  selector: 'app-course-table',
  imports: [
    CommonModule, 
    RouterModule,
    NzCardModule,
    NzTagModule,
    NzButtonModule,
    NzDividerModule,
    NzGridModule,
  ],
  templateUrl: './course-table.component.html',
  styleUrl: './course-table.component.css'
})
export class CourseTableComponent implements OnInit {
   courses: CourseDetailRes[] = [];

  constructor(private courseService: CourseService, private auth: AuthService) {}

  ngOnInit() {
    console.log('Is Teacher:', this.isTeacher());
    console.log('Is Student:', this.isStudent());
    this.loadCourses();
  }

loadCourses() {
  this.courseService.getAllCourses().subscribe({
    next: data => {
      this.courses = data;
      console.log('✅ Loaded courses:', data); // 👈 Log toàn bộ dữ liệu
    },
    error: err => {
      console.error('❌ Failed to load courses:', err); // 👈 Log lỗi nếu có
    }
  });
}

isTeacher(): boolean {
  return this.auth.isTeacher();  // Gọi trực tiếp AuthService logic
}

isStudent(): boolean {
  return this.auth.isStudent();  // Gọi trực tiếp AuthService logic
}


  deleteCourse(id: string) {
    if (confirm('Are you sure you want to delete this course?')) {
      this.courseService.deleteCourse(id).subscribe({
        next: () => this.loadCourses(),
        error: err => alert(err.error)
      });
    }
  }
}
