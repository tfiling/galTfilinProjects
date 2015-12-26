
# include "../include/TimeSliceObject.h"
#include <iostream>


//constructors:

//empty constructor:
TimeSliceObject::TimeSliceObject()
: _junctionID("0"), _timeSlice(0), _timesForGreenLight(0)
{
}
// constructor:
TimeSliceObject::TimeSliceObject(std::string JunctionID,int timeSlice,int timesForGreenLight)
: _junctionID(JunctionID), _timeSlice(timeSlice), _timesForGreenLight(timesForGreenLight)
{
}


// getters and setters:

//getters:
const int& TimeSliceObject::getTimeSlice() const
{
	return _timeSlice;
}
const int& TimeSliceObject::getTimesForGreenLight() const
{
	return _timesForGreenLight;
}
const std::string& TimeSliceObject::getJunctionID() const
{
	return _junctionID  ;
}
//setters:
void TimeSliceObject::setTimeSlice(int TimeSlice)
{
	_timeSlice=TimeSlice;
}
void TimeSliceObject::setTimesForGreenLight(int TimesForGreenLight)
{
	_timesForGreenLight=TimesForGreenLight;
}
void TimeSliceObject::setJunctionID(std::string JunctionID)
{
    _junctionID=JunctionID;
}

bool TimeSliceObject::ifGreenLightHasPassed ()
// assumption : _timesForGreenLight>=0
{

    if (_timesForGreenLight>=0)
    {
       _timesForGreenLight--;
    }

   return (_timesForGreenLight <= 0);
}


void TimeSliceObject::resetGreenLight()
{
    _timesForGreenLight = _timeSlice;
}

bool TimeSliceObject::checkFullCapacity(int carsCounter)
{
    return (carsCounter == _timeSlice);
}

void TimeSliceObject::increaseTimeSlice(const int MAX_TIME_SLICE)
{
    if (_timeSlice < MAX_TIME_SLICE)
    {
        _timeSlice++;
    }
}
void TimeSliceObject::decreaseTimeSlice(const int MIN_TIME_SLICE)
{
    if (_timeSlice > MIN_TIME_SLICE)
    {
        _timeSlice--;
    }

}


//destructor:
TimeSliceObject::~TimeSliceObject()
{
}


//oprator =:
TimeSliceObject& TimeSliceObject::operator=(const TimeSliceObject &T)
{
	 //check for "self assignment" and do nothing in that case:
	if ( this == &T)
	{
		return *this;
	}
	 //else
	 _junctionID = T.getJunctionID();
	_timeSlice = T.getTimeSlice();
	 _timesForGreenLight = T.getTimesForGreenLight();

}
