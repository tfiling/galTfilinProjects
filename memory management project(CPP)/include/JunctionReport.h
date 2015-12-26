#ifndef JUNCTIONREPORT_H_
#define JUNCTIONREPORT_H_

#include "../include/Report.h"
#include "../include/TimeSliceObject.h"
#include "../include/Junction.h"
#include "../include/Road.h"
#include <boost/lexical_cast.hpp>
#include <map>

class JunctionReport: public Report
{
public:
	//constructor:
	JunctionReport(std::string reportID, std::string junctionID, int executionTime , ptree& ptReports,
                std::map<std::string , Junction*>& juncMapRef ,std::map<std::string , Road*>& roadMapRef);

	// getters and setters:
	//getters:
	const std::string& getJunctionID() const ;
    const std::string& getReportType() const ;

	//setters:
	void setJunctionID(std::string junctionID);

	//other functions:
    void writeReport();


private: // fields:
	std::string _junctionID;
    std::string _reportId ;
	ptree &_ptReports;
    std::map<std::string , Junction*>& _juncMapRef;
    std::map<std::string , Road*>& _roadMapRef;
};



#endif
