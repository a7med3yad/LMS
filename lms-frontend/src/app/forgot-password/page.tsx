'use client';

import React, { useState } from 'react';
import Link from 'next/link';
import { motion, AnimatePresence } from 'framer-motion';
import { Mail, Lock, Eye, EyeOff, ArrowLeft } from 'lucide-react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import Logo from '@/components/brand/Logo';
import WaveDivider from '@/components/brand/WaveDivider';
import { useUIStore } from '@/stores/ui.store';
import { authApi } from '@/lib/api/auth.api';

export default function ForgotPasswordPage() {
  const { locale } = useUIStore();
  const isAr = locale === 'ar';
  const [step, setStep] = useState(1);
  const [loading, setLoading] = useState(false);
  const [email, setEmail] = useState('');
  const [otp, setOtp] = useState(['', '', '', '', '', '']);
  const [showPassword, setShowPassword] = useState(false);

  const { register, handleSubmit, formState: { errors } } = useForm();

  // Step 1: Request reset
  const handleRequestReset = async (data: { email?: string }) => {
    setLoading(true);
    try {
      await authApi.forgotPassword({ email: data.email! });
      setEmail(data.email!);
      setStep(2);
      toast.success(isAr ? 'تم إرسال رمز التحقق' : 'Verification code sent');
    } catch {
      toast.error(isAr ? 'البريد غير مسجل' : 'Email not found');
    } finally {
      setLoading(false);
    }
  };

  // Step 2: Verify OTP
  const handleVerifyOtp = async () => {
    const code = otp.join('');
    if (code.length !== 6) return;
    setLoading(true);
    try {
      await authApi.verifyReset({ email, otp: code });
      setStep(3);
    } catch {
      toast.error(isAr ? 'رمز التحقق غير صحيح' : 'Invalid code');
    } finally {
      setLoading(false);
    }
  };

  // Step 3: Reset password
  const handleResetPassword = async (data: { newPassword?: string }) => {
    setLoading(true);
    try {
      await authApi.resetPassword({ email, otp: otp.join(''), newPassword: data.newPassword! });
      toast.success(isAr ? 'تم تغيير كلمة المرور بنجاح!' : 'Password changed successfully!');
      window.location.href = '/login';
    } catch {
      toast.error(isAr ? 'فشل تغيير كلمة المرور' : 'Failed to reset password');
    } finally {
      setLoading(false);
    }
  };

  const handleOtpChange = (index: number, value: string) => {
    if (value.length > 1) return;
    const newOtp = [...otp];
    newOtp[index] = value;
    setOtp(newOtp);
    if (value && index < 5) document.getElementById(`otp-${index + 1}`)?.focus();
  };

  return (
    <div className="min-h-screen flex">
      {/* Left panel */}
      <div className="hidden lg:flex lg:w-1/2 relative hero-gradient items-center justify-center overflow-hidden">
        <div className="relative z-10 text-center px-12">
          <Logo size="xl" white className="justify-center mb-8" />
          <motion.p initial={{ opacity: 0 }} animate={{ opacity: 1 }} transition={{ delay: 0.3 }}
            className="text-xl text-white/90 font-medium"
          >
            {isAr ? 'لا تقلق، سنساعدك لاستعادة حسابك' : "Don't worry, we'll help you recover"}
          </motion.p>
        </div>
        <div className="absolute bottom-0 left-0 w-full">
          <WaveDivider variant="white-on-blue" />
        </div>
      </div>

      {/* Right panel */}
      <div className="flex-1 flex items-center justify-center p-6 bg-ocean-foam">
        <div className="w-full max-w-md">
          <div className="lg:hidden mb-8">
            <Logo size="lg" className="justify-center" />
          </div>

          {/* Progress */}
          <div className="flex items-center justify-center gap-2 mb-6">
            {[1, 2, 3].map((s) => (
              <React.Fragment key={s}>
                <div className={`w-8 h-8 rounded-full flex items-center justify-center text-sm font-bold ${
                  step >= s ? 'bg-ocean-mid text-white' : 'bg-ocean-pale text-ocean-wave'
                }`}>{s}</div>
                {s < 3 && <div className={`w-8 h-1 rounded-full ${step > s ? 'bg-ocean-mid' : 'bg-ocean-pale'}`} />}
              </React.Fragment>
            ))}
          </div>

          <div className="ocean-card p-8">
            <AnimatePresence mode="wait">
              {step === 1 && (
                <motion.div key="s1" initial={{ opacity: 0, x: 20 }} animate={{ opacity: 1, x: 0 }} exit={{ opacity: 0, x: -20 }}>
                  <h1 className="text-2xl font-bold text-ocean-deep mb-2">{isAr ? 'نسيت كلمة المرور؟' : 'Forgot Password?'}</h1>
                  <p className="text-ocean-surf text-sm mb-6">{isAr ? 'أدخل بريدك وسنرسل لك رمز التحقق' : "Enter your email and we'll send you a code"}</p>
                  <form onSubmit={handleSubmit(handleRequestReset)} className="space-y-4">
                    <div className="relative">
                      <Mail size={18} className="absolute top-1/2 -translate-y-1/2 text-ocean-surf" style={{ [isAr ? 'right' : 'left']: '12px' }} />
                      <input type="email" className="input-ocean" style={{ [isAr ? 'paddingRight' : 'paddingLeft']: '40px' }}
                        placeholder={isAr ? 'البريد الإلكتروني' : 'Email address'}
                        {...register('email', { required: true, pattern: /^\S+@\S+$/i })} />
                    </div>
                    <button type="submit" disabled={loading} className="btn-ocean btn-primary w-full py-3 rounded-xl disabled:opacity-50">
                      {loading ? (isAr ? 'جاري الإرسال...' : 'Sending...') : (isAr ? 'إرسال الرمز' : 'Send Code')}
                    </button>
                  </form>
                </motion.div>
              )}

              {step === 2 && (
                <motion.div key="s2" initial={{ opacity: 0, x: 20 }} animate={{ opacity: 1, x: 0 }} exit={{ opacity: 0, x: -20 }} className="text-center">
                  <h2 className="text-2xl font-bold text-ocean-deep mb-2">{isAr ? 'أدخل رمز التحقق' : 'Enter Verification Code'}</h2>
                  <p className="text-ocean-surf text-sm mb-6">{email}</p>
                  <div className="flex justify-center gap-2 mb-6" dir="ltr">
                    {otp.map((d, i) => (
                      <input key={i} id={`otp-${i}`} type="text" inputMode="numeric" maxLength={1} value={d}
                        onChange={(e) => handleOtpChange(i, e.target.value)}
                        onKeyDown={(e) => { if (e.key === 'Backspace' && !d && i > 0) document.getElementById(`otp-${i-1}`)?.focus(); }}
                        className="w-12 h-14 text-center text-xl font-bold input-ocean" />
                    ))}
                  </div>
                  <button onClick={handleVerifyOtp} disabled={loading || otp.join('').length !== 6}
                    className="btn-ocean btn-primary w-full py-3 rounded-xl disabled:opacity-50">
                    {loading ? (isAr ? 'جاري التحقق...' : 'Verifying...') : (isAr ? 'تأكيد' : 'Verify')}
                  </button>
                </motion.div>
              )}

              {step === 3 && (
                <motion.div key="s3" initial={{ opacity: 0, x: 20 }} animate={{ opacity: 1, x: 0 }} exit={{ opacity: 0, x: -20 }}>
                  <h2 className="text-2xl font-bold text-ocean-deep mb-2">{isAr ? 'كلمة مرور جديدة' : 'New Password'}</h2>
                  <p className="text-ocean-surf text-sm mb-6">{isAr ? 'أدخل كلمة المرور الجديدة' : 'Enter your new password'}</p>
                  <form onSubmit={handleSubmit(handleResetPassword)} className="space-y-4">
                    <div className="relative">
                      <Lock size={18} className="absolute top-1/2 -translate-y-1/2 text-ocean-surf" style={{ [isAr ? 'right' : 'left']: '12px' }} />
                      <input type={showPassword ? 'text' : 'password'} className="input-ocean"
                        style={{ [isAr ? 'paddingRight' : 'paddingLeft']: '40px', [isAr ? 'paddingLeft' : 'paddingRight']: '40px' }}
                        placeholder={isAr ? 'كلمة المرور الجديدة' : 'New password'}
                        {...register('newPassword', { required: true, minLength: 6 })} />
                      <button type="button" onClick={() => setShowPassword(!showPassword)}
                        className="absolute top-1/2 -translate-y-1/2 text-ocean-surf hover:text-ocean-mid"
                        style={{ [isAr ? 'left' : 'right']: '12px' }}>
                        {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                      </button>
                    </div>
                    <button type="submit" disabled={loading} className="btn-ocean btn-primary w-full py-3 rounded-xl disabled:opacity-50">
                      {loading ? (isAr ? 'جاري الحفظ...' : 'Saving...') : (isAr ? 'تغيير كلمة المرور' : 'Change Password')}
                    </button>
                  </form>
                </motion.div>
              )}
            </AnimatePresence>

            <div className="text-center mt-4">
              <Link href="/login" className="text-sm text-ocean-mid hover:text-ocean-light">
                {isAr ? 'العودة لتسجيل الدخول' : 'Back to Login'}
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
