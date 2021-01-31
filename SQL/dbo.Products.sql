CREATE TABLE [dbo].[Products] (
    [ProductID] INT            NOT NULL,
    [Name]      NVARCHAR (50)  NOT NULL,
    [ImagePath] NVARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([ProductID] ASC),
    UNIQUE NONCLUSTERED ([ProductID] ASC)
);

