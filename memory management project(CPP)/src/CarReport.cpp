#include "../include/CarReport.h"
#include <map>
#include <vector>
#include <iostream>
using namespace std;
//constructors:

//empty constructor:
//CarReport::CarReport()
//: Report("0", 0),_carId(0)
//{
//    ptree &ptReports;
//    _ptReports(ptReports);
//}
//constructor:
CarReport::CarReport(std::string reportID , std::string carId, int executionTime , ptree& ptReports,std::map<std::string, Car*>& carsMapRef)
: Report(reportID, executionTime),_carId(carId), _ptReports(ptReports), _carsMapRef(carsMapRef)
{

}

//getters and setters:

//getters:
const std::string& CarReport::getCarId() const
{
	return _carId;
}

const std::string& CarReport::getReportType()
{
   const std::string& ans =  "Car Report";
    return ans;
}

//setters:
void CarReport::setCarId(std::string CarId)
{
	_carId=CarId;
}

//destructor:
CarReport::~CarReport()
{
}


void CarReport::writeReport()
{
    // get the car id , go to search it in the map , gets his history, make it string .
    std::string carHistoryString = _carsMapRef[_carId]->getHistory();
    int faultyTime = _carsMapRef[_carId]->getFaultyTimeLeft();
    std::string faultyTimeString = boost::lexical_cast<std::string>(faultyTime);
    std::string reportName = getReportId(); // [report_id]
    std::string value1 = "carId";
    _ptReports.put(reportName+"."+value1, _carId);
    std::string value2 = "history";
    _ptReports.add(reportName+"."+value2, carHistoryString);
    std::string value3 = "faultyTimeLeft";
    _ptReports.add(reportName+"."+value3, faultyTimeString );

}
