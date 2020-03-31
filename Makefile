fut: fut_compile fut_shared_lib fut_result 

fut_compile: futhark/select_where.fut
	mkdir -p build && futhark c --library futhark/select_where.fut -o build/select_where

fut_shared_lib: build/select_where.c build/select_where.h
	gcc build/select_where.c -o build/lib_select_where.so -fPIC -shared

# Generate output directory
fut_result: build/lib_select_where.so db_gpu_load.cpp table.cpp metaCommand.cpp query_parser.cpp CLI.cpp
	g++ -std=c++11 build/lib_select_where.so  table.cpp db_gpu_load.cpp metaCommand.cpp query_parser.cpp CLI.cpp -o build/out

tests: buildtests
	./tests/tableTest.out
	./tests/queryRepTest.out
	./tests/metaCommandTest.out

buildtests: tableTest.out queryRepTest.out metaCommandTest.out
	
tableTest.out: table.h table.cpp tests/tableTest.cpp
	g++ -std=c++17 -lgtest -lgtest_main table.cpp tests/tableTest.cpp -o tests/tableTest.out

queryRepTest.out: table.h table.cpp query_parser.h query_parser.cpp tests/queryRepTest.cpp
	g++ -std=c++17 -lgtest -lgtest_main table.cpp query_parser.cpp tests/queryRepTest.cpp -o tests/queryRepTest.out

metaCommandTest.out: table.h table.cpp metaCommand.h metaCommand.cpp tests/metaCommandTest.cpp
	g++ -std=c++17 -lgtest -lgtest_main table.cpp metaCommand.cpp tests/metaCommandTest.cpp -o tests/metaCommandTest.out

clean: 
	rm -rf build/ && rm -f tests/*.out && rm -f *.out

