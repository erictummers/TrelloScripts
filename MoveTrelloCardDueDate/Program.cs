using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoveTrelloCardDueDate
{
    /// <summary>
    /// Move the due data of all cards in a board to 20:00
    /// </summary>
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
            foreach (var card in board.Cards)
            {
                if (card.DueDate.HasValue)
                {
                    // move to 20:00 on the same day
                    card.DueDate = card.DueDate.Value.Date.AddHours(20);
                    card.Refresh();
                }
            }
            foreach (var card in board.Cards)
            {
                Console.WriteLine("{0}: {1}", card.DueDate, card.Name);
            }
            TrelloProcessor.Shutdown();
            Console.ReadLine();
        }
    }

    static partial class TrelloIds
    {
        public static string AppKey { get; private set; }
        public static string UserToken { get; private set; }
    }
}
