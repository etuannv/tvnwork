USE QuanLyLuong;
GO
DBCC SHRINKDATABASE(N'QuanLyLuong' )
GO
-- Truncate the log by changing the database recovery model to SIMPLE.
ALTER DATABASE QuanLyLuong
SET RECOVERY SIMPLE;
GO
-- Shrink the truncated log file to 1 MB.
DBCC SHRINKFILE (QuanLyLuong_Log, 1);
GO
-- Reset the database recovery model.
ALTER DATABASE QuanLyLuong
SET RECOVERY FULL;
GO