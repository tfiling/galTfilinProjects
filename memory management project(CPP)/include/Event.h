#ifndef EVENT_H_
#define EVENT_H_

#include<string>

class Event
{
public:
	//constructors:

	//empty constructor:
	Event();

	//constructor:
	Event(int timeOfEvent, std::string carID );

	//getters and setters:
	//getters:
	const int& getTimeOfEvent() const ;
	const std::string& getCarID() const ;

	//setters:
	void setTimeOfEvent(int timeOfEvent) ;
	void setCarID(std::string carID) ;

	//destructor:
	virtual ~Event();

	//other functions
	virtual void performEvent()=0;

protected:
// fields:
	int _timeOfEvent;
	std::string _carID ;
};











#endif
