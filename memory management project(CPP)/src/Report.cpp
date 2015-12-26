
#include "../include/Report.h"


//empty constructor:
Report::Report ()
: _reportId("0", -1)
{
}

//constructor:
Report::Report (std::string reportID, int executionTime)
: _reportId(reportID), _executionTime(executionTime)
{
}

//getter:
const std::string& Report::getReportId() const
{
    return _reportId;
}

const int& Report::getExecutionTime() const
{
    return _executionTime;
}

const std::string& Report::getReportType() const { return "Report" ;}


//setter:
void Report::setRepordID(std::string reportID)
{
    _reportId =reportID;
}

//destructor:
Report::~Report ()
{
}

