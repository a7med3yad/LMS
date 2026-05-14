import api from './axios';
import type {
  AuthResponseDto,
  RegisterDto,
  LoginDto,
  VerifyEmailDto,
  ForgotPasswordDto,
  VerifyResetDto,
  ResetPasswordDto,
  ChangePasswordDto,
  AssignRoleDto,
} from '@/types';

const AUTH_BASE = '/api/auth';

export const authApi = {
  register: (data: RegisterDto) =>
    api.post<AuthResponseDto>(`${AUTH_BASE}/register`, data),

  verifyEmail: (data: VerifyEmailDto) =>
    api.post(`${AUTH_BASE}/verify-email`, data),

  login: (data: LoginDto) =>
    api.post<AuthResponseDto>(`${AUTH_BASE}/login`, data),

  forgotPassword: (data: ForgotPasswordDto) =>
    api.post(`${AUTH_BASE}/forgot-password`, data),

  verifyReset: (data: VerifyResetDto) =>
    api.post(`${AUTH_BASE}/verify-reset`, data),

  resetPassword: (data: ResetPasswordDto) =>
    api.post(`${AUTH_BASE}/reset-password`, data),

  refreshToken: () =>
    api.post<AuthResponseDto>(`${AUTH_BASE}/refresh-token`),

  logout: () =>
    api.post(`${AUTH_BASE}/logout`),

  changePassword: (data: ChangePasswordDto) =>
    api.post(`${AUTH_BASE}/change-password`, data),

  assignRole: (data: AssignRoleDto) =>
    api.post(`${AUTH_BASE}/assign-role`, data),

  googleLogin: () =>
    `${api.defaults.baseURL}${AUTH_BASE}/login/google`,

  facebookLogin: () =>
    `${api.defaults.baseURL}${AUTH_BASE}/login/facebook`,
};
