using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace UpdateTrelloTrainingBoard
{
    /// <summary>
    /// Move all cards in a board back to the Todo list
    /// and change the due date by 9 weeks
    /// </summary>
    /// <remarks>
    /// Used for my Marathon training programs
    /// </remarks>
    class Program
    {
        static void Main(string[] args)
        {
            var serializer = new ManateeSerializer();
            TrelloConfiguration.Serializer = serializer;
            TrelloConfiguration.Deserializer = serializer;
            TrelloConfiguration.JsonFactory = new ManateeFactory();
            TrelloConfiguration.RestClientProvider = new RestSharpClientProvider();
            TrelloAuthorization.Default.AppKey = TrelloIds.AppKey;
            TrelloAuthorization.Default.UserToken = TrelloIds.UserToken;

            var boardId = "SET_ME_TO_A_VALID_BOARDID";
            var board = new Board(boardId);
            Console.WriteLine(board.Name);
            var todo = board.Lists.First(x => x.Name.Equals("To do", StringComparison.InvariantCultureIgnoreCase));
            foreach (var card in board.Cards)
            {
                // remove the label
                if (card.Labels.Count() > 1) card.Labels.ToList().ForEach(card.Labels.Remove);
                // move to TODO 
                if (false == card.List.Name.Equals(todo.Name)) card.List = todo;
                // move the duedate
                var moveBy9WeeksInDays = 63;
                if (card.DueDate.HasValue)
                {
                    card.DueDate = card.DueDate.Value.Date.AddDays(moveBy9WeeksInDays);
                    //card.Refresh();
                }
            }
            TrelloProcessor.Shutdown();
            Console.WriteLine("done");
            Console.ReadLine();
        }
    }

    static partial class TrelloIds
    {
        public static string AppKey { get; private set; }
        public static string UserToken { get; private set; }
    }
}
