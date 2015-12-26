
#include "../include/Road.h"
#include "../include/Junction.h"

#include <math.h>
#include <iostream>

//constructors:

// empty constructor
Road::Road(): _startJunction(*new Junction), _endJunction(*new Junction), _length(0), _carsCounter(0)
{
	_cars = vector<Car*>();
	_carsPassed=0;
}

// constructor
Road::Road( Junction& startJunction,  Junction& endJunction, const int length)
: _startJunction(startJunction), _endJunction(endJunction), _length(length), _carsCounter(0)
{
    _cars = vector<Car*>();
    _carsPassed=0;
}


//getters and setters:

//getters:
	const Junction& Road::getStartJunction() const
	{
		return _startJunction;
	}
	const Junction& Road::getEndJunction() const
	{
		return _endJunction;
	}
	vector<Car*>& Road::getCars()
	{
		return _cars;
	}

    const int& Road::getLength() const
    {
        return _length;
    }

    int Road::getCarsCounter()
    {
        return _carsCounter;
    }



// destructor:
Road::~Road()
{
}

//oprator =:
Road& Road::operator=(Road &R)
{
		return *this;
}

    int Road::calculateBaseSpeed()
    {
        if ((_carsCounter) == 0)
            return 0;

        double dResult = (_length / (_carsCounter));
        dResult = ceil(dResult);
        int result = (int)(dResult);
        return result;
    }

    void Road::addCar( Car* newCar)
    {
        std::string ID = _startJunction.getJiD() + "," + _endJunction.getJiD();
        newCar->setCurrentRoad(ID);
        _cars.push_back(newCar);
        _carsCounter++;
        newCar->setLocation(0);
    }

	bool Road::waitingCarsExist()
	// not possible because the vector is sorted -- need to check
	{
	    if ((_carsCounter == 0) | (_cars.empty()))
            return false;
	    return ((*_cars.back()).getPassedRoad() && (*_cars.back()).getFaultyTimeLeft() <= 0);
	}

	Car* Road::popFirstCar()
	{
		// iterate on the cars vector from farthest to closest
		// find the first car that its location is equal to the length of the road and is not faulty
		// if exists - pop
		// else - return null = 0
		for (vector<Car*>::iterator it = _cars.begin(); it != _cars.end(); ++it){
			int faultyTimeLeft = (*it)->getFaultyTimeLeft();
			// if the faulty time is 0 we can move the car to the next road
			int location = (*it)->getLocation();
			if ((location == _length) && (faultyTimeLeft == 0)){
				Car* poppedCar = (*it);
				_cars.erase(it);
				(_carsCounter)--;
				_carsPassed++;
				// the cars which passed the incoming road
				return poppedCar;
			}
		}
		cout << "  return null " << endl ;
		return 0;
	}
