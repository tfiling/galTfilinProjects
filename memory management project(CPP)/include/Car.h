#ifndef CAR_H_
#define CAR_H_

 #include "../include/Junction.h"
 #include <string>

class Car

{
public:
	//constructors:

	// empty constructor
	Car();
	// constructor
	Car(std::string cID, std::string roadPlan);

	//getters and setters:

	//getters:
	std::string getID() const ;
	std::string getRoadPlan() const;
	int getFaultyTimeLeft() const;
	bool getPassedRoad() const;
	std::string getHistory() const;
	std::string getCurrentRoad() const;
	int getLocation() const;
	std::string getNextRoad() const;

	//setters:
	void setID(std::string iD);
	void setRoadPlan(std::string roadPlan);
	void setFaultyTimeLeft(int faultyTimeLeft);
	void setPassedToFalse();
	void popFirstInRoadPlan();
	void updateHistory(int time);
	void setCurrentRoad(std::string name);
	void setLocation(int i);

	// the method to advance the car:
    void advanceCar(int speed, int time, int roadLength);

    // destructor:
	virtual ~Car();

private:
//fields:
	std::string _cID ;		// unique name of car
	std::string _roadPlan;
	int _faultyTimeLeft ;  // 0 if not faulty now
	bool _passedCurrentRoad;
	std::string _history;
	std::string _currRoad;
	std::string _nextRoad;
	int _location;
// functions:
	void setNextRoad();
};



#endif
