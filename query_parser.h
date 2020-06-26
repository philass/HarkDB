/** 
 * Header file for QueryRepresentation.cpp
 */

#ifndef QUERYREPRESENTATION_H
#define QUERYREPRESENTATION_H

#include <string>
#include <vector>
#include <unordered_map>

#include "table.h"

class QueryRepresentation
{
	private:
		std::vector<int> selectColumns;
		
		std::vector<uint64_t> groupbyColumns;

		std::vector<uint64_t> groupbyColumnsType;

    int g_col;

		std::string fromTable;

    std::string queryType;

		// Need a Having Clause representation
		
		// Need a Where Clause representation
	
	public:
		QueryRepresentation(std::string query, std::unordered_map<std::string, Table> tables);

		std::vector<int> getSelectColumns() {
			return selectColumns;
		}
		
		std::vector<uint64_t> getGroupbyColumns() {
      return groupbyColumns;
    }

		std::vector<uint64_t> getGroupbyColumnsType() {
      return groupbyColumnsType;
    }

    int getGcol() {
      return g_col;
    }

		std::string getFromTable() {
			return fromTable;
		}

    std::string getQueryType() {
      return queryType;
    }
};
	
	
#endif
