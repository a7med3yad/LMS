'use client';

import React, { useState, useRef, useEffect } from 'react';
import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { motion, AnimatePresence } from 'framer-motion';
import {
  Search, Bell, Moon, Sun, Menu, X, ChevronDown,
  User, LogOut, Settings, Globe,
} from 'lucide-react';
import Logo from '@/components/brand/Logo';
import { useAuthStore } from '@/stores/auth.store';
import { useUIStore } from '@/stores/ui.store';

interface HeaderProps {
  variant?: 'public' | 'dashboard';
}

export default function Header({ variant = 'public' }: HeaderProps) {
  const { user, isAuthenticated, logout } = useAuthStore();
  const { locale, setLocale, theme, setTheme, toggleSidebar } = useUIStore();
  const [searchOpen, setSearchOpen] = useState(false);
  const [profileOpen, setProfileOpen] = useState(false);
  const [notifOpen, setNotifOpen] = useState(false);
  const profileRef = useRef<HTMLDivElement>(null);
  const notifRef = useRef<HTMLDivElement>(null);
  const pathname = usePathname();

  const isRTL = locale === 'ar';

  // Close dropdowns on outside click
  useEffect(() => {
    const handleClick = (e: MouseEvent) => {
      if (profileRef.current && !profileRef.current.contains(e.target as Node)) setProfileOpen(false);
      if (notifRef.current && !notifRef.current.contains(e.target as Node)) setNotifOpen(false);
    };
    document.addEventListener('mousedown', handleClick);
    return () => document.removeEventListener('mousedown', handleClick);
  }, []);

  const toggleTheme = () => {
    const next = theme === 'light' ? 'dark' : 'light';
    setTheme(next);
    document.documentElement.setAttribute('data-theme', next);
  };

  const toggleLocale = () => {
    const next = locale === 'ar' ? 'en' : 'ar';
    setLocale(next);
    document.documentElement.setAttribute('dir', next === 'ar' ? 'rtl' : 'ltr');
    document.documentElement.setAttribute('lang', next);
  };

  return (
    <header
      className="sticky top-0 z-50 bg-white/95 backdrop-blur-md border-b"
      style={{ borderColor: 'var(--color-border)', height: '64px' }}
    >
      <div className="h-full max-w-[1440px] mx-auto px-4 flex items-center justify-between gap-4">
        {/* Left: Logo + Mobile menu */}
        <div className="flex items-center gap-3">
          {variant === 'dashboard' && (
            <button
              onClick={toggleSidebar}
              className="lg:hidden p-2 rounded-lg hover:bg-ocean-foam transition-colors"
              aria-label="Toggle menu"
            >
              <Menu size={20} className="text-ocean-deep" />
            </button>
          )}
          <Link href="/" className="flex items-center">
            <Logo size="md" />
          </Link>
        </div>

        {/* Center: Search (desktop) */}
        <div className="hidden md:flex flex-1 max-w-md mx-4">
          <div className="relative w-full">
            <Search
              size={18}
              className="absolute top-1/2 -translate-y-1/2 text-ocean-surf"
              style={{ [isRTL ? 'right' : 'left']: '12px' }}
            />
            <input
              type="text"
              placeholder={locale === 'ar' ? 'ابحث عن دورة...' : 'Search courses...'}
              className="input-ocean w-full"
              style={{ [isRTL ? 'paddingRight' : 'paddingLeft']: '40px' }}
            />
          </div>
        </div>

        {/* Right: Actions */}
        <div className="flex items-center gap-2">
          {/* Mobile search toggle */}
          <button
            onClick={() => setSearchOpen(!searchOpen)}
            className="md:hidden p-2 rounded-lg hover:bg-ocean-foam transition-colors"
            aria-label="Search"
          >
            <Search size={18} className="text-ocean-wave" />
          </button>

          {/* Language toggle */}
          <button
            onClick={toggleLocale}
            className="flex items-center gap-1 px-3 py-1.5 rounded-lg hover:bg-ocean-foam transition-colors text-sm font-semibold text-ocean-wave"
            aria-label="Toggle language"
          >
            <Globe size={16} />
            <span>{locale === 'ar' ? 'EN' : 'ع'}</span>
          </button>

          {/* Theme toggle */}
          <button
            onClick={toggleTheme}
            className="p-2 rounded-lg hover:bg-ocean-foam transition-colors"
            aria-label={theme === 'light' ? 'Dark mode' : 'Light mode'}
          >
            {theme === 'light' ? (
              <Moon size={18} className="text-ocean-wave" />
            ) : (
              <Sun size={18} className="text-gold" />
            )}
          </button>

          {isAuthenticated ? (
            <>
              {/* Notification bell */}
              <div ref={notifRef} className="relative">
                <button
                  onClick={() => setNotifOpen(!notifOpen)}
                  className="relative p-2 rounded-lg hover:bg-ocean-foam transition-colors"
                  aria-label="Notifications"
                >
                  <Bell size={18} className="text-ocean-wave" />
                  <span className="absolute top-1 right-1 w-2 h-2 bg-ocean-mid rounded-full animate-pulse-blue" />
                </button>

                <AnimatePresence>
                  {notifOpen && (
                    <motion.div
                      initial={{ opacity: 0, y: 8, scale: 0.95 }}
                      animate={{ opacity: 1, y: 0, scale: 1 }}
                      exit={{ opacity: 0, y: 8, scale: 0.95 }}
                      className="absolute top-full mt-2 bg-white rounded-xl shadow-ocean-lg border overflow-hidden"
                      style={{
                        [isRTL ? 'left' : 'right']: 0,
                        width: '320px',
                        borderColor: 'var(--color-border-light)',
                      }}
                    >
                      <div className="p-4 border-b flex items-center justify-between" style={{ borderColor: 'var(--color-border-light)' }}>
                        <h3 className="font-bold text-ocean-deep">{locale === 'ar' ? 'الإشعارات' : 'Notifications'}</h3>
                        <button className="text-sm text-ocean-mid hover:text-ocean-light">
                          {locale === 'ar' ? 'اقرأ الكل' : 'Mark all read'}
                        </button>
                      </div>
                      <div className="max-h-80 overflow-y-auto p-2">
                        <p className="text-center text-ocean-surf py-8 text-sm">
                          {locale === 'ar' ? 'المياه هادئة — لا إشعارات جديدة' : 'Calm waters — no new notifications'}
                        </p>
                      </div>
                    </motion.div>
                  )}
                </AnimatePresence>
              </div>

              {/* Profile dropdown */}
              <div ref={profileRef} className="relative">
                <button
                  onClick={() => setProfileOpen(!profileOpen)}
                  className="flex items-center gap-2 p-1.5 rounded-xl hover:bg-ocean-foam transition-colors"
                >
                  <div className="w-8 h-8 rounded-full bg-gradient-to-br from-ocean-mid to-ocean-light flex items-center justify-center text-white text-sm font-bold">
                    {user?.fullName?.charAt(0) || 'U'}
                  </div>
                  <ChevronDown size={14} className="text-ocean-wave hidden sm:block" />
                </button>

                <AnimatePresence>
                  {profileOpen && (
                    <motion.div
                      initial={{ opacity: 0, y: 8, scale: 0.95 }}
                      animate={{ opacity: 1, y: 0, scale: 1 }}
                      exit={{ opacity: 0, y: 8, scale: 0.95 }}
                      className="absolute top-full mt-2 bg-white rounded-xl shadow-ocean-lg border overflow-hidden"
                      style={{
                        [isRTL ? 'left' : 'right']: 0,
                        width: '220px',
                        borderColor: 'var(--color-border-light)',
                      }}
                    >
                      <div className="p-3 border-b" style={{ borderColor: 'var(--color-border-light)' }}>
                        <p className="font-semibold text-ocean-deep text-sm">{user?.fullName}</p>
                        <p className="text-xs text-ocean-surf">{user?.email}</p>
                        <span className="inline-block mt-1 text-xs px-2 py-0.5 rounded-full bg-ocean-foam text-ocean-mid font-semibold">
                          {user?.role}
                        </span>
                      </div>
                      <div className="p-1">
                        <Link
                          href="/profile"
                          className="flex items-center gap-2 px-3 py-2 rounded-lg text-sm text-ocean-wave hover:bg-ocean-foam transition-colors"
                        >
                          <User size={16} />
                          {locale === 'ar' ? 'ملفي الشخصي' : 'My Profile'}
                        </Link>
                        <Link
                          href="/settings"
                          className="flex items-center gap-2 px-3 py-2 rounded-lg text-sm text-ocean-wave hover:bg-ocean-foam transition-colors"
                        >
                          <Settings size={16} />
                          {locale === 'ar' ? 'الإعدادات' : 'Settings'}
                        </Link>
                        <button
                          onClick={() => { logout(); setProfileOpen(false); }}
                          className="w-full flex items-center gap-2 px-3 py-2 rounded-lg text-sm text-error hover:bg-red-50 transition-colors"
                        >
                          <LogOut size={16} />
                          {locale === 'ar' ? 'تسجيل الخروج' : 'Logout'}
                        </button>
                      </div>
                    </motion.div>
                  )}
                </AnimatePresence>
              </div>
            </>
          ) : (
            <div className="flex items-center gap-2">
              <Link
                href="/login"
                className="btn-ocean btn-ghost text-sm px-4 py-2"
              >
                {locale === 'ar' ? 'دخول' : 'Login'}
              </Link>
              <Link
                href="/register"
                className="btn-ocean btn-primary text-sm px-4 py-2"
              >
                {locale === 'ar' ? 'إنشاء حساب' : 'Register'}
              </Link>
            </div>
          )}
        </div>
      </div>

      {/* Mobile search bar */}
      <AnimatePresence>
        {searchOpen && (
          <motion.div
            initial={{ height: 0, opacity: 0 }}
            animate={{ height: 'auto', opacity: 1 }}
            exit={{ height: 0, opacity: 0 }}
            className="md:hidden border-t px-4 py-3 bg-white"
            style={{ borderColor: 'var(--color-border-light)' }}
          >
            <div className="relative">
              <Search size={18} className="absolute top-1/2 -translate-y-1/2 left-3 text-ocean-surf" />
              <input
                type="text"
                placeholder={locale === 'ar' ? 'ابحث عن دورة...' : 'Search courses...'}
                className="input-ocean w-full pl-10"
                autoFocus
              />
            </div>
          </motion.div>
        )}
      </AnimatePresence>
    </header>
  );
}
