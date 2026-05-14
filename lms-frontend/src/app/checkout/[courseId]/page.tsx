'use client';

import React, { useState } from 'react';
import { useParams } from 'next/navigation';
import { motion } from 'framer-motion';
import { Lock, Tag, ShieldCheck, CreditCard, CheckCircle } from 'lucide-react';
import { toast } from 'sonner';
import Header from '@/components/layout/Header';
import { useUIStore } from '@/stores/ui.store';

export default function CheckoutPage() {
  const params = useParams();
  const { locale } = useUIStore();
  const isAr = locale === 'ar';
  const [voucherCode, setVoucherCode] = useState('');
  const [discount, setDiscount] = useState(0);
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);

  const originalPrice = 49.99;
  const total = originalPrice - discount;

  const handleApplyVoucher = () => {
    if (voucherCode.toLowerCase() === 'ocean20') {
      setDiscount(10);
      toast.success(isAr ? 'تم تطبيق الكوبون!' : 'Voucher applied!');
    } else {
      toast.error(isAr ? 'كوبون غير صالح' : 'Invalid voucher');
    }
  };

  const handlePayment = async () => {
    setLoading(true);
    setTimeout(() => {
      setSuccess(true);
      setLoading(false);
    }, 2000);
  };

  if (success) {
    return (
      <div className="min-h-screen bg-ocean-foam flex items-center justify-center p-6">
        <motion.div
          initial={{ opacity: 0, scale: 0.9 }}
          animate={{ opacity: 1, scale: 1 }}
          className="ocean-card p-8 max-w-md w-full text-center"
        >
          <motion.div
            initial={{ scale: 0 }}
            animate={{ scale: 1 }}
            transition={{ delay: 0.2, type: 'spring' }}
            className="w-20 h-20 rounded-full bg-emerald-100 mx-auto mb-6 flex items-center justify-center"
          >
            <CheckCircle size={40} className="text-emerald-500" />
          </motion.div>
          <h2 className="text-2xl font-bold text-ocean-deep mb-2">
            {isAr ? 'تم الدفع بنجاح! 🎉' : 'Payment Successful! 🎉'}
          </h2>
          <p className="text-ocean-surf mb-6">
            {isAr ? 'تم تسجيلك في الدورة. يمكنك البدء في التعلم الآن.' : 'You are enrolled. You can start learning now.'}
          </p>
          <a href={`/courses/${params.courseId}/learn`} className="btn-ocean btn-primary w-full py-3 rounded-xl block">
            {isAr ? 'ابدأ التعلم' : 'Start Learning'}
          </a>
        </motion.div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-ocean-foam">
      <Header variant="public" />
      <main className="max-w-4xl mx-auto px-4 md:px-6 py-8 md:py-12">
        <motion.h1
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          className="text-2xl md:text-3xl font-bold text-ocean-deep mb-8"
        >
          {isAr ? 'إتمام الدفع' : 'Checkout'}
        </motion.h1>

        <div className="grid grid-cols-1 lg:grid-cols-5 gap-8">
          {/* Left: Order summary */}
          <div className="lg:col-span-3 space-y-6">
            {/* Course info */}
            <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} className="ocean-card p-6">
              <h2 className="font-bold text-ocean-deep mb-4">{isAr ? 'ملخص الطلب' : 'Order Summary'}</h2>
              <div className="flex gap-4 items-start">
                <div className="w-24 h-16 rounded-xl bg-gradient-to-br from-ocean-mid to-ocean-light flex items-center justify-center flex-shrink-0">
                  <CreditCard size={24} className="text-white/60" />
                </div>
                <div>
                  <h3 className="font-semibold text-ocean-deep text-sm">{isAr ? 'أساسيات تطوير الويب الحديث' : 'Modern Web Dev Fundamentals'}</h3>
                  <p className="text-xs text-ocean-surf mt-1">{isAr ? 'أحمد محمد' : 'Ahmed Mohammed'}</p>
                </div>
              </div>

              {/* Price breakdown */}
              <div className="mt-4 pt-4 border-t space-y-2" style={{ borderColor: 'var(--color-border-light)' }}>
                <div className="flex justify-between text-sm">
                  <span className="text-ocean-wave">{isAr ? 'السعر الأصلي' : 'Original Price'}</span>
                  <span className="text-ocean-deep font-mono">${originalPrice}</span>
                </div>
                {discount > 0 && (
                  <div className="flex justify-between text-sm">
                    <span className="text-success">{isAr ? 'الخصم' : 'Discount'}</span>
                    <span className="text-success font-mono">-${discount}</span>
                  </div>
                )}
                <div className="flex justify-between font-bold text-base pt-2 border-t" style={{ borderColor: 'var(--color-border-light)' }}>
                  <span className="text-ocean-deep">{isAr ? 'المجموع' : 'Total'}</span>
                  <span className="text-gold font-mono">${total.toFixed(2)}</span>
                </div>
              </div>
            </motion.div>

            {/* Voucher */}
            <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.1 }} className="ocean-card p-6">
              <h3 className="font-bold text-ocean-deep mb-3 flex items-center gap-2">
                <Tag size={16} className="text-ocean-mid" />
                {isAr ? 'كود الخصم' : 'Voucher Code'}
              </h3>
              <div className="flex gap-2">
                <input
                  type="text"
                  value={voucherCode}
                  onChange={(e) => setVoucherCode(e.target.value)}
                  placeholder={isAr ? 'أدخل كود الخصم' : 'Enter voucher code'}
                  className="input-ocean flex-1"
                />
                <button onClick={handleApplyVoucher} className="btn-ocean btn-secondary px-4">
                  {isAr ? 'تطبيق' : 'Apply'}
                </button>
              </div>
            </motion.div>
          </div>

          {/* Right: Payment */}
          <div className="lg:col-span-2">
            <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.2 }} className="ocean-card p-6 lg:sticky lg:top-20">
              <h3 className="font-bold text-ocean-deep mb-4">{isAr ? 'بيانات الدفع' : 'Payment Details'}</h3>

              {/* Card input fields (Stripe placeholder) */}
              <div className="space-y-4 mb-6">
                <div>
                  <label className="text-sm text-ocean-wave mb-1 block">{isAr ? 'رقم البطاقة' : 'Card Number'}</label>
                  <input type="text" className="input-ocean font-mono text-sm" placeholder="4242 4242 4242 4242" />
                </div>
                <div className="grid grid-cols-2 gap-3">
                  <div>
                    <label className="text-sm text-ocean-wave mb-1 block">{isAr ? 'تاريخ الانتهاء' : 'Expiry'}</label>
                    <input type="text" className="input-ocean font-mono text-sm" placeholder="MM/YY" />
                  </div>
                  <div>
                    <label className="text-sm text-ocean-wave mb-1 block">CVC</label>
                    <input type="text" className="input-ocean font-mono text-sm" placeholder="123" />
                  </div>
                </div>
              </div>

              <button
                onClick={handlePayment}
                disabled={loading}
                className="btn-ocean btn-gold w-full py-3 rounded-xl text-base disabled:opacity-50 flex items-center justify-center gap-2"
              >
                {loading ? (
                  <span>{isAr ? 'جاري المعالجة...' : 'Processing...'}</span>
                ) : (
                  <>
                    <Lock size={16} />
                    {isAr ? 'أتمّ الدفع' : 'Complete Payment'} — ${total.toFixed(2)}
                  </>
                )}
              </button>

              <div className="flex items-center justify-center gap-2 mt-4">
                <ShieldCheck size={14} className="text-ocean-surf" />
                <span className="text-xs text-ocean-surf">{isAr ? 'مشفّر وآمن بـ SSL' : 'SSL encrypted & secure'}</span>
              </div>
            </motion.div>
          </div>
        </div>
      </main>
    </div>
  );
}
