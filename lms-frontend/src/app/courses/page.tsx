'use client';

import React, { useState } from 'react';
import Link from 'next/link';
import { motion } from 'framer-motion';
import { useQuery } from '@tanstack/react-query';
import {
  Search, Filter, SlidersHorizontal, Users, BookOpen, Star,
  ChevronDown, Grid3X3, List, X,
} from 'lucide-react';
import Header from '@/components/layout/Header';
import Footer from '@/components/layout/Footer';
import { useUIStore } from '@/stores/ui.store';
import { coursesApi } from '@/lib/api/courses.api';
import type { CourseSummaryDto, CourseFilterParams } from '@/types';

const waveEntrance = {
  hidden: { opacity: 0, y: 20 },
  visible: (i: number) => ({
    opacity: 1, y: 0,
    transition: { delay: i * 0.08, duration: 0.5, ease: 'easeOut' as const }
  }),
};

function CourseCard({ course, index }: { course: CourseSummaryDto; index: number }) {
  const { locale } = useUIStore();
  const isAr = locale === 'ar';

  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ delay: index * 0.06, duration: 0.4, ease: 'easeOut' }}
      whileHover={{ y: -4 }}
    >
      <Link href={`/courses/${course.id}`} className="block ocean-card overflow-hidden group cursor-pointer h-full">
        {/* Thumbnail */}
        <div className="h-44 bg-gradient-to-br from-ocean-mid to-ocean-wave relative overflow-hidden">
          {course.thumbnailUrl ? (
            <img src={course.thumbnailUrl} alt="" className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500" />
          ) : (
            <div className="absolute inset-0 flex items-center justify-center">
              <BookOpen size={40} className="text-white/40" />
            </div>
          )}
          {/* Wave overlay on hover */}
          <div className="absolute inset-0 bg-gradient-to-t from-ocean-deep/40 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300" />
          
          {/* Status badge */}
          <div className="absolute top-3 right-3">
            <span className={`badge ${
              course.status === 'Published' ? 'badge-published' :
              course.status === 'Draft' ? 'badge-draft' : 'badge-archived'
            }`}>
              {isAr ? (course.status === 'Published' ? 'منشور' : course.status === 'Draft' ? 'مسودة' : 'مؤرشف') : course.status}
            </span>
          </div>

          {/* Price */}
          <div className="absolute top-3 left-3">
            <span className={`text-xs font-bold px-2.5 py-1 rounded-full shadow-sm ${
              course.price === 0 ? 'bg-emerald-500 text-white' : 'bg-gold text-white'
            }`}>
              {course.price === 0 ? (isAr ? 'مجاني' : 'Free') : `$${course.price}`}
            </span>
          </div>
        </div>

        {/* Content */}
        <div className="p-4">
          <h3 className="font-bold text-ocean-deep line-clamp-2 mb-1 leading-snug">
            {isAr ? course.titleAr : course.titleEn}
          </h3>
          <p className="text-xs text-ocean-surf line-clamp-1 mb-3">
            {isAr ? course.titleEn : course.titleAr}
          </p>

          {/* Instructor */}
          <div className="flex items-center gap-2 mb-3">
            <div className="w-6 h-6 rounded-full bg-gradient-to-br from-ocean-mid to-ocean-light flex items-center justify-center text-white text-xs font-bold">
              {course.instructor?.fullName?.charAt(0) || '?'}
            </div>
            <span className="text-xs text-ocean-wave">{course.instructor?.fullName}</span>
          </div>

          {/* Stats */}
          <div className="flex items-center justify-between pt-3 border-t" style={{ borderColor: 'var(--color-border-light)' }}>
            <div className="flex items-center gap-3 text-xs text-ocean-surf">
              <span className="flex items-center gap-1"><Users size={12} /> {course.enrollmentCount}</span>
              <span className="flex items-center gap-1"><BookOpen size={12} /> {course.materialCount}</span>
            </div>
            <div className="flex items-center gap-0.5">
              {[1,2,3,4,5].map(s => <Star key={s} size={10} className="text-gold fill-gold" />)}
            </div>
          </div>
        </div>
      </Link>
    </motion.div>
  );
}

export default function CourseCatalogPage() {
  const { locale } = useUIStore();
  const isAr = locale === 'ar';
  const [filters, setFilters] = useState<CourseFilterParams>({ pageNumber: 1, pageSize: 12 });
  const [search, setSearch] = useState('');
  const [showFilters, setShowFilters] = useState(false);

  const { data, isLoading, isError } = useQuery({
    queryKey: ['courses', filters],
    queryFn: () => coursesApi.getAll(filters).then(r => r.data),
    retry: false,
    staleTime: 1000 * 60 * 5,
  });

  const courses = data?.items || [];

  // Demo data for when API is not available
  const demoCourses: CourseSummaryDto[] = courses.length > 0 ? courses : [
    { id: '1', titleAr: 'أساسيات تطوير الويب', titleEn: 'Web Development Fundamentals', descriptionAr: '', descriptionEn: '', price: 49.99, status: 'Published', instructor: { id: '1', fullName: 'أحمد محمد', email: '', role: 'Instructor', isActive: true, createdAt: '' }, enrollmentCount: 1234, materialCount: 24, createdAt: '', updatedAt: '' },
    { id: '2', titleAr: 'علم البيانات والذكاء الاصطناعي', titleEn: 'Data Science & AI', descriptionAr: '', descriptionEn: '', price: 79.99, status: 'Published', instructor: { id: '2', fullName: 'سارة أحمد', email: '', role: 'Instructor', isActive: true, createdAt: '' }, enrollmentCount: 892, materialCount: 36, createdAt: '', updatedAt: '' },
    { id: '3', titleAr: 'تصميم واجهات المستخدم', titleEn: 'UI/UX Design', descriptionAr: '', descriptionEn: '', price: 0, status: 'Published', instructor: { id: '3', fullName: 'ليلى حسن', email: '', role: 'Instructor', isActive: true, createdAt: '' }, enrollmentCount: 2156, materialCount: 18, createdAt: '', updatedAt: '' },
    { id: '4', titleAr: 'تطوير تطبيقات الموبايل', titleEn: 'Mobile App Development', descriptionAr: '', descriptionEn: '', price: 59.99, status: 'Published', instructor: { id: '4', fullName: 'محمد علي', email: '', role: 'Instructor', isActive: true, createdAt: '' }, enrollmentCount: 567, materialCount: 42, createdAt: '', updatedAt: '' },
    { id: '5', titleAr: 'الأمن السيبراني', titleEn: 'Cybersecurity', descriptionAr: '', descriptionEn: '', price: 99.99, status: 'Published', instructor: { id: '5', fullName: 'خالد عبدالله', email: '', role: 'Instructor', isActive: true, createdAt: '' }, enrollmentCount: 345, materialCount: 30, createdAt: '', updatedAt: '' },
    { id: '6', titleAr: 'الحوسبة السحابية', titleEn: 'Cloud Computing', descriptionAr: '', descriptionEn: '', price: 69.99, status: 'Published', instructor: { id: '6', fullName: 'فاطمة محمود', email: '', role: 'Instructor', isActive: true, createdAt: '' }, enrollmentCount: 678, materialCount: 22, createdAt: '', updatedAt: '' },
  ];

  const showSkeleton = isLoading && !isError && demoCourses.length === 0;

  return (
    <div className="min-h-screen flex flex-col">
      <Header variant="public" />

      {/* Hero banner */}
      <div className="hero-gradient py-12 px-6 text-center">
        <h1 className="text-3xl md:text-4xl font-bold text-white mb-4">
          {isAr ? 'الدورات التعليمية' : 'Course Catalog'}
        </h1>
        <p className="text-ocean-pale text-lg">
          {isAr ? 'اكتشف مئات الدورات في مختلف المجالات' : 'Discover hundreds of courses across all fields'}
        </p>

        {/* Search bar */}
        <div className="max-w-xl mx-auto mt-6">
          <div className="relative">
            <Search size={20} className="absolute top-1/2 -translate-y-1/2 text-ocean-surf" style={{ [isAr ? 'right' : 'left']: '16px' }} />
            <input
              type="text"
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              placeholder={isAr ? 'ابحث عن دورة...' : 'Search courses...'}
              className="input-ocean py-3 text-base shadow-lg"
              style={{ [isAr ? 'paddingRight' : 'paddingLeft']: '48px' }}
            />
          </div>
        </div>
      </div>

      {/* Main content */}
      <main id="main-content" className="flex-1 max-w-7xl mx-auto w-full px-4 md:px-6 py-8">
        <div className="flex gap-6">
          {/* Filter sidebar (desktop) */}
          <aside className="hidden lg:block w-64 flex-shrink-0">
            <div className="ocean-card p-5 sticky top-20">
              <h3 className="font-bold text-ocean-deep mb-4 flex items-center gap-2">
                <Filter size={16} />
                {isAr ? 'تصفية' : 'Filters'}
              </h3>

              {/* Status */}
              <div className="mb-5">
                <label className="text-sm font-medium text-ocean-wave mb-2 block">{isAr ? 'الحالة' : 'Status'}</label>
                <div className="space-y-2">
                  {['Published', 'Draft', 'Archived'].map(s => (
                    <label key={s} className="flex items-center gap-2 text-sm text-ocean-deep cursor-pointer">
                      <input type="checkbox" className="rounded border-ocean-pale text-ocean-mid focus:ring-ocean-mid" />
                      {s === 'Published' ? (isAr ? 'منشور' : 'Published') :
                       s === 'Draft' ? (isAr ? 'مسودة' : 'Draft') :
                       (isAr ? 'مؤرشف' : 'Archived')}
                    </label>
                  ))}
                </div>
              </div>

              {/* Price range */}
              <div className="mb-5">
                <label className="text-sm font-medium text-ocean-wave mb-2 block">{isAr ? 'السعر' : 'Price'}</label>
                <div className="flex items-center gap-2">
                  <input type="number" placeholder="0" className="input-ocean text-sm py-1.5 w-full" />
                  <span className="text-ocean-surf">—</span>
                  <input type="number" placeholder="200" className="input-ocean text-sm py-1.5 w-full" />
                </div>
                <label className="flex items-center gap-2 text-sm text-ocean-deep cursor-pointer mt-2">
                  <input type="checkbox" className="rounded border-ocean-pale text-ocean-mid" />
                  {isAr ? 'مجاني فقط' : 'Free only'}
                </label>
              </div>

              {/* Sort */}
              <div>
                <label className="text-sm font-medium text-ocean-wave mb-2 block">{isAr ? 'ترتيب' : 'Sort'}</label>
                <select className="input-ocean text-sm py-1.5">
                  <option>{isAr ? 'الأحدث' : 'Newest'}</option>
                  <option>{isAr ? 'الأكثر تسجيلاً' : 'Most Enrolled'}</option>
                  <option>{isAr ? 'الأقل سعراً' : 'Lowest Price'}</option>
                  <option>{isAr ? 'الأعلى سعراً' : 'Highest Price'}</option>
                </select>
              </div>
            </div>
          </aside>

          {/* Course grid */}
          <div className="flex-1">
            {/* Mobile filter toggle */}
            <div className="lg:hidden mb-4 flex items-center justify-between">
              <button
                onClick={() => setShowFilters(!showFilters)}
                className="btn-ocean btn-secondary text-sm px-4 py-2"
              >
                <SlidersHorizontal size={16} />
                {isAr ? 'تصفية' : 'Filters'}
              </button>
              <p className="text-sm text-ocean-surf">
                {demoCourses.length} {isAr ? 'دورة' : 'courses'}
              </p>
            </div>

            {/* Results count */}
            <div className="hidden lg:flex items-center justify-between mb-6">
              <p className="text-sm text-ocean-wave">
                {isAr ? `عرض ${demoCourses.length} دورة` : `Showing ${demoCourses.length} courses`}
              </p>
            </div>

            {showSkeleton ? (
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {[...Array(6)].map((_, i) => (
                  <div key={i} className="ocean-card overflow-hidden">
                    <div className="h-44 skeleton" />
                    <div className="p-4 space-y-3">
                      <div className="h-4 skeleton w-3/4" />
                      <div className="h-3 skeleton w-1/2" />
                      <div className="h-3 skeleton w-full" />
                    </div>
                  </div>
                ))}
              </div>
            ) : demoCourses.length === 0 ? (
              <div className="text-center py-20">
                <div className="w-20 h-20 rounded-full bg-ocean-foam mx-auto mb-4 flex items-center justify-center">
                  <BookOpen size={32} className="text-ocean-surf" />
                </div>
                <h3 className="text-xl font-bold text-ocean-deep mb-2">
                  {isAr ? 'لم يُعثر على ما تبحث عنه في هذه المياه' : 'Nothing found in these waters'}
                </h3>
                <p className="text-ocean-surf">{isAr ? 'حاول تعديل معايير البحث' : 'Try adjusting your search criteria'}</p>
              </div>
            ) : (
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {demoCourses.map((course, i) => (
                  <CourseCard key={course.id} course={course} index={i} />
                ))}
              </div>
            )}

            {/* Pagination */}
            {data && data.totalPages > 1 && (
              <div className="flex items-center justify-center gap-2 mt-10">
                {Array.from({ length: data.totalPages }, (_, i) => i + 1).map(page => (
                  <button
                    key={page}
                    onClick={() => setFilters(f => ({ ...f, pageNumber: page }))}
                    className={`w-9 h-9 rounded-lg text-sm font-semibold transition-all ${
                      filters.pageNumber === page
                        ? 'bg-ocean-mid text-white'
                        : 'text-ocean-wave hover:bg-ocean-foam'
                    }`}
                  >
                    {page}
                  </button>
                ))}
              </div>
            )}
          </div>
        </div>
      </main>

      <Footer />
    </div>
  );
}
