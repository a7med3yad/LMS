'use client';

import React from 'react';
import Link from 'next/link';
import { motion } from 'framer-motion';
import Logo from '@/components/brand/Logo';
import WaveDivider from '@/components/brand/WaveDivider';
import { useUIStore } from '@/stores/ui.store';

export default function NotFoundPage() {
  const { locale } = useUIStore();
  const isAr = locale === 'ar';

  return (
    <div className="min-h-screen flex flex-col items-center justify-center relative overflow-hidden bg-ocean-foam">
      {/* Animated wave background */}
      <div className="absolute inset-0 opacity-10">
        <svg className="absolute bottom-0 w-full" viewBox="0 0 1440 400" preserveAspectRatio="none">
          <path d="M0,300 C240,200 480,350 720,250 C960,150 1200,300 1440,200 L1440,400 L0,400 Z" fill="#0EA5E9" className="animate-wave" />
          <path d="M0,320 C360,220 600,370 840,270 C1080,170 1320,320 1440,220 L1440,400 L0,400 Z" fill="#38BDF8" className="animate-wave-2" />
          <path d="M0,340 C300,240 540,380 780,280 C1020,180 1260,340 1440,240 L1440,400 L0,400 Z" fill="#BAE6FD" className="animate-wave-3" />
        </svg>
      </div>

      <motion.div
        initial={{ opacity: 0, y: 30 }}
        animate={{ opacity: 1, y: 0 }}
        className="text-center relative z-10 px-6"
      >
        <motion.div
          initial={{ scale: 0.8, opacity: 0 }}
          animate={{ scale: 1, opacity: 1 }}
          transition={{ delay: 0.2 }}
          className="mb-8"
        >
          <Logo size="lg" className="justify-center" />
        </motion.div>

        <motion.h1
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.3 }}
          className="text-6xl md:text-8xl font-extrabold text-ocean-mid mb-4"
          style={{ fontFamily: "'JetBrains Mono', monospace" }}
        >
          404
        </motion.h1>

        <motion.h2
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.4 }}
          className="text-3xl md:text-4xl font-bold text-ocean-deep mb-3"
        >
          {isAr ? 'ضائع في البحر؟' : 'Lost at Sea?'}
        </motion.h2>

        <motion.p
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ delay: 0.5 }}
          className="text-ocean-surf text-lg mb-8 max-w-md mx-auto"
        >
          {isAr
            ? 'الصفحة التي تبحث عنها غير موجودة. ربما جرفتها الأمواج!'
            : "The page you're looking for doesn't exist. Perhaps the waves swept it away!"}
        </motion.p>

        <motion.div
          initial={{ opacity: 0, y: 10 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.6 }}
        >
          <Link
            href="/"
            className="btn-ocean btn-primary text-base px-8 py-3 rounded-2xl inline-flex"
          >
            {isAr ? 'العودة للشاطئ 🏖️' : 'Back to Shore 🏖️'}
          </Link>
        </motion.div>
      </motion.div>
    </div>
  );
}
