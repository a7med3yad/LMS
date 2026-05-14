import { test, expect } from '@playwright/test';

const BASE = 'http://localhost:3000';

test.describe('Courses', () => {
  test('should display course catalog', async ({ page }) => {
    await page.goto(`${BASE}/courses`);
    await expect(page.locator('text=الدورات التعليمية')).toBeVisible();
  });

  test('should show search input', async ({ page }) => {
    await page.goto(`${BASE}/courses`);
    await expect(page.locator('input[placeholder*="ابحث"]')).toBeVisible();
  });

  test('should show course cards', async ({ page }) => {
    await page.goto(`${BASE}/courses`);
    const cards = page.locator('.ocean-card');
    await expect(cards.first()).toBeVisible();
  });

  test('should navigate to course detail', async ({ page }) => {
    await page.goto(`${BASE}/courses`);
    await page.click('.ocean-card >> nth=0');
    await expect(page).toHaveURL(/\/courses\/.+/);
  });
});

test.describe('Course Detail', () => {
  test('should show course info with enrollment card', async ({ page }) => {
    await page.goto(`${BASE}/courses/1`);
    await expect(page.locator('text=سجّل الآن')).toBeVisible();
  });

  test('should show bilingual tabs', async ({ page }) => {
    await page.goto(`${BASE}/courses/1`);
    await expect(page.locator('text=العربية')).toBeVisible();
    await expect(page.locator('text=English')).toBeVisible();
  });
});
