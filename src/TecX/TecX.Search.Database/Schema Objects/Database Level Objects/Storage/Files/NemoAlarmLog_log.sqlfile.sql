ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [NemoAlarmLog_log], FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\NemoAlarmLog_log.ldf', SIZE = 3480448 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);



