#ifndef REPORT_H_
#define REPORT_H_

#include <boost/property_tree/ptree.hpp>
#include <string>
#include <iostream>
using boost::property_tree::ptree;

class Report
{
public:
	//empty constructor:
	Report ();

	//constructor:
	Report (std::string reportId, int executionTime);

	//getter:
	const std::string& getReportId() const ;
	const int& getExecutionTime() const;
    virtual const std::string& getReportType() const ;

	//setter:
	void setRepordID(std::string reportID);

	//destructor:
	virtual ~Report ();

	//other functions:
	virtual void writeReport()=0;


private: // fields:
	std::string _reportId ;
	int _executionTime;

};




#endif
