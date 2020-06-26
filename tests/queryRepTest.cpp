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
	std::unordered_map<std::string, Table> umap;
	Table table("tests/resources/DataTest.csv", ',');

	umap.insert(std::make_pair("t1", table));
	QueryRepresentation qr("select col3, col5 from t1", umap);
	std::vector<int> cols = {2, 4};
	EXPECT_TRUE(qr.getFromTable() == "t1");
	EXPECT_TRUE(qr.getSelectColumns() == cols);
}
