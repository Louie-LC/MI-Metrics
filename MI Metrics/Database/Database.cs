using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.VisualBasic.FileIO;

namespace MI_Metrics
{
    public enum Table
    {
        Products,
        Units,
        DeviceHistoryRecords,
        Tests,
        TestResults,
        ControlFiles,
        TestSteps,
        Tag
    }

    public class Database
    {
        public DatabaseProductManager productManager;
        public DatabaseUnitManager unitManager;
        public DatabaseTestManager testManager;
        public DatabaseControlFileManager controlFileManager;
        public DatabaseTestStepManager testStepManager;
        public DatabaseDeviceHistoryRecordManager deviceHistoryRecordManager;
        public DatabaseTestResultManager testResultManager;
        public DatabaseTagManager tagManager;

        public static readonly char[] InvalidCharacters = {'\0', '\'', '\"', '\b', '\n', '\r', '\t', '\u001A', '\\', '%', '_'};

        private string name;
        private static string connectionString;
        public string Name { get => name; set => name = value; }
        public static string ConnectionString { get => connectionString;}

        public Database(string name)
        {
            Name = name;
            //Retrieve the connection string from app.config using the DatabaseHelper
            connectionString = DatabaseHelper.CnnVal(name);

            productManager = new DatabaseProductManager();
            unitManager = new DatabaseUnitManager();
            testManager = new DatabaseTestManager();
            controlFileManager = new DatabaseControlFileManager();
            testStepManager = new DatabaseTestStepManager();
            deviceHistoryRecordManager = new DatabaseDeviceHistoryRecordManager();
            testResultManager = new DatabaseTestResultManager();
            tagManager = new DatabaseTagManager();
        }

        public static int GetNextID(Table targetTable)
        {
            int id = -1;
            using (var connection = new System.Data.SqlClient.SqlConnection(Database.ConnectionString))
            {
                connection.Open();

                switch (targetTable)
                {
                    case Table.Products:
                        id = connection.Query<int>("dbo.spGetNextProductID").ToList()[0];
                        break;
                    case Table.Units:
                        id = connection.Query<int>("dbo.spGetNextUnitID").ToList()[0];
                        break;
                    case Table.DeviceHistoryRecords:
                        id = connection.Query<int>("dbo.spGetNextDeviceHistoryRecordID").ToList()[0];
                        break;
                    case Table.Tests:
                        id = connection.Query<int>("dbo.spGetNextTestID").ToList()[0];
                        break;
                    case Table.TestResults:
                        id = connection.Query<int>("dbo.spGetNextTestResultID").ToList()[0];
                        break;
                    case Table.ControlFiles:
                        id = connection.Query<int>("dbo.spGetNextControlFileID").ToList()[0];
                        break;
                    case Table.TestSteps:
                        id = connection.Query<int>("dbo.spGetNextTestStepID").ToList()[0];
                        break;
                    case Table.Tag:
                        id = connection.Query<int>("dbo.spGetNextTagID").ToList()[0];
                        break;
                }
            }
            return id;
        }
    }
}
