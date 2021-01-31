CREATE TABLE [dbo].[TagUnitLinks] (
    [TagID]  INT NOT NULL,
    [UnitID] INT NOT NULL,
    CONSTRAINT [PK_TagUnitLinks] PRIMARY KEY CLUSTERED ([TagID] ASC, [UnitID] ASC),
    CONSTRAINT [FK_TagUnitLinks_Tags] FOREIGN KEY ([TagID]) REFERENCES [dbo].[Tags] ([TagID]),
    CONSTRAINT [FK_TagUnitLinks_Units] FOREIGN KEY ([UnitID]) REFERENCES [dbo].[Units] ([UnitID]) ON DELETE CASCADE
);

