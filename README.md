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
