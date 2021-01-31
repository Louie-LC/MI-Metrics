CREATE TABLE [dbo].[Tests] (
    [TestID]    INT           NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [ProductID] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([TestID] ASC),
    CONSTRAINT [FK_Tests_Products] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products] ([ProductID]) ON DELETE CASCADE
);

