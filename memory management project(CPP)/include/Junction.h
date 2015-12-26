#ifndef JUNCTION_H_
#define JUNCTION_H_

#include <vector>
#include <string>
#include "../include/TimeSliceObject.h"
#include <boost/circular_buffer.hpp>

class Junction
{
public:
//constructors:

	//empty constructor:
	Junction();
	//constructor:
	Junction(std::string JiD,std::vector <std::string>& incomingJunctions,boost::circular_buffer<TimeSliceObject>& TimeSlices);

//getters and setters:

	//getters:
	const std::string& getJiD() const ;
	const std::vector<std::string> getIncomingJunctions() const ;
	const boost::circular_buffer<TimeSliceObject> getTimeSlices() const ;

	//setters:
	void setID(std::string jiD);
	void setIncomingJunctions(std::vector <std::string> incomingJunctions);

	//destructor:
	virtual ~Junction();

	// other functions:
	void switchLightsInJunction(const int MAX_TIME_SLICE, const int MIN_TIME_SLICE);
	void increasePassedCarsCounter();

	// operator = :
	Junction& operator=(const Junction &J);

	bool checkGreenLight(std::string incomingJunctions);

private: // "fields":
	std::string _jID;
	std::vector <std::string> _incomingJunctions; // all the junctions lead to this junction
	boost::circular_buffer<TimeSliceObject> _timeSlices; // the times slices
	int _passedCarsCounter;
};


#endif
