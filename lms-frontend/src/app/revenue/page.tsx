'use client';

import React from 'react';
import { motion } from 'framer-motion';
import { DollarSign, Users, TrendingUp, BarChart3 } from 'lucide-react';
import { AreaChart, Area, BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import DashboardLayout from '@/components/layout/DashboardLayout';
import { useUIStore } from '@/stores/ui.store';

const monthlyData = [
  { month: 'يناير', revenue: 1200 }, { month: 'فبراير', revenue: 1800 },
  { month: 'مارس', revenue: 2400 }, { month: 'أبريل', revenue: 3100 },
  { month: 'مايو', revenue: 2800 }, { month: 'يونيو', revenue: 3600 },
  { month: 'يوليو', revenue: 4200 }, { month: 'أغسطس', revenue: 3900 },
];

const courseData = [
  { name: 'أساسيات الويب', revenue: 4500, students: 120 },
  { name: 'علم البيانات', revenue: 3200, students: 85 },
  { name: 'UI/UX', revenue: 2800, students: 95 },
  { name: 'الأمن السيبراني', revenue: 1900, students: 45 },
];

export default function RevenuePage() {
  const { locale } = useUIStore();
  const isAr = locale === 'ar';

  return (
    <DashboardLayout>
      <div className="space-y-6">
        <motion.h1 initial={{ opacity: 0 }} animate={{ opacity: 1 }} className="text-2xl font-bold text-ocean-deep">
          {isAr ? 'الإيرادات' : 'Revenue'}
        </motion.h1>

        {/* Stats */}
        <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
          {[
            { label: isAr ? 'إجمالي الإيرادات' : 'Total Revenue', value: '$12,450', icon: <DollarSign size={20} />, color: 'from-gold to-amber-400' },
            { label: isAr ? 'إجمالي الطلاب' : 'Total Students', value: '345', icon: <Users size={20} />, color: 'from-ocean-mid to-ocean-light' },
            { label: isAr ? 'متوسط سعر الدورة' : 'Avg Course Price', value: '$64.99', icon: <TrendingUp size={20} />, color: 'from-emerald-500 to-emerald-400' },
          ].map((stat, i) => (
            <motion.div key={stat.label} initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: i * 0.1 }} className="ocean-card p-5 flex items-start gap-4">
              <div className={`w-11 h-11 rounded-xl bg-gradient-to-br ${stat.color} flex items-center justify-center text-white flex-shrink-0`}>
                {stat.icon}
              </div>
              <div>
                <p className="text-2xl font-bold text-ocean-deep" style={{ fontFamily: "'JetBrains Mono', monospace" }}>{stat.value}</p>
                <p className="text-sm text-ocean-surf">{stat.label}</p>
              </div>
            </motion.div>
          ))}
        </div>

        {/* Charts */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
          {/* Monthly revenue */}
          <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.3 }} className="ocean-card p-6">
            <h2 className="text-lg font-bold text-ocean-deep mb-4 flex items-center gap-2">
              <BarChart3 size={18} className="text-ocean-mid" />
              {isAr ? 'الإيرادات الشهرية' : 'Monthly Revenue'}
            </h2>
            <ResponsiveContainer width="100%" height={280}>
              <AreaChart data={monthlyData}>
                <defs>
                  <linearGradient id="revenueGradient" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#0EA5E9" stopOpacity={0.3} />
                    <stop offset="95%" stopColor="#0EA5E9" stopOpacity={0} />
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" stroke="#BAE6FD" />
                <XAxis dataKey="month" tick={{ fontSize: 12, fill: '#075985' }} />
                <YAxis tick={{ fontSize: 12, fill: '#075985' }} />
                <Tooltip contentStyle={{ borderRadius: '10px', border: '1px solid #BAE6FD', fontFamily: "'Tajawal', sans-serif" }} />
                <Area type="monotone" dataKey="revenue" stroke="#0EA5E9" fill="url(#revenueGradient)" strokeWidth={2} />
              </AreaChart>
            </ResponsiveContainer>
          </motion.div>

          {/* Per-course revenue */}
          <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.4 }} className="ocean-card p-6">
            <h2 className="text-lg font-bold text-ocean-deep mb-4">
              {isAr ? 'إيرادات الدورات' : 'Course Revenue'}
            </h2>
            <ResponsiveContainer width="100%" height={280}>
              <BarChart data={courseData}>
                <CartesianGrid strokeDasharray="3 3" stroke="#BAE6FD" />
                <XAxis dataKey="name" tick={{ fontSize: 11, fill: '#075985' }} />
                <YAxis tick={{ fontSize: 12, fill: '#075985' }} />
                <Tooltip contentStyle={{ borderRadius: '10px', border: '1px solid #BAE6FD', fontFamily: "'Tajawal', sans-serif" }} />
                <Bar dataKey="revenue" fill="#0EA5E9" radius={[6, 6, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </motion.div>
        </div>
      </div>
    </DashboardLayout>
  );
}
