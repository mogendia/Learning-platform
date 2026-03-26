# Frontend AI API Spec

> ???? ????? ??? Frontend AI (Angular/React) ?????? `services`, `models`, ?`api client`.

## Base URL
- Local HTTP: `http://localhost:5063`
- API prefix: `/api`
- SignalR Hub: `/hubs/live-session`

## Auth
- ??? ??????: `Bearer JWT`
- Header:

```http
Authorization: Bearer <token>
```

## Unified Response Shape
?? ??? endpoints ???? ????? ??????:

```ts
interface BaseResponse<T> {
  message: string;
  data: T;
  success: boolean;
  errors: string[];
}
```

---

## 1) Auth

### `POST /api/Auth/register`
- Auth: No
- Content-Type: `application/json`
- Body: `RegisterDto`
- Response: `BaseResponse<AuthResponseDto>`

### `POST /api/Auth/login`
- Auth: No
- Content-Type: `application/json`
- Body: `LoginDto`
- Response: `BaseResponse<AuthResponseDto>`

---

## 2) Course

### `GET /api/Course`
- Auth: No
- Response: `BaseResponse<CourseListDto[]>`

### `GET /api/Course/{id}`
- Auth: No (???? user context ?? ?????)
- Response: `BaseResponse<CourseDetailsDto>`

### `POST /api/Course`
- Auth: `Instructor`
- Content-Type: `multipart/form-data`
- Body: `CreateCourseDto`
- Response: `BaseResponse<CourseDetailsDto>`

### `PUT /api/Course/{id}`
- Auth: `Instructor`
- Content-Type: `multipart/form-data`
- Body: `UpdateCourseDto` (???? `id` ?? ??? body? ??? id ?? route)
- Response: `BaseResponse<CourseDetailsDto>`

### `DELETE /api/Course/{id}`
- Auth: `Instructor`
- Response: `BaseResponse<boolean>`

### `GET /api/Course/my-courses`
- Auth: `Instructor`
- Response: `BaseResponse<CourseListDto[]>`

### `GET /api/Course/{id}/details-for-instructor`
- Auth: `Instructor`
- Response: `BaseResponse<CourseInstructorDetailsDto>`

### `POST /api/Course/{id}/students`
- Auth: `Instructor`
- Content-Type: `application/json`
- Body: `string` (raw JSON string ??????)
- Example body:
```json
"student@mail.com"
```
- Response: `BaseResponse<boolean>`

### `DELETE /api/Course/{id}/students/{studentEmail}`
- Auth: `Instructor`
- Response: `BaseResponse<boolean>`

### `GET /api/Course/my-enrollments`
- Auth: Any authenticated user
- Response: `BaseResponse<CourseListDto[]>`

---

## 3) Lesson

### `POST /api/Lesson`
- Auth: `Instructor`
- Content-Type: `multipart/form-data`
- Body: `CreateLessonDto`
- Response: `BaseResponse<LessonDetailsDto>`

### `GET /api/Lesson`
- Auth: No
- Response: `BaseResponse<LessonListDto[]>`

### `GET /api/Lesson/{id}`
- Auth: Any authenticated user
- Response: `BaseResponse<LessonDetailsDto>`

### `DELETE /api/Lesson/{id}`
- Auth: `Instructor`
- Response: `BaseResponse<boolean>`

### `POST /api/Lesson/{id}/progress`
- Auth: `Student`
- Content-Type: `application/json`
- Body: `number` (watched seconds)
- Example body:
```json
120
```
- Response: `BaseResponse<boolean>`

### `GET /api/Lesson/course/{courseId}`
- Auth: `Instructor`
- Response: `BaseResponse<LessonListDto[]>`

---

## 4) Dashboard

### `GET /api/Dashboard/instructor`
- Auth: `Instructor`
- Response: `BaseResponse<InstructorDashboardDto>`

### `GET /api/Dashboard/student`
- Auth: `Student`
- Response: `BaseResponse<StudentDashboardDto>`

### `GET /api/Dashboard/course/{courseId}`
- Auth: `Instructor`
- Response: `BaseResponse<CourseStatisticsDto>`

---

## 5) Review

### `GET /api/Review/active`
- Auth: No
- Response: `BaseResponse<ReviewDto[]>`

### `GET /api/Review`
- Auth: `Instructor`
- Response: `BaseResponse<ReviewDto[]>`

### `POST /api/Review`
- Auth: `Instructor`
- Content-Type: `multipart/form-data`
- Body: `CreateReviewDto`
- Response: `BaseResponse<ReviewDto>`

### `PUT /api/Review/{id}`
- Auth: `Instructor`
- Content-Type: `multipart/form-data`
- Body: `UpdateReviewDto` (???? `id` ?? ??? body)
- Response: `BaseResponse<ReviewDto>`

### `DELETE /api/Review/{id}`
- Auth: `Instructor`
- Response: `BaseResponse<boolean>`

---

## 6) Live Sessions

### `POST /api/LiveSession`
- Auth: `Instructor`
- Content-Type: `application/json`
- Body: `CreateLiveSessionDto`
- Response: `BaseResponse<LiveSessionCreateResponseDto>`

### `POST /api/LiveSession/{sessionId}/start`
- Auth: `Instructor`
- Response: `BaseResponse<LiveSessionDetailsDto>`

### `POST /api/LiveSession/{sessionId}/end`
- Auth: `Instructor`
- Response: `BaseResponse<LiveSessionDetailsDto>`

### `GET /api/LiveSession/{sessionId}`
- Auth: Any authenticated user
- Response: `BaseResponse<LiveSessionDetailsDto>`

### `POST /api/LiveSession/{sessionId}/join`
- Auth: Any authenticated user
- Response: `BaseResponse<LiveJoinResponseDto>`

---

## 7) Live Questions

Base route: `/api/live-sessions/{sessionId}/questions`

### `POST /api/live-sessions/{sessionId}/questions`
- Auth: `Student`
- Content-Type: `application/json`
- Body: `CreateLiveQuestionDto`
- Response: `BaseResponse<LiveQuestionDto>`

### `GET /api/live-sessions/{sessionId}/questions`
- Auth: `Instructor`
- Response: `BaseResponse<LiveQuestionDto[]>`

### `POST /api/live-sessions/{sessionId}/questions/{questionId}/approve`
- Auth: `Instructor`
- Response: `BaseResponse<LiveQuestionDto>`

### `POST /api/live-sessions/{sessionId}/questions/{questionId}/revoke`
- Auth: `Instructor`
- Response: `BaseResponse<LiveQuestionDto>`

---

## 8) SignalR Contract (Live)

### Hub URL
- `ws(s)://<host>/hubs/live-session`

### Client -> Server methods
- `JoinSessionGroup(sessionId: string)`
- `LeaveSessionGroup(sessionId: string)`

### Server -> Client events
- `liveSession.updated` payload:
```ts
{
  sessionId: string;
  status: string;
  startedAt: string | null;
  endedAt: string | null;
}
```
- `liveQuestion.created` payload: `LiveQuestionDto`
- `liveQuestion.approved` payload: `LiveQuestionDto`
- `liveQuestion.revoked` payload: `LiveQuestionDto`
- `participant.speakGranted` payload:
```ts
{ sessionId: string; studentId: string; }
```
- `participant.speakRevoked` payload:
```ts
{ sessionId: string; studentId: string; }
```

---

## DTOs (exact fields)

```ts
// Auth
interface RegisterDto {
  userName: string;
  email: string;
  phone: string;
  fullName: string;
  password: string;
}

interface LoginDto {
  email: string;
  password: string;
}

interface AuthResponseDto {
  token: string;
  userId: string;
  userName: string;
  email: string;
  role: string;
  expiresAt: string;
}

// Course
interface CreateCourseDto {
  title: string;
  description: string;
  coverImage: File;
  price: number;
}

interface UpdateCourseDto {
  id: number;
  title: string;
  description: string;
  coverImage?: File;
  price: number;
  discountPercentage?: number;
  discountEndDate?: string;
  isPublished: boolean;
}

interface CourseListDto {
  id: number;
  title: string;
  coverImageUrl: string;
  price: number;
  finalPrice: number;
  instructorName: string;
  totalLessons: number;
  totalStudents: number;
}

interface CourseDetailsDto {
  id: number;
  title: string;
  description: string;
  coverImageUrl: string;
  price: number;
  discountPercentage?: number;
  discountEndDate?: string;
  finalPrice: number;
  isPublished: boolean;
  instructorName: string;
  totalLessons: number;
  totalStudents: number;
  isEnrolled: boolean;
  lessons: LessonListDto[];
}

interface CourseInstructorDetailsDto {
  id: number;
  title: string;
  lessons: LessonDto[];
  students: StudentDto[];
}

// Lesson
interface CreateLessonDto {
  title: string;
  description: string;
  videoFile: File;
  thumbnail: File;
  courseId: number;
  orderIndex: number;
}

interface LessonDetailsDto {
  id: number;
  title: string;
  description: string;
  videoUrl: string;
  thumbnailUrl: string;
  durationInMins: number;
  courseId: number;
}

interface LessonListDto {
  id: number;
  title: string;
  thumbnailUrl: string;
  durationInMins: number;
  orderIndex: number;
  isCompleted: boolean;
  watchedMins: number;
}

interface LessonDto {
  id: number;
  title: string;
  durationInMinutes: number;
}

// User
interface StudentDto {
  id: string;
  name: string;
  email: string;
}

// Dashboard
interface InstructorDashboardDto {
  totalCourses: number;
  totalStudents: number;
  totalRevenue: number;
  courseStatistics: CourseStatisticsDto[];
}

interface StudentDashboardDto {
  totalEnrolledCourses: number;
  completedCourses: number;
  enrolledCourses: EnrolledCourseDto[];
}

interface CourseStatisticsDto {
  courseId: number;
  courseTitle: string;
  totalEnrolled: number;
  revenue: number;
  students: StudentProgressDto[];
}

interface EnrolledCourseDto {
  courseId: number;
  courseTitle: string;
  coverImageUrl: string;
  instructorName: string;
  completedLessons: number;
  totalLessons: number;
  progressPercentage: number;
  enrolledAt: string;
}

interface StudentProgressDto {
  studentId: string;
  studentName: string;
  email: string;
  phoneNumber: string;
  enrolledAt: string;
  completedLessons: number;
  totalLessons: number;
  progressPercentage: number;
}

// Review
interface CreateReviewDto {
  image: File;
  orderIndex: number;
  studentName?: string;
  description?: string;
}

interface UpdateReviewDto {
  id: number;
  image?: File;
  orderIndex: number;
  isActive: boolean;
  studentName?: string;
  description?: string;
}

interface ReviewDto {
  id: number;
  imageUrl: string;
  orderIndex: number;
  isActive: boolean;
  createdAt: string;
  studentName?: string;
  description?: string;
}

// Live
interface CreateLiveSessionDto {
  courseId: number;
}

interface LiveSessionCreateResponseDto {
  sessionId: string;
  status: string;
  streamRoomId: string;
}

interface LiveSessionDetailsDto {
  sessionId: string;
  courseId: number;
  instructorId: string;
  streamRoomId: string;
  status: string;
  startedAt?: string;
  endedAt?: string;
}

interface LiveJoinResponseDto {
  roomId: string;
  token: string;
  role: string;
  canSpeak: boolean;
}

interface CreateLiveQuestionDto {
  message: string;
}

interface LiveQuestionDto {
  questionId: string;
  sessionId: string;
  studentId: string;
  studentName: string;
  message: string;
  status: string;
  createdAt: string;
}
```

---

## Notes for Frontend AI
- ??? endpoints ???? `multipart/form-data` ???? ?????? `FormData`.
- Endpoint `POST /api/Course/{id}/students` ?????? `raw string` ???? object.
- ?????? ??? casing ?? ???????? ??? ?? ????? (`/api/Auth`, `/api/Course`, ...).
- ?? ?? CORS ??????? ?????? ??? ?????????? ??? ??????? ?????? ?? ???? API ??? HTTPS ???????? ??????.
