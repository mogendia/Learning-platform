# API Endpoints Documentation

## Base URL
```
api/
```

---

## Authentication Endpoints

### 1. Register User
- **Method**: `POST`
- **Route**: `/api/auth/register`
- **Authorization**: None
- **Request** (JSON Body):
  ```json
  {
    "userName": "string",
    "email": "string",
    "phone": "string",
    "fullName": "string",
    "password": "string"
  }
  ```
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": {
      "token": "string",
      "userId": "string",
      "userName": "string",
      "email": "string",
      "role": "string",
      "expiresAt": "datetime"
    }
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

### 2. Login User
- **Method**: `POST`
- **Route**: `/api/auth/login`
- **Authorization**: None
- **Request** (JSON Body):
  ```json
  {
    "email": "string",
    "password": "string"
  }
  ```
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": {
      "token": "string",
      "userId": "string",
      "userName": "string",
      "email": "string",
      "role": "string",
      "expiresAt": "datetime"
    }
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

---

## Course Endpoints

### 1. Get All Courses
- **Method**: `GET`
- **Route**: `/api/course`
- **Authorization**: None
- **Request**: No body required
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": [
      {
        "id": "integer",
        "title": "string",
        "description": "string",
        "coverImage": "string (URL)",
        "price": "decimal",
        "discountPercentage": "decimal",
        "discountEndDate": "datetime",
        "instructorName": "string",
        "studentsCount": "integer"
      }
    ]
  }
  ```

### 2. Get Course Details
- **Method**: `GET`
- **Route**: `/api/course/{id}`
- **Authorization**: None
- **Parameters**: 
  - `id` (path parameter): integer - Course ID
- **Request**: No body required
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": {
      "id": "integer",
      "title": "string",
      "description": "string",
      "coverImage": "string (URL)",
      "price": "decimal",
      "discountPercentage": "decimal",
      "discountEndDate": "datetime",
      "instructorName": "string",
      "lessonsCount": "integer",
      "lessons": [
        {
          "id": "integer",
          "title": "string",
          "description": "string",
          "thumbnail": "string (URL)",
          "duration": "integer (seconds)",
          "orderIndex": "integer"
        }
      ]
    }
  }
  ```
- **Response** (Failure - 404 Not Found):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

### 3. Create Course
- **Method**: `POST`
- **Route**: `/api/course`
- **Authorization**: Required - Instructor Role
- **Request** (Form Data - multipart/form-data):
  ```
  title: string (required)
  description: string (required)
  coverImage: file (required - image file)
  price: decimal (required)
  ```
- **Response** (Success - 201 Created):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": {
      "id": "integer",
      "title": "string",
      "description": "string",
      "coverImage": "string (URL)",
      "price": "decimal"
    }
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

### 4. Update Course
- **Method**: `PUT`
- **Route**: `/api/course/{id}`
- **Authorization**: Required - Instructor Role
- **Parameters**: 
  - `id` (path parameter): integer - Course ID
- **Request** (Form Data - multipart/form-data):
  ```
  title: string (required)
  description: string (required)
  coverImage: file (optional - image file)
  price: decimal (required)
  discountPercentage: decimal (optional)
  discountEndDate: datetime (optional)
  isPublished: boolean (required)
  ```
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": {
      "id": "integer",
      "title": "string",
      "description": "string",
      "coverImage": "string (URL)",
      "price": "decimal",
      "discountPercentage": "decimal",
      "discountEndDate": "datetime",
      "isPublished": "boolean"
    }
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

### 5. Delete Course
- **Method**: `DELETE`
- **Route**: `/api/course/{id}`
- **Authorization**: Required - Instructor Role
- **Parameters**: 
  - `id` (path parameter): integer - Course ID
- **Request**: No body required
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string"
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

### 6. Get Instructor's Courses
- **Method**: `GET`
- **Route**: `/api/course/my-courses`
- **Authorization**: Required - Instructor Role
- **Request**: No body required
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": [
      {
        "id": "integer",
        "title": "string",
        "description": "string",
        "coverImage": "string (URL)",
        "price": "decimal",
        "isPublished": "boolean",
        "studentsCount": "integer",
        "lessonsCount": "integer"
      }
    ]
  }
  ```

### 7. Get Course Details For Instructor
- **Method**: `GET`
- **Route**: `/api/course/{id}/details-for-instructor`
- **Authorization**: Required - Instructor Role
- **Parameters**: 
  - `id` (path parameter): integer - Course ID
- **Request**: No body required
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": {
      "id": "integer",
      "title": "string",
      "description": "string",
      "coverImage": "string (URL)",
      "price": "decimal",
      "isPublished": "boolean",
      "studentsCount": "integer",
      "lessonsCount": "integer",
      "lessons": [
        {
          "id": "integer",
          "title": "string",
          "description": "string",
          "orderIndex": "integer"
        }
      ],
      "students": [
        {
          "id": "string",
          "email": "string",
          "fullName": "string",
          "enrollmentDate": "datetime"
        }
      ]
    }
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

### 8. Add Student To Course
- **Method**: `POST`
- **Route**: `/api/course/{id}/students`
- **Authorization**: Required - Instructor Role
- **Parameters**: 
  - `id` (path parameter): integer - Course ID
- **Request** (JSON Body - raw string):
  ```
  "student@email.com"
  ```
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string"
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

### 9. Remove Student From Course
- **Method**: `DELETE`
- **Route**: `/api/course/{id}/students/{studentEmail}`
- **Authorization**: Required - Instructor Role
- **Parameters**: 
  - `id` (path parameter): integer - Course ID
  - `studentEmail` (path parameter): string - Student email
- **Request**: No body required
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string"
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

### 10. Get Student's Enrolled Courses
- **Method**: `GET`
- **Route**: `/api/course/my-enrollments`
- **Authorization**: Required - Any Authenticated User
- **Request**: No body required
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": [
      {
        "id": "integer",
        "title": "string",
        "description": "string",
        "coverImage": "string (URL)",
        "price": "decimal",
        "instructorName": "string",
        "progress": "integer (percentage)",
        "lessonsCount": "integer",
        "completedLessons": "integer"
      }
    ]
  }
  ```

---

## Lesson Endpoints

### 1. Create Lesson
- **Method**: `POST`
- **Route**: `/api/lesson`
- **Authorization**: Required - Instructor Role
- **Request** (Form Data - multipart/form-data):
  ```
  title: string (required)
  description: string (required)
  videoFile: file (required - video file)
  thumbnail: file (required - image file)
  courseId: integer (required)
  orderIndex: integer (required)
  ```
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": {
      "id": "integer",
      "title": "string",
      "description": "string",
      "thumbnail": "string (URL)",
      "videoUrl": "string (URL)",
      "duration": "integer (seconds)",
      "orderIndex": "integer"
    }
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

### 2. Get All Lessons
- **Method**: `GET`
- **Route**: `/api/lesson`
- **Authorization**: None
- **Request**: No body required
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": [
      {
        "id": "integer",
        "title": "string",
        "description": "string",
        "thumbnail": "string (URL)",
        "duration": "integer (seconds)",
        "courseId": "integer",
        "courseName": "string",
        "orderIndex": "integer"
      }
    ]
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

### 3. Get Lesson Details
- **Method**: `GET`
- **Route**: `/api/lesson/{id}`
- **Authorization**: None
- **Parameters**: 
  - `id` (path parameter): integer - Lesson ID
- **Request**: No body required
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": {
      "id": "integer",
      "title": "string",
      "description": "string",
      "thumbnail": "string (URL)",
      "videoUrl": "string (URL)",
      "duration": "integer (seconds)",
      "courseId": "integer",
      "courseName": "string",
      "orderIndex": "integer",
      "studentProgress": {
        "watchedSeconds": "integer",
        "progress": "integer (percentage)"
      }
    }
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

### 4. Delete Lesson
- **Method**: `DELETE`
- **Route**: `/api/lesson/{id}`
- **Authorization**: Required - Instructor Role
- **Parameters**: 
  - `id` (path parameter): integer - Lesson ID
- **Request**: No body required
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string"
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

### 5. Update Lesson Progress
- **Method**: `POST`
- **Route**: `/api/lesson/{id}/progress`
- **Authorization**: Required - Student Role
- **Parameters**: 
  - `id` (path parameter): integer - Lesson ID
- **Request** (JSON Body - integer):
  ```
  120
  ```
  (Example: 120 seconds watched)
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": {
      "watchedSeconds": "integer",
      "progress": "integer (percentage)"
    }
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

---

## Dashboard Endpoints

### 1. Get Instructor Dashboard
- **Method**: `GET`
- **Route**: `/api/dashboard/instructor`
- **Authorization**: Required - Instructor Role
- **Request**: No body required
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": {
      "totalCourses": "integer",
      "totalStudents": "integer",
      "totalRevenue": "decimal",
      "courseStatistics": [
        {
          "courseId": "integer",
          "courseName": "string",
          "studentCount": "integer",
          "lessonsCount": "integer",
          "avgCompletion": "decimal (percentage)"
        }
      ]
    }
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

### 2. Get Student Dashboard
- **Method**: `GET`
- **Route**: `/api/dashboard/student`
- **Authorization**: Required - Student Role
- **Request**: No body required
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": {
      "enrolledCoursesCount": "integer",
      "totalHoursSpent": "integer",
      "enrolledCourses": [
        {
          "courseId": "integer",
          "courseName": "string",
          "instructorName": "string",
          "progress": "integer (percentage)",
          "completedLessons": "integer",
          "totalLessons": "integer",
          "hoursSpent": "integer"
        }
      ]
    }
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

### 3. Get Course Statistics
- **Method**: `GET`
- **Route**: `/api/dashboard/course/{courseId}`
- **Authorization**: Required - Instructor Role
- **Parameters**: 
  - `courseId` (path parameter): integer - Course ID
- **Request**: No body required
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": {
      "courseId": "integer",
      "courseName": "string",
      "studentCount": "integer",
      "lessonsCount": "integer",
      "totalViewTime": "integer (seconds)",
      "avgCompletion": "decimal (percentage)",
      "studentProgress": [
        {
          "studentId": "string",
          "studentName": "string",
          "progress": "integer (percentage)",
          "completedLessons": "integer",
          "hoursSpent": "integer"
        }
      ]
    }
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

---

## Review Endpoints

### 1. Get Active Review
- **Method**: `GET`
- **Route**: `/api/review/active`
- **Authorization**: None
- **Request**: No body required
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": [
      {
        "id": "integer",
        "image": "string (URL)",
        "studentName": "string",
        "description": "string",
        "orderIndex": "integer"
      }
    ]
  }
  ```

### 2. Get All Reviews
- **Method**: `GET`
- **Route**: `/api/review`
- **Authorization**: Required - Instructor Role
- **Request**: No body required
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": [
      {
        "id": "integer",
        "image": "string (URL)",
        "studentName": "string",
        "description": "string",
        "orderIndex": "integer",
        "isActive": "boolean"
      }
    ]
  }
  ```

### 3. Create Review
- **Method**: `POST`
- **Route**: `/api/review`
- **Authorization**: Required - Instructor Role
- **Request** (Form Data - multipart/form-data):
  ```
  image: file (required - image file)
  orderIndex: integer (required)
  studentName: string (optional)
  description: string (optional)
  ```
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": {
      "id": "integer",
      "image": "string (URL)",
      "studentName": "string",
      "description": "string",
      "orderIndex": "integer",
      "isActive": "boolean"
    }
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

### 4. Update Review
- **Method**: `PUT`
- **Route**: `/api/review/{id}`
- **Authorization**: Required - Instructor Role
- **Parameters**: 
  - `id` (path parameter): integer - Review ID
- **Request** (Form Data - multipart/form-data):
  ```
  image: file (optional - image file)
  orderIndex: integer (required)
  isActive: boolean (required)
  studentName: string (optional)
  description: string (optional)
  ```
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string",
    "data": {
      "id": "integer",
      "image": "string (URL)",
      "studentName": "string",
      "description": "string",
      "orderIndex": "integer",
      "isActive": "boolean"
    }
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

### 5. Delete Review
- **Method**: `DELETE`
- **Route**: `/api/review/{id}`
- **Authorization**: Required - Instructor Role
- **Parameters**: 
  - `id` (path parameter): integer - Review ID
- **Request**: No body required
- **Response** (Success - 200 OK):
  ```json
  {
    "success": "boolean",
    "message": "string"
  }
  ```
- **Response** (Failure - 400 Bad Request):
  ```json
  {
    "success": false,
    "message": "string (error description)"
  }
  ```

---

## Authentication Headers

For protected endpoints (marked as "Authorization: Required"), include the token in the request header:

```
Authorization: Bearer <token>
```

The token is received from the Login/Register endpoints in the `data.token` field.

---

## Error Handling

All endpoints follow a consistent response format:

**Success Response**:
```json
{
  "success": true,
  "message": "Operation successful",
  "data": { /* endpoint-specific data */ }
}
```

**Error Response**:
```json
{
  "success": false,
  "message": "Error description"
}
```

---

## Notes

- All timestamps are in ISO 8601 format (UTC)
- File uploads use multipart/form-data content type
- All monetary values (price, revenue) are in decimal format
- Progress percentages range from 0-100
- User roles: "Student", "Instructor", "Admin"
