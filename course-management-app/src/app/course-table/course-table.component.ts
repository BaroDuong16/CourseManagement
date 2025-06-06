import { Component, OnInit } from '@angular/core';
import { CourseService, CourseDetailRes } from '../services/course.service';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-course-table',
  imports: [CommonModule],
  templateUrl: './course-table.component.html',
  styleUrl: './course-table.component.css'
})
export class CourseTableComponent implements OnInit {
   courses: CourseDetailRes[] = [];

  constructor(private courseService: CourseService) {}

  ngOnInit(): void {
    this.courseService.getAllCourses().subscribe({
      next: (data) => this.courses = data,
      error: (err) => console.error('Error fetching courses:', err)
    });
  }
}
