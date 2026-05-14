'use client';

import React, { useState } from 'react';
import { motion } from 'framer-motion';
import { ClipboardList, Search, Ban } from 'lucide-react';
import DashboardLayout from '@/components/layout/DashboardLayout';
import { useUIStore } from '@/stores/ui.store';

const demoEnrollments = [
  { id: '1', courseTitleAr: 'أساسيات تطوير الويب', courseTitleEn: 'Web Dev', studentName: 'سارة أحمد', status: 'Active', paidAmount: 49.99, enrolledAt: '2024-03-01' },
  { id: '2', courseTitleAr: 'علم البيانات', courseTitleEn: 'Data Science', studentName: 'محمد علي', status: 'Completed', paidAmount: 79.99, enrolledAt: '2024-02-15' },
  { id: '3', courseTitleAr: 'تصميم واجهات', courseTitleEn: 'UI/UX', studentName: 'ليلى حسن', status: 'Suspended', paidAmount: 0, enrolledAt: '2024-01-20' },
  { id: '4', courseTitleAr: 'الذكاء الاصطناعي', courseTitleEn: 'AI', studentName: 'خالد عبدالله', status: 'Active', paidAmount: 99.99, enrolledAt: '2024-03-10' },
];

export default function AdminEnrollmentsPage() {
  const { locale } = useUIStore();
  const isAr = locale === 'ar';
  const [search, setSearch] = useState('');

  const statusBadge = (status: string) => {
    const map: Record<string, string> = { Active: 'badge-active', Completed: 'badge-completed', Suspended: 'badge-suspended', PendingPayment: 'badge-pending' };
    const labelMap: Record<string, string> = { Active: isAr ? 'نشط' : 'Active', Completed: isAr ? 'مكتمل' : 'Completed', Suspended: isAr ? 'معلّق' : 'Suspended', PendingPayment: isAr ? 'في انتظار الدفع' : 'Pending' };
    return <span className={`badge ${map[status] || 'badge-draft'}`}>{labelMap[status] || status}</span>;
  };

  return (
    <DashboardLayout>
      <div className="space-y-6">
        <motion.h1 initial={{ opacity: 0 }} animate={{ opacity: 1 }} className="text-2xl font-bold text-ocean-deep flex items-center gap-2">
          <ClipboardList size={24} className="text-ocean-mid" />
          {isAr ? 'إدارة التسجيلات' : 'Enrollment Management'}
        </motion.h1>

        <div className="relative max-w-md">
          <Search size={18} className="absolute top-1/2 -translate-y-1/2 text-ocean-surf" style={{ [isAr ? 'right' : 'left']: '12px' }} />
          <input type="text" value={search} onChange={(e) => setSearch(e.target.value)}
            placeholder={isAr ? 'ابحث...' : 'Search...'} className="input-ocean" style={{ [isAr ? 'paddingRight' : 'paddingLeft']: '40px' }} />
        </div>

        <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} className="ocean-card overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full text-sm">
              <thead>
                <tr className="bg-ocean-foam border-b" style={{ borderColor: 'var(--color-border-light)' }}>
                  <th className="px-4 py-3 text-start font-semibold text-ocean-deep">{isAr ? 'الطالب' : 'Student'}</th>
                  <th className="px-4 py-3 text-start font-semibold text-ocean-deep">{isAr ? 'الدورة' : 'Course'}</th>
                  <th className="px-4 py-3 text-center font-semibold text-ocean-deep">{isAr ? 'الحالة' : 'Status'}</th>
                  <th className="px-4 py-3 text-center font-semibold text-ocean-deep">{isAr ? 'المبلغ' : 'Amount'}</th>
                  <th className="px-4 py-3 text-center font-semibold text-ocean-deep">{isAr ? 'التاريخ' : 'Date'}</th>
                  <th className="px-4 py-3 text-center font-semibold text-ocean-deep">{isAr ? 'إجراءات' : 'Actions'}</th>
                </tr>
              </thead>
              <tbody>
                {demoEnrollments.map((e) => (
                  <tr key={e.id} className="border-b hover:bg-ocean-foam/50 transition-colors" style={{ borderColor: 'var(--color-border-light)' }}>
                    <td className="px-4 py-3 font-medium text-ocean-deep">{e.studentName}</td>
                    <td className="px-4 py-3 text-ocean-wave">{isAr ? e.courseTitleAr : e.courseTitleEn}</td>
                    <td className="px-4 py-3 text-center">{statusBadge(e.status)}</td>
                    <td className="px-4 py-3 text-center font-mono text-ocean-deep">{e.paidAmount === 0 ? (isAr ? 'مجاني' : 'Free') : `$${e.paidAmount}`}</td>
                    <td className="px-4 py-3 text-center text-ocean-surf text-xs font-mono">{e.enrolledAt}</td>
                    <td className="px-4 py-3 text-center">
                      {e.status === 'Active' && (
                        <button className="p-1.5 rounded-lg text-error hover:bg-red-50 transition-colors" aria-label="Suspend">
                          <Ban size={16} />
                        </button>
                      )}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </motion.div>
      </div>
    </DashboardLayout>
  );
}
