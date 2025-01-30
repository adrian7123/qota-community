/*
  Warnings:

  - You are about to alter the column `score` on the `Player` table. The data in that column could be lost. The data in that column will be cast from `BigInt` to `Int`.

*/
-- AlterTable
ALTER TABLE `Player` MODIFY `score` INTEGER NOT NULL DEFAULT 0;
