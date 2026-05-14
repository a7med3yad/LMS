'use client';

import React from 'react';
import { motion } from 'framer-motion';
import { CreditCard, CheckCircle, XCircle, Clock } from 'lucide-react';
import DashboardLayout from '@/components/layout/DashboardLayout';
import { useUIStore } from '@/stores/ui.store';

const demoPayments = [
  { id: '1', courseTitleAr: 'أساسيات تطوير الويب', courseTitleEn: 'Web Dev Fundamentals', amount: 49.99, currency: 'USD', status: 'Completed', createdAt: '2024-03-01T10:30:00' },
  { id: '2', courseTitleAr: 'علم البيانات', courseTitleEn: 'Data Science', amount: 79.99, currency: 'USD', status: 'Completed', createdAt: '2024-02-15T14:00:00' },
  { id: '3', courseTitleAr: 'الذكاء الاصطناعي', courseTitleEn: 'AI Fundamentals', amount: 99.99, currency: 'USD', status: 'Pending', createdAt: '2024-03-10T09:00:00' },
];

export default function PaymentsPage() {
  const { locale } = useUIStore();
  const isAr = locale === 'ar';

  const statusBadge = (status: string) => {
    switch (status) {
      case 'Completed': return <span className="badge badge-completed flex items-center gap-1"><CheckCircle size={12} /> {isAr ? 'مكتمل' : 'Completed'}</span>;
      case 'Pending': return <span className="badge badge-pending flex items-center gap-1"><Clock size={12} /> {isAr ? 'معلق' : 'Pending'}</span>;
      default: return <span className="badge badge-suspended flex items-center gap-1"><XCircle size={12} /> {isAr ? 'فشل' : 'Failed'}</span>;
    }
  };

  return (
    <DashboardLayout>
      <div className="space-y-6">
        <motion.h1 initial={{ opacity: 0 }} animate={{ opacity: 1 }} className="text-2xl font-bold text-ocean-deep flex items-center gap-2">
          <CreditCard size={24} className="text-ocean-mid" />
          {isAr ? 'مدفوعاتي' : 'My Payments'}
        </motion.h1>

        <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} className="ocean-card overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full text-sm">
              <thead>
                <tr className="bg-ocean-foam border-b" style={{ borderColor: 'var(--color-border-light)' }}>
                  <th className="px-4 py-3 text-start font-semibold text-ocean-deep">{isAr ? 'الدورة' : 'Course'}</th>
                  <th className="px-4 py-3 text-center font-semibold text-ocean-deep">{isAr ? 'المبلغ' : 'Amount'}</th>
                  <th className="px-4 py-3 text-center font-semibold text-ocean-deep">{isAr ? 'الحالة' : 'Status'}</th>
                  <th className="px-4 py-3 text-center font-semibold text-ocean-deep">{isAr ? 'التاريخ' : 'Date'}</th>
                </tr>
              </thead>
              <tbody>
                {demoPayments.map((payment) => (
                  <tr key={payment.id} className="border-b hover:bg-ocean-foam/50 transition-colors" style={{ borderColor: 'var(--color-border-light)' }}>
                    <td className="px-4 py-3 font-medium text-ocean-deep">
                      {isAr ? payment.courseTitleAr : payment.courseTitleEn}
                    </td>
                    <td className="px-4 py-3 text-center font-mono font-semibold text-ocean-deep">
                      ${payment.amount}
                    </td>
                    <td className="px-4 py-3 text-center">
                      {statusBadge(payment.status)}
                    </td>
                    <td className="px-4 py-3 text-center text-ocean-surf text-xs font-mono">
                      {new Date(payment.createdAt).toLocaleDateString()}
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
