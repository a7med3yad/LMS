import api from './axios';
import type { NotificationDto, UnreadCountDto } from '@/types';

const BASE = '/api/v1/notifications';

export const notificationsApi = {
  getAll: () =>
    api.get<NotificationDto[]>(BASE),

  getUnreadCount: () =>
    api.get<UnreadCountDto>(`${BASE}/unread-count`),

  markRead: (ids: string[]) =>
    api.patch(`${BASE}/mark-read`, { ids }),

  markAllRead: () =>
    api.patch(`${BASE}/mark-all-read`),

  delete: (id: string) =>
    api.delete(`${BASE}/${id}`),
};
