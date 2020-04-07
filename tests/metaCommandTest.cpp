/*
 * Contains tests for the h8.csv
 */

#include <gtest/gtest.h>
#include <vector>
#include <string>

#include "../table.h"
#include "../metaCommand.h"

TEST(MetaCommandTest, ImportTest) {
	std::unordered_map<std::string, Table> umap;
	std::string line = ".import tests/resources/DataTest.csv t1";
	metaCommandParser(line, umap);
	Table table = umap.at("t1");
	std::vector<std::string> headers = 
	  {"col1", "col2", "col3", "col4", 
	   "col5", "col6", "col7", "col8"};
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
