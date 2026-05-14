'use client';

import React, { useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import { Upload, ChevronLeft, ChevronRight, Eye, Save } from 'lucide-react';
import DashboardLayout from '@/components/layout/DashboardLayout';
import WaveDivider from '@/components/brand/WaveDivider';
import { useUIStore } from '@/stores/ui.store';
import type { CreateCourseDto } from '@/types';

export default function CreateCoursePage() {
  const { locale } = useUIStore();
  const isAr = locale === 'ar';
  const [step, setStep] = useState(1);
  const [loading, setLoading] = useState(false);

  const { register, handleSubmit, watch, formState: { errors } } = useForm<CreateCourseDto>({
    defaultValues: { price: 0 },
  });

  const formData = watch();

  const onSubmit = async (data: CreateCourseDto) => {
    setLoading(true);
    try {
      toast.success(isAr ? 'تم إنشاء الدورة بنجاح!' : 'Course created successfully!');
      setTimeout(() => { window.location.href = '/my-courses'; }, 1500);
    } catch {
      toast.error(isAr ? 'فشل إنشاء الدورة' : 'Failed to create course');
    } finally {
      setLoading(false);
    }
  };

  const steps = [
    { num: 1, labelAr: 'معلومات الدورة', labelEn: 'Course Info' },
    { num: 2, labelAr: 'التسعير والصورة', labelEn: 'Pricing & Thumbnail' },
    { num: 3, labelAr: 'مراجعة ونشر', labelEn: 'Review & Publish' },
  ];

  return (
    <DashboardLayout>
      <div className="max-w-3xl mx-auto space-y-6">
        <motion.h1 initial={{ opacity: 0 }} animate={{ opacity: 1 }} className="text-2xl font-bold text-ocean-deep">
          {isAr ? 'إنشاء دورة جديدة' : 'Create New Course'}
        </motion.h1>

        {/* Step progress */}
        <div className="flex items-center justify-center gap-2 mb-6">
          {steps.map((s, i) => (
            <React.Fragment key={s.num}>
              <div className="flex flex-col items-center gap-1">
                <div className={`w-10 h-10 rounded-full flex items-center justify-center text-sm font-bold transition-all ${
                  step >= s.num ? 'bg-ocean-mid text-white' : 'bg-ocean-pale text-ocean-wave'
                }`}>
                  {s.num}
                </div>
                <span className="text-xs text-ocean-surf hidden sm:block">{isAr ? s.labelAr : s.labelEn}</span>
              </div>
              {i < steps.length - 1 && <div className={`w-12 h-1 rounded-full mt-[-20px] ${step > s.num ? 'bg-ocean-mid' : 'bg-ocean-pale'}`} />}
            </React.Fragment>
          ))}
        </div>

        <form onSubmit={handleSubmit(onSubmit)}>
          <AnimatePresence mode="wait">
            {/* Step 1: Info */}
            {step === 1 && (
              <motion.div key="s1" initial={{ opacity: 0, x: 30 }} animate={{ opacity: 1, x: 0 }} exit={{ opacity: 0, x: -30 }} className="ocean-card p-6 space-y-5">
                <div className="relative overflow-hidden rounded-xl">
                  <div className="hero-gradient h-3" />
                </div>
                <h2 className="text-lg font-bold text-ocean-deep">{isAr ? 'معلومات الدورة ثنائية اللغة' : 'Bilingual Course Information'}</h2>
                
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-ocean-wave mb-1.5">{isAr ? 'العنوان (عربي)' : 'Title (Arabic)'}</label>
                    <input className="input-ocean" dir="rtl" placeholder="أدخل عنوان الدورة بالعربية" {...register('titleAr', { required: true })} />
                    {errors.titleAr && <p className="text-error text-xs mt-1">{isAr ? 'مطلوب' : 'Required'}</p>}
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-ocean-wave mb-1.5">{isAr ? 'العنوان (إنجليزي)' : 'Title (English)'}</label>
                    <input className="input-ocean" dir="ltr" placeholder="Enter course title in English" {...register('titleEn', { required: true })} />
                    {errors.titleEn && <p className="text-error text-xs mt-1">{isAr ? 'مطلوب' : 'Required'}</p>}
                  </div>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-ocean-wave mb-1.5">{isAr ? 'الوصف (عربي)' : 'Description (Arabic)'}</label>
                    <textarea className="input-ocean min-h-[120px] resize-y" dir="rtl" placeholder="وصف شامل للدورة بالعربية..." {...register('descriptionAr', { required: true })} />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-ocean-wave mb-1.5">{isAr ? 'الوصف (إنجليزي)' : 'Description (English)'}</label>
                    <textarea className="input-ocean min-h-[120px] resize-y" dir="ltr" placeholder="Comprehensive description in English..." {...register('descriptionEn', { required: true })} />
                  </div>
                </div>

                <div className="flex justify-end">
                  <button type="button" onClick={() => setStep(2)} className="btn-ocean btn-primary px-6 py-2.5">
                    {isAr ? 'التالي' : 'Next'}
                    {isAr ? <ChevronLeft size={16} /> : <ChevronRight size={16} />}
                  </button>
                </div>
              </motion.div>
            )}

            {/* Step 2: Pricing */}
            {step === 2 && (
              <motion.div key="s2" initial={{ opacity: 0, x: 30 }} animate={{ opacity: 1, x: 0 }} exit={{ opacity: 0, x: -30 }} className="ocean-card p-6 space-y-5">
                <div className="relative overflow-hidden rounded-xl">
                  <div className="hero-gradient h-3" />
                </div>
                <h2 className="text-lg font-bold text-ocean-deep">{isAr ? 'التسعير والصورة' : 'Pricing & Thumbnail'}</h2>

                <div>
                  <label className="block text-sm font-medium text-ocean-wave mb-1.5">{isAr ? 'السعر (USD)' : 'Price (USD)'}</label>
                  <input type="number" step="0.01" min="0" className="input-ocean max-w-xs font-mono" placeholder="0.00" {...register('price', { valueAsNumber: true })} />
                  <p className="text-xs text-ocean-surf mt-1">{isAr ? 'اجعله 0 للدورات المجانية' : 'Set to 0 for free courses'}</p>
                </div>

                <div>
                  <label className="block text-sm font-medium text-ocean-wave mb-1.5">{isAr ? 'الصورة المصغرة' : 'Thumbnail'}</label>
                  <div className="border-2 border-dashed border-ocean-pale rounded-2xl p-8 text-center hover:border-ocean-mid transition-colors cursor-pointer">
                    <Upload size={32} className="text-ocean-surf mx-auto mb-3" />
                    <p className="text-sm text-ocean-wave">{isAr ? 'اسحب الصورة هنا أو اضغط للاختيار' : 'Drag image here or click to upload'}</p>
                    <p className="text-xs text-ocean-surf mt-1">PNG, JPG — max 2MB</p>
                  </div>
                </div>

                <div className="flex justify-between">
                  <button type="button" onClick={() => setStep(1)} className="btn-ocean btn-ghost px-6 py-2.5">
                    {isAr ? <ChevronRight size={16} /> : <ChevronLeft size={16} />}
                    {isAr ? 'رجوع' : 'Back'}
                  </button>
                  <button type="button" onClick={() => setStep(3)} className="btn-ocean btn-primary px-6 py-2.5">
                    {isAr ? 'التالي' : 'Next'}
                    {isAr ? <ChevronLeft size={16} /> : <ChevronRight size={16} />}
                  </button>
                </div>
              </motion.div>
            )}

            {/* Step 3: Review */}
            {step === 3 && (
              <motion.div key="s3" initial={{ opacity: 0, x: 30 }} animate={{ opacity: 1, x: 0 }} exit={{ opacity: 0, x: -30 }} className="ocean-card p-6 space-y-5">
                <div className="relative overflow-hidden rounded-xl">
                  <div className="hero-gradient h-3" />
                </div>
                <h2 className="text-lg font-bold text-ocean-deep flex items-center gap-2">
                  <Eye size={18} />
                  {isAr ? 'مراجعة ونشر' : 'Review & Publish'}
                </h2>

                <div className="space-y-4 text-sm">
                  <div className="grid grid-cols-2 gap-4">
                    <div>
                      <p className="text-ocean-surf text-xs mb-1">{isAr ? 'العنوان (عربي)' : 'Title (Arabic)'}</p>
                      <p className="font-semibold text-ocean-deep">{formData.titleAr || '—'}</p>
                    </div>
                    <div>
                      <p className="text-ocean-surf text-xs mb-1">{isAr ? 'العنوان (إنجليزي)' : 'Title (English)'}</p>
                      <p className="font-semibold text-ocean-deep">{formData.titleEn || '—'}</p>
                    </div>
                  </div>
                  <div>
                    <p className="text-ocean-surf text-xs mb-1">{isAr ? 'السعر' : 'Price'}</p>
                    <p className="font-bold text-ocean-deep text-lg font-mono">
                      {formData.price === 0 ? (isAr ? 'مجاني' : 'Free') : `$${formData.price}`}
                    </p>
                  </div>
                </div>

                <div className="flex justify-between pt-4 border-t" style={{ borderColor: 'var(--color-border-light)' }}>
                  <button type="button" onClick={() => setStep(2)} className="btn-ocean btn-ghost px-6 py-2.5">
                    {isAr ? 'رجوع' : 'Back'}
                  </button>
                  <button type="submit" disabled={loading} className="btn-ocean btn-gold px-8 py-2.5 disabled:opacity-50">
                    <Save size={16} />
                    {loading ? (isAr ? 'جاري الإنشاء...' : 'Creating...') : (isAr ? 'إنشاء ونشر' : 'Create & Publish')}
                  </button>
                </div>
              </motion.div>
            )}
          </AnimatePresence>
        </form>
      </div>
    </DashboardLayout>
  );
}
