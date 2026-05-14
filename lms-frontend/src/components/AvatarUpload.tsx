'use client';

import React, { useState, useRef } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { Camera, Loader2, CheckCircle2, XCircle } from 'lucide-react';
import { toast } from 'sonner';
import { usersApi } from '@/lib/api/users.api';
import { useAuthStore } from '@/stores/auth.store';
import { useUIStore } from '@/stores/ui.store';

interface AvatarUploadProps {
  /** Current avatar URL from user profile */
  currentAvatarUrl?: string;
  /** User's display name (used for fallback initial) */
  displayName?: string;
  /** Size of the avatar in pixels */
  size?: number;
  /** Callback after successful upload */
  onSuccess?: (newUrl: string) => void;
}

type UploadStatus = 'idle' | 'uploading' | 'success' | 'error';

export default function AvatarUpload({
  currentAvatarUrl,
  displayName = 'U',
  size = 96,
  onSuccess,
}: AvatarUploadProps) {
  const { updateUser } = useAuthStore();
  const { locale } = useUIStore();
  const isAr = locale === 'ar';

  const [preview, setPreview] = useState<string | null>(null);
  const [status, setStatus] = useState<UploadStatus>('idle');
  const fileInputRef = useRef<HTMLInputElement>(null);

  const displayUrl = preview || currentAvatarUrl;
  const initial = displayName.charAt(0).toUpperCase();

  const handleFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;

    // Validate on client side
    const maxSize = 5 * 1024 * 1024; // 5 MB
    if (file.size > maxSize) {
      toast.error(isAr ? 'حجم الملف يتجاوز 5 ميجا' : 'File size exceeds 5 MB');
      return;
    }

    const allowedTypes = ['image/jpeg', 'image/png', 'image/webp'];
    if (!allowedTypes.includes(file.type)) {
      toast.error(isAr ? 'يرجى اختيار صورة (JPEG, PNG, WebP)' : 'Please select an image (JPEG, PNG, WebP)');
      return;
    }

    // Show local preview immediately for instant feedback
    const localUrl = URL.createObjectURL(file);
    setPreview(localUrl);
    setStatus('uploading');

    try {
      // Upload to OCI → save URL in DB
      const newUrl = await usersApi.uploadAndSetAvatar(file);

      // Update local state
      updateUser({ avatarUrl: newUrl });
      setPreview(newUrl);
      setStatus('success');
      toast.success(isAr ? 'تم تحديث الصورة بنجاح' : 'Avatar updated successfully');
      onSuccess?.(newUrl);

      // Reset status after animation
      setTimeout(() => setStatus('idle'), 2000);
    } catch (err) {
      console.error('Avatar upload failed:', err);
      setPreview(null); // revert preview
      setStatus('error');
      toast.error(isAr ? 'فشل رفع الصورة' : 'Avatar upload failed');
      setTimeout(() => setStatus('idle'), 2000);
    } finally {
      // Clean up object URL
      URL.revokeObjectURL(localUrl);
      // Reset file input so same file can be selected again
      if (fileInputRef.current) fileInputRef.current.value = '';
    }
  };

  return (
    <div className="relative inline-block" style={{ width: size, height: size }}>
      {/* Avatar circle */}
      <motion.div
        className="w-full h-full rounded-full overflow-hidden border-4 border-white shadow-ocean"
        whileHover={{ scale: 1.05 }}
        transition={{ type: 'spring', stiffness: 300 }}
      >
        {displayUrl ? (
          <img
            src={displayUrl}
            alt={displayName}
            className="w-full h-full object-cover"
            style={{ imageRendering: 'auto' }}
          />
        ) : (
          <div className="w-full h-full bg-gradient-to-br from-ocean-mid to-ocean-light flex items-center justify-center text-white font-bold"
            style={{ fontSize: size * 0.35 }}
          >
            {initial}
          </div>
        )}

        {/* Overlay during upload */}
        <AnimatePresence>
          {status === 'uploading' && (
            <motion.div
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              exit={{ opacity: 0 }}
              className="absolute inset-0 rounded-full bg-black/40 flex items-center justify-center"
            >
              <Loader2 className="text-white animate-spin" size={size * 0.3} />
            </motion.div>
          )}
          {status === 'success' && (
            <motion.div
              initial={{ opacity: 0, scale: 0.5 }}
              animate={{ opacity: 1, scale: 1 }}
              exit={{ opacity: 0 }}
              className="absolute inset-0 rounded-full bg-green-500/30 flex items-center justify-center"
            >
              <CheckCircle2 className="text-white" size={size * 0.3} />
            </motion.div>
          )}
          {status === 'error' && (
            <motion.div
              initial={{ opacity: 0, scale: 0.5 }}
              animate={{ opacity: 1, scale: 1 }}
              exit={{ opacity: 0 }}
              className="absolute inset-0 rounded-full bg-red-500/30 flex items-center justify-center"
            >
              <XCircle className="text-white" size={size * 0.3} />
            </motion.div>
          )}
        </AnimatePresence>
      </motion.div>

      {/* Camera button */}
      <button
        type="button"
        onClick={() => fileInputRef.current?.click()}
        disabled={status === 'uploading'}
        className="absolute bottom-0 right-0 w-8 h-8 rounded-full bg-white shadow-ocean-sm flex items-center justify-center text-ocean-mid hover:bg-ocean-foam hover:text-ocean-deep transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
        aria-label={isAr ? 'تغيير الصورة الشخصية' : 'Change avatar'}
      >
        <Camera size={14} />
      </button>

      {/* Hidden file input */}
      <input
        ref={fileInputRef}
        type="file"
        accept="image/jpeg,image/png,image/webp"
        onChange={handleFileChange}
        className="hidden"
        aria-hidden="true"
      />
    </div>
  );
}
