﻿CREATE PROCEDURE [dbo].[spGetNextTestStepID]
AS 
BEGIN
	SET NOCOUNT ON
    SELECT NEXT VALUE FOR dbo.seqTestStepID;
END