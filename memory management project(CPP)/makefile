
all: runSim

runSim: bin/AddCarEvent.o bin/Car.o bin/CarFaultEvent.o bin/CarReport.o bin/Event.o bin/JunctionReport.o bin/Junction.o bin/Report.o bin/Road.o bin/RoadReport.o bin/run.o bin/TimeSliceObject.o
	@echo 'Building target: hello'
	@echo 'Invoking: C++ Linker'
	g++ -o bin/RoadSimulator bin/AddCarEvent.o bin/Car.o bin/CarFaultEvent.o bin/CarReport.o bin/Event.o bin/JunctionReport.o bin/Junction.o bin/Report.o bin/Road.o bin/RoadReport.o bin/run.o bin/TimeSliceObject.o
	@echo 'Finished building target: hello'
	@echo ' '

bin/AddCarEvent.o: src/AddCarEvent.cpp
	g++ -g -Wall -Weffc++ -c -Linclude -o bin/AddCarEvent.o src/AddCarEvent.cpp

bin/Car.o: src/Car.cpp
	g++ -g -Wall -Weffc++ -c -Linclude -o bin/Car.o src/Car.cpp
	
bin/CarFaultEvent.o: src/CarFaultEvent.cpp
	g++ -g -Wall -Weffc++ -c -Linclude -o bin/CarFaultEvent.o src/CarFaultEvent.cpp

bin/CarReport.o: src/CarReport.cpp
	g++ -g -Wall -Weffc++ -c -Linclude -o bin/CarReport.o src/CarReport.cpp

bin/Event.o: src/Event.cpp
	g++ -g -Wall -Weffc++ -c -Linclude -o bin/Event.o src/Event.cpp

bin/JunctionReport.o: src/JunctionReport.cpp
	g++ -g -Wall -Weffc++ -c -Linclude -o bin/JunctionReport.o src/JunctionReport.cpp

bin/Junction.o: src/Junction.cpp
	g++ -g -Wall -Weffc++ -c -Linclude -o bin/Junction.o src/Junction.cpp

bin/Report.o: src/Report.cpp
	g++ -g -Wall -Weffc++ -c -Linclude -o bin/Report.o src/Report.cpp

bin/Road.o: src/Road.cpp
	g++ -g -Wall -Weffc++ -c -Linclude -o bin/Road.o src/Road.cpp

bin/RoadReport.o: src/RoadReport.cpp
	g++ -g -Wall -Weffc++ -c -Linclude -o bin/RoadReport.o src/RoadReport.cpp

bin/TimeSliceObject.o: src/TimeSliceObject.cpp
	g++ -g -Wall -Weffc++ -c -Linclude -o bin/TimeSliceObject.o src/TimeSliceObject.cpp

bin/run.o: src/run.cpp
	g++ -g -Wall -Weffc++ -c -Linclude -o bin/run.o src/run.cpp
	


clean: 
	rm -f bin/* 
