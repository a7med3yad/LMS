'use client';

import React from 'react';
import { motion } from 'framer-motion';
import {
  BookOpen, ClipboardList, FileCheck, Award,
  RefreshCw, Bell, Check, CheckCheck, Trash2,
} from 'lucide-react';
import DashboardLayout from '@/components/layout/DashboardLayout';
import { useUIStore } from '@/stores/ui.store';
import type { NotificationType } from '@/types';

const typeIcons: Record<NotificationType, React.ReactNode> = {
  NewMaterial: <BookOpen size={18} className="text-ocean-mid" />,
  NewAssignment: <ClipboardList size={18} className="text-amber-500" />,
  NewExam: <FileCheck size={18} className="text-ocean-wave" />,
  AssignmentGraded: <Award size={18} className="text-gold" />,
  CourseUpdate: <RefreshCw size={18} className="text-teal-500" />,
  General: <Bell size={18} className="text-ocean-surf" />,
};

const demoNotifs = [
  { id: '1', type: 'NewMaterial' as NotificationType, titleAr: 'مادة جديدة: مقدمة في React', titleEn: 'New Material: Intro to React', isRead: false, createdAt: '2024-03-01T10:00:00' },
  { id: '2', type: 'AssignmentGraded' as NotificationType, titleAr: 'تم تقييم واجبك — 95/100', titleEn: 'Your assignment was graded — 95/100', isRead: false, createdAt: '2024-02-28T15:30:00' },
  { id: '3', type: 'NewExam' as NotificationType, titleAr: 'اختبار جديد متاح في دورة الويب', titleEn: 'New exam available in Web Course', isRead: true, createdAt: '2024-02-27T09:00:00' },
  { id: '4', type: 'CourseUpdate' as NotificationType, titleAr: 'تم تحديث دورة علم البيانات', titleEn: 'Data Science course updated', isRead: true, createdAt: '2024-02-25T12:00:00' },
  { id: '5', type: 'NewAssignment' as NotificationType, titleAr: 'واجب جديد: مشروع CSS', titleEn: 'New Assignment: CSS Project', isRead: true, createdAt: '2024-02-24T08:00:00' },
];

export default function NotificationsPage() {
  const { locale } = useUIStore();
  const isAr = locale === 'ar';

  const timeAgo = (date: string) => {
    const diff = Date.now() - new Date(date).getTime();
    const hours = Math.floor(diff / 3600000);
    if (hours < 1) return isAr ? 'الآن' : 'Just now';
    if (hours < 24) return `${hours}${isAr ? 'س' : 'h'}`;
    const days = Math.floor(hours / 24);
    return `${days}${isAr ? 'ي' : 'd'}`;
  };

  return (
    <DashboardLayout>
      <div className="max-w-2xl mx-auto space-y-6">
        <div className="flex items-center justify-between">
          <motion.h1 initial={{ opacity: 0 }} animate={{ opacity: 1 }} className="text-2xl font-bold text-ocean-deep">
            {isAr ? 'الإشعارات' : 'Notifications'}
          </motion.h1>
          <button className="btn-ocean btn-ghost text-sm px-4 py-2 flex items-center gap-1">
            <CheckCheck size={16} />
            {isAr ? 'اقرأ الكل' : 'Mark All Read'}
          </button>
        </div>

        {demoNotifs.length === 0 ? (
          <div className="ocean-card p-12 text-center">
            <Bell size={48} className="text-ocean-surf mx-auto mb-4" />
            <h3 className="text-xl font-bold text-ocean-deep mb-2">
              {isAr ? 'المياه هادئة — لا إشعارات جديدة' : 'Calm waters — no new notifications'}
            </h3>
          </div>
        ) : (
          <div className="space-y-2">
            {demoNotifs.map((notif, i) => (
              <motion.div
                key={notif.id}
                initial={{ opacity: 0, y: 10 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ delay: i * 0.05 }}
                className={`ocean-card p-4 flex items-start gap-4 transition-all ${
                  !notif.isRead ? 'border-ocean-mid/30 bg-ocean-foam/50' : ''
                }`}
              >
                <div className="w-10 h-10 rounded-xl bg-ocean-foam flex items-center justify-center flex-shrink-0">
                  {typeIcons[notif.type]}
                </div>
                <div className="flex-1 min-w-0">
                  <div className="flex items-start justify-between gap-2">
                    <p className={`text-sm leading-relaxed ${!notif.isRead ? 'font-semibold text-ocean-deep' : 'text-ocean-wave'}`}>
                      {isAr ? notif.titleAr : notif.titleEn}
                    </p>
                    {!notif.isRead && <div className="w-2 h-2 rounded-full bg-ocean-mid flex-shrink-0 mt-1.5" />}
                  </div>
                  <p className="text-xs text-ocean-surf mt-1">{timeAgo(notif.createdAt)}</p>
                </div>
                <div className="flex gap-1 flex-shrink-0">
                  {!notif.isRead && (
                    <button className="p-1.5 rounded-lg hover:bg-ocean-foam text-ocean-surf hover:text-ocean-mid" aria-label="Mark read">
                      <Check size={14} />
                    </button>
                  )}
                  <button className="p-1.5 rounded-lg hover:bg-red-50 text-ocean-surf hover:text-error" aria-label="Delete">
                    <Trash2 size={14} />
                  </button>
                </div>
              </motion.div>
            ))}
          </div>
        )}
      </div>
    </DashboardLayout>
  );
}
