import type { Metadata } from "next";
import "./globals.css";
import Providers from "@/components/Providers";

export const metadata: Metadata = {
  title: "أنا البحر — منصة تعليمية عربية",
  description: "اغمر نفسك في بحر المعرفة. منصة تعليمية متكاملة للعالم العربي تربط الطلاب بأفضل المدربين.",
  keywords: ["تعليم", "دورات", "عربي", "LMS", "أنا البحر"],
  icons: {
    icon: "/favicon.svg",
  },
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="ar" dir="rtl" className="h-full antialiased" suppressHydrationWarning>
      <head>
        <link
          href="https://fonts.googleapis.com/css2?family=Tajawal:wght@300;400;500;600;700;800&family=JetBrains+Mono:wght@400;500;600&display=swap"
          rel="stylesheet"
        />
      </head>
      <body
        className="min-h-full flex flex-col"
        style={{ fontFamily: "'Tajawal', sans-serif" }}
      >
        <a href="#main-content" className="skip-to-content">
          تخطي إلى المحتوى
        </a>
        <Providers>{children}</Providers>
      </body>
    </html>
  );
}
