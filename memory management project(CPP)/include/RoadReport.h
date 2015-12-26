#ifndef ROADREPORT_H_
#define ROADREPORT_H_

#include "../include/Report.h"
#include "../include/Road.h"
#include "../include/Car.h"
#include <boost/lexical_cast.hpp>
#include <vector>
#include <map>
class RoadReport: public Report

{
public:
    //empty constructor:
	RoadReport();

	// constructor:
	RoadReport(std::string reportID, std::string startJunction,std::string endJunction, int executionTime, ptree& ptReports , std::map<std::string, Road*>& roadsMapRef);

	//getters:
    const std::string& getStartJunction() const ;
    const std::string& getEndJunction() const ;
    const std::string& getReportType() const ;


	//setters:
    void setStartJunction(std::string startJunction);
    void setEndJunction(std::string endJunction);


    //distructor:
	virtual ~RoadReport();

	// the write report method of report abstract class:
    void writeReport();

private:
    std::string _startJunction;
    std::string _endJunction;
    std::vector<Car> _cars;
    ptree &_ptReports;
	std::string _reportId ;
    std::map<std::string, Road*>& _roadsMapRef;
};




#endif
