CREATE TABLE [dbo].[TestResults] (
    [TestResultID]          BIGINT     NOT NULL,
    [Value]                 FLOAT (53) NOT NULL,
    [Date]                  DATE       NOT NULL,
    [TestID]                INT        NOT NULL,
    [DeviceHistoryRecordID] INT        NOT NULL,
    PRIMARY KEY CLUSTERED ([TestResultID] ASC),
    CONSTRAINT [FK_TestResults_DeviceHistoryRecords] FOREIGN KEY ([DeviceHistoryRecordID]) REFERENCES [dbo].[DeviceHistoryRecords] ([DeviceHistoryRecordID]) ON DELETE CASCADE,
    CONSTRAINT [FK_TestResults_Tests] FOREIGN KEY ([TestID]) REFERENCES [dbo].[Tests] ([TestID])
);

