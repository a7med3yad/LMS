import api from './axios';
import { uploadApi } from './upload.api';
import type {
  AssignmentDto, CreateAssignmentDto,
  SubmissionDto, SubmitAssignmentDto, GradeSubmissionDto,
} from '@/types';

export const assignmentsApi = {
  getAll: (courseId: string) =>
    api.get<AssignmentDto[]>(`/api/v1/courses/${courseId}/assignments`),

  getById: (courseId: string, id: string) =>
    api.get<AssignmentDto>(`/api/v1/courses/${courseId}/assignments/${id}`),

  create: (courseId: string, data: CreateAssignmentDto) =>
    api.post<AssignmentDto>(`/api/v1/courses/${courseId}/assignments`, data),

  update: (courseId: string, id: string, data: CreateAssignmentDto) =>
    api.put<AssignmentDto>(`/api/v1/courses/${courseId}/assignments/${id}`, data),

  publish: (courseId: string, id: string) =>
    api.patch(`/api/v1/courses/${courseId}/assignments/${id}/publish`),

  delete: (courseId: string, id: string) =>
    api.delete(`/api/v1/courses/${courseId}/assignments/${id}`),

  /** Submit assignment with optional file and/or text */
  submit: (courseId: string, id: string, data: SubmitAssignmentDto) =>
    api.post<SubmissionDto>(`/api/v1/courses/${courseId}/assignments/${id}/submit`, data),

  /**
   * Upload file to OCI first, then submit assignment with the returned URL.
   * Convenience method that handles the two-step upload + submit flow.
   */
  submitWithFile: async (
    courseId: string,
    id: string,
    file: File,
    textContent?: string,
  ): Promise<SubmissionDto> => {
    const { data: upload } = await uploadApi.submission(file);
    const { data } = await api.post<SubmissionDto>(
      `/api/v1/courses/${courseId}/assignments/${id}/submit`,
      { fileUrl: upload.url, textContent },
    );
    return data;
  },

  getMySubmission: (courseId: string, id: string) =>
    api.get<SubmissionDto>(`/api/v1/courses/${courseId}/assignments/${id}/my-submission`),

  getSubmissions: (courseId: string, id: string) =>
    api.get<SubmissionDto[]>(`/api/v1/courses/${courseId}/assignments/${id}/submissions`),

  gradeSubmission: (id: string, data: GradeSubmissionDto) =>
    api.patch(`/api/v1/assignments/submissions/${id}/grade`, data),
};
