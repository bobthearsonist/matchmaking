The baseline matchmaking algorithm is based on the Elo rating system for determining player selection for a match. At the start the matchmaker 
selects a single player, and then searches for players that are within 25 elo rating of the initial player (plus or minus 12 elo). If enough players
are not found to complete the match, the matchmaker will expand out the search window by 12 in both directions and then search again. This will 
continue until a match is completely constructed or a timeout is reached. 

In this rating system, the expected result of a match is calculated by comparing the Elo ratings of the opposing players using the following calculation,
where the expected result is the odds of winning, plus half the odds of drawing:

ExpectedResultA = 1 / (1 + 10^((RatingB - RatingA)/400))

This calcuation results in a 55% / 45% split in odds at a 25 point elo difference.