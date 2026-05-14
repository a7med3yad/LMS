'use client';

import React from 'react';

interface WaveDividerProps {
  variant?: 'white-on-blue' | 'blue-on-white' | 'dark';
  flip?: boolean;
  className?: string;
}

export default function WaveDivider({ variant = 'white-on-blue', flip = false, className = '' }: WaveDividerProps) {
  const colors = {
    'white-on-blue': {
      wave1: '#FFFFFF',
      wave2: 'rgba(255,255,255,0.6)',
      wave3: 'rgba(255,255,255,0.3)',
    },
    'blue-on-white': {
      wave1: '#0EA5E9',
      wave2: '#38BDF8',
      wave3: '#BAE6FD',
    },
    dark: {
      wave1: '#0C2340',
      wave2: '#075985',
      wave3: '#0EA5E9',
    },
  };

  const c = colors[variant];

  return (
    <div
      className={`wave-divider ${className}`}
      style={{ transform: flip ? 'scaleY(-1)' : 'none' }}
      aria-hidden="true"
    >
      <svg
        viewBox="0 0 1440 120"
        preserveAspectRatio="none"
        style={{ display: 'block', width: 'calc(100% + 50px)', height: '80px' }}
      >
        {/* Wave 3 (back) */}
        <path
          d="M0,80 C240,40 480,100 720,60 C960,20 1200,80 1440,50 L1440,120 L0,120 Z"
          fill={c.wave3}
          className="animate-wave-3"
          style={{ animationDuration: '12s' }}
        />
        {/* Wave 2 (middle) */}
        <path
          d="M0,70 C320,30 560,90 800,50 C1040,10 1280,70 1440,40 L1440,120 L0,120 Z"
          fill={c.wave2}
          className="animate-wave-2"
          style={{ animationDuration: '10s' }}
        />
        {/* Wave 1 (front) */}
        <path
          d="M0,90 C360,50 600,100 840,70 C1080,40 1320,90 1440,60 L1440,120 L0,120 Z"
          fill={c.wave1}
          className="animate-wave"
          style={{ animationDuration: '8s' }}
        />
      </svg>
    </div>
  );
}
