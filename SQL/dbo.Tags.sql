CREATE TABLE [dbo].[Tags] (
    [TagID]       INT           NOT NULL,
    [Description] NVARCHAR (20) NOT NULL,
    [ProductID]   INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([TagID] ASC),
    CONSTRAINT [FK_Tags_Products] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products] ([ProductID]) ON DELETE CASCADE
);

