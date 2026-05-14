/* ════════════════════════════════════════
   أنا البحر — TYPE DEFINITIONS
   All DTOs matching the backend API
   ════════════════════════════════════════ */

// ─── Auth ────────────────────────────────
export type UserRole = 'Student' | 'Instructor' | 'Admin';

export interface AuthResponseDto {
  accessToken: string;
  refreshToken: string;
  userId: string;
  fullName: string;
  email: string;
  role: UserRole;
}

export interface RegisterDto {
  fullName: string;
  email: string;
  password: string;
  role: 'Student' | 'Instructor';
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface VerifyEmailDto {
  email: string;
  otp: string;
}

export interface ForgotPasswordDto {
  email: string;
}

export interface VerifyResetDto {
  email: string;
  otp: string;
}

export interface ResetPasswordDto {
  email: string;
  otp: string;
  newPassword: string;
}

export interface ChangePasswordDto {
  currentPassword: string;
  newPassword: string;
}

export interface AssignRoleDto {
  userId: string;
  role: UserRole;
}

// ─── Users ───────────────────────────────
export interface UserSummaryDto {
  id: string;
  fullName: string;
  email: string;
  avatarUrl?: string;
  role: UserRole;
  isActive: boolean;
  createdAt: string;
}

export interface UserProfileDto extends UserSummaryDto {
  courseCount?: number;
  enrollmentCount?: number;
}

export interface UpdateProfileDto {
  fullName: string;
}

export interface UpdateAvatarDto {
  avatarUrl: string;
}

// ─── Pagination ──────────────────────────
export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface PaginationParams {
  pageNumber?: number;
  pageSize?: number;
  search?: string;
  sortBy?: string;
  sortDirection?: 'asc' | 'desc';
}

// ─── Courses ─────────────────────────────
export type CourseStatus = 'Draft' | 'Published' | 'Archived';

export interface CourseSummaryDto {
  id: string;
  titleAr: string;
  titleEn: string;
  descriptionAr: string;
  descriptionEn: string;
  thumbnailUrl?: string;
  price: number;
  status: CourseStatus;
  instructor: UserSummaryDto;
  enrollmentCount: number;
  materialCount: number;
  createdAt: string;
  updatedAt: string;
}

export interface CourseDto extends CourseSummaryDto {
  materials?: MaterialDto[];
  assignments?: AssignmentDto[];
  exams?: ExamDto[];
}

export interface CreateCourseDto {
  titleAr: string;
  titleEn: string;
  descriptionAr: string;
  descriptionEn: string;
  thumbnailUrl?: string;
  price: number;
}

export interface UpdateCourseDto extends CreateCourseDto {}

export interface CourseFilterParams extends PaginationParams {
  status?: CourseStatus;
  minPrice?: number;
  maxPrice?: number;
  instructorId?: string;
}

// ─── Materials ───────────────────────────
export type MaterialType = 'Video' | 'Pdf' | 'Text' | 'Link';

export interface MaterialDto {
  id: string;
  titleAr: string;
  titleEn: string;
  type: MaterialType;
  contentUrl?: string;
  textContent?: string;
  order: number;
  isPublished: boolean;
  courseId: string;
  createdAt: string;
}

export interface CreateMaterialDto {
  titleAr: string;
  titleEn: string;
  type: MaterialType;
  contentUrl?: string;
  textContent?: string;
}

export interface UpdateMaterialDto extends CreateMaterialDto {}

// ─── Assignments ─────────────────────────
export type SubmissionType = 'TextOnly' | 'FileUpload' | 'Both';

export interface AssignmentDto {
  id: string;
  titleAr: string;
  titleEn: string;
  descriptionAr: string;
  descriptionEn: string;
  submissionType: SubmissionType;
  deadLine: string;
  maxGrade: number;
  isPublished: boolean;
  courseId: string;
  createdAt: string;
}

export interface CreateAssignmentDto {
  titleAr: string;
  titleEn: string;
  descriptionAr: string;
  descriptionEn: string;
  submissionType: SubmissionType;
  deadLine: string;
  maxGrade: number;
}

export interface SubmissionDto {
  id: string;
  assignmentId: string;
  student: UserSummaryDto;
  textContent?: string;
  fileUrl?: string;
  grade?: number;
  feedback?: string;
  submittedAt: string;
  gradedAt?: string;
}

export interface SubmitAssignmentDto {
  textContent?: string;
  fileUrl?: string;
}

export interface GradeSubmissionDto {
  grade: number;
  feedback?: string;
}

// ─── Exams ───────────────────────────────
export type QuestionType = 'MultipleChoice' | 'TrueFalse' | 'OpenEnded';

export interface ExamDto {
  id: string;
  titleAr: string;
  titleEn: string;
  descriptionAr?: string;
  descriptionEn?: string;
  durationMinutes: number;
  maxGrade: number;
  isPublished: boolean;
  courseId: string;
  questionCount?: number;
  createdAt: string;
}

export interface CreateExamDto {
  titleAr: string;
  titleEn: string;
  descriptionAr?: string;
  descriptionEn?: string;
  durationMinutes: number;
  maxGrade: number;
}

export interface QuestionDto {
  id: string;
  textAr: string;
  textEn: string;
  type: QuestionType;
  points: number;
  order: number;
  choices?: ChoiceDto[];
}

export interface ChoiceDto {
  id: string;
  textAr: string;
  textEn: string;
  isCorrect: boolean;
}

export interface CreateQuestionDto {
  textAr: string;
  textEn: string;
  type: QuestionType;
  points: number;
  choices?: Omit<ChoiceDto, 'id'>[];
}

export interface StartExamResponseDto {
  attemptId: string;
  examId: string;
  titleAr: string;
  titleEn: string;
  durationMinutes: number;
  startedAt: string;
  expiresAt: string;
  questions: QuestionDto[];
}

export interface ExamAnswerDto {
  questionId: string;
  selectedChoiceId?: string;
  openEndedAnswer?: string;
  trueFalseAnswer?: boolean;
}

export interface SubmitExamDto {
  answers: ExamAnswerDto[];
}

export interface ExamAttemptDto {
  id: string;
  examId: string;
  student: UserSummaryDto;
  score?: number;
  maxScore: number;
  startedAt: string;
  completedAt?: string;
  answers?: ExamAttemptAnswerDto[];
}

export interface ExamAttemptAnswerDto {
  questionId: string;
  question: QuestionDto;
  selectedChoiceId?: string;
  openEndedAnswer?: string;
  trueFalseAnswer?: boolean;
  isCorrect?: boolean;
  points: number;
}

// ─── Enrollments ─────────────────────────
export type EnrollmentStatus = 'Active' | 'Completed' | 'Suspended' | 'PendingPayment';

export interface EnrollmentDto {
  id: string;
  courseId: string;
  courseTitleAr: string;
  courseTitleEn: string;
  student: UserSummaryDto;
  status: EnrollmentStatus;
  paidAmount: number;
  voucherCode?: string;
  enrolledAt: string;
  completedAt?: string;
}

export interface EnrollRequestDto {
  courseId: string;
  voucherCode?: string;
}

// ─── Payments ────────────────────────────
export interface PaymentIntentResponseDto {
  paymentIntentId: string;
  clientSecret: string;
  amount: number;
  originalPrice: number;
  discountAmount?: number;
  voucherCode?: string;
  currency: string;
}

export interface CreatePaymentIntentDto {
  courseId: string;
  voucherCode?: string;
}

export interface ConfirmPaymentDto {
  paymentIntentId: string;
}

export interface ApplyVoucherDto {
  courseId: string;
  voucherCode: string;
}

export interface PaymentDto {
  id: string;
  courseId: string;
  courseTitleAr: string;
  courseTitleEn: string;
  amount: number;
  currency: string;
  status: string;
  createdAt: string;
}

export interface RevenueDto {
  totalRevenue: number;
  totalStudents: number;
  averageCoursePrice: number;
  monthlyRevenue: { month: string; revenue: number }[];
  courseRevenue: { courseId: string; title: string; revenue: number; students: number }[];
}

// ─── Vouchers ────────────────────────────
export interface VoucherDto {
  id: string;
  code: string;
  courseId: string;
  courseTitleEn: string;
  discountPercent: number;
  discountAmount?: number;
  maxUses: number;
  usedCount: number;
  expiresAt: string;
  isActive: boolean;
  createdAt: string;
}

export interface CreateVoucherDto {
  code: string;
  courseId: string;
  discountPercent: number;
  discountAmount?: number;
  maxUses: number;
  expiresAt: string;
}

export interface ValidateVoucherDto {
  courseId: string;
  code: string;
}

// ─── Notifications ───────────────────────
export type NotificationType =
  | 'NewMaterial'
  | 'NewAssignment'
  | 'NewExam'
  | 'AssignmentGraded'
  | 'CourseUpdate'
  | 'General';

export interface NotificationDto {
  id: string;
  type: NotificationType;
  titleAr: string;
  titleEn: string;
  messageAr?: string;
  messageEn?: string;
  isRead: boolean;
  relatedEntityId?: string;
  createdAt: string;
}

export interface UnreadCountDto {
  count: number;
}
