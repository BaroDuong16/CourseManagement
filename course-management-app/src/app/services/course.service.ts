import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
// Interface phản ánh dữ liệu CourseDetailRes trả về từ API
export interface RoomDto {
  roomName: string;
  description: string;
  createDate: string; // ISO string
}

export interface CourseDetailRes {
  courseId: number;
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

@Injectable({
  providedIn: 'root'
})
export class CourseService {
  private apiUrl = `${environment.apiUrl}/Courses`; // Đổi URL phù hợp backend của bạn

  constructor(private http: HttpClient) {}

  getAllCourses(): Observable<CourseDetailRes[]> {
    return this.http.get<CourseDetailRes[]>(`${this.apiUrl}`);
  }
}
