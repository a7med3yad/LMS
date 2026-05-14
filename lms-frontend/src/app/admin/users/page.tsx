'use client';

import React, { useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { Search, Users, Shield, ShieldOff, ChevronDown, AlertTriangle } from 'lucide-react';
import DashboardLayout from '@/components/layout/DashboardLayout';
import { useUIStore } from '@/stores/ui.store';
import type { UserRole } from '@/types';

const roleColors: Record<UserRole, string> = {
  Student: 'bg-sky-50 text-sky-700 border-sky-200',
  Instructor: 'bg-blue-50 text-blue-700 border-blue-200',
  Admin: 'bg-ocean-deep text-white border-ocean-deep',
};

const demoUsers = [
  { id: '1', fullName: 'أحمد محمد', email: 'ahmed@example.com', role: 'Instructor' as UserRole, isActive: true, createdAt: '2024-01-15' },
  { id: '2', fullName: 'سارة أحمد', email: 'sara@example.com', role: 'Student' as UserRole, isActive: true, createdAt: '2024-02-20' },
  { id: '3', fullName: 'محمد علي', email: 'mohammed@example.com', role: 'Student' as UserRole, isActive: false, createdAt: '2024-03-01' },
  { id: '4', fullName: 'ليلى حسن', email: 'laila@example.com', role: 'Instructor' as UserRole, isActive: true, createdAt: '2024-01-10' },
  { id: '5', fullName: 'خالد عبدالله', email: 'khalid@example.com', role: 'Admin' as UserRole, isActive: true, createdAt: '2023-11-01' },
  { id: '6', fullName: 'فاطمة محمود', email: 'fatima@example.com', role: 'Student' as UserRole, isActive: true, createdAt: '2024-04-05' },
];

export default function AdminUsersPage() {
  const { locale } = useUIStore();
  const isAr = locale === 'ar';
  const [search, setSearch] = useState('');
  const [confirmAction, setConfirmAction] = useState<{ userId: string; action: 'activate' | 'deactivate' } | null>(null);

  const filtered = demoUsers.filter(u =>
    u.fullName.includes(search) || u.email.includes(search)
  );

  return (
    <DashboardLayout>
      <div className="space-y-6">
        <div className="flex items-center justify-between flex-wrap gap-4">
          <motion.h1 initial={{ opacity: 0 }} animate={{ opacity: 1 }} className="text-2xl font-bold text-ocean-deep">
            <Users size={24} className="inline text-ocean-mid ml-2" />
            {isAr ? 'إدارة المستخدمين' : 'User Management'}
          </motion.h1>
        </div>

        {/* Search */}
        <div className="relative max-w-md">
          <Search size={18} className="absolute top-1/2 -translate-y-1/2 text-ocean-surf" style={{ [isAr ? 'right' : 'left']: '12px' }} />
          <input
            type="text"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            placeholder={isAr ? 'ابحث بالاسم أو البريد...' : 'Search by name or email...'}
            className="input-ocean"
            style={{ [isAr ? 'paddingRight' : 'paddingLeft']: '40px' }}
          />
        </div>

        {/* Table */}
        <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} className="ocean-card overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full text-sm">
              <thead>
                <tr className="bg-ocean-foam border-b" style={{ borderColor: 'var(--color-border-light)' }}>
                  <th className="px-4 py-3 text-start font-semibold text-ocean-deep">{isAr ? 'المستخدم' : 'User'}</th>
                  <th className="px-4 py-3 text-start font-semibold text-ocean-deep">{isAr ? 'البريد' : 'Email'}</th>
                  <th className="px-4 py-3 text-center font-semibold text-ocean-deep">{isAr ? 'الدور' : 'Role'}</th>
                  <th className="px-4 py-3 text-center font-semibold text-ocean-deep">{isAr ? 'الحالة' : 'Status'}</th>
                  <th className="px-4 py-3 text-center font-semibold text-ocean-deep">{isAr ? 'الانضمام' : 'Joined'}</th>
                  <th className="px-4 py-3 text-center font-semibold text-ocean-deep">{isAr ? 'إجراءات' : 'Actions'}</th>
                </tr>
              </thead>
              <tbody>
                {filtered.map((user) => (
                  <tr key={user.id} className="border-b hover:bg-ocean-foam/50 transition-colors" style={{ borderColor: 'var(--color-border-light)' }}>
                    <td className="px-4 py-3">
                      <div className="flex items-center gap-3">
                        <div className="w-8 h-8 rounded-full bg-gradient-to-br from-ocean-mid to-ocean-light flex items-center justify-center text-white text-xs font-bold">
                          {user.fullName.charAt(0)}
                        </div>
                        <span className="font-medium text-ocean-deep">{user.fullName}</span>
                      </div>
                    </td>
                    <td className="px-4 py-3 text-ocean-wave font-mono text-xs">{user.email}</td>
                    <td className="px-4 py-3 text-center">
                      <span className={`badge border ${roleColors[user.role]}`}>{user.role}</span>
                    </td>
                    <td className="px-4 py-3 text-center">
                      <span className={`badge ${user.isActive ? 'badge-published' : 'badge-suspended'}`}>
                        {user.isActive ? (isAr ? 'نشط' : 'Active') : (isAr ? 'معطّل' : 'Inactive')}
                      </span>
                    </td>
                    <td className="px-4 py-3 text-center text-ocean-surf text-xs font-mono">
                      {new Date(user.createdAt).toLocaleDateString()}
                    </td>
                    <td className="px-4 py-3 text-center">
                      <button
                        onClick={() => setConfirmAction({ userId: user.id, action: user.isActive ? 'deactivate' : 'activate' })}
                        className={`p-1.5 rounded-lg transition-colors ${
                          user.isActive
                            ? 'text-error hover:bg-red-50'
                            : 'text-success hover:bg-emerald-50'
                        }`}
                        aria-label={user.isActive ? 'Deactivate' : 'Activate'}
                      >
                        {user.isActive ? <ShieldOff size={16} /> : <Shield size={16} />}
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </motion.div>

        {/* Confirm modal */}
        <AnimatePresence>
          {confirmAction && (
            <motion.div initial={{ opacity: 0 }} animate={{ opacity: 1 }} exit={{ opacity: 0 }}
              className="fixed inset-0 bg-black/40 z-50 flex items-center justify-center p-4"
              onClick={() => setConfirmAction(null)}
            >
              <motion.div
                initial={{ scale: 0.9 }} animate={{ scale: 1 }} exit={{ scale: 0.9 }}
                className="ocean-card p-6 max-w-sm w-full text-center"
                onClick={(e) => e.stopPropagation()}
              >
                <AlertTriangle size={40} className="text-amber-500 mx-auto mb-4" />
                <h3 className="text-lg font-bold text-ocean-deep mb-2">
                  {confirmAction.action === 'activate'
                    ? (isAr ? 'تفعيل المستخدم؟' : 'Activate User?')
                    : (isAr ? 'تعطيل المستخدم؟' : 'Deactivate User?')}
                </h3>
                <div className="flex gap-3 mt-6">
                  <button onClick={() => setConfirmAction(null)} className="btn-ocean btn-secondary flex-1 py-2.5">
                    {isAr ? 'إلغاء' : 'Cancel'}
                  </button>
                  <button
                    onClick={() => setConfirmAction(null)}
                    className={`btn-ocean flex-1 py-2.5 ${confirmAction.action === 'activate' ? 'btn-primary' : 'btn-danger'}`}
                  >
                    {isAr ? 'تأكيد' : 'Confirm'}
                  </button>
                </div>
              </motion.div>
            </motion.div>
          )}
        </AnimatePresence>
      </div>
    </DashboardLayout>
  );
}
