go: fut_compile shared_lib result 

fut_compile: futhark/select_where.fut
	mkdir build && futhark c --library futhark/select_where.fut -o build/select_where

shared_lib: build/select_where.c build/select_where.h
	gcc build/select_where.c -o build/lib_select_where.so -fPIC -shared


# Generate output directory
result: build/lib_select_where.so db_gpu_load.cpp
	g++ -std=c++11 build/lib_select_where.so  db_gpu_load.cpp -o build/out





queryrep: table.h table.cpp query_parser.h query_parser.cpp
	g++ -std=c++17 table.cpp query_parser.cpp -o queryrep.o





tests: buildtests
	./tests/tableTest.out

buildtests: tableTest.out
	
tableTest.out: table.h table.cpp tests/tableTest.cpp
	g++ -std=c++17 -lgtest -lgtest_main table.cpp tests/tableTest.cpp -o tests/tableTest.out

clean: 
	rm -rf build/ && rm tests/*.out && rm *.out

