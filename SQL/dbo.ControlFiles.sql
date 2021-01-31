CREATE TABLE [dbo].[ControlFiles] (
    [ControlFileID] INT          NOT NULL,
    [Number]        BIGINT       NOT NULL,
    [Type]          NVARCHAR (4) NOT NULL,
    [Part]          NVARCHAR (4) NOT NULL,
    [Version]       NVARCHAR (3) NOT NULL,
    [ProductID]     INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([ControlFileID] ASC),
    CONSTRAINT [FK_ControlFiles_Products] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products] ([ProductID])
);

