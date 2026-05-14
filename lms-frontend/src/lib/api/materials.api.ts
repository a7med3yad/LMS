import api from './axios';
import { uploadApi } from './upload.api';
import type { MaterialDto, CreateMaterialDto, UpdateMaterialDto } from '@/types';

export const materialsApi = {
  getAll: (courseId: string) =>
    api.get<MaterialDto[]>(`/api/v1/courses/${courseId}/materials`),

  getById: (courseId: string, id: string) =>
    api.get<MaterialDto>(`/api/v1/courses/${courseId}/materials/${id}`),

  create: (courseId: string, data: CreateMaterialDto) =>
    api.post<MaterialDto>(`/api/v1/courses/${courseId}/materials`, data),

  /**
   * Upload a material file (video, PDF, etc.) to OCI, then create
   * the material record with the returned URL as contentUrl.
   */
  createWithFile: async (
    courseId: string,
    data: CreateMaterialDto,
    file: File,
    onProgress?: (percent: number) => void,
  ): Promise<MaterialDto> => {
    const { data: upload } = await uploadApi.material(file, onProgress);
    const payload = { ...data, contentUrl: upload.url };
    const { data: material } = await api.post<MaterialDto>(
      `/api/v1/courses/${courseId}/materials`,
      payload,
    );
    return material;
  },

  update: (courseId: string, id: string, data: UpdateMaterialDto) =>
    api.put<MaterialDto>(`/api/v1/courses/${courseId}/materials/${id}`, data),

  publish: (courseId: string, id: string) =>
    api.patch(`/api/v1/courses/${courseId}/materials/${id}/publish`),

  reorder: (courseId: string, ids: string[]) =>
    api.patch(`/api/v1/courses/${courseId}/materials/reorder`, ids),

  delete: (courseId: string, id: string) =>
    api.delete(`/api/v1/courses/${courseId}/materials/${id}`),
};
