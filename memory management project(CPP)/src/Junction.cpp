#include "../include/Junction.h"
#include <vector>
#include <queue>
#include <string>
#include <iostream>
#include <boost/circular_buffer.hpp>

//constructors:

//empty constructor:
Junction::Junction()
: _jID("j0"), _passedCarsCounter(0)
{
}
//constructor:
Junction::Junction(std::string JiD,std::vector <std::string>& incomingJunctions,boost::circular_buffer<TimeSliceObject>& TimeSlices)
: _jID(JiD), _incomingJunctions(incomingJunctions), _timeSlices(TimeSlices), _passedCarsCounter(0)
{
}

//getters and setters:

//getters:
const std::string& Junction::getJiD() const
{
	return _jID;
}
const std::vector<std::string> Junction::getIncomingJunctions() const
{
	return _incomingJunctions;
}
const boost::circular_buffer<TimeSliceObject> Junction::getTimeSlices() const
{
	return _timeSlices;
}

//setters:
void Junction::setID(std::string jiD)
{
	_jID=jiD;
}
void Junction::setIncomingJunctions(std::vector <std::string> IncomingJunctions)
{
	_incomingJunctions= IncomingJunctions;
}

void Junction::switchLightsInJunction(const int MAX_TIME_SLICE, const int MIN_TIME_SLICE)
{

   bool ifGreenLightHasPassed = _timeSlices.front().ifGreenLightHasPassed();
   if (ifGreenLightHasPassed)
    // if the allocate time for the green light in incoming road has passed,
    // then the next incoming road in the vector will have green light:
   {
       if (_passedCarsCounter == 0)
       {//zero capacity
            _timeSlices.front().decreaseTimeSlice(MIN_TIME_SLICE);
       }
       else if (_timeSlices.front().checkFullCapacity(_passedCarsCounter))
       {//full capacity
            _timeSlices.front().increaseTimeSlice(MAX_TIME_SLICE);
       }
       _passedCarsCounter = 0;        																																																																																																								if (_timeSlices.front().getTimesForGreenLight()== 0 ) _timeSlices.front().setTimesForGreenLight(-1);

        boost::circular_buffer<TimeSliceObject>::iterator it = _timeSlices.begin();
        it++;

       if (it!=_timeSlices.end())
           {
               _timeSlices.rotate(it);
               // now the second element is in front . "begin() "
           }
        _timeSlices.front().resetGreenLight();


   }

}

void Junction::increasePassedCarsCounter()
{
    _passedCarsCounter++;
}
//destructor:
Junction::~Junction()
{
}

// operator = :
Junction& Junction::operator=(const Junction &J)
{

	// check for "self assignment" and do nothing in that case:
	if ( this == &J)
	{
		return *this;
	}

    _jID = J.getJiD();
    _incomingJunctions	= J.getIncomingJunctions();
    _timeSlices	= J.getTimeSlices();
}

bool Junction::checkGreenLight(std::string incomingJunctions)
// checks if this incoming road has a green light
{
    for (boost::circular_buffer<TimeSliceObject>::iterator timeSliceIt = _timeSlices.begin(); timeSliceIt != _timeSlices.end(); timeSliceIt++)
    {
        if (timeSliceIt->getJunctionID() == incomingJunctions)
        {
            return (timeSliceIt->getTimesForGreenLight() > 0);
        }
    }
    return false;

}
