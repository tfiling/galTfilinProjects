#include "../include/RoadReport.h"



// constructors:
//empty constructor:
//RoadReport::RoadReport()
//:  Report("", -1),_startJunction("j0"), _endJunction("j0"),_ptReports(ptReports)
//{
//}

// constructor:
RoadReport::RoadReport(std::string reportID, std::string startJunction,std::string endJunction, int executionTime , ptree& ptReports, std::map<std::string, Road*>& roadsMapRef)
:  Report(reportID, executionTime),_startJunction(startJunction), _endJunction(endJunction), _ptReports(ptReports), _roadsMapRef(roadsMapRef)
{
}

//getters:
const std::string& RoadReport::getStartJunction() const
{
   return _startJunction ;
}
const std::string& RoadReport::getEndJunction() const
{
    return _endJunction;
}
const std::string& RoadReport::getReportType() const
{
    const std::string& ans =  "Road Report";
    return ans;
}
//setters:
void RoadReport::setStartJunction(std::string startJunction)
{
    _startJunction=startJunction;
}
void RoadReport::setEndJunction(std::string endJunction)
{
    _endJunction=endJunction;
}


RoadReport::~RoadReport()
{

}


//RoadReport& operator=(const RoadReport &R);

void RoadReport::writeReport()
{
    // get the car id , go to search it in the map , gets his history, make it string .
    //vector<Car> _cars;
    std::string name = _startJunction+","+_endJunction;
    std::vector<Car*> theCarVector = _roadsMapRef[name]->getCars();
    std::string carListString = "";
    for (unsigned int i=0 ; i<theCarVector.size() ; i++)
    {
        //catID, car loation
        std::string carId =   (*theCarVector.at(i)).getID(); // int
        std::string carLocation = boost::lexical_cast<std::string>((*theCarVector.at(i)).getLocation()); // int
        carListString.append ("("+carId+","+carLocation+")");
    }
    std::string reportName = getReportId(); // [report_id]
    std::string value1 = "startJunction";
    _ptReports.put(reportName+"."+value1, _startJunction);
    std::string value2 = "endJunction";
    _ptReports.add(reportName+"."+value2, _endJunction);
    std::string value3 = "cars";
    _ptReports.add(reportName+"."+value3, carListString );

}
