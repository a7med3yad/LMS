import api from './axios';
import { uploadApi } from './upload.api';
import type {
  CourseSummaryDto,
  CourseDto,
  CreateCourseDto,
  UpdateCourseDto,
  CourseFilterParams,
  PagedResult,
} from '@/types';

const BASE = '/api/v1/courses';

export const coursesApi = {
  getAll: (params?: CourseFilterParams) =>
    api.get<PagedResult<CourseSummaryDto>>(BASE, { params }),

  getById: (id: string) =>
    api.get<CourseDto>(`${BASE}/${id}`),

  getMyCourses: () =>
    api.get<CourseSummaryDto[]>(`${BASE}/my`),

  getEnrolled: () =>
    api.get<CourseSummaryDto[]>(`${BASE}/enrolled`),

  create: (data: CreateCourseDto) =>
    api.post<CourseDto>(BASE, data),

  update: (id: string, data: UpdateCourseDto) =>
    api.put<CourseDto>(`${BASE}/${id}`, data),

  publish: (id: string) =>
    api.patch(`${BASE}/${id}/publish`),

  archive: (id: string) =>
    api.patch(`${BASE}/${id}/archive`),

  delete: (id: string) =>
    api.delete(`${BASE}/${id}`),

  /**
   * Upload thumbnail to OCI, then create course with the URL.
   * Convenience method combining upload + create in one call.
   */
  createWithThumbnail: async (data: CreateCourseDto, thumbnailFile?: File) => {
    if (thumbnailFile) {
      const { data: upload } = await uploadApi.thumbnail(thumbnailFile);
      data = { ...data, thumbnailUrl: upload.url };
    }
    return api.post<CourseDto>(BASE, data);
  },

  /**
   * Upload a new thumbnail to OCI.
   * Returns the public URL to include in a course create/update payload.
   */
  uploadThumbnail: async (file: File): Promise<string> => {
    const { data } = await uploadApi.thumbnail(file);
    return data.url;
  },
};
