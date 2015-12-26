#ifndef TIMESLICEOBJECT_H_
#define TIMESLICEOBJECT_H_

#include <string>

class TimeSliceObject
{
public:
	//constructors:

	//empty constructor:
	TimeSliceObject();
	// constructor:
	TimeSliceObject(std::string JunctionID,int timeSlice,int timesForGreenLight);


	// getters and setters:

	//getters:
	const int& getTimeSlice() const ;
    const std::string& getJunctionID() const;
	const int& getTimesForGreenLight() const ;

	//setters:
	void setTimeSlice(int TimeSlice) ;
    void setJunctionID(std::string JunctionID) ;
	void setTimesForGreenLight(int TimesForGreenLight) ;


    void resetGreenLight();
    bool checkFullCapacity(int carsCounter);
    void increaseTimeSlice(const int MAX_TIME_SLICE);
    void decreaseTimeSlice(const int MIN_TIME_SLICE);

    //destructor:
	~TimeSliceObject();

	//oprator =:
	TimeSliceObject& operator=(const TimeSliceObject &T);

	// other functions:
    bool ifGreenLightHasPassed ();

private:
	// pointers!!
    std::string _junctionID;
	int _timeSlice; // the current time slice the junction defined
	int _timesForGreenLight; // -1 if its red now,
	//a number , like 4: 4 time slices for green light and then it would be red

};


#endif
