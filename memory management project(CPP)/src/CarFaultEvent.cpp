

#include "../include/CarFaultEvent.h"
#include <string>
#include <vector>
#include <iostream>


//constructor:
CarFaultEvent::CarFaultEvent(int timeOfEvent,std::string carID , int timeOfFault ,std::map<string, Car*>& carsMap )
: Event(timeOfEvent, carID), _timeOfFault(timeOfFault),_carsMapRef(carsMap)
{
}

//getters and setters:

//getters:
const int& CarFaultEvent::getTimeOfFault() const
{
	return _timeOfFault;
}

//setters:
void CarFaultEvent::setTimeOfFault(int timeOfFault)
{
	_timeOfFault = timeOfFault;
}


//destructor:
CarFaultEvent::~CarFaultEvent()
{
}

void CarFaultEvent::performEvent()
{
    // set the faulty time for the car
    Car *faultCar = _carsMapRef[_carID];
    faultCar->setFaultyTimeLeft(_timeOfFault);
}
