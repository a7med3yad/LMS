'use client';

import React from 'react';
import Header from './Header';
import Sidebar from './Sidebar';

export default function DashboardLayout({ children }: { children: React.ReactNode }) {
  return (
    <div className="min-h-screen bg-ocean-foam flex flex-col">
      <Header variant="dashboard" />
      <div className="flex flex-1">
        <Sidebar />
        <main
          id="main-content"
          className="flex-1 p-4 md:p-6 lg:p-8 overflow-x-hidden"
        >
          {children}
        </main>
      </div>
    </div>
  );
}
