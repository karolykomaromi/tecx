ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [NemoAlarmLog], FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\MessageStore.mdf', SIZE = 4177152 KB, FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];



