'use client';

import React, { useState, useEffect, useCallback } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { motion, AnimatePresence } from 'framer-motion';
import { Clock, ChevronLeft, ChevronRight, AlertTriangle } from 'lucide-react';
import { toast } from 'sonner';
import { useUIStore } from '@/stores/ui.store';
import type { QuestionDto, ExamAnswerDto } from '@/types';

const demoQuestions: QuestionDto[] = [
  { id: 'q1', textAr: 'ما هي اللغة الأساسية لتطوير واجهات الويب؟', textEn: 'What is the primary language for web frontend development?', type: 'MultipleChoice', points: 10, order: 1, choices: [
    { id: 'c1', textAr: 'Python', textEn: 'Python', isCorrect: false },
    { id: 'c2', textAr: 'JavaScript', textEn: 'JavaScript', isCorrect: true },
    { id: 'c3', textAr: 'Java', textEn: 'Java', isCorrect: false },
    { id: 'c4', textAr: 'C++', textEn: 'C++', isCorrect: false },
  ]},
  { id: 'q2', textAr: 'HTML هي لغة برمجة', textEn: 'HTML is a programming language', type: 'TrueFalse', points: 5, order: 2 },
  { id: 'q3', textAr: 'اشرح مفهوم الـ Responsive Design', textEn: 'Explain the concept of Responsive Design', type: 'OpenEnded', points: 15, order: 3 },
];

export default function ExamTakerPage() {
  const params = useParams();
  const router = useRouter();
  const { locale } = useUIStore();
  const isAr = locale === 'ar';

  const [currentIndex, setCurrentIndex] = useState(0);
  const [answers, setAnswers] = useState<Record<string, ExamAnswerDto>>({});
  const [timeLeft, setTimeLeft] = useState(30 * 60); // 30 min
  const [showConfirm, setShowConfirm] = useState(false);
  const [submitted, setSubmitted] = useState(false);

  const currentQ = demoQuestions[currentIndex];

  // Timer
  useEffect(() => {
    if (submitted) return;
    const timer = setInterval(() => {
      setTimeLeft(prev => {
        if (prev <= 1) {
          handleSubmit();
          return 0;
        }
        return prev - 1;
      });
    }, 1000);
    return () => clearInterval(timer);
  }, [submitted]);

  const formatTime = (seconds: number) => {
    const m = Math.floor(seconds / 60);
    const s = seconds % 60;
    return `${m.toString().padStart(2, '0')}:${s.toString().padStart(2, '0')}`;
  };

  const timerColor = timeLeft > 300 ? 'text-ocean-mid' : timeLeft > 60 ? 'text-amber-500' : 'text-red-500';

  const setAnswer = (qId: string, answer: Partial<ExamAnswerDto>) => {
    setAnswers(prev => ({ ...prev, [qId]: { ...prev[qId], ...answer, questionId: qId } }));
  };

  const handleSubmit = useCallback(() => {
    setSubmitted(true);
    toast.success(isAr ? 'تم تسليم الاختبار بنجاح!' : 'Exam submitted successfully!');
  }, [isAr]);

  if (submitted) {
    const totalPoints = demoQuestions.reduce((sum, q) => sum + q.points, 0);
    const score = Math.floor(totalPoints * 0.75); // Demo score

    return (
      <div className="min-h-screen bg-ocean-foam flex items-center justify-center p-6">
        <motion.div
          initial={{ opacity: 0, scale: 0.9 }}
          animate={{ opacity: 1, scale: 1 }}
          className="ocean-card p-8 max-w-md w-full text-center"
        >
          {/* Circular score gauge */}
          <div className="relative w-40 h-40 mx-auto mb-6">
            <svg className="circular-progress w-full h-full" viewBox="0 0 120 120">
              <circle cx="60" cy="60" r="52" fill="none" stroke="#BAE6FD" strokeWidth="8" />
              <motion.circle
                cx="60" cy="60" r="52" fill="none"
                stroke={score >= totalPoints * 0.5 ? '#10B981' : '#EF4444'}
                strokeWidth="8" strokeLinecap="round"
                strokeDasharray={`${2 * Math.PI * 52}`}
                initial={{ strokeDashoffset: 2 * Math.PI * 52 }}
                animate={{ strokeDashoffset: 2 * Math.PI * 52 * (1 - score / totalPoints) }}
                transition={{ duration: 1.5, ease: 'easeOut' }}
              />
            </svg>
            <div className="absolute inset-0 flex flex-col items-center justify-center">
              <span className="text-3xl font-bold text-ocean-deep" style={{ fontFamily: "'JetBrains Mono', monospace" }}>
                {score}
              </span>
              <span className="text-sm text-ocean-surf">/ {totalPoints}</span>
            </div>
          </div>

          <h2 className="text-2xl font-bold text-ocean-deep mb-2">
            {score >= totalPoints * 0.5
              ? (isAr ? '🎉 ناجح!' : '🎉 Passed!')
              : (isAr ? 'لم ينجح' : 'Failed')}
          </h2>
          <p className="text-ocean-surf mb-6">
            {isAr ? `أجبت على ${Object.keys(answers).length} من ${demoQuestions.length} أسئلة` : `Answered ${Object.keys(answers).length} of ${demoQuestions.length} questions`}
          </p>

          <button
            onClick={() => router.push(`/courses/${params.id}`)}
            className="btn-ocean btn-primary w-full py-3 rounded-xl"
          >
            {isAr ? 'العودة للدورة' : 'Back to Course'}
          </button>
        </motion.div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-ocean-foam flex flex-col">
      {/* Top bar */}
      <header className="h-14 bg-white border-b flex items-center px-4 justify-between sticky top-0 z-30" style={{ borderColor: 'var(--color-border-light)' }}>
        <h1 className="text-sm font-semibold text-ocean-deep truncate">
          {isAr ? 'اختبار أساسيات الويب' : 'Web Basics Exam'}
        </h1>
        <div className={`flex items-center gap-2 font-mono font-bold text-lg ${timerColor}`}>
          <Clock size={18} />
          {formatTime(timeLeft)}
        </div>
        <span className="text-sm text-ocean-surf">
          {currentIndex + 1} {isAr ? 'من' : 'of'} {demoQuestions.length}
        </span>
      </header>

      {/* Question */}
      <main className="flex-1 flex items-center justify-center p-4 md:p-8">
        <div className="w-full max-w-2xl">
          <AnimatePresence mode="wait">
            <motion.div
              key={currentQ.id}
              initial={{ opacity: 0, x: 30 }}
              animate={{ opacity: 1, x: 0 }}
              exit={{ opacity: 0, x: -30 }}
              className="ocean-card p-6 md:p-8"
            >
              <div className="flex items-start justify-between mb-4">
                <span className="badge badge-active">
                  {isAr ? `سؤال ${currentIndex + 1}` : `Question ${currentIndex + 1}`}
                </span>
                <span className="text-sm text-ocean-surf font-mono">{currentQ.points} {isAr ? 'نقاط' : 'pts'}</span>
              </div>

              <h2 className="text-lg font-bold text-ocean-deep mb-6 leading-relaxed">
                {isAr ? currentQ.textAr : currentQ.textEn}
              </h2>

              {/* Multiple Choice */}
              {currentQ.type === 'MultipleChoice' && currentQ.choices && (
                <div className="space-y-3">
                  {currentQ.choices.map((choice) => (
                    <button
                      key={choice.id}
                      onClick={() => setAnswer(currentQ.id, { selectedChoiceId: choice.id })}
                      className={`w-full p-4 rounded-xl border-2 text-start transition-all flex items-center gap-3 ${
                        answers[currentQ.id]?.selectedChoiceId === choice.id
                          ? 'border-ocean-mid bg-ocean-foam'
                          : 'border-ocean-pale hover:border-ocean-surf'
                      }`}
                    >
                      <div className={`w-5 h-5 rounded-full border-2 flex-shrink-0 flex items-center justify-center ${
                        answers[currentQ.id]?.selectedChoiceId === choice.id
                          ? 'border-ocean-mid bg-ocean-mid'
                          : 'border-ocean-surf'
                      }`}>
                        {answers[currentQ.id]?.selectedChoiceId === choice.id && (
                          <div className="w-2 h-2 rounded-full bg-white" />
                        )}
                      </div>
                      <span className="text-sm text-ocean-deep">{isAr ? choice.textAr : choice.textEn}</span>
                    </button>
                  ))}
                </div>
              )}

              {/* True/False */}
              {currentQ.type === 'TrueFalse' && (
                <div className="grid grid-cols-2 gap-4">
                  {[true, false].map((val) => (
                    <button
                      key={val.toString()}
                      onClick={() => setAnswer(currentQ.id, { trueFalseAnswer: val })}
                      className={`p-6 rounded-2xl border-2 text-center text-lg font-bold transition-all ${
                        answers[currentQ.id]?.trueFalseAnswer === val
                          ? 'border-ocean-mid bg-ocean-foam text-ocean-mid'
                          : 'border-ocean-pale text-ocean-wave hover:border-ocean-surf'
                      }`}
                    >
                      {val ? (isAr ? 'صح ✓' : 'True ✓') : (isAr ? 'خطأ ✗' : 'False ✗')}
                    </button>
                  ))}
                </div>
              )}

              {/* Open Ended */}
              {currentQ.type === 'OpenEnded' && (
                <textarea
                  value={answers[currentQ.id]?.openEndedAnswer || ''}
                  onChange={(e) => setAnswer(currentQ.id, { openEndedAnswer: e.target.value })}
                  placeholder={isAr ? 'اكتب إجابتك هنا...' : 'Write your answer here...'}
                  className="input-ocean min-h-[160px] resize-y"
                />
              )}
            </motion.div>
          </AnimatePresence>

          {/* Question dots */}
          <div className="flex items-center justify-center gap-2 mt-6">
            {demoQuestions.map((q, i) => (
              <button
                key={q.id}
                onClick={() => setCurrentIndex(i)}
                className={`w-3 h-3 rounded-full transition-all ${
                  i === currentIndex
                    ? 'bg-ocean-mid scale-125'
                    : answers[q.id]
                      ? 'bg-ocean-light'
                      : 'bg-ocean-pale'
                }`}
              />
            ))}
          </div>

          {/* Navigation */}
          <div className="flex items-center justify-between mt-6">
            <button
              onClick={() => setCurrentIndex(Math.max(0, currentIndex - 1))}
              disabled={currentIndex === 0}
              className="btn-ocean btn-ghost px-4 py-2 text-sm disabled:opacity-30"
            >
              {isAr ? <ChevronRight size={16} /> : <ChevronLeft size={16} />}
              {isAr ? 'السابق' : 'Previous'}
            </button>

            {currentIndex === demoQuestions.length - 1 ? (
              <button
                onClick={() => setShowConfirm(true)}
                className="btn-ocean btn-gold px-6 py-2.5 rounded-xl"
              >
                {isAr ? 'تسليم الاختبار' : 'Submit Exam'}
              </button>
            ) : (
              <button
                onClick={() => setCurrentIndex(Math.min(demoQuestions.length - 1, currentIndex + 1))}
                className="btn-ocean btn-primary px-4 py-2 text-sm"
              >
                {isAr ? 'التالي' : 'Next'}
                {isAr ? <ChevronLeft size={16} /> : <ChevronRight size={16} />}
              </button>
            )}
          </div>
        </div>
      </main>

      {/* Confirm modal */}
      <AnimatePresence>
        {showConfirm && (
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            className="fixed inset-0 bg-black/40 z-50 flex items-center justify-center p-4"
          >
            <motion.div
              initial={{ scale: 0.9, opacity: 0 }}
              animate={{ scale: 1, opacity: 1 }}
              exit={{ scale: 0.9, opacity: 0 }}
              className="ocean-card p-6 max-w-sm w-full text-center"
            >
              <AlertTriangle size={40} className="text-amber-500 mx-auto mb-4" />
              <h3 className="text-lg font-bold text-ocean-deep mb-2">
                {isAr ? 'تأكيد التسليم' : 'Confirm Submission'}
              </h3>
              <p className="text-sm text-ocean-surf mb-6">
                {isAr
                  ? `أجبت على ${Object.keys(answers).length} من ${demoQuestions.length} أسئلة. هل تريد التسليم؟`
                  : `You answered ${Object.keys(answers).length} of ${demoQuestions.length} questions. Submit?`}
              </p>
              <div className="flex gap-3">
                <button onClick={() => setShowConfirm(false)} className="btn-ocean btn-secondary flex-1 py-2.5">
                  {isAr ? 'رجوع' : 'Go Back'}
                </button>
                <button onClick={handleSubmit} className="btn-ocean btn-gold flex-1 py-2.5">
                  {isAr ? 'تسليم' : 'Submit'}
                </button>
              </div>
            </motion.div>
          </motion.div>
        )}
      </AnimatePresence>
    </div>
  );
}
