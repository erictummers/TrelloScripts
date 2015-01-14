TrelloScripts
=============

My Trello scripts to automate repetitive tasks. 
Visual Studio 2013 projects written in csharp.

|Project|Description|
|:-------|:-----------|
|**CreateTrelloWeekBoard**|I use this to create a weekboard in trello. A todo list, doing list and a list for each working day. More info on [my blog](https://erictummers.wordpress.com/2014/01/14/comments-on-a-better-you-thru-machines/)|
|**MoveTrelloCardDueDate**|To move the due date of all cards in a board to the same time. Used it to move all my training to 20:00.|
|**UpdateTrelloTrainingBoard**|Copy a board and move the due date by some weeks. My (half) marathon training template gets copied and the dates get moved to the new match day.|


To get the code working you must add this partial class to the projects. 
```csharp
static partial class TrelloIds
{
	static TrelloIds()
	{
		AppKey = "YOUR_APP_KEY";
		UserToken = "YOUR_REQUESTED_TOKEN";
	}
}
```

It will set the TrelloIds with your AppKey and UserToken. Request 
- AppKey with https://trello.com/1/appKey/generate and 
- UserToken with https://trello.com/1/authorize?key=YOUR_APP_KEY&name=YOUR_APP_NAME&expiration=1day&response_type=token&scope=read,write.


