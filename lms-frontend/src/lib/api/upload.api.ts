import api from './axios';

/**
 * Centralized upload API — files go to Oracle OCI Object Storage
 * via the backend. Each method returns { url } which is then saved
 * to the corresponding entity via its own endpoint.
 *
 * Flow:
 *   1. uploadAvatar(file)   → POST /api/v1/upload/avatar   → { url }
 *   2. PATCH /api/v1/users/me/avatar   { avatarUrl: url }
 */

export type UploadTarget = 'avatar' | 'thumbnail' | 'material' | 'submission' | 'attachment';

interface UploadResponse {
  url: string;
}

function createFormData(file: File): FormData {
  const form = new FormData();
  form.append('file', file);
  return form;
}

const UPLOAD_HEADERS = { 'Content-Type': 'multipart/form-data' };

export const uploadApi = {
  /** Upload user avatar image (max 5 MB, JPEG/PNG/WebP) */
  avatar: (file: File) =>
    api.post<UploadResponse>('/api/v1/upload/avatar', createFormData(file), {
      headers: UPLOAD_HEADERS,
    }),

  /** Upload course thumbnail image (max 10 MB) */
  thumbnail: (file: File) =>
    api.post<UploadResponse>('/api/v1/upload/thumbnail', createFormData(file), {
      headers: UPLOAD_HEADERS,
    }),

  /** Upload course material (video, PDF — max 500 MB) */
  material: (file: File, onProgress?: (percent: number) => void) =>
    api.post<UploadResponse>('/api/v1/upload/material', createFormData(file), {
      headers: UPLOAD_HEADERS,
      onUploadProgress: onProgress
        ? (e) => onProgress(Math.round((e.loaded * 100) / (e.total ?? 1)))
        : undefined,
    }),

  /** Upload assignment submission file (max 50 MB) */
  submission: (file: File) =>
    api.post<UploadResponse>('/api/v1/upload/submission', createFormData(file), {
      headers: UPLOAD_HEADERS,
    }),

  /** Upload generic attachment (max 25 MB) */
  attachment: (file: File) =>
    api.post<UploadResponse>('/api/v1/upload/attachment', createFormData(file), {
      headers: UPLOAD_HEADERS,
    }),

  /** Delete a file by its public URL */
  delete: (fileUrl: string) =>
    api.delete('/api/v1/upload', { params: { fileUrl } }),
};
