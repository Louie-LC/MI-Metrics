CREATE PROCEDURE [dbo].[spInsertDeviceHistoryRecord]
	@deviceHistoryRecordID int,
	@partNumber bigint,
	@type nvarchar(4),
	@part nvarchar(4),
	@version nvarchar(3),
	@serialNumber bigint,
	@productID int
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO DeviceHistoryRecords (DeviceHistoryRecordID, PartNumber, Type, Part, Version, SerialNumber, ProductID)
	VALUES (@deviceHistoryRecordID, @partNumber, @type, @part, @version, @serialNumber, @productID);
END