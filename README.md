# SimpleElevator
Simple Elevator example for Intuit interview
/*
Solution
	Intuit Elevator Simulator

Team 
	Tillman Dickson, Jr.
	
Requestor
	Russell Bolles, Intuit
	
Summary
	Requested by Intuit API team as homework.
		To be used as a basis for talking points during interviews.
		Preliminary evaluation of problem decomposition
		
Requirements
	(as stated)
	Design and implement 
		a basic elevator control system for a N-story (N>1) building with only 1 elevator
		a simulator that can accept requests from passengers and change states (idle, up, down, open, close) 
			based on decisions made by the control system. 
			
	Options (do not implement)
		1. Support for multiple elevators
		2. Calling for an elevator via a REST API 

	Deliverables
		1. A document explaining the design principles and algorithm
		2. Working production ready code and corresponding test cases
		3. A README file explaining the usage of simulator:
		   1. How to build it
		   2. How to run it (e.g. command line, main function, etc),
		   3. What is the expected output?

Implementation
	Compromises
	
		Issue 1
			A proper elevator control system would need to be an event driven time critical system. 
			Such systems require decoupling such as 
					message prioritization
					queueing
					logging
					monitoring
					-of course- thread control
					...
			Given the objectives as stated implementing such decoupling would put the solution well outside the time-scope of deliverables.
		Workaround
			Will implement the control system as a state machine with a fairly straightforward processing loop
			
		Issue 2
			Elevators do not always travel at a constant velocity (V=0 or +/-x). The spend some amount of time accelerating.
			Accounting for this adds complexity to costing elevator travel. Note: we'd presolve the integral and use a lookup table, but the added complexity is unneeded here.
		Workaround
			Will assume an elevator's velocity is either 0 (stopped) or a predifined constant rate of travel.
			
		For now we will implement only a single elevator and only really rely on a single elevator to avoiding costing multiple elevator paths
			 


	Solution terminology
		Actors
			SimpleElevatorController
				Takes queued requests from which it determines needed elevator operation then comnmands elevator
				
			Bank of Elevators
				set, "bank", of 1 to n elevators all serving the same Floors
			
			Elevator
				single elevator
				
		Actions
			Stop Request
				request to send an elevator to a specific Floor
					-domain- someone pushed an up, down, or floor button
				
				Types of Floor Requests
					Pickup Request	
						servicable by any Elevator in the Elevator Bank
						-domain- passenger requesting an elevator, will include requested direction
						
					Dropoff Request
						Servicable a specific elevator
						-domain- passenger on elevator requesting floor for dropoff
				
			Commands
				Ascend
					move up one floor
					
				Descend
					move down one floor
					
				Open Doors
					open doors to allow passenger board/deboarding
					
				Park
					waiting for request with doors closed
					
		Responsibilities
		
		Interactions
			
	
	
	Description
		The deliverable will consist of two primary components:
			A control system capable of:
				Managing
			
			A test rig simulating single elevator requests and operations:
				Test Scenario
					Create a single elevator bank servicing six floors with one elevator and controller.
					Elevator starts Parked at first floor 

				  1) Queue request for Pickup on floor 1 
				  	 Queue request for Dropoff on floor 6 
				  	 
						 Execute controller twice
						 
						 Queue request for Dropoff on 3 (elevator should be here on way to floor 6)
						 Queue request for Pickup on 4
						 
						 Execute controller until no queued commands remain
						 
						 Queue request for Pickup on 4 
						 Queue request for Dropoff on 3 
						 
						 Execute controller until no queued commands remain
						 
				Expected result
					Elevator Parked on 1
					Elevator DoorsOpen on 1
					Elevator Travelling on 2
					Elevator Travelling on 3
					Elevator DoorsOpen on 3
					Elevator Travelling on 4
					Elevator DoorsOpen on 4
					Elevator Travelling on 5
					Elevator Travelling on 6
					Elevator DoorsOpen on 6
					Elevator Travelling on 5
					Elevator Travelling on 4
					Elevator DoorsOpen on 4
					Elevator Travelling on 3
					Elevator DoorsOpen on 3
	
	Build instructions needed
					
*/


Description of what, why, and how.

Build a basic elevator controller to simulate basic elevator control: ascend, descend, open doors, & park.

Main components:
	A simulated Elevator (lift) and Elevator Bank to be commanded by controller
	A controller to receive queued elevator requests, process requests based on priorities, dispatch commands to Elevator(s) to complete requests
	A queue to decouple elevator requests from the controller
	A ui to send requests

Elevator and Elevator Bank
	Elevator Bank 
		a simple collection of elevator. Serves as a container with a few helper methods for finding a specific elevator.
		
	Elevator
		a simulated elevator. Maintains basic elevator state information such as its ID, current floor (landing), floors it can access, its current operation.
				It also receives and responds to basic commands: Ascend, Descend, Open Doors, Park.
				
Controller
	responsible for maintaining a queue of elevator requests, analyzing elevator requests for priority -see Prioritizing Requests-, and dispatching elevator commands to fullfill the requests.
	To eliminate the real-time nature of such a controller for this project I have modeled the controller to follow a simple cycle: 
		update queue with new requests
		process the queue to generate elevator operation commands
		dispatch command(s) to elevator(s)
		repeat.
		
Request queue
	a very simple object to accept elevator requests from the UI and queue them.
	The intent is to decouple the controller from being coupled to request submission.
	The queue's stop requests are all passed to controller at the beginning of its processing cycle.
	
	

Logic
	Most of the logic is basic wiring stuff together.
	
	Prioritizing Requests
		Assumptions
			For simplification purposes the following assumptions of behavior have been made.
				All single elevator operations cost the same time. 
					There's no accounting for acceleration, slow boarding of passengers et cetera.
				There is a single elevator in the bank. In general this assumption isn't relied on, but there
					is a marked shortcut in processing that takes advantage of this assumption to eliminate the need to create 
					formal costing algorithms.
				Elevator requests come in two flavors
					Pickup Request - represents a passenger on a floor (landing) pressing an up or down
													 	button to request an elevator.
													 In an elevator bank of more than one elevator Pickup Requests could be 
													 	serviced by any of the elevators in the bank.
					Dropoff Request - represents a passenger on an elevator requesting to be let off on a
														specified floor. 
														Dropoff Requests must be serviced by a certain elevator, the one from 
																which the request was made.
														
		Given that formal costing isn't needed in our constrained scenario we use simple logic, roughly akin to the
				ladder logic which runs most elevators.
			An elevator has two biases:
				To continue in its current or most recent direction of travel (tracked by elevator).
				When breaking a tie drop offs come before pickups.
				
			Decision making
				Per elevator a decision is made based on the elevator's current floor (landing).
				The result of a decision can be:
					Ascend one floor
					Descend one floor
					Open doors
					Park (accumulator and simple logic to autopark not done)
					Nothing
					
				1) Are there stop requests for the current floor?
						Yes
							Open Doors
							Done this cycle
						
				2) Are there requests along the elevators current direction of travel?
						Yes
							Ascend
							Done this cycle
					
				3) Are there any drop off requests?
						Yes
							Ascend/Descend towards drop off floor
							
				4) Are there any pcik up requests?
						Yes
							Ascend/Descend towards pick up floor
				
				5) Nothing left
						Idle
						Not implemented, but should accumulate idle cycle count and when threshold reached Park the elevator
