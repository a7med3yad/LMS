'use client';

import React from 'react';

interface LogoProps {
  size?: 'sm' | 'md' | 'lg' | 'xl';
  showText?: boolean;
  white?: boolean;
  className?: string;
}

const sizes = {
  sm: { icon: 32, text: '1.125rem' },
  md: { icon: 40, text: '1.5rem' },
  lg: { icon: 56, text: '2rem' },
  xl: { icon: 80, text: '3rem' },
};

export default function Logo({ size = 'md', showText = true, white = false, className = '' }: LogoProps) {
  const s = sizes[size];
  const primaryColor = white ? '#FFFFFF' : '#0EA5E9';
  const secondaryColor = white ? 'rgba(255,255,255,0.5)' : '#BAE6FD';
  const textColor = white ? '#FFFFFF' : '#0C2340';

  return (
    <div className={`flex items-center gap-3 ${className}`} aria-label="أنا البحر - منصة تعليمية">
      {/* Wave SVG Icon */}
      <svg
        width={s.icon}
        height={s.icon}
        viewBox="0 0 40 40"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
        aria-hidden="true"
      >
        {/* Top wave */}
        <path
          d="M4 16 C8 12, 12 12, 16 16 S24 20, 28 16 S32 12, 36 16"
          stroke={primaryColor}
          strokeWidth="3"
          strokeLinecap="round"
          fill="none"
        />
        {/* Bottom wave */}
        <path
          d="M4 24 C8 20, 12 20, 16 24 S24 28, 28 24 S32 20, 36 24"
          stroke={secondaryColor}
          strokeWidth="2.5"
          strokeLinecap="round"
          fill="none"
          opacity="0.7"
        />
        {/* Foam highlight */}
        <circle cx="20" cy="14" r="1.5" fill={white ? 'rgba(255,255,255,0.6)' : '#FFFFFF'} opacity="0.5" />
      </svg>

      {/* Arabic Wordmark */}
      {showText && (
        <span
          style={{
            fontSize: s.text,
            fontFamily: "'Tajawal', sans-serif",
            fontWeight: 700,
            color: textColor,
            letterSpacing: '-0.02em',
            lineHeight: 1,
          }}
        >
          أنا البحر
        </span>
      )}
    </div>
  );
}
