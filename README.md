[![Build status](https://ci.appveyor.com/api/projects/status/87o4wqq178t5o4x5?svg=true)](https://ci.appveyor.com/project/bobthearsonist/matchmaking/branch/master)

## Product Vision

Enhance online gameplay and restore community feel to gaming via the creation of matchmaking teams of selectable size from a known list of players with attributes based on game skill, playing style, and social behavior.

### Stakeholders

Developers of multiplayer games seeking a matchmaking tool using more than just game skill for selection.
Players looking for an enhanced, communal gaming experience.

## Definition of Ready

* Clear, succinct title describes deliverable item
* Description expressed as a user story in "poem" form e.g. "As a ____, I want a ____, so that ____"
* Description may also contain clarifying details like "store this in s3" or "use the following endpoint to get the data"
* Card represents identifiable single deliverable item of work
* Card is pointed

## Backlog Rules

The backlog is ranked by order to complete. The cards at the top of the backlog or sprint will be worked prior to the cards below them.

A card is done when it has moved through every swimlane in the board INCLUDING REVIEW.

When a member is done with their card they will take the next card at the top of the backlog. No exceptions unless discussed and agreed buy the team.

No member shall have more than one card at any one time. This results in a max number of "in flight" cards of 4.

### Pointing/Estimating/Tracking

Story points are estimated in "level of effort" using the "fibonacci" method using the set [0.5,1,2,3,5]. 

Cards are pointed in person during the planning meeting using a "poker" approach with each individual ranking the card at the same time.

Story points go in parenthesis as the first item  in the subject line of the card. Time spent goes in aquare brackets as the last item in the subject line of the card

## Merges and reviews

Code cannot be merged into master without a PR.

A PR cannot be closed without a review.

## Roster and Team Roles

* Martin Perry - Product Owner
* Sourav Debnath - Dev
* Kristin Hegna - Dev
* Chris Taylor - Scrum Master

## Backlog Order Rationale

The player definition was chosen as the keystone of the project given all other services center around it. A store for these objects will back all services so it was chosen second.

The services were chosen next in an order that was ranked by necessity, you need players for groups, and then by difficulty. This let us build an MVP and add features incrementally.

The algorithm was added last, while it is the core of the deliverable product it is also the most difficult, and doing a less featured implementation of it gets us to software that works faster, iterations could then improve it.

## Links

[Hosted application](http://ksuplayermatcher.azurewebsites.net/Users)

[Trello board for product backlog](https://trello.com/b/SxTVJTQK/matchmaking-product-backlog)
[Slack channel invite link](https://join.slack.com/t/emergingprocesses/shared_invite/enQtNTQ0NjkyNjIyNDU0LTcyMzZkZmViZWMzYmQyYThiMmVmZmZiOTI5YzY0YmEzYzdiOTJiNzk2Mzg3ZGI5ZTMyOTFjMTVlMTM1ZWU0NWM)

[match algorithm documentation](PlayerMatcher/Matchmaker/MatchmakerDocumentation.md)

[final presentation](https://docs.google.com/presentation/d/1eBvsf4SeJzLRH6KwdEDspDUQuYFX4R_Xm-tedLnyjKU/edit?usp=sharing)

[Burndown charts and other sprint information are in the documentation folder and are ordered by sprint](documentation/)
