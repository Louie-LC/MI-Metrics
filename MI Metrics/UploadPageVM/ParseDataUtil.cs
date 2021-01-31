using System;
using System.Collections.Generic;

namespace MI_Metrics
{
    public class ParseDataUtil
    {
        // The key for these two dictionaries are the full description of the system: 10248668_FPP_01S_00_60013
        public Dictionary<string, DeviceHistoryRecord> newDeviceHistoryRecords;
        public Dictionary<string, DeviceHistoryRecord> existingDeviceHistoryRecords;

        // The key for this dictionary is the full system description with the test ID appended to the front: 31-10248668_FPP_01S_00_60013
        Dictionary<string, List<IntermediateTestResult>> resultCollisions;

        public ParseDataUtil()
        {
            newDeviceHistoryRecords = new Dictionary<string, DeviceHistoryRecord>();
            existingDeviceHistoryRecords = new Dictionary<string, DeviceHistoryRecord>();
            resultCollisions = new Dictionary<string, List<IntermediateTestResult>>();
        }

        public void ValidateTest(IntermediateTestResult result, Product product)
        {
            CheckForCSVTestCollision(result);
            HandleDeviceHistoryRecord(result, product);
            CheckForDatabaseTestCollision(result);
            ValidateTestFields(result);
        }

        private void CheckForCSVTestCollision(IntermediateTestResult result)
        {
            // uniqueIdentifier will represent a unique combination of a test combined with a system.
            // If a second result ends up with the same uniqueIdentifier, then two results for the same system
            // and same test were pulled from the CSV file.
            string uniqueIdentifier = String.Format("{0}-{1}", result.TestID, result.SystemDescription);
            if (resultCollisions.ContainsKey(uniqueIdentifier))
            {
                if (resultCollisions[uniqueIdentifier].Count == 1)
                {
                    // If there was exactly one other existing result, it must be retroactively marked as a collision.
                    resultCollisions[uniqueIdentifier][0].AddErrorMesssage(ResultError.CSVConflict);

                }
                result.AddErrorMesssage(ResultError.CSVConflict);
            }
            else
            {
                resultCollisions.Add(uniqueIdentifier, new List<IntermediateTestResult>());
            }
            resultCollisions[uniqueIdentifier].Add(result);
        }


        public IntermediateTestResult RemoveTestAndCheckForCollisions (IntermediateTestResult result)
        {
            string uniqueIdentifier = String.Format("{0}-{1}", result.TestID, result.SystemDescription);
            if (resultCollisions.ContainsKey(uniqueIdentifier))
            {
                List<IntermediateTestResult> collisionList = resultCollisions[uniqueIdentifier];
                collisionList.Remove(result);
                if (collisionList.Count == 1)
                {
                    collisionList[0].RemoveErrorMessage(ResultError.CSVConflict);
                    return collisionList[0];
                }
            }
            return null;
        }

        private void HandleDeviceHistoryRecord(IntermediateTestResult result, Product product)
        {
            if (!newDeviceHistoryRecords.ContainsKey(result.SystemDescription) && !existingDeviceHistoryRecords.ContainsKey(result.SystemDescription))
            {
                // This will produce a DeviceHistoryRecord and store it in either newDeviceHistoryRecords or existingDeviceHistoryRecords.
                // If the record exists in the database, the information from the database is used to DeviceHistoryRecord.
                string[] resultInformation = result.SystemDescription.Split('_');

                long partNumber = 0;
                long serialNumber = 0;
                string type = "0";
                string part = "0";
                string version = "0";
                // If there are 5 fields in result information and the first and fifth are numbers, then result.SystemDescription is valid as a system description
                if (resultInformation.Length == 5 && Int64.TryParse(resultInformation[0], out partNumber) && Int64.TryParse(resultInformation[4], out serialNumber))
                {
                    type = resultInformation[1];
                    part = resultInformation[2];
                    version = resultInformation[3];
                }
                else
                {
                    result.AddErrorMesssage(ResultError.IncorrectSystemID);
                }

                DeviceHistoryRecord dhr = CheckDatabaseForRecord(partNumber, type, part, version, serialNumber);
                if (dhr == null)
                {
                    dhr = new DeviceHistoryRecord(partNumber, type, part, version, serialNumber, product.ProductID);
                    if (!newDeviceHistoryRecords.ContainsKey(dhr.FullDescription))
                    {
                        newDeviceHistoryRecords.Add(dhr.FullDescription, dhr);
                    }
                }
                else
                {
                    if(!existingDeviceHistoryRecords.ContainsKey(dhr.FullDescription))
                    {
                        existingDeviceHistoryRecords.Add(dhr.FullDescription, dhr);
                        dhr.BuildTestResultsAndDate();
                    }
                }
            }
        }

        private DeviceHistoryRecord CheckDatabaseForRecord(long partNumber, string type, string part, string version, long serialNumber)
        {
            // The combination of partNumber, type, part, version, and serialNumber should be unique so there should only be one record found in the database.
            List<DeviceHistoryRecord> records = MainWindow.database.deviceHistoryRecordManager.GetDeviceHistoryRecords(partNumber, type, part, version, serialNumber);
            if (records != null && records.Count > 0) return records[0];
            return null;
        }

        private void CheckForDatabaseTestCollision(IntermediateTestResult result)
        {
            if (existingDeviceHistoryRecords.ContainsKey(result.SystemDescription))
            {
                DeviceHistoryRecord record = existingDeviceHistoryRecords[result.SystemDescription];
                if (record.TestResults.ContainsKey(result.TestID))
                {
                    result.AddWarningMessage(ResultWarning.DatabaseConflict);
                }
            }
        }

        private void ValidateTestFields(IntermediateTestResult result)
        {
            double dummy;
            if(!Double.TryParse(result.Value, out dummy))
            {
                result.AddErrorMesssage(ResultError.IncorrectValue);
            }
            DateTime dummyDate;
            if(!DateTime.TryParse(result.Date, out dummyDate))
            {
                result.AddErrorMesssage(ResultError.IncorrectDate);
            }
        }
    }
}
