
#include "../include/Event.h"
#include <string>

//empty constructor:
Event::Event()
:  _carID("car"), _timeOfEvent(-1)
{
}

//constructor:
Event::Event(int timeOfEvent, std::string carID )
:  _timeOfEvent(timeOfEvent), _carID(carID)
{
}

//getters:
const int& Event::getTimeOfEvent() const
{
	return _timeOfEvent;
}
const std::string& Event::getCarID() const
{
	return _carID;
}


//setters:
void Event::setTimeOfEvent(int TimeOfEvent)
{
    _timeOfEvent= TimeOfEvent;
}
void Event::setCarID(std::string CarID)
{
	_carID=CarID;
}


//destructor:
Event:: ~Event()
{
}

