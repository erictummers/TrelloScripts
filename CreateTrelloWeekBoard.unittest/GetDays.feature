Feature: GetDays
	In order to avoid wrong headers in Trello lists
	As a datetime processor
	I want to check the correct dates for the weekdays

Scenario Outline: Get correct mondays
	Given Program has Now set to <NOW>
	Then GetMonday returns <Monday>
	
	Examples: 
	| NOW        | Monday |
	| 08-12-2014 | 15 dec |
	| 10-12-2014 | 15 dec |
	| 12-12-2014 | 15 dec |
	| 15-12-2014 | 22 dec |

Scenario Outline: Get correct tuesdays
	Given Program has Now set to <NOW>
	Then GetTuesday returns <Tuesday>
	
	Examples: 
	| NOW        | Tuesday |
	| 08-12-2014 | 16 dec  |
	| 10-12-2014 | 16 dec  |
	| 12-12-2014 | 16 dec  |
	| 15-12-2014 | 23 dec  |

Scenario Outline: Get correct wednesday
	Given Program has Now set to <NOW>
	Then GetWednesday returns <Wednesday>
	
	Examples: 
	| NOW        | Wednesday |
	| 08-12-2014 | 17 dec    |
	| 10-12-2014 | 17 dec    |
	| 12-12-2014 | 17 dec    |
	| 15-12-2014 | 24 dec    |

Scenario Outline: Get correct thursday
	Given Program has Now set to <NOW>
	Then GetThursday returns <Thursday>
	
	Examples: 
	| NOW        | Thursday |
	| 08-12-2014 | 18 dec   |
	| 10-12-2014 | 18 dec   |
	| 12-12-2014 | 18 dec   |
	| 15-12-2014 | 25 dec   |

Scenario Outline: Get correct fridays
	Given Program has Now set to <NOW>
	Then GetFriday returns <Friday>
	
	Examples: 
	| NOW        | Friday |
	| 08-12-2014 | 19 dec |
	| 10-12-2014 | 19 dec |
	| 12-12-2014 | 19 dec |
	| 15-12-2014 | 26 dec |
