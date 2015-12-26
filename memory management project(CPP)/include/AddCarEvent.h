#ifndef ADDCAREVENT_H_
#define ADDCAREVENT_H_

#include <string>
#include <map>
#include "../include/Junction.h"
#include "../include/Road.h"
#include "../include/Car.h"
#include "../include/Event.h"

class AddCarEvent : public Event
{
public:
//constructors:

//empty constructor:
AddCarEvent();

//constructor:
AddCarEvent(int TimeOfEvent,std::string CarID ,std::string roadPlan ,
			 std::map<std::string, Road*>& roadsMap ,std::map<std::string, Car*>& carsMap);

//getters and setters:

//getters:
const std::string getRoad_plan() const ;
const std::map<std::string, Car*>& getCarsMapRef() const;
const std::map<std::string, Road*>& getRoadsMapRef() const ;

//setters:
void setRoadPlan(std::string roadPlan) ;

//destructor:
virtual ~AddCarEvent();

//perform event from event abstract class:
void performEvent();


private:
// fields:
	std::string _roadPlan;
	map<string, Car*>& _carsMapRef;
	map<string, Road*>& _roadsMapRef;
// functions:
	std::string getFirstRoad(); // returns the first road in the car's road plan
};



#endif
