'use client';

import React, { useState } from 'react';
import Link from 'next/link';
import { motion, AnimatePresence } from 'framer-motion';
import { Mail, Lock, Eye, EyeOff, User as UserIcon, GraduationCap, BookOpen } from 'lucide-react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import Logo from '@/components/brand/Logo';
import WaveDivider from '@/components/brand/WaveDivider';
import { useAuthStore } from '@/stores/auth.store';
import { useUIStore } from '@/stores/ui.store';
import { authApi } from '@/lib/api/auth.api';
import type { RegisterDto } from '@/types';

interface RegisterFormData extends RegisterDto {
  confirmPassword: string;
}

export default function RegisterPage() {
  const { setUser } = useAuthStore();
  const { locale } = useUIStore();
  const isAr = locale === 'ar';
  const [step, setStep] = useState(1);
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const [otp, setOtp] = useState(['', '', '', '', '', '']);
  const [registeredEmail, setRegisteredEmail] = useState('');

  const { register, handleSubmit, watch, setValue, formState: { errors } } = useForm<RegisterFormData>({
    defaultValues: { role: 'Student' }
  });

  const selectedRole = watch('role');

  const onSubmitStep1 = async (data: RegisterFormData) => {
    setLoading(true);
    try {
      await authApi.register({
        fullName: data.fullName,
        email: data.email,
        password: data.password,
        role: data.role as 'Student' | 'Instructor',
      });
      setRegisteredEmail(data.email);
      setStep(2);
      toast.success(isAr ? 'تم إرسال رمز التحقق إلى بريدك' : 'Verification code sent to your email');
    } catch {
      toast.error(isAr ? 'حدث خطأ أثناء التسجيل' : 'Registration failed');
    } finally {
      setLoading(false);
    }
  };

  const handleOtpChange = (index: number, value: string) => {
    if (value.length > 1) return;
    const newOtp = [...otp];
    newOtp[index] = value;
    setOtp(newOtp);
    // Auto-focus next
    if (value && index < 5) {
      const next = document.getElementById(`otp-${index + 1}`);
      next?.focus();
    }
  };

  const handleVerify = async () => {
    const code = otp.join('');
    if (code.length !== 6) return;
    setLoading(true);
    try {
      await authApi.verifyEmail({ email: registeredEmail, otp: code });
      toast.success(isAr ? 'تم التحقق بنجاح! جاري تسجيل الدخول...' : 'Verified! Logging in...');
      // Auto login
      const { password } = watch() as RegisterFormData;
      const res = await authApi.login({ email: registeredEmail, password });
      const { accessToken, userId, fullName, email, role } = res.data;
      setUser({ userId, fullName, email, role }, accessToken);
      window.location.href = '/dashboard';
    } catch {
      toast.error(isAr ? 'رمز التحقق غير صحيح' : 'Invalid verification code');
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
          <motion.h2
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            className="text-3xl text-white font-bold mb-4"
          >
            {isAr ? 'انضم إلى بحر المعرفة' : 'Join the Sea of Knowledge'}
          </motion.h2>
          <motion.p
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: 0.2 }}
            className="text-ocean-pale/80 text-lg"
          >
            {isAr ? 'سجّل الآن وابدأ رحلتك التعليمية' : 'Register now and start your learning journey'}
          </motion.p>
        </div>
        <div className="absolute bottom-0 left-0 w-full">
          <WaveDivider variant="white-on-blue" />
        </div>
        <div className="absolute top-20 left-20 w-32 h-32 rounded-full bg-ocean-mid/20 animate-float" />
        <div className="absolute bottom-40 right-16 w-24 h-24 rounded-full bg-ocean-light/10 animate-float" style={{ animationDelay: '2s' }} />
      </div>

      {/* Right: Register form */}
      <div className="flex-1 flex items-center justify-center p-6 bg-ocean-foam">
        <motion.div
          initial={{ opacity: 0, x: 20 }}
          animate={{ opacity: 1, x: 0 }}
          className="w-full max-w-md"
        >
          <div className="lg:hidden mb-8">
            <Logo size="lg" className="justify-center" />
          </div>

          {/* Step Progress */}
          <div className="flex items-center justify-center gap-2 mb-6">
            {[1, 2].map((s) => (
              <React.Fragment key={s}>
                <div className={`w-8 h-8 rounded-full flex items-center justify-center text-sm font-bold transition-colors ${
                  step >= s ? 'bg-ocean-mid text-white' : 'bg-ocean-pale text-ocean-wave'
                }`}>
                  {s}
                </div>
                {s < 2 && <div className={`w-12 h-1 rounded-full transition-colors ${step > s ? 'bg-ocean-mid' : 'bg-ocean-pale'}`} />}
              </React.Fragment>
            ))}
          </div>

          <div className="ocean-card p-8">
            <AnimatePresence mode="wait">
              {step === 1 && (
                <motion.div
                  key="step1"
                  initial={{ opacity: 0, x: 20 }}
                  animate={{ opacity: 1, x: 0 }}
                  exit={{ opacity: 0, x: -20 }}
                >
                  <h1 className="text-2xl font-bold text-ocean-deep mb-2">
                    {isAr ? 'إنشاء حساب' : 'Create Account'}
                  </h1>
                  <p className="text-ocean-surf text-sm mb-6">
                    {isAr ? 'أدخل بياناتك لإنشاء حسابك' : 'Enter your details to create your account'}
                  </p>

                  <form onSubmit={handleSubmit(onSubmitStep1)} className="space-y-4">
                    {/* Role selector */}
                    <div>
                      <label className="block text-sm font-medium text-ocean-wave mb-2">
                        {isAr ? 'اختر دورك' : 'Select your role'}
                      </label>
                      <div className="grid grid-cols-2 gap-3">
                        {[
                          { value: 'Student', icon: <GraduationCap size={24} />, labelAr: 'طالب', labelEn: 'Student', descAr: 'تعلّم من أفضل المدربين', descEn: 'Learn from the best' },
                          { value: 'Instructor', icon: <BookOpen size={24} />, labelAr: 'مدرّب', labelEn: 'Instructor', descAr: 'شارك معرفتك', descEn: 'Share your knowledge' },
                        ].map((role) => (
                          <button
                            key={role.value}
                            type="button"
                            onClick={() => setValue('role', role.value as 'Student' | 'Instructor')}
                            className={`p-4 rounded-xl border-2 text-center transition-all ${
                              selectedRole === role.value
                                ? 'border-ocean-mid bg-ocean-foam'
                                : 'border-ocean-pale hover:border-ocean-surf'
                            }`}
                          >
                            <div className={`mb-2 mx-auto w-fit ${selectedRole === role.value ? 'text-ocean-mid' : 'text-ocean-surf'}`}>
                              {role.icon}
                            </div>
                            <p className="font-semibold text-sm text-ocean-deep">{isAr ? role.labelAr : role.labelEn}</p>
                            <p className="text-xs text-ocean-surf mt-1">{isAr ? role.descAr : role.descEn}</p>
                          </button>
                        ))}
                      </div>
                    </div>

                    {/* Full Name */}
                    <div>
                      <label className="block text-sm font-medium text-ocean-wave mb-1.5">
                        {isAr ? 'الاسم الكامل' : 'Full Name'}
                      </label>
                      <div className="relative">
                        <UserIcon size={18} className="absolute top-1/2 -translate-y-1/2 text-ocean-surf" style={{ [isAr ? 'right' : 'left']: '12px' }} />
                        <input
                          type="text"
                          className="input-ocean"
                          style={{ [isAr ? 'paddingRight' : 'paddingLeft']: '40px' }}
                          placeholder={isAr ? 'أدخل اسمك الكامل' : 'Enter your full name'}
                          {...register('fullName', { required: isAr ? 'الاسم مطلوب' : 'Name is required' })}
                        />
                      </div>
                      {errors.fullName && <p className="text-error text-xs mt-1">{errors.fullName.message}</p>}
                    </div>

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
                            required: isAr ? 'البريد مطلوب' : 'Email is required',
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
                            required: isAr ? 'كلمة المرور مطلوبة' : 'Password required',
                            minLength: { value: 6, message: isAr ? '6 أحرف على الأقل' : 'Min 6 characters' }
                          })}
                        />
                        <button
                          type="button"
                          onClick={() => setShowPassword(!showPassword)}
                          className="absolute top-1/2 -translate-y-1/2 text-ocean-surf hover:text-ocean-mid"
                          style={{ [isAr ? 'left' : 'right']: '12px' }}
                        >
                          {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                        </button>
                      </div>
                      {errors.password && <p className="text-error text-xs mt-1">{errors.password.message}</p>}
                    </div>

                    <button
                      type="submit"
                      disabled={loading}
                      className="btn-ocean btn-primary w-full py-3 rounded-xl text-base disabled:opacity-50"
                    >
                      {loading ? (isAr ? 'جاري التسجيل...' : 'Registering...') : (isAr ? 'التالي' : 'Next')}
                    </button>
                  </form>

                  <p className="text-center text-sm text-ocean-surf mt-6">
                    {isAr ? 'لديك حساب بالفعل؟' : 'Already have an account?'}{' '}
                    <Link href="/login" className="text-ocean-mid font-semibold hover:text-ocean-light">
                      {isAr ? 'سجّل دخول' : 'Login'}
                    </Link>
                  </p>
                </motion.div>
              )}

              {step === 2 && (
                <motion.div
                  key="step2"
                  initial={{ opacity: 0, x: 20 }}
                  animate={{ opacity: 1, x: 0 }}
                  exit={{ opacity: 0, x: -20 }}
                  className="text-center"
                >
                  <div className="w-16 h-16 rounded-full bg-ocean-foam mx-auto mb-4 flex items-center justify-center">
                    <Mail size={28} className="text-ocean-mid" />
                  </div>
                  <h2 className="text-2xl font-bold text-ocean-deep mb-2">
                    {isAr ? 'تأكيد البريد الإلكتروني' : 'Verify Your Email'}
                  </h2>
                  <p className="text-ocean-surf text-sm mb-6">
                    {isAr ? 'أدخل رمز التحقق المرسل إلى' : 'Enter the code sent to'}{' '}
                    <strong className="text-ocean-deep">{registeredEmail}</strong>
                  </p>

                  {/* OTP Input */}
                  <div className="flex justify-center gap-2 mb-6" dir="ltr">
                    {otp.map((digit, i) => (
                      <input
                        key={i}
                        id={`otp-${i}`}
                        type="text"
                        inputMode="numeric"
                        maxLength={1}
                        value={digit}
                        onChange={(e) => handleOtpChange(i, e.target.value)}
                        onKeyDown={(e) => {
                          if (e.key === 'Backspace' && !digit && i > 0) {
                            document.getElementById(`otp-${i - 1}`)?.focus();
                          }
                        }}
                        className="w-12 h-14 text-center text-xl font-bold input-ocean"
                      />
                    ))}
                  </div>

                  <button
                    onClick={handleVerify}
                    disabled={loading || otp.join('').length !== 6}
                    className="btn-ocean btn-primary w-full py-3 rounded-xl text-base disabled:opacity-50"
                  >
                    {loading ? (isAr ? 'جاري التحقق...' : 'Verifying...') : (isAr ? 'تأكيد' : 'Verify')}
                  </button>

                  <button
                    onClick={() => setStep(1)}
                    className="mt-4 text-sm text-ocean-mid hover:text-ocean-light transition-colors"
                  >
                    {isAr ? 'رجوع' : 'Back'}
                  </button>
                </motion.div>
              )}
            </AnimatePresence>
          </div>
        </motion.div>
      </div>
    </div>
  );
}
