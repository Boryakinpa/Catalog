RESTORE DATABASE YourRestoredDatabaseName1
FROM DISK = 'D:\Trade_completed.bak'
WITH 
MOVE 'Trade_completed' TO 'D:\Trade_completed.mdf',
MOVE 'Trade_completed_log' TO 'D:\Trade_completed_log.ldf',
RECOVERY;
