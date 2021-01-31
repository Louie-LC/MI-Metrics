CREATE TABLE [dbo].[TestSteps] (
    [TestStepID]    INT           NOT NULL,
    [Step]          NVARCHAR (12) NOT NULL,
    [MStep]         NVARCHAR (3)  NOT NULL,
    [Substep]       NVARCHAR (3)  NOT NULL,
    [LowerLimit]    FLOAT (53)    NULL,
    [UpperLimit]    FLOAT (53)    NULL,
    [ControlFileID] INT           NOT NULL,
    [TestID]        INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([TestStepID] ASC),
    CONSTRAINT [FK_TestSteps_Tests] FOREIGN KEY ([TestID]) REFERENCES [dbo].[Tests] ([TestID]),
    CONSTRAINT [FK_TestSteps_ControleFiles] FOREIGN KEY ([ControlFileID]) REFERENCES [dbo].[ControlFiles] ([ControlFileID]) ON DELETE CASCADE
);

