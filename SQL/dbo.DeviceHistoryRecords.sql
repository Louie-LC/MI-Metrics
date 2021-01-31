CREATE TABLE [dbo].[DeviceHistoryRecords] (
    [DeviceHistoryRecordID] INT          NOT NULL,
    [PartNumber]            BIGINT       NOT NULL,
    [Type]                  NVARCHAR (4) NOT NULL,
    [Part]                  NVARCHAR (4) NOT NULL,
    [Version]               NVARCHAR (3) NOT NULL,
    [SerialNumber]          BIGINT       NOT NULL,
    [ProductID]             INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([DeviceHistoryRecordID] ASC),
    CONSTRAINT [FK_DeviceHistoryRecords_Products] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products] ([ProductID]) ON DELETE CASCADE
);

