// course.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface RoomDto {
  roomName: string;
  description: string;
  createDate: string;
}

export interface CourseDetailRes {
  courseId: string;
  courseName: string;
  description: string;
  price: number;
  maxStudentQuantity: number;
  startDate: string;
  endDate: string;
  rooms: RoomDto[];
  registeredStudentCount: number;
  teacherName: string;
  phoneNumber: string;
}

export interface CourseDto {
  courseName: string;
  description: string;
  price: number;
  maxStudentQuantity: number;
  startDate: string;
  endDate: string;
}

@Injectable({
  providedIn: 'root'
})
export class CourseService {
  private apiUrl = `${environment.apiUrl}/Course`;

  constructor(private http: HttpClient) {}

  // ✅ lấy tất cả courses (đã có trong Controller)
  getAllCourses(): Observable<CourseDetailRes[]> {
    return this.http.get<CourseDetailRes[]>(this.apiUrl);
  }

  getCourseById(id: string): Observable<CourseDetailRes> {
    return this.http.get<CourseDetailRes>(`${this.apiUrl}/${id}`);
  }

  // ✅ Tạo course (cho Teacher)
  createCourse(course: CourseDto): Observable<any> {
    return this.http.post(`${this.apiUrl}/CreateCourse`, course);
  }

  // ✅ Cập nhật course
  updateCourse(id: string, updatedCourse: CourseDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, updatedCourse);
  }

  // ✅ Xoá course
  deleteCourse(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
