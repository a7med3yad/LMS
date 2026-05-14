import { test, expect } from '@playwright/test';

const BASE = 'http://localhost:3000';

test.describe('Exam Flow', () => {
  test('should show exam taker with timer', async ({ page }) => {
    await page.goto(`${BASE}/courses/1/exams/1/take`);
    // Timer should be visible
    await expect(page.locator('text=/\\d{2}:\\d{2}/')).toBeVisible();
  });

  test('should show question navigation dots', async ({ page }) => {
    await page.goto(`${BASE}/courses/1/exams/1/take`);
    await expect(page.locator('text=سؤال 1')).toBeVisible();
  });

  test('should navigate between questions', async ({ page }) => {
    await page.goto(`${BASE}/courses/1/exams/1/take`);
    await page.click('text=التالي');
    await expect(page.locator('text=سؤال 2')).toBeVisible();
  });
});

test.describe('Enrollment Flow', () => {
  test('should display my courses', async ({ page }) => {
    await page.goto(`${BASE}/my-courses`);
    await expect(page.locator('text=دوراتي')).toBeVisible();
  });
});

test.describe('Checkout Flow', () => {
  test('should show checkout page with order summary', async ({ page }) => {
    await page.goto(`${BASE}/checkout/1`);
    await expect(page.locator('text=ملخص الطلب')).toBeVisible();
  });

  test('should accept voucher code', async ({ page }) => {
    await page.goto(`${BASE}/checkout/1`);
    await expect(page.locator('input[placeholder*="كود"]')).toBeVisible();
  });
});

test.describe('Notifications', () => {
  test('should show notifications page', async ({ page }) => {
    await page.goto(`${BASE}/notifications`);
    await expect(page.locator('text=الإشعارات')).toBeVisible();
  });

  test('should have mark all read button', async ({ page }) => {
    await page.goto(`${BASE}/notifications`);
    await expect(page.locator('text=اقرأ الكل')).toBeVisible();
  });
});

test.describe('Instructor', () => {
  test('should show revenue dashboard', async ({ page }) => {
    await page.goto(`${BASE}/revenue`);
    await expect(page.locator('text=الإيرادات')).toBeVisible();
  });
});
