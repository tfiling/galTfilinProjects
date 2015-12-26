#ifndef CARREPORT_H_
#define CARREPORT_H_

#include <boost/lexical_cast.hpp>
#include <string>
#include <map>
#include "../include/Report.h"
#include "../include/Car.h"


class CarReport: public Report
{
public:

	//constructors:

	//empty constructor:
	CarReport();
	//constructor:
	CarReport(std::string reportID , std::string carId, int executionTime, ptree& ptReports ,std::map<std::string, Car*>& carsMapRef);

	// getters and setters:

	//getters:
	const std::string& getCarId() const ;
    const std::string& getReportType()  ;

	//setters:
	void setCarId(std::string CarId);

	//destructor:
	virtual ~CarReport();


	void writeReport();


	//oprator = :
	//CarReport& operator=(const CarReport &C);

private:
	//report id of super: report
	std::string _carId;
	ptree &_ptReports;
    std::string _reportId ;
    std::map<std::string, Car*>& _carsMapRef;
};


#endif
