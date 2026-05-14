import api from './axios';
import type {
  PaymentIntentResponseDto, CreatePaymentIntentDto,
  ConfirmPaymentDto, ApplyVoucherDto, PaymentDto, RevenueDto,
} from '@/types';

const BASE = '/api/v1/payments';

export const paymentsApi = {
  createIntent: (data: CreatePaymentIntentDto) =>
    api.post<PaymentIntentResponseDto>(`${BASE}/intent`, data),

  confirm: (data: ConfirmPaymentDto) =>
    api.post(`${BASE}/confirm`, data),

  applyVoucher: (data: ApplyVoucherDto) =>
    api.post<PaymentIntentResponseDto>(`${BASE}/apply-voucher`, data),

  getRevenue: (courseId: string) =>
    api.get<RevenueDto>(`${BASE}/courses/${courseId}/revenue`),

  getMyPayments: () =>
    api.get<PaymentDto[]>(`${BASE}/my`),
};
