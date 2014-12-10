using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TechTalk.SpecFlow;

namespace CreateTrelloWeekBoard.unittest
{
    [Binding]
    public class GetDaysSteps
    {
        [Given(@"Program has Now set to (.*)")]
        public void GivenProgramHasNowSetTo(string now)
        {
            Program.Now = DateTime.ParseExact(now, "dd-MM-yyyy", null);
        }

        [Then(@"GetMonday returns (.*)")]
        public void GetMondayReturns(string expectedDate)
        {
            var result = Program.GetMonday();
            Assert.AreEqual<string>(expectedDate.ToLower(), result.ToLower());
        }

        [Then(@"GetTuesday returns (.*)")]
        public void GetTuesdayReturns(string expectedDate)
        {
            var result = Program.GetTuesday();
            Assert.AreEqual<string>(expectedDate.ToLower(), result.ToLower());
        }

        [Then(@"GetWednesday returns (.*)")]
        public void GetWednesdayReturns(string expectedDate)
        {
            var result = Program.GetWednesday();
            Assert.AreEqual<string>(expectedDate.ToLower(), result.ToLower());
        }

        [Then(@"GetThursday returns (.*)")]
        public void GetThursdayReturns(string expectedDate)
        {
            var result = Program.GetThursday();
            Assert.AreEqual<string>(expectedDate.ToLower(), result.ToLower());
        }

        [Then(@"GetFriday returns (.*)")]
        public void GetFridayReturns(string expectedDate)
        {
            var result = Program.GetFriday();
            Assert.AreEqual<string>(expectedDate.ToLower(), result.ToLower());
        }
    }
}
