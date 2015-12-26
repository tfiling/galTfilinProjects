
#include <string>
#include <vector>
#include "../include/AddCarEvent.h"
#include "../include/Road.h"
#include <iostream>


//constructors:

//constructor:
AddCarEvent::AddCarEvent(int timeOfEvent, std::string carID ,std::string roadPlan ,
                         std::map<string, Road*>& roadsMap, std::map<string, Car*>& carsMap):Event(timeOfEvent, carID),
                         _roadPlan(roadPlan),_carsMapRef(carsMap) ,_roadsMapRef(roadsMap)

{
}

//getters and setters:

//getters:
const std::string AddCarEvent::getRoad_plan() const
{
	return _roadPlan;
}
const std::map<std::string, Car*>& AddCarEvent::getCarsMapRef() const
{
    return _carsMapRef;
}
const std::map<std::string, Road*>& AddCarEvent::getRoadsMapRef() const
{
    return _roadsMapRef;
}
//setters:
void AddCarEvent::setRoadPlan(std::string roadPlan)
{
	_roadPlan= roadPlan;
}


//destructor:
AddCarEvent::~AddCarEvent()
{
}

void AddCarEvent::performEvent()
{
    // a new car is inserting the simulation
    Car *newCar = new Car( _carID, _roadPlan);
    // location = 0 by default
    //the car is inserted to the cars map:
    _carsMapRef[_carID] = newCar;
      //the car is inserted to the road's cars vector:
    std::string firstRoad = getFirstRoad();
   _roadsMapRef[firstRoad]->addCar(newCar);
}

std::string AddCarEvent::getFirstRoad(){
	// the function returns the first road the car need to insert:
	size_t firstComma = _roadPlan.find_first_of(',');
	std::string firstJunction = _roadPlan.substr(0, firstComma);
	std::string temp = _roadPlan.substr(firstComma+1);
	size_t secondComma = temp.find_first_of(',');
	std::string secondJunction = temp.substr(0, secondComma);
	return firstJunction + "," + secondJunction;
}
