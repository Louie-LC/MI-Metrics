﻿CREATE PROCEDURE dbo.spGetNextProductID	 
AS 
BEGIN
	SET NOCOUNT ON
    SELECT NEXT VALUE FOR dbo.seqProductID;
END