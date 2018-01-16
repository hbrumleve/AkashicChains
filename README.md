# Akashic Chains
Akashic Limited's flavor of Markov Chains. We may be duplicating other's work or somehow bastardizing terms. Please let us know and give us a chance to correct our ways. :-)

Using this for a guide in nomenclature
http://chain-guide.com/basics/1-1-4-functions-of-chain-parts.html

# Design Whiteboards
Here is a link to our [basic Akashic Chains design whiteboard](https://wbd.ms/share/v2/aHR0cHM6Ly93aGl0ZWJvYXJkcy1zZXJ2aWNlLmF6dXJld2Vic2l0ZXMubmV0L2FwaS92MS4wL3doaXRlYm9hcmRzL3JlZGVlbS9kZjYwZmRjOTZhNmM0ZTFhOWY1MzNjNzdhOWY2ODUzOF9CQkE3MTc2Mi0xMkUwLTQyRTEtQjMyNC01QjEzMUY0MjRFM0Q=)

Here is a link to our [Defined Pattern Execution design whiteboard](https://wbd.ms/share/v2/aHR0cHM6Ly93aGl0ZWJvYXJkcy1zZXJ2aWNlLmF6dXJld2Vic2l0ZXMubmV0L2FwaS92MS4wL3doaXRlYm9hcmRzL3JlZGVlbS9iNTcwMjg0YjRlZDQ0M2M1YTA0MGI0YTcxNTIxOTI1Ml9CQkE3MTc2Mi0xMkUwLTQyRTEtQjMyNC01QjEzMUY0MjRFM0Q=)

Here is a link to the [extended features design whiteboard](https://wbd.ms/share/v2/aHR0cHM6Ly93aGl0ZWJvYXJkcy1zZXJ2aWNlLmF6dXJld2Vic2l0ZXMubmV0L2FwaS92MS4wL3doaXRlYm9hcmRzL3JlZGVlbS8zMmY1Yzg0NWQwMzY0ZmY2YmIyNjA5ZTZlOWI5OTY0Ml9CQkE3MTc2Mi0xMkUwLTQyRTEtQjMyNC01QjEzMUY0MjRFM0Q=
). 

You'll need [Microsoft's Whiteboard app](https://www.microsoft.com/en-us/store/p/microsoft-whiteboard-preview/9mspc6mp8fm4) installed ... (I think) ... you might need an invite, too. 

# Akashic Chains
Akashic Chains are based upon [Markov Chains](https://en.wikipedia.org/wiki/Markov_chain). Markov Chains are usually defined as a timeseries set of events that resemble a flow chart, although they do not have to be linear. The chain helps determine the possibility of moving from one state to another. We are interested in creating a knowledge structure that will help us gain insight into possible 'next states' of a given event stream based upon the characteristics of the stream and its events.

There are 4 main components in our model.

* Trunk - This is the entry point for all events. It contains references to all braids and distributes the events to the approrpriate destinations. It is also the single point of communication for queries or domain event subscriptions.

* Braid - A braid represents a collection of chains. It receives an incoming event from the Trunk and determines if the event should be added to a chain within its responsibility. In [Domain Driven Design](https://en.wikipedia.org/wiki/Domain-driven_design) (DDD) terms, this may be a classification/definition of an aggregate or entity. It also can have a more functional and business impact such as detecting behavior or mapping a workflow.

* Chain - A chain is a collection of correlated events. It receives events from a braid and adds the link to itself. This would be an instance of an aggregate or an entity in DDD terms. Also it could represent an actual workflow occurring in the system.

* Link - When an event enters the system, it is encapsulated with a link. The link contains a representation of the original event in JSON. Each unique event is contained by 1 link. This means that the same link may appear in multiple chains in multiple braids.

# Master Braid
Each instance of a Trunk contains a master braid that uses the event name across an axis of time. This master braid can be used to perform recalculations of the entire chain or act as a source for a new calculation that is added at run time. 

# Default Chain within all Braids
In addition to a user defined chain, each braid will automatically collect the same information as the master braid, but using the discrimination algorithm of the braid itself. So each braid's default chain may have all or some of the Master Braid's links.

# Transits
To be designed: Create a way to watch a specific stream and find a matching path in the braids. Should enable us to find predictive behavior.

# Segmented Akashic Chains (Links?)
To be designed: Create another default chain that would allow each event in the chain to be segmented by its internal values. This would give us greater insight into possible paths for a given transit.

# Longitudinal Analysis (LA)
Each braid of chains contains the ability to create an axis of longitudinal analysis that can exist at a braid level or for each individual chain.

In order to process the LA an evaluator must be set at the braid or chain level. The evaluator contains a name (which must be all lowercase, between 5 and 50 characters long, unique to the braid or chain it references) and 3 functions that are written in javascript in the following formats:
* Evaluation Function =>  " function Evaluate(event, state) { }" the function returns a JSON object that represents the evaluation of the event.

* Distance Function => " function Distance(eventA, eventB, state) { } " the function returns a JSON object that represents the distance between the events. The event data is pulled from evaluations, not the original events. 

# Defined Pattern Detection and Link Analysis
Pattern detectors will listen to a given braid or chain (possibly the trunk, too). The detector will keep a state and look for series of events, specific thresholds, and occurences across multiple streams.

When a pattern is detected the detector can notify the trunk, which in turn may notify subscribers of the pattern. The payload would be a JSON object determined by the detector.

In addition to the notification, the pattern may also yield 0 or more defined or synthetic events. These events are submitted to the trunk and delivered via normal means. (figure out a way to kill a feedback loop?) 

# Synthetic Events and Braids
There are 2 types of events in our system:
* Defined Events - Defined events are events that come into the system from external sources.

* Synthetic Events - Synthetic events are events that are defined by pattern detectors within our system. They do not exist in the wild.

A braid can be defined to accept synthetic events and it is considered synthetic ...

# Future
Find a way for patterns to watch Synthetic braids? Or is that already taken care of?

* Intuitions - Can we define a set of patterns and give the user an idea of their likelihood or impact?
* Undefined Pattern analysis / Pattern discovery - based on parameters and dynamic range of values ... hmmm
