'use client';

import React from 'react';
import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { motion, AnimatePresence } from 'framer-motion';
import {
  LayoutDashboard, BookOpen, Bell, CreditCard, User,
  DollarSign, Users, ClipboardList, LogOut, ChevronLeft,
  ChevronRight, X,
} from 'lucide-react';
import Logo from '@/components/brand/Logo';
import { useAuthStore } from '@/stores/auth.store';
import { useUIStore } from '@/stores/ui.store';
import type { UserRole } from '@/types';

interface NavItem {
  labelAr: string;
  labelEn: string;
  href: string;
  icon: React.ReactNode;
  roles: UserRole[];
}

const navItems: NavItem[] = [
  {
    labelAr: 'لوحة التحكم', labelEn: 'Dashboard',
    href: '/dashboard', icon: <LayoutDashboard size={20} />,
    roles: ['Student', 'Instructor', 'Admin'],
  },
  {
    labelAr: 'دوراتي', labelEn: 'My Courses',
    href: '/my-courses', icon: <BookOpen size={20} />,
    roles: ['Student', 'Instructor'],
  },
  {
    labelAr: 'الإيرادات', labelEn: 'Revenue',
    href: '/revenue', icon: <DollarSign size={20} />,
    roles: ['Instructor'],
  },
  {
    labelAr: 'المستخدمون', labelEn: 'Users',
    href: '/admin/users', icon: <Users size={20} />,
    roles: ['Admin'],
  },
  {
    labelAr: 'التسجيلات', labelEn: 'Enrollments',
    href: '/admin/enrollments', icon: <ClipboardList size={20} />,
    roles: ['Admin'],
  },
  {
    labelAr: 'الإشعارات', labelEn: 'Notifications',
    href: '/notifications', icon: <Bell size={20} />,
    roles: ['Student', 'Instructor', 'Admin'],
  },
  {
    labelAr: 'المدفوعات', labelEn: 'Payments',
    href: '/payments', icon: <CreditCard size={20} />,
    roles: ['Student'],
  },
  {
    labelAr: 'ملفي', labelEn: 'Profile',
    href: '/profile', icon: <User size={20} />,
    roles: ['Student', 'Instructor', 'Admin'],
  },
];

export default function Sidebar() {
  const { user, logout } = useAuthStore();
  const { sidebarOpen, sidebarCollapsed, locale, setSidebarOpen, toggleSidebarCollapse } = useUIStore();
  const pathname = usePathname();

  const isRTL = locale === 'ar';
  const filteredItems = navItems.filter((item) => user?.role && item.roles.includes(user.role));

  const sidebarContent = (
    <div className="flex flex-col h-full">
      {/* Logo area */}
      <div className="p-4 flex items-center justify-between border-b" style={{ borderColor: 'var(--color-border-light)', height: '64px' }}>
        {!sidebarCollapsed && <Logo size="sm" />}
        <button
          onClick={toggleSidebarCollapse}
          className="hidden lg:flex p-1.5 rounded-lg hover:bg-ocean-foam transition-colors"
          aria-label="Collapse sidebar"
        >
          {sidebarCollapsed ?
            (isRTL ? <ChevronLeft size={18} className="text-ocean-wave" /> : <ChevronRight size={18} className="text-ocean-wave" />) :
            (isRTL ? <ChevronRight size={18} className="text-ocean-wave" /> : <ChevronLeft size={18} className="text-ocean-wave" />)
          }
        </button>
      </div>

      {/* Nav items */}
      <nav className="flex-1 py-3 px-2 space-y-1 overflow-y-auto">
        {filteredItems.map((item) => {
          const isActive = pathname === item.href || pathname.startsWith(item.href + '/');
          return (
            <Link
              key={item.href}
              href={item.href}
              onClick={() => setSidebarOpen(false)}
              className={`
                relative flex items-center gap-3 px-3 py-2.5 rounded-xl text-sm font-medium
                transition-all duration-200 group
                ${isActive
                  ? 'bg-ocean-foam text-ocean-mid'
                  : 'text-ocean-wave hover:bg-ocean-foam hover:text-ocean-mid'
                }
              `}
              title={sidebarCollapsed ? (locale === 'ar' ? item.labelAr : item.labelEn) : undefined}
            >
              {/* Active indicator bar */}
              {isActive && (
                <motion.div
                  layoutId="sidebar-active"
                  className="absolute top-1/2 -translate-y-1/2 w-[3px] h-6 bg-ocean-mid rounded-full"
                  style={{ [isRTL ? 'right' : 'left']: '-8px' }}
                  transition={{ type: 'spring', stiffness: 300, damping: 30 }}
                />
              )}

              <span className="flex-shrink-0">{item.icon}</span>
              {!sidebarCollapsed && (
                <span>{locale === 'ar' ? item.labelAr : item.labelEn}</span>
              )}
            </Link>
          );
        })}
      </nav>

      {/* User section */}
      <div className="border-t p-3" style={{ borderColor: 'var(--color-border-light)' }}>
        {!sidebarCollapsed ? (
          <div className="flex items-center gap-3">
            <div className="w-9 h-9 rounded-full bg-gradient-to-br from-ocean-mid to-ocean-light flex items-center justify-center text-white font-bold text-sm flex-shrink-0">
              {user?.fullName?.charAt(0) || 'U'}
            </div>
            <div className="flex-1 min-w-0">
              <p className="text-sm font-semibold text-ocean-deep truncate">{user?.fullName}</p>
              <span className="text-xs px-1.5 py-0.5 rounded-full bg-ocean-foam text-ocean-mid font-medium">
                {user?.role === 'Student' ? (locale === 'ar' ? 'طالب' : 'Student') :
                 user?.role === 'Instructor' ? (locale === 'ar' ? 'مدرّب' : 'Instructor') :
                 (locale === 'ar' ? 'مدير' : 'Admin')}
              </span>
            </div>
            <button
              onClick={logout}
              className="p-1.5 rounded-lg hover:bg-red-50 text-ocean-surf hover:text-error transition-colors"
              aria-label={locale === 'ar' ? 'تسجيل الخروج' : 'Logout'}
            >
              <LogOut size={16} />
            </button>
          </div>
        ) : (
          <button
            onClick={logout}
            className="w-full flex justify-center p-2 rounded-lg hover:bg-red-50 text-ocean-surf hover:text-error transition-colors"
            aria-label={locale === 'ar' ? 'تسجيل الخروج' : 'Logout'}
          >
            <LogOut size={18} />
          </button>
        )}
      </div>
    </div>
  );

  return (
    <>
      {/* Desktop sidebar */}
      <aside
        className="hidden lg:flex flex-col bg-white border-ocean-surf/20 h-screen sticky top-0 transition-all duration-300"
        style={{
          width: sidebarCollapsed ? '64px' : '240px',
          [isRTL ? 'borderLeft' : 'borderRight']: '1px solid var(--color-border-light)',
        }}
      >
        {sidebarContent}
      </aside>

      {/* Mobile sidebar overlay */}
      <AnimatePresence>
        {sidebarOpen && (
          <>
            <motion.div
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              exit={{ opacity: 0 }}
              className="lg:hidden fixed inset-0 bg-black/40 z-40"
              onClick={() => setSidebarOpen(false)}
            />
            <motion.aside
              initial={{ x: isRTL ? 280 : -280 }}
              animate={{ x: 0 }}
              exit={{ x: isRTL ? 280 : -280 }}
              transition={{ type: 'spring', stiffness: 300, damping: 30 }}
              className="lg:hidden fixed top-0 h-full w-[240px] bg-white z-50 shadow-ocean-xl"
              style={{ [isRTL ? 'right' : 'left']: 0 }}
            >
              <button
                onClick={() => setSidebarOpen(false)}
                className="absolute top-4 p-1.5 rounded-lg hover:bg-ocean-foam"
                style={{ [isRTL ? 'left' : 'right']: '12px' }}
                aria-label="Close sidebar"
              >
                <X size={18} className="text-ocean-wave" />
              </button>
              {sidebarContent}
            </motion.aside>
          </>
        )}
      </AnimatePresence>
    </>
  );
}
