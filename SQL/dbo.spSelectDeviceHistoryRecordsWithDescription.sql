CREATE PROCEDURE [dbo].[spSelectDeviceHistoryRecordsWithDescription]
	@partNumber bigint,
	@type nvarchar(4),
	@part nvarchar(4),
	@version nvarchar(3),
	@serialNumber bigint
AS
BEGIN
	SET NOCOUNT ON
	SELECT DeviceHistoryRecordID, PartNumber, Type, Part, Version, SerialNumber, ProductID
	FROM DeviceHistoryRecords
	WHERE PartNumber = @partNumber AND Type = @type AND Part = @part AND Version = @version AND SerialNumber = @serialNumber;
END