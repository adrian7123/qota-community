-- AlterTable
ALTER TABLE `Player` ADD COLUMN `deaths` INTEGER NOT NULL DEFAULT 0,
    ADD COLUMN `kills` INTEGER NOT NULL DEFAULT 0;
