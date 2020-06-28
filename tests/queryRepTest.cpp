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

TEST(QueryRepresentationTest, SelectTest) {
	std::unordered_map<std::string, Table> umap;
	Table table("tests/resources/DataTest.csv", ',');
	umap.insert(std::make_pair("t1", table));
	QueryRepresentation qr("select col3, col5 from t1", umap);
	std::vector<int> cols = {2, 4};
	EXPECT_TRUE(qr.getFromTable() == "t1");
	EXPECT_TRUE(qr.getSelectColumns() == cols);
}



TEST(QueryRepresentationTest, GroupbyTest) {
	std::unordered_map<std::string, Table> umap;
	Table table("tests/resources/DataTest.csv", ',');
	umap.insert(std::make_pair("t1", table));
	QueryRepresentation qr("select MAX(col3), MIN(col5) groupby col1 from t1", umap);
	std::vector<int> cols = {2, 4};
	std::vector<int> type = {3, 4};
  int should_be_gcol = 0;
	EXPECT_TRUE(qr.getFromTable() == "t1");
	EXPECT_TRUE(qr.getGroupbyColumns() == cols);
  EXPECT_TRUE(qr.getGroupbyColumnsType() == type);
  EXPECT_TRUE(qr.getGcol() == should_be_gcol);

}
