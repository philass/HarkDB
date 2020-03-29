/*
 * Contains tests for the h8.csv
 */

#include <gtest/gtest.h>
#include <vector>
#include <string>

#include "../table.h"


TEST(TableTest, DataTest) {
	Table table("tests/resources/DataTest.csv");
	std::vector<std::string> headers = 
	  {"col1", "col2", "col3", "col4", 
	   "col5", "col6", "col7", "col8"};
	for (auto i: headers)
	  std::cout << i << ' ';
	std::cout << std::endl;
	std::cout << "Implementation result : ";
	for (auto i: table.getHeaders())
	  std::cout << i << ' ';
	std::vector<std::vector<uint32_t> > data = 
	 {{6, 6, 6, 6, 6, 6, 6, 6}, 
          {0,  0,  0,  0,  0,  0,  0,  0},
          {0,  0,  0,  0,  0,  0,  0,  0},
          {0,  0,  0,  0,  0,  0,  0,  0}, 
          {0,  0,  0,  0,  0,  0,  0,  0},
          {6,  6,  6,  6,  6,  6,  6,  6},    
          {1,  2,  3,  4,  5,  3,  2,  1}}; 
	EXPECT_TRUE(headers == table.getHeaders());
	EXPECT_TRUE(data == table.getData());
}

