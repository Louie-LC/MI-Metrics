using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class TemplateChange
    {
        public List<TestStep> addedTestSteps;
        public List<TestStep> editedTestSteps;
        public List<TestStep> deletedTestSteps;

        public TemplateChange()
        {
            addedTestSteps = new List<TestStep>();
            editedTestSteps = new List<TestStep>();
            deletedTestSteps = new List<TestStep>();
        }

        public void AddTestStep(TestStep testStep)
        {
            addedTestSteps.Add(testStep);
        }

        public void EditTestStep(TestStep testStep)
        {
            // The first clauses ensures that only edits that are done to existing items are tracked.
            // If a new item is created it should be added to addedTestSteps first and then never to the editedTestSteps.
            // The second clauses endusres that an edited TestStep isn't added to editedTestSteps multiple times.
            if (!addedTestSteps.Contains(testStep) && !editedTestSteps.Contains(testStep))
            {
                editedTestSteps.Add(testStep);
            }
        }

        public void DeleteTestStep(TestStep testStep)
        {
            // The only time a deleted testStep needs to be added to deletedTestSteps is if the TestStep exists as a database entry.
            // If an item was just added to the template, it will be in addedTestSteps but not in the database.
            // In this case, removing it from the view and from addedTestSteps will be enough to stop tracking it.
            // If the item is in editedTestSteps, then it already existed in the database and will need to be marked for removal.
            if (addedTestSteps.Contains(testStep))
            {
                addedTestSteps.Remove(testStep);
            }
            else if (editedTestSteps.Contains(testStep))
            {
                editedTestSteps.Remove(testStep);
                deletedTestSteps.Add(testStep);
            }
            else
            {
                deletedTestSteps.Add(testStep);
            }
        }

    }
}
