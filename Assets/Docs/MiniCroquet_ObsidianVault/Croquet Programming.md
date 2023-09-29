## Gameplay
Right, so to begin with in terms of gameplay, I want to start with the data for the Match. This data is for the match that is actually happening in realtime:
*Match* Class:
- MatchPlayers
	- (int or enum) # of Players (2 or 4, singles or doubles)
	- (int, int) Team 1 Points, Team 2 Points
	- (function(enum)) Turn order of Players
	- (bool[]) Which Players are AI or not
	- (MatchPlayer) *Current* Player Whose Turn it is
	- (bool[]) Which Players have Completed their Turn
	- (bool) Is Player Input Complete? / Final Check
	
- Wickets
	- (List<*Wicket*>) Wickets on the map
	- (Wicket) Currently Active Wicket
	- (int) Currently Active WicketGate Index
	- (WicketGate) Currently Active WicketGate

Next, I *can* store the data in terms of the current turn, but because the physics aren't really *mine*, I'm not sure how reliably the hits could be reproduced. Besides this, I definitely *could* store which turns teams scored in. I won't, for now.

Now, let's take a look at the scoring zones and possibilities with Wickets. In croquet, the Wicket refers to the structure itself, but *the direction the ball enters the wicket IS relevant*. As such, I'm writing the following struct to clear any confusion.

*WicketGate* Struct:
- (int) WicketIndex
- (bool) FromFront
	- NOTE: Marking this is arbitrary, could be done with a projected light cookie and arrow locally to the "front" side of the wicket, but the differentiation is necessary.
*Wicket* Class:
	- (List<*WicketGate*>) WicketGates
	
	