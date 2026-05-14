'use client';

import React, { useState } from 'react';
import { motion } from 'framer-motion';
import { Save, BookOpen, Users, Calendar } from 'lucide-react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import DashboardLayout from '@/components/layout/DashboardLayout';
import WaveDivider from '@/components/brand/WaveDivider';
import AvatarUpload from '@/components/AvatarUpload';
import { useAuthStore } from '@/stores/auth.store';
import { useUIStore } from '@/stores/ui.store';
import { usersApi } from '@/lib/api/users.api';

export default function ProfilePage() {
  const { user, updateUser } = useAuthStore();
  const { locale } = useUIStore();
  const isAr = locale === 'ar';
  const [editing, setEditing] = useState(false);
  const [loading, setLoading] = useState(false);

  const { register, handleSubmit } = useForm({
    defaultValues: { fullName: user?.fullName || '' },
  });

  const onSubmit = async (data: { fullName: string }) => {
    setLoading(true);
    try {
      await usersApi.updateMe(data);
      updateUser({ fullName: data.fullName });
      setEditing(false);
      toast.success(isAr ? 'تم تحديث الملف الشخصي' : 'Profile updated');
    } catch {
      toast.error(isAr ? 'فشل التحديث' : 'Update failed');
    } finally {
      setLoading(false);
    }
  };

  return (
    <DashboardLayout>
      <div className="max-w-3xl mx-auto space-y-6">
        {/* Hero banner */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          className="relative rounded-2xl overflow-hidden"
        >
          <div className="hero-gradient h-40 md:h-52" />
          <div className="absolute bottom-0 left-0 w-full">
            <WaveDivider variant="white-on-blue" />
          </div>

          {/* Avatar — now uses the AvatarUpload component */}
          <div className="absolute bottom-0 left-1/2 -translate-x-1/2 translate-y-1/2">
            <AvatarUpload
              currentAvatarUrl={user?.avatarUrl}
              displayName={user?.fullName || 'U'}
              size={96}
            />
          </div>
        </motion.div>

        {/* Profile card */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.2 }}
          className="ocean-card p-6 pt-16 text-center"
        >
          <h1 className="text-2xl font-bold text-ocean-deep">{user?.fullName}</h1>
          <p className="text-ocean-surf text-sm">{user?.email}</p>
          <span className="inline-block mt-2 badge badge-active">{user?.role}</span>

          {/* Stats */}
          <div className="grid grid-cols-3 gap-4 mt-6 pt-6 border-t" style={{ borderColor: 'var(--color-border-light)' }}>
            <div className="text-center">
              <div className="flex items-center justify-center gap-1 text-ocean-mid mb-1">
                <BookOpen size={16} />
              </div>
              <p className="text-lg font-bold text-ocean-deep" style={{ fontFamily: "'JetBrains Mono', monospace" }}>5</p>
              <p className="text-xs text-ocean-surf">{isAr ? 'الدورات' : 'Courses'}</p>
            </div>
            <div className="text-center">
              <div className="flex items-center justify-center gap-1 text-ocean-mid mb-1">
                <Users size={16} />
              </div>
              <p className="text-lg font-bold text-ocean-deep" style={{ fontFamily: "'JetBrains Mono', monospace" }}>12</p>
              <p className="text-xs text-ocean-surf">{isAr ? 'التسجيلات' : 'Enrollments'}</p>
            </div>
            <div className="text-center">
              <div className="flex items-center justify-center gap-1 text-ocean-mid mb-1">
                <Calendar size={16} />
              </div>
              <p className="text-sm font-bold text-ocean-deep">2024</p>
              <p className="text-xs text-ocean-surf">{isAr ? 'الانضمام' : 'Joined'}</p>
            </div>
          </div>
        </motion.div>

        {/* Edit form */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.3 }}
          className="ocean-card p-6"
        >
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-lg font-bold text-ocean-deep">
              {isAr ? 'المعلومات الشخصية' : 'Personal Information'}
            </h2>
            <button
              onClick={() => setEditing(!editing)}
              className="btn-ocean btn-ghost text-sm px-4 py-2"
            >
              {editing ? (isAr ? 'إلغاء' : 'Cancel') : (isAr ? 'تعديل' : 'Edit')}
            </button>
          </div>

          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-ocean-wave mb-1.5">
                {isAr ? 'الاسم الكامل' : 'Full Name'}
              </label>
              <input
                type="text"
                className="input-ocean"
                disabled={!editing}
                {...register('fullName', { required: true })}
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-ocean-wave mb-1.5">
                {isAr ? 'البريد الإلكتروني' : 'Email'}
              </label>
              <input
                type="email"
                className="input-ocean"
                disabled
                value={user?.email || ''}
              />
            </div>

            {editing && (
              <button
                type="submit"
                disabled={loading}
                className="btn-ocean btn-primary px-6 py-2.5 disabled:opacity-50"
              >
                <Save size={16} />
                {loading ? (isAr ? 'جاري الحفظ...' : 'Saving...') : (isAr ? 'حفظ التغييرات' : 'Save Changes')}
              </button>
            )}
          </form>
        </motion.div>
      </div>
    </DashboardLayout>
  );
}
