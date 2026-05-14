'use client';

import React from 'react';
import Link from 'next/link';
import { motion } from 'framer-motion';
import { BookOpen, Users, Play, Award, Clock } from 'lucide-react';
import DashboardLayout from '@/components/layout/DashboardLayout';
import { useAuthStore } from '@/stores/auth.store';
import { useUIStore } from '@/stores/ui.store';

const waveEntrance = {
  hidden: { opacity: 0, y: 20 },
  visible: (i: number) => ({ opacity: 1, y: 0, transition: { delay: i * 0.08, duration: 0.5 } }),
};

const demoCourses = [
  { id: '1', titleAr: 'أساسيات تطوير الويب', titleEn: 'Web Dev Basics', status: 'Active', progress: 65, instructor: 'أحمد محمد' },
  { id: '2', titleAr: 'علم البيانات', titleEn: 'Data Science', status: 'Active', progress: 30, instructor: 'سارة أحمد' },
  { id: '3', titleAr: 'تصميم واجهات المستخدم', titleEn: 'UI/UX Design', status: 'Completed', progress: 100, instructor: 'ليلى حسن' },
  { id: '4', titleAr: 'الذكاء الاصطناعي', titleEn: 'AI Fundamentals', status: 'Active', progress: 10, instructor: 'محمد علي' },
];

export default function MyCoursesPage() {
  const { user } = useAuthStore();
  const { locale } = useUIStore();
  const isAr = locale === 'ar';
  const isInstructor = user?.role === 'Instructor';

  return (
    <DashboardLayout>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <motion.h1 initial={{ opacity: 0 }} animate={{ opacity: 1 }} className="text-2xl font-bold text-ocean-deep">
            {isAr ? 'دوراتي' : 'My Courses'}
          </motion.h1>
          {isInstructor && (
            <Link href="/instructor/courses/new" className="btn-ocean btn-primary text-sm px-5 py-2.5">
              + {isAr ? 'دورة جديدة' : 'New Course'}
            </Link>
          )}
        </div>

        {demoCourses.length === 0 ? (
          <div className="ocean-card p-12 text-center">
            <BookOpen size={48} className="text-ocean-surf mx-auto mb-4" />
            <h3 className="text-xl font-bold text-ocean-deep mb-2">{isAr ? 'البحر واسع — ابدأ رحلتك' : 'The sea is vast — start your journey'}</h3>
            <Link href="/courses" className="btn-ocean btn-primary mt-4 inline-flex px-6 py-2.5">
              {isAr ? 'تصفح الدورات' : 'Browse Courses'}
            </Link>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {demoCourses.map((course, i) => (
              <motion.div
                key={course.id}
                initial="hidden"
                animate="visible"
                variants={waveEntrance}
                custom={i}
                className="ocean-card overflow-hidden"
              >
                <div className="h-32 bg-gradient-to-br from-ocean-mid to-ocean-wave relative">
                  <div className="absolute inset-0 flex items-center justify-center">
                    <BookOpen size={32} className="text-white/30" />
                  </div>
                  <div className="absolute top-3 right-3">
                    <span className={`badge ${course.status === 'Completed' ? 'badge-completed' : 'badge-active'}`}>
                      {course.status === 'Completed' ? (isAr ? 'مكتمل' : 'Completed') : (isAr ? 'نشط' : 'Active')}
                    </span>
                  </div>
                </div>

                <div className="p-4">
                  <h3 className="font-bold text-ocean-deep mb-1">{isAr ? course.titleAr : course.titleEn}</h3>
                  <p className="text-xs text-ocean-surf mb-3">{course.instructor}</p>

                  {/* Progress bar */}
                  <div className="mb-3">
                    <div className="flex justify-between text-xs mb-1">
                      <span className="text-ocean-wave">{isAr ? 'التقدم' : 'Progress'}</span>
                      <span className="text-ocean-deep font-mono font-semibold">{course.progress}%</span>
                    </div>
                    <div className="h-2 bg-ocean-pale rounded-full overflow-hidden">
                      <motion.div
                        initial={{ width: 0 }}
                        animate={{ width: `${course.progress}%` }}
                        transition={{ duration: 1, delay: 0.2 + i * 0.1 }}
                        className="h-full rounded-full bg-gradient-to-r from-ocean-mid to-ocean-light"
                      />
                    </div>
                  </div>

                  <div className="flex gap-2">
                    <Link
                      href={`/courses/${course.id}/learn`}
                      className="btn-ocean btn-primary flex-1 text-sm py-2"
                    >
                      <Play size={14} />
                      {course.status === 'Completed' ? (isAr ? 'مراجعة' : 'Review') : (isAr ? 'تابع التعلم' : 'Continue')}
                    </Link>
                  </div>
                </div>
              </motion.div>
            ))}
          </div>
        )}
      </div>
    </DashboardLayout>
  );
}
