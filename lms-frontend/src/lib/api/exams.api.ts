import api from './axios';
import type {
  ExamDto, CreateExamDto, QuestionDto, CreateQuestionDto,
  StartExamResponseDto, SubmitExamDto, ExamAttemptDto,
} from '@/types';

export const examsApi = {
  getAll: (courseId: string) =>
    api.get<ExamDto[]>(`/api/v1/courses/${courseId}/exams`),

  getById: (courseId: string, id: string) =>
    api.get<ExamDto>(`/api/v1/courses/${courseId}/exams/${id}`),

  create: (courseId: string, data: CreateExamDto) =>
    api.post<ExamDto>(`/api/v1/courses/${courseId}/exams`, data),

  update: (courseId: string, id: string, data: CreateExamDto) =>
    api.put<ExamDto>(`/api/v1/courses/${courseId}/exams/${id}`, data),

  publish: (courseId: string, id: string) =>
    api.patch(`/api/v1/courses/${courseId}/exams/${id}/publish`),

  delete: (courseId: string, id: string) =>
    api.delete(`/api/v1/courses/${courseId}/exams/${id}`),

  // Questions
  addQuestion: (courseId: string, examId: string, data: CreateQuestionDto) =>
    api.post<QuestionDto>(`/api/v1/courses/${courseId}/exams/${examId}/questions`, data),

  updateQuestion: (courseId: string, examId: string, qId: string, data: CreateQuestionDto) =>
    api.put<QuestionDto>(`/api/v1/courses/${courseId}/exams/${examId}/questions/${qId}`, data),

  deleteQuestion: (courseId: string, examId: string, qId: string) =>
    api.delete(`/api/v1/courses/${courseId}/exams/${examId}/questions/${qId}`),

  // Attempts
  start: (courseId: string, examId: string) =>
    api.post<StartExamResponseDto>(`/api/v1/courses/${courseId}/exams/${examId}/start`),

  submit: (courseId: string, examId: string, data: SubmitExamDto) =>
    api.post(`/api/v1/courses/${courseId}/exams/${examId}/submit`, data),

  getMyAttempts: (courseId: string, examId: string) =>
    api.get<ExamAttemptDto[]>(`/api/v1/courses/${courseId}/exams/${examId}/attempts/my`),

  getAllAttempts: (courseId: string, examId: string) =>
    api.get<ExamAttemptDto[]>(`/api/v1/courses/${courseId}/exams/${examId}/attempts`),

  getAttempt: (attemptId: string) =>
    api.get<ExamAttemptDto>(`/api/v1/exams/attempts/${attemptId}`),

  gradeOpen: (courseId: string, examId: string) =>
    api.patch(`/api/v1/courses/${courseId}/exams/${examId}/grade-open`),
};
