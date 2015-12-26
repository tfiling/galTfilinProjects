#ifndef CARFAULTEVENT_H_
#define CARFAULTEVENT_H_
#include "../include/Event.h"
#include "../include/Road.h"
#include "../include/Car.h"
#include<map>

class CarFaultEvent : public Event
{

public:

	//constructor:
	CarFaultEvent(int timeOfEvent,std::string carID , int timeOfFault ,std::map<std::string, Car*>& carsMap );

	//getters and setters:

	//getters:
	const int& getTimeOfFault() const ;

	//setters:
	void setTimeOfFault(int TimeOfFault) ;

	//destructor:
	virtual ~CarFaultEvent();

	//operator = :
    void performEvent();

private:// fields:
	int _timeOfFault;
    map<string, Car*>& _carsMapRef;
};

#endif
