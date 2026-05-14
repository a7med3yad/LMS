import { test, expect } from '@playwright/test';

const BASE = 'http://localhost:3000';

test.describe('Auth Flow', () => {
  test('should show login page with ocean branding', async ({ page }) => {
    await page.goto(`${BASE}/login`);
    await expect(page.locator('text=أنا البحر')).toBeVisible();
    await expect(page.locator('input[type="email"]')).toBeVisible();
    await expect(page.locator('input[type="password"]')).toBeVisible();
  });

  test('should navigate to register page', async ({ page }) => {
    await page.goto(`${BASE}/login`);
    await page.click('text=أنشئ حسابك');
    await expect(page).toHaveURL(`${BASE}/register`);
  });

  test('should show register form with role selector', async ({ page }) => {
    await page.goto(`${BASE}/register`);
    await expect(page.locator('text=طالب')).toBeVisible();
    await expect(page.locator('text=مدرّب')).toBeVisible();
  });

  test('should navigate to forgot password', async ({ page }) => {
    await page.goto(`${BASE}/login`);
    await page.click('text=نسيت كلمة المرور؟');
    await expect(page).toHaveURL(`${BASE}/forgot-password`);
  });

  test('should show validation errors on empty login', async ({ page }) => {
    await page.goto(`${BASE}/login`);
    await page.click('button[type="submit"]');
    // Form should show validation messages
    await expect(page.locator('text=/مطلوب|required/i')).toBeVisible();
  });
});
