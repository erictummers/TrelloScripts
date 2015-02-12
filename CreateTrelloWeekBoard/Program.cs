using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CreateTrelloWeekBoard
{
    /// <summary>
    /// Create a trello board for next weeks planning
    /// </summary>
    public class Program
    {
        static Program() { Now = DateTime.Now; }
        // for testing purposes
        public static DateTime Now
        {
            get;
            set;
        }

        public static void Main(string[] args)
        {
            var serializer = new ManateeSerializer();
            TrelloConfiguration.Serializer = serializer;
            TrelloConfiguration.Deserializer = serializer;
            TrelloConfiguration.JsonFactory = new ManateeFactory();
            TrelloConfiguration.RestClientProvider = new RestSharpClientProvider();
            TrelloAuthorization.Default.AppKey = TrelloIds.AppKey;
            TrelloAuthorization.Default.UserToken = TrelloIds.UserToken;

            var nextweeknr = GetNextWeekNumber();
            var nextboardname = string.Format("week {0}", nextweeknr);

            var nextboard = Member.Me.Boards.Add(nextboardname);
            nextboard.Description = "This board was generated with CreateTrelloWeekBoard from " + 
                                    "Eric Tummers (https://github.com/erictummers/trelloscripts)\n" +
                                    "Using Manatee.Trello to communicate with Trello";
            nextboard.Lists.Where(x => x.Name.Equals("to do", StringComparison.InvariantCultureIgnoreCase)).First().IsArchived = true;
            nextboard.Lists.Where(x => x.Name.Equals("done", StringComparison.InvariantCultureIgnoreCase)).First().IsArchived = true;
            nextboard.Lists.Where(x => x.Name.Equals("doing", StringComparison.InvariantCultureIgnoreCase)).First().IsArchived = true;
            nextboard.Lists.Add(GetFriday());
            nextboard.Lists.Add(GetThursday());
            nextboard.Lists.Add(GetWednesday());
            nextboard.Lists.Add(GetTuesday());
            nextboard.Lists.Add(GetMonday());
            nextboard.Lists.Add("Doing");
            var todo = nextboard.Lists.Add("To Do");

            var email = GetEmailFromPrefs(nextboard.Id);
            var cardSetupEmail = todo.Cards.Add("Setup Email");
            cardSetupEmail.Description = string.Format("Use IFTTT with {0}", email);

            var createWeekboardName = string.Format("Create week {0} board", GetNextNextWeekNumber());
            var cardCreateWeekboard = todo.Cards.Add(createWeekboardName);
            cardCreateWeekboard.Description = "Run CreateTrelloWeekBoard script again on Friday";

            Console.WriteLine("Card {0} created on {1}", cardSetupEmail.Name, cardSetupEmail.List.Name);
            Console.WriteLine("Card {0} created on {1}", cardCreateWeekboard.Name, cardCreateWeekboard.List.Name);

            Console.Write("Shutting down ...");
            TrelloProcessor.Shutdown();
            Console.WriteLine(" done");

            //var weeknr = GetCurrentWeekNumber();
            //var boardname = string.Format("week {0}", weeknr);
            //var board = Member.Me.Boards.Where(x => x.Name.Equals(boardname, StringComparison.InvariantCultureIgnoreCase)).First();
            //var open = board.Lists.Where(x => x.Name.Equals("to do", StringComparison.InvariantCultureIgnoreCase)).First();
            //foreach(var c in open.Cards)
            //{
            //    if (c.IsArchived == false)
            //    {
            //        // move to next week
            //        var moved = todo.Cards.Add(c);
            //        moved.Comments.Add(string.Format("Moved from {0}", open.Name));
            //        c.IsArchived = true;
            //    }
            //}
            // close the board?
            //board.IsClosed = true;
            //Console.WriteLine("Moved cards from {0}.{1} to {2}", board.Name, open.Name, nextboard.Name);

            Console.ReadLine();
        }

        // should report this to https://bitbucket.org/gregsdennis/manatee.trello
        public static string GetEmailFromPrefs(string boardId)
        {
            var email = default(string);
            var url = string.Format("https://api.trello.com/1/boards/{2}/emailKey/generate?key={0}&token={1}", TrelloIds.AppKey, TrelloIds.UserToken, boardId);
            var request = WebRequest.Create(url);
            request.Method = "POST";
            var jsonResponse = default(string);
            try {
                using (var response = request.GetResponse())
                    using (var stream = response.GetResponseStream())
                        using (var reader = new StreamReader(stream))
                            jsonResponse = reader.ReadToEnd();
            } catch (Exception ex) {
                Trace.TraceError("Generate EmailKey failed with {0}", ex.Message);
            }
            if (string.IsNullOrEmpty(jsonResponse) == false)
            {
                Trace.TraceInformation(jsonResponse);
                try {
                    var prefs = new Manatee.Json.JsonObject(jsonResponse);
                    email = prefs["myPrefs"].Object["fullEmail"].String;
                } catch (Exception ex) {
                    Trace.TraceError("Get FullEmail from response failed with {0}", ex.Message);
                }
            }
            return email;
        }

        public static int GetCurrentWeekNumber()
        {
            return GetWeekNumber(Now);
        }
        public static int GetNextWeekNumber()
        {
            return GetWeekNumber(Now.AddDays(7));
        }
        public static int GetNextNextWeekNumber()
        {
            return GetWeekNumber(Now.AddDays(14));
        }
        public static int GetWeekNumber(DateTime date)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        public static string GetFriday()
        {
            return GetDayOfWeek(Now.AddDays(7), DayOfWeek.Friday).ToString("dd MMM");
        }
        public static string GetThursday()
        {
            return GetDayOfWeek(Now.AddDays(7), DayOfWeek.Thursday).ToString("dd MMM");
        }
        public static string GetWednesday()
        {
            return GetDayOfWeek(Now.AddDays(7), DayOfWeek.Wednesday).ToString("dd MMM");
        }
        public static string GetTuesday()
        {
            return GetDayOfWeek(Now.AddDays(7), DayOfWeek.Tuesday).ToString("dd MMM");
        }
        public static string GetMonday()
        {
            return GetDayOfWeek(Now.AddDays(7), DayOfWeek.Monday).ToString("dd MMM");
        }
        public static DateTime GetDayOfWeek(DateTime date, DayOfWeek weekday)
        {
            var dt = date;
            for (; weekday != dt.DayOfWeek && dt.DayOfWeek != DayOfWeek.Monday; dt = dt.AddDays(-1));
            if (weekday == dt.DayOfWeek) return dt;
            for (; weekday != dt.DayOfWeek; dt = dt.AddDays(1));
            return dt;
        }
    }

    static partial class TrelloIds
    {
        public static string AppKey { get; private set; }
        public static string UserToken { get; private set; }
    }
}
