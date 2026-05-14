'use client';

import React from 'react';
import Link from 'next/link';
import Logo from '@/components/brand/Logo';
import WaveDivider from '@/components/brand/WaveDivider';
import { useUIStore } from '@/stores/ui.store';

export default function Footer() {
  const { locale } = useUIStore();
  const isAr = locale === 'ar';

  return (
    <footer className="relative mt-auto">
      <WaveDivider variant="blue-on-white" />
      <div
        className="bg-ocean-deep text-white pt-12 pb-8"
      >
        <div className="max-w-6xl mx-auto px-6">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8 mb-8">
            {/* Brand */}
            <div>
              <Logo size="md" white />
              <p className="mt-3 text-ocean-pale/80 text-sm leading-relaxed">
                {isAr
                  ? 'منصة تعليمية عربية متكاملة تربط الطلاب بأفضل المدربين في العالم العربي.'
                  : 'A comprehensive Arabic learning platform connecting students with top instructors.'}
              </p>
            </div>

            {/* Quick Links */}
            <div>
              <h4 className="text-sm font-bold text-ocean-pale mb-4">
                {isAr ? 'روابط سريعة' : 'Quick Links'}
              </h4>
              <ul className="space-y-2">
                {[
                  { href: '/courses', labelAr: 'الدورات', labelEn: 'Courses' },
                  { href: '/about', labelAr: 'عن المنصة', labelEn: 'About' },
                  { href: '/register', labelAr: 'إنشاء حساب', labelEn: 'Register' },
                  { href: '/login', labelAr: 'تسجيل الدخول', labelEn: 'Login' },
                ].map((link) => (
                  <li key={link.href}>
                    <Link
                      href={link.href}
                      className="text-sm text-ocean-pale/70 hover:text-white transition-colors"
                    >
                      {isAr ? link.labelAr : link.labelEn}
                    </Link>
                  </li>
                ))}
              </ul>
            </div>

            {/* Contact */}
            <div>
              <h4 className="text-sm font-bold text-ocean-pale mb-4">
                {isAr ? 'تواصل معنا' : 'Contact Us'}
              </h4>
              <p className="text-sm text-ocean-pale/70">
                {isAr ? 'البريد: info@ana-albahr.com' : 'Email: info@ana-albahr.com'}
              </p>
            </div>
          </div>

          {/* Bottom bar */}
          <div className="border-t border-ocean-wave/30 pt-6 flex flex-col md:flex-row items-center justify-between gap-2">
            <p className="text-xs text-ocean-pale/50">
              © {new Date().getFullYear()} أنا البحر. {isAr ? 'جميع الحقوق محفوظة.' : 'All rights reserved.'}
            </p>
            <p className="text-xs text-ocean-pale/50">
              {isAr ? 'صُنع بشغف للعالم العربي ❤️' : 'Made with passion for the Arab world ❤️'}
            </p>
          </div>
        </div>
      </div>
    </footer>
  );
}
