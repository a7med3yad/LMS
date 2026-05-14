'use client';

import React, { useState } from 'react';
import Link from 'next/link';
import { motion } from 'framer-motion';
import { Mail, Lock, Eye, EyeOff, ArrowLeft } from 'lucide-react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import Logo from '@/components/brand/Logo';
import WaveDivider from '@/components/brand/WaveDivider';
import { useAuthStore } from '@/stores/auth.store';
import { useUIStore } from '@/stores/ui.store';
import { authApi } from '@/lib/api/auth.api';
import type { LoginDto } from '@/types';

export default function LoginPage() {
  const { setUser } = useAuthStore();
  const { locale } = useUIStore();
  const isAr = locale === 'ar';
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);

  const { register, handleSubmit, formState: { errors } } = useForm<LoginDto>();

  const onSubmit = async (data: LoginDto) => {
    setLoading(true);
    try {
      const res = await authApi.login(data);
      const { accessToken, userId, fullName, email, role } = res.data;
      setUser({ userId, fullName, email, role }, accessToken);
      toast.success(isAr ? 'تم تسجيل الدخول بنجاح!' : 'Logged in successfully!');
      window.location.href = '/dashboard';
    } catch {
      toast.error(isAr ? 'البريد أو كلمة المرور غير صحيحة' : 'Invalid email or password');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex">
      {/* Left: Ocean gradient */}
      <div className="hidden lg:flex lg:w-1/2 relative hero-gradient items-center justify-center overflow-hidden">
        <div className="relative z-10 text-center px-12">
          <Logo size="xl" white className="justify-center mb-8" />
          <motion.blockquote
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: 0.5 }}
            className="text-2xl text-white/90 font-medium leading-relaxed mb-4"
            style={{ fontFamily: "'Tajawal', sans-serif" }}
          >
            "{isAr ? 'العلم بحر لا ساحل له' : 'Knowledge is a sea with no shore'}"
          </motion.blockquote>
          <p className="text-ocean-pale/70 text-sm">— {isAr ? 'الإمام الشافعي' : "Imam Al-Shafi'i"}</p>
        </div>

        {/* Wave decoration */}
        <div className="absolute bottom-0 left-0 w-full">
          <WaveDivider variant="white-on-blue" />
        </div>

        {/* Floating circles */}
        <div className="absolute top-20 left-20 w-32 h-32 rounded-full bg-ocean-mid/20 animate-float" />
        <div className="absolute bottom-40 right-16 w-24 h-24 rounded-full bg-ocean-light/10 animate-float" style={{ animationDelay: '2s' }} />
      </div>

      {/* Right: Login form */}
      <div className="flex-1 flex items-center justify-center p-6 bg-ocean-foam">
        <motion.div
          initial={{ opacity: 0, x: 20 }}
          animate={{ opacity: 1, x: 0 }}
          className="w-full max-w-md"
        >
          {/* Mobile logo */}
          <div className="lg:hidden mb-8">
            <Logo size="lg" className="justify-center" />
          </div>

          <div className="ocean-card p-8">
            <h1 className="text-2xl font-bold text-ocean-deep mb-2">
              {isAr ? 'تسجيل الدخول' : 'Login'}
            </h1>
            <p className="text-ocean-surf text-sm mb-6">
              {isAr ? 'مرحباً بعودتك! أدخل بياناتك للمتابعة' : 'Welcome back! Enter your details to continue'}
            </p>

            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
              {/* Email */}
              <div>
                <label className="block text-sm font-medium text-ocean-wave mb-1.5">
                  {isAr ? 'البريد الإلكتروني' : 'Email'}
                </label>
                <div className="relative">
                  <Mail size={18} className="absolute top-1/2 -translate-y-1/2 text-ocean-surf" style={{ [isAr ? 'right' : 'left']: '12px' }} />
                  <input
                    type="email"
                    className="input-ocean"
                    style={{ [isAr ? 'paddingRight' : 'paddingLeft']: '40px' }}
                    placeholder={isAr ? 'أدخل بريدك الإلكتروني' : 'Enter your email'}
                    {...register('email', {
                      required: isAr ? 'البريد الإلكتروني مطلوب' : 'Email is required',
                      pattern: { value: /^\S+@\S+$/i, message: isAr ? 'بريد غير صالح' : 'Invalid email' }
                    })}
                  />
                </div>
                {errors.email && <p className="text-error text-xs mt-1">{errors.email.message}</p>}
              </div>

              {/* Password */}
              <div>
                <label className="block text-sm font-medium text-ocean-wave mb-1.5">
                  {isAr ? 'كلمة المرور' : 'Password'}
                </label>
                <div className="relative">
                  <Lock size={18} className="absolute top-1/2 -translate-y-1/2 text-ocean-surf" style={{ [isAr ? 'right' : 'left']: '12px' }} />
                  <input
                    type={showPassword ? 'text' : 'password'}
                    className="input-ocean"
                    style={{ [isAr ? 'paddingRight' : 'paddingLeft']: '40px', [isAr ? 'paddingLeft' : 'paddingRight']: '40px' }}
                    placeholder={isAr ? 'أدخل كلمة المرور' : 'Enter your password'}
                    {...register('password', {
                      required: isAr ? 'كلمة المرور مطلوبة' : 'Password is required',
                      minLength: { value: 6, message: isAr ? '6 أحرف على الأقل' : 'At least 6 characters' }
                    })}
                  />
                  <button
                    type="button"
                    onClick={() => setShowPassword(!showPassword)}
                    className="absolute top-1/2 -translate-y-1/2 text-ocean-surf hover:text-ocean-mid"
                    style={{ [isAr ? 'left' : 'right']: '12px' }}
                    aria-label={showPassword ? 'Hide password' : 'Show password'}
                  >
                    {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                  </button>
                </div>
                {errors.password && <p className="text-error text-xs mt-1">{errors.password.message}</p>}
              </div>

              {/* Forgot password */}
              <div className="text-end">
                <Link href="/forgot-password" className="text-sm text-ocean-mid hover:text-ocean-light transition-colors">
                  {isAr ? 'نسيت كلمة المرور؟' : 'Forgot password?'}
                </Link>
              </div>

              {/* Submit */}
              <button
                type="submit"
                disabled={loading}
                className="btn-ocean btn-primary w-full py-3 rounded-xl text-base disabled:opacity-50"
              >
                {loading ? (isAr ? 'جاري الدخول...' : 'Logging in...') : (isAr ? 'تسجيل الدخول' : 'Login')}
              </button>
            </form>

            {/* Divider */}
            <div className="flex items-center gap-3 my-6">
              <div className="flex-1 h-px bg-ocean-pale" />
              <span className="text-xs text-ocean-surf">{isAr ? 'أو' : 'OR'}</span>
              <div className="flex-1 h-px bg-ocean-pale" />
            </div>

            {/* Social login */}
            <div className="space-y-3">
              <a
                href={authApi.googleLogin()}
                className="flex items-center justify-center gap-3 w-full py-2.5 rounded-xl border border-ocean-pale hover:bg-ocean-foam transition-colors text-sm font-medium text-ocean-deep"
              >
                <svg width="18" height="18" viewBox="0 0 18 18"><path d="M17.64 9.2c0-.637-.057-1.251-.164-1.84H9v3.481h4.844c-.209 1.125-.843 2.078-1.796 2.717v2.258h2.908c1.702-1.567 2.684-3.874 2.684-6.615z" fill="#4285F4"/><path d="M9 18c2.43 0 4.467-.806 5.956-2.18l-2.908-2.259c-.806.54-1.837.86-3.048.86-2.344 0-4.328-1.584-5.036-3.711H.957v2.332C2.438 15.983 5.482 18 9 18z" fill="#34A853"/><path d="M3.964 10.71c-.18-.54-.282-1.117-.282-1.71s.102-1.17.282-1.71V4.958H.957C.347 6.173 0 7.548 0 9s.348 2.827.957 4.042l3.007-2.332z" fill="#FBBC05"/><path d="M9 3.58c1.321 0 2.508.454 3.44 1.345l2.582-2.58C13.463.891 11.426 0 9 0 5.482 0 2.438 2.017.957 4.958L3.964 7.29C4.672 5.163 6.656 3.58 9 3.58z" fill="#EA4335"/></svg>
                Google
              </a>
              <a
                href={authApi.facebookLogin()}
                className="flex items-center justify-center gap-3 w-full py-2.5 rounded-xl border border-ocean-pale hover:bg-ocean-foam transition-colors text-sm font-medium text-ocean-deep"
              >
                <svg width="18" height="18" viewBox="0 0 18 18"><path d="M18 9c0-4.97-4.03-9-9-9S0 4.03 0 9c0 4.49 3.29 8.21 7.59 8.89v-6.29H5.31V9h2.28V7.02c0-2.25 1.34-3.5 3.39-3.5.98 0 2.01.18 2.01.18v2.21h-1.13c-1.12 0-1.46.69-1.46 1.4V9h2.49l-.4 2.6h-2.09v6.29C14.71 17.21 18 13.49 18 9z" fill="#1877F2"/></svg>
                Facebook
              </a>
            </div>

            {/* Register link */}
            <p className="text-center text-sm text-ocean-surf mt-6">
              {isAr ? 'ليس لديك حساب؟' : "Don't have an account?"}{' '}
              <Link href="/register" className="text-ocean-mid font-semibold hover:text-ocean-light transition-colors">
                {isAr ? 'أنشئ حسابك' : 'Register'}
              </Link>
            </p>
          </div>
        </motion.div>
      </div>
    </div>
  );
}
