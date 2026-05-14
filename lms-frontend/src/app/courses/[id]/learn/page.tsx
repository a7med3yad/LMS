'use client';

import React, { useState } from 'react';
import { useParams } from 'next/navigation';
import { motion } from 'framer-motion';
import {
  Play, FileText, AlignLeft, Link as LinkIcon,
  ChevronLeft, ChevronRight, Menu, X, ExternalLink, CheckCircle,
} from 'lucide-react';
import { useUIStore } from '@/stores/ui.store';

const materialIcons = {
  Video: <Play size={16} className="text-ocean-mid" />,
  Pdf: <FileText size={16} className="text-red-500" />,
  Text: <AlignLeft size={16} className="text-green-500" />,
  Link: <LinkIcon size={16} className="text-amber-500" />,
};

const demoMaterials = [
  { id: '1', titleAr: 'مقدمة في تطوير الويب', titleEn: 'Intro to Web Dev', type: 'Video' as const, order: 1, isPublished: true, courseId: '', createdAt: '', textContent: '' },
  { id: '2', titleAr: 'أساسيات HTML5', titleEn: 'HTML5 Basics', type: 'Video' as const, order: 2, isPublished: true, courseId: '', createdAt: '', textContent: '' },
  { id: '3', titleAr: 'ملخص CSS', titleEn: 'CSS Summary', type: 'Pdf' as const, order: 3, isPublished: true, courseId: '', createdAt: '', textContent: '' },
  { id: '4', titleAr: 'أفضل ممارسات JavaScript', titleEn: 'JS Best Practices', type: 'Text' as const, order: 4, isPublished: true, courseId: '', createdAt: '', textContent: '# أفضل ممارسات JavaScript\n\nتعلم كيف تكتب كود جافاسكربت نظيف وقابل للصيانة.\n\n## ١. استخدام const و let\n\nتجنب استخدام var واستبدلها بـ const أو let حسب الحاجة.\n\n## ٢. الوظائف السهمية\n\nاستخدم Arrow Functions للاختصار والوضوح.\n\n## ٣. التفكيك (Destructuring)\n\nاستخدم التفكيك لاستخراج القيم من المصفوفات والكائنات.' },
  { id: '5', titleAr: 'موارد إضافية', titleEn: 'Extra Resources', type: 'Link' as const, order: 5, isPublished: true, courseId: '', createdAt: '', contentUrl: 'https://developer.mozilla.org', textContent: '' },
];

export default function CourseLearnPage() {
  const params = useParams();
  const { locale } = useUIStore();
  const isAr = locale === 'ar';
  const [activeMaterial, setActiveMaterial] = useState(demoMaterials[0]);
  const [sidebarOpen, setSidebarOpen] = useState(true);

  return (
    <div className="min-h-screen flex flex-col bg-ocean-foam">
      {/* Top bar */}
      <header className="h-14 bg-white border-b flex items-center px-4 gap-3 sticky top-0 z-30" style={{ borderColor: 'var(--color-border-light)' }}>
        <button
          onClick={() => setSidebarOpen(!sidebarOpen)}
          className="lg:hidden p-2 rounded-lg hover:bg-ocean-foam"
          aria-label="Toggle sidebar"
        >
          {sidebarOpen ? <X size={18} /> : <Menu size={18} />}
        </button>
        <a href={`/courses/${params.id}`} className="text-sm text-ocean-mid hover:text-ocean-light flex items-center gap-1">
          {isAr ? <ChevronRight size={14} /> : <ChevronLeft size={14} />}
          {isAr ? 'العودة للدورة' : 'Back to Course'}
        </a>
        <span className="text-sm text-ocean-surf mx-2">|</span>
        <h1 className="text-sm font-semibold text-ocean-deep truncate">
          {isAr ? 'أساسيات تطوير الويب' : 'Web Development Fundamentals'}
        </h1>
      </header>

      <div className="flex flex-1 relative">
        {/* Sidebar */}
        <motion.aside
          initial={false}
          animate={{ width: sidebarOpen ? 280 : 0 }}
          className="bg-white border-l overflow-hidden flex-shrink-0 h-[calc(100vh-56px)] sticky top-14"
          style={{ borderColor: 'var(--color-border-light)' }}
        >
          <div className="w-[280px] h-full flex flex-col">
            <div className="p-4 border-b" style={{ borderColor: 'var(--color-border-light)' }}>
              <h2 className="font-bold text-ocean-deep text-sm">
                {isAr ? 'المواد التعليمية' : 'Materials'}
              </h2>
            </div>
            <nav className="flex-1 overflow-y-auto p-2 space-y-1">
              {demoMaterials.map((m) => (
                <button
                  key={m.id}
                  onClick={() => setActiveMaterial(m)}
                  className={`w-full flex items-center gap-3 p-3 rounded-xl text-start transition-all text-sm ${
                    activeMaterial.id === m.id
                      ? 'bg-ocean-pale/40 text-ocean-mid'
                      : 'text-ocean-wave hover:bg-ocean-foam'
                  }`}
                >
                  <span className="flex-shrink-0">{materialIcons[m.type]}</span>
                  <span className="flex-1 truncate font-medium">{isAr ? m.titleAr : m.titleEn}</span>
                  <span className="flex-shrink-0">
                    {m.isPublished ? (
                      <CheckCircle size={14} className="text-success" />
                    ) : (
                      <div className="w-3 h-3 rounded-full bg-ocean-surf/30" />
                    )}
                  </span>
                </button>
              ))}
            </nav>
          </div>
        </motion.aside>

        {/* Main content */}
        <main className="flex-1 p-4 md:p-6 lg:p-8">
          <motion.div
            key={activeMaterial.id}
            initial={{ opacity: 0, y: 10 }}
            animate={{ opacity: 1, y: 0 }}
            className="max-w-4xl mx-auto"
          >
            <h2 className="text-xl font-bold text-ocean-deep mb-4">
              {isAr ? activeMaterial.titleAr : activeMaterial.titleEn}
            </h2>

            {/* Content based on type */}
            {activeMaterial.type === 'Video' && (
              <div className="aspect-video bg-ocean-deep rounded-2xl flex items-center justify-center relative overflow-hidden">
                <div className="absolute inset-0 bg-gradient-to-br from-ocean-deep to-ocean-wave opacity-80" />
                <button className="relative z-10 w-20 h-20 rounded-full bg-ocean-mid/90 hover:bg-ocean-mid flex items-center justify-center transition-colors shadow-ocean-lg">
                  <Play size={32} className="text-white ml-1" />
                </button>
              </div>
            )}

            {activeMaterial.type === 'Pdf' && (
              <div className="ocean-card p-6 text-center">
                <FileText size={48} className="text-red-400 mx-auto mb-4" />
                <p className="text-ocean-deep font-medium mb-4">{isAr ? 'عرض ملف PDF' : 'View PDF File'}</p>
                <button className="btn-ocean btn-primary px-6 py-2.5">
                  {isAr ? 'فتح الملف' : 'Open File'}
                </button>
              </div>
            )}

            {activeMaterial.type === 'Text' && (
              <div className="ocean-card p-6 md:p-8">
                <div className="prose prose-ocean max-w-none text-ocean-deep leading-relaxed whitespace-pre-wrap">
                  {activeMaterial.textContent}
                </div>
              </div>
            )}

            {activeMaterial.type === 'Link' && (
              <div className="ocean-card p-6 text-center">
                <ExternalLink size={48} className="text-amber-400 mx-auto mb-4" />
                <p className="text-ocean-deep font-medium mb-2">{isAr ? activeMaterial.titleAr : activeMaterial.titleEn}</p>
                <p className="text-sm text-ocean-surf mb-4">{activeMaterial.contentUrl}</p>
                <a
                  href={activeMaterial.contentUrl}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="btn-ocean btn-gold px-6 py-2.5 inline-flex"
                >
                  {isAr ? 'افتح الرابط' : 'Open Link'}
                  <ExternalLink size={16} />
                </a>
              </div>
            )}

            {/* Navigation */}
            <div className="flex items-center justify-between mt-6">
              <button
                onClick={() => {
                  const idx = demoMaterials.findIndex(m => m.id === activeMaterial.id);
                  if (idx > 0) setActiveMaterial(demoMaterials[idx - 1]);
                }}
                disabled={demoMaterials[0].id === activeMaterial.id}
                className="btn-ocean btn-ghost px-4 py-2 text-sm disabled:opacity-30"
              >
                {isAr ? <ChevronRight size={16} /> : <ChevronLeft size={16} />}
                {isAr ? 'السابق' : 'Previous'}
              </button>
              <span className="text-sm text-ocean-surf">
                {demoMaterials.findIndex(m => m.id === activeMaterial.id) + 1} / {demoMaterials.length}
              </span>
              <button
                onClick={() => {
                  const idx = demoMaterials.findIndex(m => m.id === activeMaterial.id);
                  if (idx < demoMaterials.length - 1) setActiveMaterial(demoMaterials[idx + 1]);
                }}
                disabled={demoMaterials[demoMaterials.length - 1].id === activeMaterial.id}
                className="btn-ocean btn-ghost px-4 py-2 text-sm disabled:opacity-30"
              >
                {isAr ? 'التالي' : 'Next'}
                {isAr ? <ChevronLeft size={16} /> : <ChevronRight size={16} />}
              </button>
            </div>
          </motion.div>
        </main>
      </div>
    </div>
  );
}
