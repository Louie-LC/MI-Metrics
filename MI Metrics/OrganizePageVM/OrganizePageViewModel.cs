using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class OrganizePageViewModel
    {
        OrganizePage page;

        public ObservableCollection<Product> ProductView { get => productView; set => productView = value; }
        private ObservableCollection<Product> productView;

        public ObservableCollection<Unit> UnitView { get => unitView; set => unitView = value; }
        private ObservableCollection<Unit> unitView;

        public ObservableCollection<Tag> TagView { get => tagView; set => tagView = value; }
        private ObservableCollection<Tag> tagView;

        public Tag SelectedTag { get => selectedTag; set => selectedTag = value; }
        private Tag selectedTag;

        public ObservableCollection<Test> TestView { get => testView; set => testView = value; }
        private ObservableCollection<Test> testView;


        public OrganizePageViewModel(OrganizePage page)
        {
            this.page = page;
            this.page.DataContext = this;

            productView = new ObservableCollection<Product>();
            unitView = new ObservableCollection<Unit>();
            tagView = new ObservableCollection<Tag>();
            testView = new ObservableCollection<Test>();

            BuildProductView();
            SelectedTag = null;
            
        }

        private void BuildProductView()
        {
            productView = new ObservableCollection<Product>(MainWindow.productManager.Products);
        }

        public void UpdateUnitView(Product product)
        {
            UnitView.Clear();

            List<Unit> units = MainWindow.database.unitManager.RetrieveUnitList(product);

            foreach (Unit unit in units)
            {
                UnitView.Add(unit);
            }
        }

        public void UpdateTagView(Product product)
        {
            TagView.Clear();

            List<Tag> tags = MainWindow.database.tagManager.GetTags(product);

            foreach (Tag tag in tags)
            {
                TagView.Add(tag);
            }

        }

        public bool AddTag(string description, Product product)
        {
            if (DatabaseHelper.ValidateCharacters(description))
            {
                Tag tag = MainWindow.database.tagManager.AddTag(description, product);
                TagView.Add(tag);
            }
            else
            {
                return false;
            }
            return true;
        }

        public void CheckTagSelectionCheckBoxes(Unit unit)
        {
            List<TagUnitLink> tagUnitLinks = MainWindow.database.tagManager.GetTagUnitLinks(unit);
            Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();
            foreach(TagUnitLink link in tagUnitLinks)
            {
                if(!keyValuePairs.ContainsKey(link.TagID))
                    keyValuePairs.Add(link.TagID, 1);
            }

            foreach(Tag tag in TagView)
            {
                tag.ApplicableToUnit = false;
                if (keyValuePairs.ContainsKey(tag.TagID))
                {
                    tag.ApplicableToUnit = true;
                }
            }
        }

        public void CreateLink(Tag tag, Unit unit)
        {
            MainWindow.database.tagManager.AddTagUnitLink(tag, unit);
        }

        public void DestoryLink(Tag tag, Unit unit)
        {
            MainWindow.database.tagManager.DeleteTagUnitLink(tag, unit);
        }
        
        public bool UpdateSelectedTag(string description)
        {
            if (DatabaseHelper.ValidateCharacters(description))
            {
                MainWindow.database.tagManager.UpdateTag(selectedTag, description);
                selectedTag.Description = description;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddUnit(string partNumberString, string description, Product product)
        {
            long partNumber;
            if(!Int64.TryParse(partNumberString, out partNumber))
            {
                return false;
            }
            if (!DatabaseHelper.ValidateCharacters(description))
            {
                return false;
            }

            Unit unit = MainWindow.database.unitManager.AddUnit(partNumber, description, product);
            UnitView.Add(unit);
            return true;
        }

        public bool UpdateUnit(string partNumberString, string description, Unit unit)
        {
            long partNumber;
            if (!Int64.TryParse(partNumberString, out partNumber))
            {
                return false;
            }
            if (!DatabaseHelper.ValidateCharacters(description))
            {
                return false;
            }

            MainWindow.database.unitManager.UpdateUnit(unit, partNumber, description);
            unit.PartNumber = partNumber;
            unit.Description = description;
            return true;
        }

        public bool AddTest(string name, Product product)
        {
            Test test = MainWindow.database.testManager.AddTest(name, product);
            TestView.Add(test);
            product.Tests.Add(test);
            return true;
        }

        public bool UpdateTest(Test test, string name)
        {
            MainWindow.database.testManager.UpdateTest(test.TestID, name);
            test.Name = name;
            return true;
        }

        public void BuilTestView(Product product)
        {
            TestView.Clear();
            foreach(Test test in product.Tests)
            {
                TestView.Add(test);
            }
        }
        
    }
}
