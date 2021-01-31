CREATE PROCEDURE [dbo].[spSelectDeviceHistoryRecordsWithProductID]
	@productID int
AS
BEGIN
	SET NOCOUNT ON
	SELECT DeviceHistoryRecordID, PartNumber, Type, Part, Version, SerialNumber, ProductID
	FROM DeviceHistoryRecords
	WHERE ProductID = @productID
END