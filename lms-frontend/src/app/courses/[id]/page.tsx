'use client';

import React, { useState } from 'react';
import { useParams } from 'next/navigation';
import { motion } from 'framer-motion';
import { useQuery } from '@tanstack/react-query';
import {
  Users, BookOpen, Clock, Award, FileText, Play, Link as LinkIcon,
  AlignLeft, CheckCircle, Tag, ShieldCheck, User,
} from 'lucide-react';
import Header from '@/components/layout/Header';
import Footer from '@/components/layout/Footer';
import WaveDivider from '@/components/brand/WaveDivider';
import { useUIStore } from '@/stores/ui.store';
import { useAuthStore } from '@/stores/auth.store';
import { coursesApi } from '@/lib/api/courses.api';
import Link from 'next/link';

const materialIcons = {
  Video: <Play size={16} className="text-ocean-mid" />,
  Pdf: <FileText size={16} className="text-red-500" />,
  Text: <AlignLeft size={16} className="text-green-500" />,
  Link: <LinkIcon size={16} className="text-amber-500" />,
};

export default function CourseDetailPage() {
  const params = useParams();
  const courseId = params.id as string;
  const { locale } = useUIStore();
  const { isAuthenticated } = useAuthStore();
  const isAr = locale === 'ar';
  const [activeTab, setActiveTab] = useState<'ar' | 'en'>('ar');
  const [voucherCode, setVoucherCode] = useState('');

  const { data: course, isLoading } = useQuery({
    queryKey: ['course', courseId],
    queryFn: () => coursesApi.getById(courseId).then(r => r.data),
  });

  // Demo data
  const demo = course || {
    id: courseId,
    titleAr: 'أساسيات تطوير الويب الحديث',
    titleEn: 'Modern Web Development Fundamentals',
    descriptionAr: 'دورة شاملة تغطي أساسيات تطوير الويب الحديث باستخدام أحدث التقنيات والأدوات. تتضمن HTML5، CSS3، JavaScript، React، وNode.js. ستتعلم كيف تبني تطبيقات ويب متكاملة من الصفر حتى النشر.',
    descriptionEn: 'A comprehensive course covering modern web development fundamentals using the latest technologies and tools. Includes HTML5, CSS3, JavaScript, React, and Node.js. You will learn how to build full-stack web applications from scratch to deployment.',
    price: 49.99,
    status: 'Published' as const,
    thumbnailUrl: null,
    instructor: { id: '1', fullName: 'أحمد محمد', email: 'ahmed@example.com', role: 'Instructor' as const, isActive: true, createdAt: '' },
    enrollmentCount: 1234,
    materialCount: 24,
    createdAt: '2024-01-15',
    updatedAt: '2024-03-01',
    materials: [
      { id: '1', titleAr: 'مقدمة في تطوير الويب', titleEn: 'Introduction to Web Dev', type: 'Video' as const, order: 1, isPublished: true, courseId: '', createdAt: '' },
      { id: '2', titleAr: 'أساسيات HTML', titleEn: 'HTML Fundamentals', type: 'Video' as const, order: 2, isPublished: true, courseId: '', createdAt: '' },
      { id: '3', titleAr: 'ملخص CSS', titleEn: 'CSS Summary', type: 'Pdf' as const, order: 3, isPublished: true, courseId: '', createdAt: '' },
      { id: '4', titleAr: 'مقال: أفضل الممارسات', titleEn: 'Best Practices Article', type: 'Text' as const, order: 4, isPublished: true, courseId: '', createdAt: '' },
      { id: '5', titleAr: 'موارد إضافية', titleEn: 'Additional Resources', type: 'Link' as const, order: 5, isPublished: true, courseId: '', createdAt: '' },
    ],
  };

  return (
    <div className="min-h-screen flex flex-col">
      <Header variant="public" />

      {/* Hero */}
      <div className="hero-gradient py-12 md:py-16 px-6 relative">
        <div className="max-w-6xl mx-auto">
          <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }}>
            <span className={`badge ${demo.status === 'Published' ? 'badge-published' : 'badge-draft'} mb-4`}>
              {isAr ? (demo.status === 'Published' ? 'منشور' : 'مسودة') : demo.status}
            </span>
            <h1 className="text-3xl md:text-4xl font-bold text-white mb-3">
              {isAr ? demo.titleAr : demo.titleEn}
            </h1>
            <p className="text-ocean-pale/80 text-lg mb-4">
              {isAr ? demo.titleEn : demo.titleAr}
            </p>
            <div className="flex items-center gap-4 text-ocean-pale text-sm flex-wrap">
              <span className="flex items-center gap-1.5">
                <User size={14} /> {demo.instructor.fullName}
              </span>
              <span className="flex items-center gap-1.5">
                <Users size={14} /> {demo.enrollmentCount} {isAr ? 'طالب' : 'students'}
              </span>
              <span className="flex items-center gap-1.5">
                <BookOpen size={14} /> {demo.materialCount} {isAr ? 'مادة' : 'materials'}
              </span>
            </div>
          </motion.div>
        </div>
        <div className="absolute bottom-0 left-0 w-full">
          <WaveDivider variant="white-on-blue" />
        </div>
      </div>

      {/* Content */}
      <main id="main-content" className="flex-1 max-w-6xl mx-auto w-full px-4 md:px-6 py-8">
        <div className="flex flex-col lg:flex-row gap-8">
          {/* Left: Course content */}
          <div className="flex-1">
            {/* Description tabs */}
            <div className="ocean-card p-6 mb-6">
              <div className="flex border-b mb-4" style={{ borderColor: 'var(--color-border-light)' }}>
                <button
                  onClick={() => setActiveTab('ar')}
                  className={`px-4 py-2 text-sm font-semibold transition-colors border-b-2 ${
                    activeTab === 'ar'
                      ? 'border-ocean-mid text-ocean-mid'
                      : 'border-transparent text-ocean-surf hover:text-ocean-wave'
                  }`}
                >
                  العربية
                </button>
                <button
                  onClick={() => setActiveTab('en')}
                  className={`px-4 py-2 text-sm font-semibold transition-colors border-b-2 ${
                    activeTab === 'en'
                      ? 'border-ocean-mid text-ocean-mid'
                      : 'border-transparent text-ocean-surf hover:text-ocean-wave'
                  }`}
                >
                  English
                </button>
              </div>
              <div className="prose prose-sm max-w-none text-ocean-deep leading-relaxed">
                {activeTab === 'ar' ? demo.descriptionAr : demo.descriptionEn}
              </div>
            </div>

            {/* Materials preview */}
            <div className="ocean-card p-6 mb-6">
              <h2 className="text-lg font-bold text-ocean-deep mb-4 flex items-center gap-2">
                <BookOpen size={20} className="text-ocean-mid" />
                {isAr ? 'المواد التعليمية' : 'Course Materials'}
              </h2>
              <div className="space-y-2">
                {(demo.materials || []).map((m, i) => (
                  <div
                    key={m.id}
                    className="flex items-center gap-3 p-3 rounded-xl hover:bg-ocean-foam transition-colors"
                  >
                    <span className="w-8 h-8 rounded-lg bg-ocean-foam flex items-center justify-center flex-shrink-0">
                      {materialIcons[m.type]}
                    </span>
                    <div className="flex-1 min-w-0">
                      <p className="text-sm font-medium text-ocean-deep truncate">
                        {isAr ? m.titleAr : m.titleEn}
                      </p>
                      <p className="text-xs text-ocean-surf capitalize">{m.type}</p>
                    </div>
                    <span className="text-xs text-ocean-surf font-mono">{i + 1}</span>
                  </div>
                ))}
              </div>
            </div>
          </div>

          {/* Right: Enrollment card (sticky) */}
          <div className="lg:w-80 flex-shrink-0">
            <div className="ocean-card p-6 lg:sticky lg:top-20">
              {/* Price */}
              <div className="text-center mb-6">
                {demo.price === 0 ? (
                  <span className="text-3xl font-bold text-emerald-600">{isAr ? 'مجاني' : 'Free'}</span>
                ) : (
                  <div>
                    <span className="text-3xl font-bold text-ocean-deep" style={{ fontFamily: "'JetBrains Mono', monospace" }}>
                      ${demo.price}
                    </span>
                    <span className="text-sm text-ocean-surf mr-1"> USD</span>
                  </div>
                )}
              </div>

              {/* Voucher */}
              {demo.price > 0 && (
                <div className="mb-4">
                  <div className="flex gap-2">
                    <input
                      type="text"
                      value={voucherCode}
                      onChange={(e) => setVoucherCode(e.target.value)}
                      placeholder={isAr ? 'كود خصم' : 'Voucher code'}
                      className="input-ocean text-sm flex-1"
                    />
                    <button className="btn-ocean btn-ghost text-sm px-3">
                      <Tag size={14} />
                      {isAr ? 'تطبيق' : 'Apply'}
                    </button>
                  </div>
                </div>
              )}

              {/* Enroll button */}
              <Link
                href={demo.price > 0 ? `/checkout/${demo.id}` : '#'}
                className="btn-ocean btn-gold w-full py-3 text-base rounded-xl shadow-md block text-center"
              >
                {demo.price === 0
                  ? (isAr ? 'سجّل الآن مجاناً' : 'Enroll for Free')
                  : (isAr ? 'سجّل الآن' : 'Enroll Now')}
              </Link>

              {/* Security badges */}
              <div className="flex items-center justify-center gap-2 mt-3">
                <ShieldCheck size={14} className="text-ocean-surf" />
                <span className="text-xs text-ocean-surf">{isAr ? 'دفع آمن' : 'Secure payment'}</span>
              </div>

              {/* Instructor mini-profile */}
              <div className="mt-6 pt-6 border-t" style={{ borderColor: 'var(--color-border-light)' }}>
                <div className="flex items-center gap-3">
                  <div className="w-10 h-10 rounded-full bg-gradient-to-br from-ocean-mid to-ocean-light flex items-center justify-center text-white font-bold">
                    {demo.instructor.fullName.charAt(0)}
                  </div>
                  <div>
                    <p className="text-sm font-semibold text-ocean-deep">{demo.instructor.fullName}</p>
                    <p className="text-xs text-ocean-surf">{isAr ? 'مدرّب' : 'Instructor'}</p>
                  </div>
                </div>
              </div>

              {/* Stats */}
              <div className="mt-4 grid grid-cols-2 gap-3">
                <div className="text-center p-3 rounded-xl bg-ocean-foam">
                  <p className="text-lg font-bold text-ocean-deep" style={{ fontFamily: "'JetBrains Mono', monospace" }}>
                    {demo.enrollmentCount}
                  </p>
                  <p className="text-xs text-ocean-surf">{isAr ? 'طالب' : 'Students'}</p>
                </div>
                <div className="text-center p-3 rounded-xl bg-ocean-foam">
                  <p className="text-lg font-bold text-ocean-deep" style={{ fontFamily: "'JetBrains Mono', monospace" }}>
                    {demo.materialCount}
                  </p>
                  <p className="text-xs text-ocean-surf">{isAr ? 'مادة' : 'Materials'}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </main>

      <Footer />
    </div>
  );
}
