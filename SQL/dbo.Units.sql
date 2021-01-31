CREATE TABLE [dbo].[Units] (
    [UnitID]      INT            NOT NULL,
    [PartNumber]  BIGINT         NOT NULL,
    [Description] NVARCHAR (100) NOT NULL,
    [ProductID]   INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([UnitID] ASC),
    CONSTRAINT [FK_Units_Products] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products] ([ProductID]) ON DELETE CASCADE
);

