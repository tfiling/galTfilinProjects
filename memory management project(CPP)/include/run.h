#ifndef RUN_H_
#define RUN_H_

#include <map>

#include "../include/Junction.h"
#include "../include/Road.h"
#include "../include/Event.h"
#include "../include/TimeSliceObject.h"
#include "../include/Report.h"


bool eventCompareAsTime ( Event* event1, Event* event2);
bool reportCompareAsTime ( Report* report1, Report* report2);
int readCommands(std::vector<Report*>& commands, int& terminationTime, ptree &ptReports
                  ,map<string, Junction*>& junctionsMap, map<string, Car>& carsMap ,map<string, Road>& roadsMap);
int readEvents(std::vector<Event*>& eventsVector , map<string, Road>& roadsMap ,map<string, Car>& carsMap);
void readMap(map<string, Junction*>& junctions, map<string, Road>& roads, const int DEFAULT_TIME_SLICE);
void advanceCarsInRoads(map<string, Road*>& roads, int currentTime, int MAX_SPEED);
void advanceCarsInJunctions(map<string, Junction*>& junctions, map<string, Car>& carsMap, map<string, Road>& roads, int currentTime);
void changeTrafficLights(map<string, Junction*>& junctions, const int MAX_TIME_SLICE, const int MIN_TIME_SLICE);
void sortAllCars(map<string, Road*>& roads);
static bool carComperator(Car*, Car*);


#endif // RUN_H_INCLUDED
