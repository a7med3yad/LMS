'use client';

import React from 'react';
import { motion } from 'framer-motion';
import {
  BookOpen, Users, DollarSign, TrendingUp,
  Bell, Clock, Award, BarChart3,
} from 'lucide-react';
import DashboardLayout from '@/components/layout/DashboardLayout';
import { useAuthStore } from '@/stores/auth.store';
import { useUIStore } from '@/stores/ui.store';

const waveEntrance = {
  hidden: { opacity: 0, y: 20 },
  visible: (i: number) => ({
    opacity: 1, y: 0,
    transition: { delay: i * 0.1, duration: 0.5 }
  }),
};

export default function DashboardPage() {
  const { user } = useAuthStore();
  const { locale } = useUIStore();
  const isAr = locale === 'ar';

  const greeting = () => {
    const hour = new Date().getHours();
    if (hour < 12) return isAr ? 'صباح الخير' : 'Good morning';
    if (hour < 18) return isAr ? 'مساء الخير' : 'Good afternoon';
    return isAr ? 'مساء الخير' : 'Good evening';
  };

  const studentStats = [
    { label: isAr ? 'الدورات المسجلة' : 'Enrolled Courses', value: '5', icon: <BookOpen size={20} />, color: 'from-ocean-mid to-ocean-light' },
    { label: isAr ? 'الدورات المكتملة' : 'Completed', value: '3', icon: <Award size={20} />, color: 'from-emerald-500 to-emerald-400' },
    { label: isAr ? 'الواجبات المعلقة' : 'Pending Assignments', value: '2', icon: <Clock size={20} />, color: 'from-amber-500 to-amber-400' },
    { label: isAr ? 'الاختبارات القادمة' : 'Upcoming Exams', value: '1', icon: <BarChart3 size={20} />, color: 'from-purple-500 to-purple-400' },
  ];

  const instructorStats = [
    { label: isAr ? 'دوراتي' : 'My Courses', value: '8', icon: <BookOpen size={20} />, color: 'from-ocean-mid to-ocean-light' },
    { label: isAr ? 'إجمالي الطلاب' : 'Total Students', value: '1,234', icon: <Users size={20} />, color: 'from-emerald-500 to-emerald-400' },
    { label: isAr ? 'إجمالي الإيرادات' : 'Total Revenue', value: '$12,450', icon: <DollarSign size={20} />, color: 'from-gold to-amber-400' },
    { label: isAr ? 'معدل التقييم' : 'Average Rating', value: '4.8', icon: <TrendingUp size={20} />, color: 'from-purple-500 to-purple-400' },
  ];

  const adminStats = [
    { label: isAr ? 'إجمالي المستخدمين' : 'Total Users', value: '5,432', icon: <Users size={20} />, color: 'from-ocean-mid to-ocean-light' },
    { label: isAr ? 'إجمالي الدورات' : 'Total Courses', value: '156', icon: <BookOpen size={20} />, color: 'from-emerald-500 to-emerald-400' },
    { label: isAr ? 'التسجيلات النشطة' : 'Active Enrollments', value: '3,421', icon: <TrendingUp size={20} />, color: 'from-gold to-amber-400' },
    { label: isAr ? 'الإيرادات الشهرية' : 'Monthly Revenue', value: '$45,200', icon: <DollarSign size={20} />, color: 'from-purple-500 to-purple-400' },
  ];

  const stats = user?.role === 'Admin' ? adminStats : user?.role === 'Instructor' ? instructorStats : studentStats;

  return (
    <DashboardLayout>
      <div className="space-y-8">
        {/* Welcome */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
        >
          <h1 className="text-2xl md:text-3xl font-bold text-ocean-deep mb-1">
            {greeting()}، {user?.fullName || (isAr ? 'مستخدم' : 'User')} 👋
          </h1>
          <p className="text-ocean-surf">
            {isAr ? 'إليك ملخص نشاطك اليوم' : "Here's your activity summary for today"}
          </p>
        </motion.div>

        {/* Stats cards */}
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
          {stats.map((stat, i) => (
            <motion.div
              key={stat.label}
              initial="hidden"
              animate="visible"
              variants={waveEntrance}
              custom={i}
              className="ocean-card p-5 flex items-start gap-4"
            >
              <div className={`w-11 h-11 rounded-xl bg-gradient-to-br ${stat.color} flex items-center justify-center text-white flex-shrink-0`}>
                {stat.icon}
              </div>
              <div>
                <p className="text-2xl font-bold text-ocean-deep" style={{ fontFamily: "'JetBrains Mono', monospace" }}>
                  {stat.value}
                </p>
                <p className="text-sm text-ocean-surf">{stat.label}</p>
              </div>
            </motion.div>
          ))}
        </div>

        {/* Recent activity */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
          {/* Recent notifications */}
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: 0.4 }}
            className="ocean-card p-6"
          >
            <h2 className="text-lg font-bold text-ocean-deep mb-4 flex items-center gap-2">
              <Bell size={18} className="text-ocean-mid" />
              {isAr ? 'آخر الإشعارات' : 'Recent Notifications'}
            </h2>
            <div className="space-y-3">
              {[
                { titleAr: 'مادة جديدة في أساسيات الويب', titleEn: 'New material in Web Basics', time: '2h' },
                { titleAr: 'تم تقييم واجبك', titleEn: 'Your assignment was graded', time: '5h' },
                { titleAr: 'اختبار جديد متاح', titleEn: 'New exam available', time: '1d' },
              ].map((n, i) => (
                <div key={i} className="flex items-start gap-3 p-3 rounded-xl hover:bg-ocean-foam transition-colors">
                  <div className="w-2 h-2 rounded-full bg-ocean-mid mt-2 flex-shrink-0" />
                  <div className="flex-1">
                    <p className="text-sm text-ocean-deep font-medium">{isAr ? n.titleAr : n.titleEn}</p>
                    <p className="text-xs text-ocean-surf">{n.time} {isAr ? 'مضت' : 'ago'}</p>
                  </div>
                </div>
              ))}
            </div>
          </motion.div>

          {/* Quick actions */}
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: 0.5 }}
            className="ocean-card p-6"
          >
            <h2 className="text-lg font-bold text-ocean-deep mb-4">
              {isAr ? 'إجراءات سريعة' : 'Quick Actions'}
            </h2>
            <div className="grid grid-cols-2 gap-3">
              {[
                { labelAr: 'تصفح الدورات', labelEn: 'Browse Courses', icon: <BookOpen size={20} />, href: '/courses' },
                { labelAr: 'دوراتي', labelEn: 'My Courses', icon: <Award size={20} />, href: '/my-courses' },
                { labelAr: 'الإشعارات', labelEn: 'Notifications', icon: <Bell size={20} />, href: '/notifications' },
                { labelAr: 'ملفي', labelEn: 'My Profile', icon: <Users size={20} />, href: '/profile' },
              ].map((action) => (
                <a
                  key={action.href}
                  href={action.href}
                  className="flex flex-col items-center gap-2 p-4 rounded-xl hover:bg-ocean-foam transition-colors text-center group"
                >
                  <div className="w-10 h-10 rounded-xl bg-ocean-foam group-hover:bg-ocean-pale transition-colors flex items-center justify-center text-ocean-mid">
                    {action.icon}
                  </div>
                  <span className="text-sm text-ocean-wave font-medium">{isAr ? action.labelAr : action.labelEn}</span>
                </a>
              ))}
            </div>
          </motion.div>
        </div>
      </div>
    </DashboardLayout>
  );
}
