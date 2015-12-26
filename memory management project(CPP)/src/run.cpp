#include "../include/run.h"
#include "../include/AddCarEvent.h"
#include "../include/CarFaultEvent.h"
#include "../include/Event.h"
#include "../include/CarReport.h"
#include "../include/JunctionReport.h"
#include "../include/RoadReport.h"
#include <queue>
#include <iostream>
#include <algorithm>
#include <boost/property_tree/ini_parser.hpp>
#include <boost/property_tree/ptree.hpp>
#include <boost/lexical_cast.hpp>
#include <boost/lambda/lambda.hpp>
#include <boost/function.hpp>
#include <boost/lambda/casts.hpp>
#include <boost/circular_buffer.hpp>
#include <map>
#include <stdlib.h>
using namespace std;

bool eventCompareAsTime ( Event* event1, Event* event2)
{
    return (event1->getTimeOfEvent() < event2->getTimeOfEvent());
}

bool reportCompareAsTime ( Report* report1, Report* report2)
{
    return (report1->getExecutionTime() < report2->getExecutionTime());
}

void readMap(map<string, Junction*>& junctions, map<string, Road*>& roads, const int DEFAULT_TIME_SLICE)
{
    boost::property_tree::ptree pt;
    boost::property_tree::ini_parser::read_ini("RoadMap.ini", pt);
    for (boost::property_tree::ptree::const_iterator section = pt.begin();section != pt.end(); section++)
        {
        string junctionID = (section->first);
        vector<string> incomingRoads;
        int timeSlicesAmount = 0;
        for (boost::property_tree::ptree::const_iterator property = section->second.begin();property != section->second.end(); property++)
            {
                timeSlicesAmount++;
            }
        boost::circular_buffer<TimeSliceObject>* timeSlices = new boost::circular_buffer<TimeSliceObject>(timeSlicesAmount);
        for (boost::property_tree::ptree::const_iterator property = section->second.begin();property != section->second.end(); property++)
            {
                incomingRoads.push_back(boost::lexical_cast<string>(property->first.data()));
                string incomingRoadStartJunc = incomingRoads.back();
                TimeSliceObject newTimeSlice;
                if ((*timeSlices).empty())//first incoming road in timeslices queue
                    newTimeSlice = TimeSliceObject(incomingRoadStartJunc, DEFAULT_TIME_SLICE, DEFAULT_TIME_SLICE);
                else//not the first timeslice
                    newTimeSlice = TimeSliceObject(incomingRoadStartJunc, DEFAULT_TIME_SLICE,  -1);
                (*timeSlices).push_back(newTimeSlice);
            }
            Junction* newJunc = new Junction(junctionID, incomingRoads, *timeSlices); //= Junction(junctionID,incomingRoads,TimeSlices);
            junctions.insert(pair<string, Junction*>(junctionID, newJunc));
            delete timeSlices;
        }

        string endJunction, startJunction;
        int roadLength;
    for (boost::property_tree::ptree::const_iterator section = pt.begin();section != pt.end(); section++)
        {
        endJunction = (section->first);
        for (boost::property_tree::ptree::const_iterator property =    section->second.begin();property != section->second.end(); property++)
            {
                startJunction = (boost::lexical_cast<string>(property->first.data()));
                roadLength = (boost::lexical_cast<int>(property->second.data()));
                Road *newRoad = new Road((*junctions[startJunction]), (*junctions[endJunction]), roadLength);
                string name = startJunction+","+endJunction ;
                roads[name] = newRoad;
            }
        }

}

int readCommands(std::vector<Report*>& commands, int& terminationTime, ptree& ptReports, map<string, Junction*>& junctionsMap,map<string, Car*>& carsMap ,map<string, Road*>& roadsMap)
{

    string type, reportID, ID, startJunction, endJunction;
    int TimeOfEvent;
    int commandsAmount = 0;
    boost::property_tree::ptree pt;
    boost::property_tree::ini_parser::read_ini("Commands.ini", pt);
    for (boost::property_tree::ptree::const_iterator section = pt.begin();section != pt.end(); section++) {
        type = section->second.get<string>("type");
        TimeOfEvent = section->second.get<int>("time");
        if (type=="car_report")
        {
            reportID = section->second.get<string>("id");
            ID = section->second.get<string>("carId");
            CarReport &newCarReport = *new CarReport(reportID, ID, TimeOfEvent,ptReports, carsMap);
            commands.push_back(&newCarReport);
            commandsAmount++;
        }
        else if (type=="junction_report")
        {
        	reportID = section->second.get<string>("id");
            ID = section->second.get<string>("junctionId");
            JunctionReport &newJuncReport = *new JunctionReport(reportID, ID, TimeOfEvent,ptReports , junctionsMap , roadsMap);
            commands.push_back(&newJuncReport);
            commandsAmount++;
        }
        else if (type == "road_report")
        {
            reportID = section->second.get<string>("id");
            startJunction = section->second.get<string>("startJunction");
            endJunction = section->second.get<string>("endJunction");
            RoadReport &newRoadReport = *new RoadReport(reportID, startJunction, endJunction, TimeOfEvent,ptReports, roadsMap);
            commands.push_back(&newRoadReport);
            commandsAmount++;
        }
        else if (type == "termination")
        {
            terminationTime = TimeOfEvent;
        }
    }
    return commandsAmount;
}

int readEvents(std::vector<Event*>& eventsVector , map<string, Road*>& roadsMap ,map<string, Car*>& carsMap)
{
    string type, CarID, roadPlan;
    int TimeOfEvent, timeOfFault;
    int eventsAmount = 0;
    boost::property_tree::ptree pt;
    boost::property_tree::ini_parser::read_ini("Events.ini", pt);
    for (boost::property_tree::ptree::const_iterator section = pt.begin();section != pt.end(); section++)
    {
        eventsAmount++;
        type = section->second.get<string>("type");
        CarID = section->second.get<string>("carId");
        TimeOfEvent = section->second.get<int>("time");
            if (type=="car_arrival")
            {
                roadPlan = section->second.get<string>("roadPlan");
                AddCarEvent &newEvent = *new AddCarEvent(TimeOfEvent,CarID ,roadPlan, roadsMap, carsMap);
                eventsVector.push_back(&newEvent);

            }
            else if (type=="car_fault")
            {
                timeOfFault = section->second.get<int>("timeOfFault");

                CarFaultEvent &newEvent = *new CarFaultEvent(TimeOfEvent,CarID ,timeOfFault, carsMap);
                eventsVector.push_back(&newEvent);
            }
        }
    return eventsAmount;

}

void advanceCarsInRoads(map<string, Road*>& roads, int currentTime, int MAX_SPEED)
{
    int roadLength;
    for (map<string, Road*>::iterator roadIt = roads.begin(); roadIt != roads.end(); roadIt++) // iterating over each road
    {
        int baseSpeed = roadIt->second->calculateBaseSpeed();
        if (baseSpeed > 0) //baseSpeed = 0 means there are no cars in the road
        {
            roadLength = roadIt->second->getLength();
            vector<Car*> carsInCurrentRoad = roadIt->second->getCars();
            for (vector<Car*>::iterator carIt = carsInCurrentRoad.begin(); carIt != carsInCurrentRoad.end(); carIt++) // iterating over cars in the road
            {
            	int faultyTimeLeft = (*carIt)->getFaultyTimeLeft();
                if (faultyTimeLeft > 0){
                    baseSpeed /= 2;
                	(*carIt)->setFaultyTimeLeft(-faultyTimeLeft); // zeroes the faulty time left for the car
                	(*carIt)->setFaultyTimeLeft(faultyTimeLeft-1); // now the faulty time left has only been decreased by 1

                }
                else if (baseSpeed > MAX_SPEED)
                    (*carIt)->advanceCar(MAX_SPEED, currentTime, roadLength);
                else
                    (*carIt)->advanceCar(baseSpeed, currentTime, roadLength);
            }
        }
    }
}

void  advanceCarsInJunctions(map<string, Junction*>& junctions, map<string, Car*>& carsMap, map<string, Road*>& roads, int currentTime)
{
    for (map<string, Road*>::iterator roadIt = roads.begin(); roadIt != roads.end(); roadIt++) // for each road
    {
        Junction startJunction = roadIt->second->getStartJunction(); // the junction the road starts from
        string s_StartJunction = startJunction.getJiD(); // the junction's name
        Junction endJUnction = roadIt->second->getEndJunction(); // the junction at the end of the road
        if (roadIt->second->waitingCarsExist() && endJUnction.checkGreenLight(s_StartJunction))
        // there are cars waiting at the end of the road and the road has a green light
        {

            Car* passedCar = roadIt->second->popFirstCar(); // might return null
            // maybe to add a bool function..
            if (passedCar != 0 ){
            	// need to debug and move cars to the next road
            	string nextJunc = passedCar->getNextRoad() ;

            	if ( roads.find(nextJunc) == roads.end() )
            	{
            		// not found
            		// create history in the main
            	} else {
            		roads[nextJunc]->addCar(passedCar);
            		junctions[endJUnction.getJiD()]->increasePassedCarsCounter();
            	}
            	passedCar->setPassedToFalse();
            }
        }
    }
}

void changeTrafficLights(map<string, Junction*>& junctions, const int MAX_TIME_SLICE, const int MIN_TIME_SLICE)
{
    for(map<string, Junction*>::iterator junctionIt = junctions.begin(); junctionIt != junctions.end(); junctionIt++)
    {
        junctionIt->second->switchLightsInJunction(MAX_TIME_SLICE, MIN_TIME_SLICE);
    }
}

void sortAllCars(map<string, Road*>& roads)
{
    for (map<string, Road*>::iterator roadIt = roads.begin(); roadIt != roads.end(); roadIt++)
    {
        vector<Car*>& carsForSort = roadIt->second->getCars();
        std::sort(carsForSort.begin(), carsForSort.end(), carComperator);

    }
}

bool carComperator(Car* car1, Car* car2)
{
    return (car1->getLocation() < car2->getLocation());
}

void destruct(map<string, Junction*>& junctionsMap ,map<string, Car*>& carsMap,
		map<string, Road*>& roadsMap,vector<Event*>& eventsVector,vector<Report*>& commandsVector)
{
	 //deletes the events
	 vector<Event*>::iterator eventIterator=eventsVector.begin();
	 while(eventIterator!=eventsVector.end())
	 {
		 delete (*eventIterator);
		 eventIterator++;
	 }

	 //deletes the events
	 vector<Report*>::iterator reportIterator=commandsVector.begin();
	 while(reportIterator!=commandsVector.end())
	 {
		 delete (*reportIterator);
		 reportIterator++;
	 }

	 //deletes roads
	 for (map<string, Road*>::iterator roadIt = roadsMap.begin(); roadIt != roadsMap.end(); roadIt++)
	 {
	     delete (roadIt->second);
	 }


	//deletes the junctions
    for(map<string, Junction*>::iterator junctionIt = junctionsMap.begin(); junctionIt != junctionsMap.end(); junctionIt++)
    {
        delete (junctionIt->second);
    }

	//delete all cars
	//declaring an iterator
	 map<string, Car*>::iterator carsIterator;
	 //foreach car, executes the destractor
	 for (carsIterator = carsMap.begin(); carsIterator != carsMap.end(); ++carsIterator)
	 {
	   	delete (carsIterator->second);
	 }

}



int main()
{
	// read the constants variables
    cout << "Starting simulation" << endl;

    boost::property_tree::ptree ptConf;
    boost::property_tree::ini_parser::read_ini("Configuration.ini", ptConf);
	int MAX_SPEED = ptConf.get<int>("Configuration.MAX_SPEED");
	int DEFAULT_TIME_SLICE = ptConf.get<int>("Configuration.DEFAULT_TIME_SLICE");
	int MAX_TIME_SLICE =  ptConf.get<int>("Configuration.MAX_TIME_SLICE");
	int MIN_TIME_SLICE =  ptConf.get<int>("Configuration.MIN_TIME_SLICE");

    map<string, Junction*> junctionsMap ;
    map<string, Car*> carsMap;
    map<string, Road*> roadsMap;

    readMap(junctionsMap, roadsMap, DEFAULT_TIME_SLICE);

    //reading events to queue and then read them :
    vector<Event*> eventsVector;
    vector<Report*> commandsVector;

    int terminationTime;
    int eventsIndex = 0;
    int commandsIndex = 0;
    int currentTime = 1;

    // the pt is now on the heap
    using boost::property_tree::ptree;
    ptree ptReports;
    readEvents(eventsVector , roadsMap ,carsMap);
    readCommands(commandsVector, terminationTime,ptReports, junctionsMap,carsMap ,roadsMap);

    std::sort(eventsVector.begin(), eventsVector.end(),eventCompareAsTime);
    std::sort(commandsVector.begin(), commandsVector.end(), reportCompareAsTime);

    int nextEventExecutionTime, nextCommandExecutionTime;
    nextEventExecutionTime = eventsVector.at(eventsIndex)->getTimeOfEvent();
    nextCommandExecutionTime = commandsVector.at(commandsIndex)->getExecutionTime();

    while (currentTime <= terminationTime)
    {
    	// performing events
        while (nextEventExecutionTime == currentTime && eventsIndex < (int)eventsVector.size())
        {
            // execute:
            eventsVector.at(eventsIndex)->performEvent();
            eventsIndex++;
            if (eventsIndex < (int)eventsVector.size())
            {
                 nextEventExecutionTime = eventsVector.at(eventsIndex)->getTimeOfEvent();
            }
        }



        // update cars history
        map<string, Car*>::iterator carsIterator;
        for (carsIterator = carsMap.begin(); carsIterator != carsMap.end(); ++carsIterator){
        	carsIterator->second->updateHistory(currentTime);
        }

        // write reports
        while (nextCommandExecutionTime == currentTime && commandsIndex < (int)commandsVector.size() )
        {
            // execute
            commandsVector.at(commandsIndex)->writeReport();
            write_ini( "Report.ini" , ptReports );
            commandsIndex++;
             if (commandsIndex < (int)commandsVector.size())
             {
                 nextCommandExecutionTime = commandsVector.at(commandsIndex)->getExecutionTime();
             }
        }

        // advance cars in roads
        sortAllCars(roadsMap); // makes sure the cars vector is sorted (in case a car was added)
        advanceCarsInRoads(roadsMap, currentTime, MAX_SPEED);
        sortAllCars(roadsMap); // makes sure the cars vector is sorted (in case a car has passed a faulty car)

        // advance cars in junctions
        advanceCarsInJunctions(junctionsMap, carsMap, roadsMap, currentTime);
        changeTrafficLights(junctionsMap, MAX_TIME_SLICE, MIN_TIME_SLICE);
        currentTime++ ;
    }
    destruct(junctionsMap,carsMap,roadsMap,eventsVector,commandsVector);
    cout << "Ending simulation" << endl;
    // implement destructors
 return 0;
}
