'use client';

import React, { useEffect, useRef, useState } from 'react';
import Link from 'next/link';
import { motion, useInView } from 'framer-motion';
import { BookOpen, Users, Award, Clock, ChevronDown, ArrowLeft, ArrowRight, Star, GraduationCap, Lightbulb, Trophy } from 'lucide-react';
import Header from '@/components/layout/Header';
import Footer from '@/components/layout/Footer';
import Logo from '@/components/brand/Logo';
import OceanBackground from '@/components/brand/OceanBackground';
import WaveDivider from '@/components/brand/WaveDivider';
import { useUIStore } from '@/stores/ui.store';

// ─── Animated Counter ────────────────────
function AnimatedCounter({ end, suffix }: { end: number; suffix: string }) {
  const [count, setCount] = useState(0);
  const ref = useRef(null);
  const isInView = useInView(ref, { once: true, margin: '-100px' });

  useEffect(() => {
    if (!isInView) return;
    let start = 0;
    const duration = 2000;
    const increment = end / (duration / 16);
    const timer = setInterval(() => {
      start += increment;
      if (start >= end) {
        setCount(end);
        clearInterval(timer);
      } else {
        setCount(Math.floor(start));
      }
    }, 16);
    return () => clearInterval(timer);
  }, [isInView, end]);

  return (
    <div ref={ref} className="text-center">
      <span className="text-4xl md:text-5xl font-bold text-white" style={{ fontFamily: "'JetBrains Mono', monospace" }}>
        {count.toLocaleString()}+
      </span>
      <p className="mt-2 text-ocean-pale text-lg font-medium">{suffix}</p>
    </div>
  );
}

// ─── Fade-in animation ─────────────
const fadeUp = {
  hidden: { opacity: 0, y: 24 },
  visible: (i: number) => ({
    opacity: 1,
    y: 0,
    transition: { delay: i * 0.1, duration: 0.5, ease: 'easeOut' as const },
  }),
};

export default function LandingPage() {
  const { locale } = useUIStore();
  const isAr = locale === 'ar';
  const [mounted, setMounted] = useState(false);

  useEffect(() => setMounted(true), []);

  // Demo courses
  const featuredCourses = [
    { id: '1', titleAr: 'أساسيات تطوير الويب', titleEn: 'Web Development Fundamentals', instructor: 'أحمد محمد', price: 49.99, students: 1234 },
    { id: '2', titleAr: 'علم البيانات بالبايثون', titleEn: 'Data Science with Python', instructor: 'سارة أحمد', price: 79.99, students: 892 },
    { id: '3', titleAr: 'تصميم واجهات المستخدم', titleEn: 'UI/UX Design Mastery', instructor: 'ليلى حسن', price: 0, students: 2156 },
    { id: '4', titleAr: 'الذكاء الاصطناعي', titleEn: 'Artificial Intelligence', instructor: 'محمد علي', price: 99.99, students: 567 },
  ];

  return (
    <div className="min-h-screen flex flex-col">
      <Header variant="public" />

      <main id="main-content">
        {/* ═══════ HERO SECTION ═══════ */}
        <OceanBackground className="relative">
          <div className="min-h-[85vh] flex items-center justify-center relative z-10">
            <div
              className="text-center px-6 py-20 max-w-4xl mx-auto"
              style={{
                opacity: mounted ? 1 : 0,
                transform: mounted ? 'translateY(0)' : 'translateY(30px)',
                transition: 'opacity 0.8s ease, transform 0.8s ease',
              }}
            >
              <Logo size="xl" white showText={false} className="justify-center mb-8" />

              <h1
                className="text-4xl md:text-5xl lg:text-7xl font-extrabold text-white leading-tight mb-6"
                style={{
                  fontFamily: "'Tajawal', sans-serif",
                  textShadow: '0 2px 20px rgba(0,0,0,0.3)',
                }}
              >
                {isAr ? 'اغمر نفسك في بحر المعرفة' : 'Immerse Yourself in the Sea of Knowledge'}
              </h1>

              <p className="text-lg md:text-xl text-white/80 mb-10 max-w-2xl mx-auto leading-relaxed">
                {isAr
                  ? 'منصة تعليمية عربية متكاملة تربط الطلاب بأفضل المدربين في العالم العربي'
                  : 'A comprehensive Arabic learning platform connecting students with top instructors'}
              </p>

              <div className="flex flex-col sm:flex-row items-center justify-center gap-4">
                <Link
                  href="/register"
                  className="btn-ocean btn-gold text-lg px-10 py-4 rounded-2xl shadow-lg hover:shadow-xl transition-all hover:scale-105"
                >
                  {isAr ? '🚀 ابدأ رحلتك' : '🚀 Start Your Journey'}
                </Link>
                <Link
                  href="/courses"
                  className="btn-ocean text-lg px-8 py-3.5 rounded-2xl border-2 border-white/30 text-white hover:bg-white/10 backdrop-blur-sm transition-all"
                >
                  {isAr ? 'استكشف الدورات' : 'Explore Courses'}
                </Link>
              </div>
            </div>
          </div>

          {/* Scroll indicator */}
          <div className="absolute bottom-6 left-1/2 -translate-x-1/2 z-10 animate-bounce">
            <ChevronDown size={28} className="text-white/40" />
          </div>

          {/* Bottom waves */}
          <div className="absolute bottom-0 left-0 w-full z-10">
            <WaveDivider variant="white-on-blue" />
          </div>
        </OceanBackground>

        {/* ═══════ FEATURED COURSES ═══════ */}
        <section className="py-16 md:py-24 bg-ocean-foam">
          <div className="max-w-6xl mx-auto px-6">
            <motion.div
              initial="hidden"
              whileInView="visible"
              viewport={{ once: true, margin: '-50px' }}
              className="text-center mb-12"
            >
              <motion.span variants={fadeUp} custom={0} className="inline-block badge badge-active mb-4 text-sm">
                {isAr ? '🌊 مختارات بحرية' : '🌊 Ocean Picks'}
              </motion.span>
              <motion.h2
                variants={fadeUp}
                custom={1}
                className="text-3xl md:text-4xl font-bold text-ocean-deep mb-3"
              >
                {isAr ? 'الدورات المميزة' : 'Featured Courses'}
              </motion.h2>
              <motion.p
                variants={fadeUp}
                custom={2}
                className="text-ocean-wave text-lg"
              >
                {isAr ? 'اكتشف أبرز الدورات التعليمية في منصتنا' : 'Discover the best courses on our platform'}
              </motion.p>
            </motion.div>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
              {featuredCourses.map((course, i) => (
                <motion.div
                  key={course.id}
                  initial="hidden"
                  whileInView="visible"
                  viewport={{ once: true }}
                  variants={fadeUp}
                  custom={i + 3}
                  whileHover={{ y: -6 }}
                  className="ocean-card overflow-hidden cursor-pointer group"
                >
                  {/* Thumbnail gradient */}
                  <div className="h-40 bg-gradient-to-br from-ocean-mid to-ocean-wave relative overflow-hidden">
                    <div className="absolute inset-0 bg-ocean-deep/20 group-hover:bg-ocean-deep/5 transition-all duration-500" />
                    <div className="absolute inset-0 flex items-center justify-center">
                      <BookOpen size={40} className="text-white/50 group-hover:scale-110 transition-transform duration-500" />
                    </div>
                    {/* Price badge */}
                    <div className="absolute top-3 left-3">
                      <span className={`text-xs font-bold px-2.5 py-1 rounded-full shadow-sm ${
                        course.price === 0
                          ? 'bg-emerald-500 text-white'
                          : 'bg-gold text-white'
                      }`}>
                        {course.price === 0
                          ? (isAr ? 'مجاني' : 'Free')
                          : `$${course.price}`}
                      </span>
                    </div>
                  </div>

                  <div className="p-4">
                    <h3 className="font-bold text-ocean-deep text-sm mb-1 line-clamp-2 leading-snug">
                      {isAr ? course.titleAr : course.titleEn}
                    </h3>
                    <p className="text-xs text-ocean-surf mb-3">{course.instructor}</p>
                    <div className="flex items-center justify-between text-xs text-ocean-wave">
                      <span className="flex items-center gap-1">
                        <Users size={12} />
                        {course.students.toLocaleString()}
                      </span>
                      <div className="flex items-center gap-0.5">
                        {[1,2,3,4,5].map(s => (
                          <Star key={s} size={10} className="text-gold fill-gold" />
                        ))}
                      </div>
                    </div>
                  </div>
                </motion.div>
              ))}
            </div>

            <div className="text-center mt-10">
              <Link
                href="/courses"
                className="btn-ocean btn-secondary text-sm px-6 py-2.5 rounded-xl"
              >
                {isAr ? 'عرض كل الدورات' : 'View All Courses'}
                {isAr ? <ArrowLeft size={16} /> : <ArrowRight size={16} />}
              </Link>
            </div>
          </div>
        </section>

        {/* ═══════ STATS SECTION ═══════ */}
        <section className="relative">
          <WaveDivider variant="blue-on-white" flip />
          <div className="bg-gradient-to-r from-ocean-deep via-ocean-wave to-ocean-mid py-16 md:py-20">
            <div className="max-w-5xl mx-auto px-6">
              <div className="grid grid-cols-2 md:grid-cols-4 gap-8">
                <AnimatedCounter end={500} suffix={isAr ? 'دورة' : 'Courses'} />
                <AnimatedCounter end={15000} suffix={isAr ? 'طالب' : 'Students'} />
                <AnimatedCounter end={120} suffix={isAr ? 'مدرّب' : 'Instructors'} />
                <AnimatedCounter end={5000} suffix={isAr ? 'ساعة تعليمية' : 'Learning Hours'} />
              </div>
            </div>
          </div>
          <WaveDivider variant="white-on-blue" />
        </section>

        {/* ═══════ HOW IT WORKS ═══════ */}
        <section className="py-16 md:py-24 bg-ocean-foam">
          <div className="max-w-4xl mx-auto px-6">
            <motion.div
              initial="hidden"
              whileInView="visible"
              viewport={{ once: true }}
              className="text-center mb-16"
            >
              <motion.h2
                variants={fadeUp}
                custom={0}
                className="text-3xl md:text-4xl font-bold text-ocean-deep"
              >
                {isAr ? 'كيف تبدأ؟' : 'How It Works'}
              </motion.h2>
            </motion.div>

            <div className="grid grid-cols-1 md:grid-cols-3 gap-10 relative">
              {/* Connection line */}
              <div className="hidden md:block absolute top-[60px] left-[16%] right-[16%] h-[2px] bg-gradient-to-r from-ocean-pale via-ocean-mid to-ocean-pale" />

              {[
                { num: '١', icon: <GraduationCap size={28} />, labelAr: 'سجّل', labelEn: 'Register', descAr: 'أنشئ حسابك مجاناً في ثوانٍ', descEn: 'Create your free account in seconds' },
                { num: '٢', icon: <Lightbulb size={28} />, labelAr: 'تعلّم', labelEn: 'Learn', descAr: 'اختر دورتك وابدأ التعلم فوراً', descEn: 'Choose your course and start learning' },
                { num: '٣', icon: <Trophy size={28} />, labelAr: 'تميّز', labelEn: 'Excel', descAr: 'أتم دوراتك واحصل على شهادتك', descEn: 'Complete courses and earn your certificate' },
              ].map((step, i) => (
                <motion.div
                  key={i}
                  initial="hidden"
                  whileInView="visible"
                  viewport={{ once: true }}
                  variants={fadeUp}
                  custom={i + 1}
                  className="text-center relative"
                >
                  <div className="w-[72px] h-[72px] rounded-2xl bg-gradient-to-br from-ocean-mid to-ocean-light text-white flex items-center justify-center mx-auto mb-5 shadow-ocean-lg relative z-10">
                    {step.icon}
                  </div>
                  <h3 className="text-xl font-bold text-ocean-deep mb-2">
                    {isAr ? step.labelAr : step.labelEn}
                  </h3>
                  <p className="text-ocean-wave text-sm leading-relaxed max-w-[240px] mx-auto">
                    {isAr ? step.descAr : step.descEn}
                  </p>
                </motion.div>
              ))}
            </div>
          </div>
        </section>

        {/* ═══════ TESTIMONIAL ═══════ */}
        <section className="py-16 bg-white">
          <div className="max-w-3xl mx-auto px-6 text-center">
            <motion.div initial="hidden" whileInView="visible" viewport={{ once: true }} variants={fadeUp} custom={0}>
              <div className="flex justify-center gap-1 mb-6">
                {[1,2,3,4,5].map(i => <Star key={i} size={20} className="text-gold fill-gold" />)}
              </div>
              <blockquote className="text-xl md:text-2xl text-ocean-deep font-medium leading-relaxed mb-6" style={{ fontFamily: "'Tajawal', sans-serif" }}>
                "{isAr
                  ? 'منصة أنا البحر غيّرت مسار تعلّمي بالكامل. المحتوى عالي الجودة والمدربون محترفون. أنصح بها لكل طالب عربي.'
                  : 'Ana AlBahr transformed my learning journey completely. High-quality content and professional instructors. I recommend it to every Arabic-speaking student.'}"
              </blockquote>
              <div className="flex items-center justify-center gap-3">
                <div className="w-10 h-10 rounded-full bg-gradient-to-br from-ocean-mid to-ocean-light flex items-center justify-center text-white font-bold text-sm">
                  س
                </div>
                <div className="text-start">
                  <p className="font-semibold text-ocean-deep text-sm">{isAr ? 'سارة أحمد' : 'Sara Ahmed'}</p>
                  <p className="text-xs text-ocean-surf">{isAr ? 'مهندسة برمجيات' : 'Software Engineer'}</p>
                </div>
              </div>
            </motion.div>
          </div>
        </section>

        {/* ═══════ CTA BANNER ═══════ */}
        <section className="relative">
          <WaveDivider variant="dark" flip />
          <div className="bg-ocean-deep py-16 md:py-20 relative overflow-hidden">
            {/* Animated wave background */}
            <div className="absolute inset-0 opacity-[0.07]">
              <svg className="w-full h-full" preserveAspectRatio="none" viewBox="0 0 1440 400">
                <path d="M0,200 C240,100 480,300 720,200 C960,100 1200,300 1440,200 L1440,400 L0,400 Z" fill="#0EA5E9" />
              </svg>
            </div>

            <div className="max-w-3xl mx-auto text-center px-6 relative z-10">
              <motion.div initial="hidden" whileInView="visible" viewport={{ once: true }}>
                <motion.h2
                  variants={fadeUp}
                  custom={0}
                  className="text-3xl md:text-4xl font-bold text-white mb-4"
                >
                  {isAr ? 'ابدأ رحلتك في بحر المعرفة اليوم' : 'Start your journey in the sea of knowledge today'}
                </motion.h2>
                <motion.p variants={fadeUp} custom={1} className="text-ocean-pale/70 mb-8 text-lg">
                  {isAr ? 'أنشئ حسابك المجاني وابدأ التعلم في أقل من دقيقة' : 'Create your free account and start learning in under a minute'}
                </motion.p>
                <motion.div variants={fadeUp} custom={2}>
                  <Link
                    href="/register"
                    className="btn-ocean btn-gold text-lg px-10 py-4 rounded-2xl shadow-xl hover:shadow-2xl hover:scale-105 transition-all inline-flex"
                  >
                    {isAr ? 'سجّل الآن مجاناً' : 'Register for Free'}
                  </Link>
                </motion.div>
              </motion.div>
            </div>
          </div>
        </section>
      </main>

      <Footer />
    </div>
  );
}
