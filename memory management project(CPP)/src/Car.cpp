

#include "../include/Car.h"
#include <boost/lexical_cast.hpp>
using namespace std;


//constructors:

// empty constructor
Car::Car()
: _cID("car"), _roadPlan(""), _faultyTimeLeft(0), _passedCurrentRoad(false), _history(""), _currRoad(""), _nextRoad(""), _location(0)
{
}

// constructor
Car::Car(std::string ID, std::string roadPlan)
: _cID(ID), _roadPlan(roadPlan), _faultyTimeLeft(0), _passedCurrentRoad( false), _history(""), _currRoad(""), _nextRoad(""), _location(0)
{
}

//getters and setters:

//getters:
std::string Car::getID() const   // const - can't change after the value will return
{
	return _cID;
}

std::string Car::getRoadPlan() const
{
	return _roadPlan;
}

int Car::getFaultyTimeLeft() const
{
	return _faultyTimeLeft;
}

bool Car::getPassedRoad() const
{
    return _passedCurrentRoad;
}

std::string Car::getHistory() const{
	return _history;
}

std::string Car::getCurrentRoad() const{
	return _currRoad;
}

int Car::getLocation() const{
	return _location;
}

std::string Car::getNextRoad() const{
	return _nextRoad;
}

//setters:
void Car::setID(string iD)
{
	_cID=iD;
}

void Car::setRoadPlan(std::string roadPlan)
{
	_roadPlan=roadPlan;
}
void Car::setFaultyTimeLeft(int faultyTimeLeft)
{
	_faultyTimeLeft = _faultyTimeLeft + faultyTimeLeft;
}

void Car::popFirstInRoadPlan(){} // irrelevant for current implementation

void Car::updateHistory(int time){
	// the function create the car history by appending the information to one string
	_history = _history + "(" + boost::lexical_cast<string>(time) + "," + _currRoad + "," + boost::lexical_cast<string>(_location) + ")";
}

void Car::setCurrentRoad(string roadID){
	// the functions sets a new road and updated the next road to be in
  _currRoad = roadID;
  setNextRoad();
}

void Car::setNextRoad(){
	// the function sets the next road field of the car in the road plan (not the current)
	size_t currRoadLocation = _roadPlan.find(_currRoad);
	if (currRoadLocation!=string::npos){
		std::string roadsLeft = _roadPlan.substr(currRoadLocation);
		size_t firstComma = roadsLeft.find_first_of(',');
		roadsLeft = roadsLeft.substr(firstComma+1);
		std::string remainingRoadPlan = roadsLeft;
		size_t secondComma = roadsLeft.find_first_of(',');
		std::string firstJunction = roadsLeft.substr(0, secondComma);
		std::string temp = roadsLeft.substr(secondComma+1);
		size_t thirdComma = roadsLeft.find_first_of(',');
		std::string secondJunction = temp.substr(0, thirdComma);
		_nextRoad = firstJunction + "," + secondJunction;
		_roadPlan = remainingRoadPlan;
	}
}

void Car::advanceCar(int speed, int time, int roadLength)
// the function advances the car in the road
{
	if (_passedCurrentRoad)
		// if already pass the road length - return
	{
		return;
	}
	int newLocation = _location + speed;
	// else  = add the speed to location

	if (newLocation > roadLength){
		_location = roadLength;
		// can't be bigger than the road length
	}
	else{
		_location = newLocation;
	}
	if (_location == roadLength && !_passedCurrentRoad)
		// sets the passed car to true
	{
		_passedCurrentRoad = true;
	}
}

void Car::setPassedToFalse()
{
	_passedCurrentRoad = false;
}

void Car::setLocation(int i)
{
	_location=i;
}

// destructor:
Car::~Car()
{
}
