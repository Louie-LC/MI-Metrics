using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MI_Metrics
{
    public class ViewPageViewModel
    {
        private ViewPage page;

        public Product ViewedProduct { get => viewedProduct; set => viewedProduct = value; }
        private Product viewedProduct;

        public List<DeviceHistoryRecord> DeviceHistoryRecords { get => deviceHistoryRecords; set => deviceHistoryRecords = value; }
        private List<DeviceHistoryRecord> deviceHistoryRecords;

        public ObservableCollection<DeviceHistoryRecord> DeviceHistoryRecordView { get => deviceHistoryRecordView; set => deviceHistoryRecordView = value; }
        private ObservableCollection<DeviceHistoryRecord> deviceHistoryRecordView;

        public ObservableCollection<Tag> TagView { get => tagView; set => tagView = value; }
        private ObservableCollection<Tag> tagView;

        public ObservableCollection<Tag> SelectedTags { get => selectedTags; set => selectedTags = value; }

        private ObservableCollection<Tag> selectedTags;

        private Dictionary<int, UnitFrequencyPair> unitTagFrequencies;

        public string SearchText { get => searchText; set => searchText = value; }
        private string searchText;

        public DateTime? SearchStartDate { get => searchStartDate; set => searchStartDate = value; }
        private DateTime? searchStartDate;

        public DateTime? SearchEndDate { get => searchEndDate; set => searchEndDate = value; }
        private DateTime? searchEndDate;
        

        public ViewPageViewModel(ViewPage page, Product product)
        {
            this.page = page;
            this.page.DataContext = this;
            ViewedProduct = product;
            DeviceHistoryRecords = new List<DeviceHistoryRecord>();
            DeviceHistoryRecordView = new ObservableCollection<DeviceHistoryRecord>();
            BuildRecords(ViewedProduct);

            TagView = new ObservableCollection<Tag>();
            SelectedTags = new ObservableCollection<Tag>();
            GatherTags(ViewedProduct);

            unitTagFrequencies = new Dictionary<int, UnitFrequencyPair>();
            SearchText = "";
            SearchStartDate = null;
            SearchEndDate = null;
        }

        private void BuildRecords(Product product)
        {
            DeviceHistoryRecords.Clear();
            DeviceHistoryRecordView.Clear();

            List<DeviceHistoryRecord> records = MainWindow.database.deviceHistoryRecordManager.GetDeviceHistoryRecords(viewedProduct);
            List<Unit> units = MainWindow.database.unitManager.RetrieveUnitList(product);
            foreach (DeviceHistoryRecord record in records)
            {
                record.BuildTestResultsAndDate();
                record.BuildTestValues(product);
                record.BuildUnitDescription(units);
                AddRecord(record);
            }
        }

        public void AddRecord(DeviceHistoryRecord record)
        {
            DeviceHistoryRecords.Add(record);
            DeviceHistoryRecordView.Add(record);
        }

        private void GatherTags(Product product)
        {
            List<Tag> tags = MainWindow.database.tagManager.GetTags(product);
            //tags.Sort(delegate (Tag x, Tag y)
            //{
            //    if (x.Description == null && y.Description == null) return 0;
            //    else if (x.Description == null) return -1;
            //    else if (y.Description == null) return 1;
            //    else return x.Description.CompareTo(y.Description);
            //});
            foreach (Tag tag in tags)
            {
                TagView.Add(tag);
                BuildUnitsForTag(tag);
            }
        }

        private void BuildUnitsForTag(Tag tag)
        {
            if (tag.ApplicableUnits == null)
                tag.ApplicableUnits = new List<Unit>();
            List<TagUnitLink> tagUnitLinks = MainWindow.database.tagManager.GetTagUnitLinks(tag);
            foreach (TagUnitLink link in tagUnitLinks)
            {
                List<Unit> units = MainWindow.database.unitManager.RetrieveUnitList(link.UnitID);
                foreach (Unit unit in units)
                {
                    tag.ApplicableUnits.Add(unit);
                }
            }
        }

        public void TagSelected(Tag tag)
        {
            // If the tag cannot be removed, then it should be added.
            if (!SelectedTags.Remove(tag))
            {
                SelectedTags.Add(tag);
                IncreaseUnitFrequency(tag);
            }
            else
            {
                DecreaseUnitFrequency(tag);
            }
            FilterDHRView();
        }

        public void RemoveSelectedTag(Tag tag)
        {
            SelectedTags.Remove(tag);
            DecreaseUnitFrequency(tag);
            FilterDHRView();
        }

        public void IncreaseUnitFrequency(Tag tag)
        {
            foreach (Unit unit in tag.ApplicableUnits)
            {
                if (unitTagFrequencies.ContainsKey(unit.UnitID))
                {
                    unitTagFrequencies[unit.UnitID].Frequency++;
                }
                else
                {
                    unitTagFrequencies.Add(unit.UnitID, new UnitFrequencyPair(unit, 1));
                }
            }
        }

        public void DecreaseUnitFrequency(Tag tag)
        {
            foreach (Unit unit in tag.ApplicableUnits)
            {
                unitTagFrequencies[unit.UnitID].Frequency--;
            }
        }


        public void FilterDHRView()
        {
            DeviceHistoryRecordView.Clear();

            List<Unit> matchingUnits = GatherUnitsFromFrequencies();

            foreach (DeviceHistoryRecord record in DeviceHistoryRecords)
            {
                if ( (SearchStartDate == null || record.Date >= SearchStartDate) && (SearchEndDate == null || record.Date <= SearchEndDate) && (SelectedTags.Count == 0 || DHRMatchesTags(record, matchingUnits)) && (SearchText.Length == 0 || DHRMatchesSearch(record)))
                {
                    DeviceHistoryRecordView.Add(record);
                }
            }
        }

        private List<Unit> GatherUnitsFromFrequencies()
        {
            // The tags that are being selected are going to only allow DHRs that match each tag that is selected.
            // Since this process is additive, a unit is only considered if it appears in each Tag's unit list.
            // The number of times a unit appears in any tag list is compared against the total number of tags selected.
            // If the unit is in each tag list, then tagSelectedCount should match the unitTagFrequency for that unit.

            int tagSelectedCount = SelectedTags.Count;
            List<Unit> units = new List<Unit>();

            foreach (var pair in unitTagFrequencies)
            {
                if (pair.Value.Frequency == tagSelectedCount)
                    units.Add(pair.Value.Unit);
            }
            return units;
        }

        private bool DHRMatchesTags(DeviceHistoryRecord record, List<Unit> units)
        {
            foreach (Unit unit in units)
            {
                if (record.PartNumber == unit.PartNumber)
                    return true;
            }
            return false;
        }

        private bool DHRMatchesSearch(DeviceHistoryRecord record)
        {
            if (record.FullDescription.Contains(searchText) || (record.UnitDescription != null && record.UnitDescription.Contains(searchText)))
            {
                return true;
            }
            return false;
        }
    }
}
