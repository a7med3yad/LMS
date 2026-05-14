import api from './axios';
import type { EnrollmentDto, EnrollRequestDto, PagedResult } from '@/types';

const BASE = '/api/v1/enrollments';

export const enrollmentsApi = {
  enroll: (data: EnrollRequestDto) =>
    api.post<EnrollmentDto>(BASE, data),

  getMyEnrollments: () =>
    api.get<EnrollmentDto[]>(`${BASE}/my`),

  getById: (id: string) =>
    api.get<EnrollmentDto>(`${BASE}/${id}`),

  getByCourse: (courseId: string) =>
    api.get<PagedResult<EnrollmentDto>>(`${BASE}/course/${courseId}`),

  complete: (id: string) =>
    api.patch(`${BASE}/${id}/complete`),

  suspend: (id: string) =>
    api.patch(`${BASE}/${id}/suspend`),
};
