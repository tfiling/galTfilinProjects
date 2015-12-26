#include "../include/JunctionReport.h"
#include <boost/circular_buffer.hpp>

//constructors:

//empty constructor:
//JunctionReport::JunctionReport()
//: Report("j", -1), _ptReports(ptReports)
//{
//}
//constructor:
JunctionReport::JunctionReport(std::string reportID, std::string junctionID, int executionTime , ptree& ptReports,
                               std::map<std::string , Junction*>& juncMapRef , std::map<std::string , Road*>& roadMapRef):
Report(reportID, executionTime), _junctionID(junctionID), _ptReports(ptReports), _juncMapRef(juncMapRef),
_roadMapRef(roadMapRef)
{
}
// getters and setters:

//getters:
const std::string& JunctionReport::getJunctionID() const
{
	return _junctionID;
}

const std::string& JunctionReport::getReportType() const
{
    const std::string& ans =  "Junction Report";
    return ans;
}

//setters:
void JunctionReport::setJunctionID(std::string junctionid)
{
	_junctionID=junctionid;
}

void JunctionReport::writeReport()
{
    boost::circular_buffer<TimeSliceObject> theTimeSlicesVector = _juncMapRef[_junctionID]->getTimeSlices();
    std::string theTimeSlicesString = "";
    for (boost::circular_buffer<TimeSliceObject>::iterator timeSliceIt = theTimeSlicesVector.begin(); timeSliceIt != theTimeSlicesVector.end(); timeSliceIt++)
    {
        std::string timeSlices =          boost::lexical_cast<std::string>(timeSliceIt->getTimeSlice()); // int
        std::string timesGreenLight =     boost::lexical_cast<std::string>(timeSliceIt->getTimesForGreenLight()); // int
        theTimeSlicesString.append("("+timeSlices+","+timesGreenLight+")") ;
    }

    std::string reportName = getReportId(); // [report_id]
    std::string value1 = "junctionId";
    _ptReports.put(reportName+"."+value1, _junctionID);
    std::string value2 = "timeSlices";
    _ptReports.add(reportName+"."+value2, theTimeSlicesString);

	std::vector <std::string> theIncomingJunctions =  _juncMapRef[_junctionID]->getIncomingJunctions();
	for (unsigned int i=0 ; i<theIncomingJunctions.size() ; i++)
    {
        //foreach incoming junction , list of cars standing in line and about to insert the junction
        std::string incomeJunc = theIncomingJunctions.at(i); //vector of string
        std::string name = incomeJunc+","+_junctionID;
        std::string carsAns = "";
        // (incomeJunc,thisJunc) is a road!
        std::vector <Car*> listOfCars = _roadMapRef[name]->getCars() ; //vector of cars
        if (listOfCars.empty())
        {
            carsAns="";
        }
        else
        {
            for (unsigned int k=0 ; k<listOfCars.size() ; k++) // assumption - not empty
            {
                  string currentCarID = (*listOfCars.at(k)).getID();
                  if ((*listOfCars.at(k)).getLocation() == _roadMapRef[name]->getLength()) // if the car is in the end of the road
                  {
                      carsAns += "(" + (*listOfCars.at(k)).getID() + ")" ;
                  }
            }
        }
        if (i==theIncomingJunctions.size()-1) // last element
        {
             _ptReports.add(reportName+"."+incomeJunc, carsAns);
        }
        else
        {
            _ptReports.add(reportName+"."+incomeJunc, carsAns);
        }

    }
}

