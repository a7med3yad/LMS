import api from './axios';
import { uploadApi } from './upload.api';
import type { UserSummaryDto, UserProfileDto, UpdateProfileDto, PagedResult, PaginationParams } from '@/types';

const BASE = '/api/v1/users';

export const usersApi = {
  getMe: () =>
    api.get<UserProfileDto>(`${BASE}/me`),

  updateMe: (data: UpdateProfileDto) =>
    api.put(`${BASE}/me`, data),

  updateAvatar: (avatarUrl: string) =>
    api.patch(`${BASE}/me/avatar`, { avatarUrl }),

  /**
   * Full avatar upload flow:
   *   1. Upload file to OCI → get public URL
   *   2. PATCH profile with { avatarUrl: url }
   *   Returns the new avatar URL.
   */
  uploadAndSetAvatar: async (file: File): Promise<string> => {
    const { data } = await uploadApi.avatar(file);
    await api.patch(`${BASE}/me/avatar`, { avatarUrl: data.url });
    return data.url;
  },

  getAll: (params?: PaginationParams) =>
    api.get<PagedResult<UserSummaryDto>>(BASE, { params }),

  getById: (id: string) =>
    api.get<UserSummaryDto>(`${BASE}/${id}`),

  deactivate: (id: string) =>
    api.patch(`${BASE}/${id}/deactivate`),

  activate: (id: string) =>
    api.patch(`${BASE}/${id}/activate`),

  getInstructors: () =>
    api.get<UserSummaryDto[]>(`${BASE}/instructors`),
};
