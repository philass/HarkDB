/*
 * Contains tests for the h8.csv
 */

#include <gtest/gtest.h>
#include <vector>
#include <string>
//#include <utility>
//#include <unordered_map>

#include "../table.h"
#include "../query_parser.h"

TEST(QueryRepresentationTest, ResultTest) {
  std::cout << "got to the start" << std::endl;
	std::unordered_map<std::string, Table> umap;
  std::cout << "about to load table" << std::endl;
	Table table("tests/resources/DataTest.csv", ',');
  std::cout << "Loaded table" << std::endl;

	umap.insert(std::make_pair("t1", table));
  std::cout << "made the pair" << std::endl;
	QueryRepresentation qr("select col3, col5 from t1 groupby col3", umap);
  std::cout << "made the qr rep" << std::endl;
	std::vector<int> cols = {2, 4};
	EXPECT_TRUE(qr.getFromTable() == "t1");
	EXPECT_TRUE(qr.getSelectColumns() == cols);
}


