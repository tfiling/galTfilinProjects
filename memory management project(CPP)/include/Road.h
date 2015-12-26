#ifndef ROAD_H_
#define ROAD_H_

#include <string>
#include <vector>
#include "../include/Car.h"

using namespace std;

class Road
{
public:

	//constructors:
	Road();
	Road( Junction& startJunction,  Junction& endJunction, const int length);


	//destructor:
	virtual ~Road();

	//oprator =:
	Road& operator=(Road &R);


	//getters and setters:

	//getters:
	const Junction& getStartJunction() const ;
	const Junction& getEndJunction() const ;
	vector<Car*>& getCars();
	const int& getLength() const;
	int getCarsCounter();

	//setters:
	void setStartJunction(const Junction& StartJunction) ;
	void setEndJunction(const Junction& EndJunction) ;
	void setCars(vector<Car> cars) ;
    void setLength(const int lenght) ;

    int calculateBaseSpeed();
    void addCar( Car* newCar);
    bool waitingCarsExist();
	Car* popFirstCar();

private:
    Junction& _startJunction;
    Junction& _endJunction;
    vector<Car*> _cars;
    int _length;
    int _carsCounter;
    int _carsPassed;
};




#endif
