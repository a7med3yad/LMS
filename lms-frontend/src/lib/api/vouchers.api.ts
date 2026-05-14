import api from './axios';
import type { VoucherDto, CreateVoucherDto, ValidateVoucherDto } from '@/types';

export const vouchersApi = {
  getAll: (courseId: string) =>
    api.get<VoucherDto[]>(`/api/v1/courses/${courseId}/vouchers`),

  create: (courseId: string, data: CreateVoucherDto) =>
    api.post<VoucherDto>(`/api/v1/courses/${courseId}/vouchers`, data),

  update: (courseId: string, id: string, data: CreateVoucherDto) =>
    api.put<VoucherDto>(`/api/v1/courses/${courseId}/vouchers/${id}`, data),

  delete: (courseId: string, id: string) =>
    api.delete(`/api/v1/courses/${courseId}/vouchers/${id}`),

  validate: (courseId: string, data: ValidateVoucherDto) =>
    api.post(`/api/v1/courses/${courseId}/vouchers/validate`, data),
};
