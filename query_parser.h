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
		std::vector<uint64_t> selectColumns;
		
		std::vector<uint64_t> groupbyColumns;

		std::vector<uint64_t> groupbyColumnsType;

		std::string fromTable;

		// Need a Having Clause representation
		
		// Need a Where Clause representation
	
	public:
		QueryRepresentation(std::string query, std::unordered_map<std::string, Table> tables);

		std::vector<uint64_t> getSelectColumns() {
			return selectColumns;
		}
		
		std::vector<uint64_t> getGroupbyColumns();

		std::vector<uint64_t> getGroupbyColumnsType();

		std::string getFromTable() {
			return fromTable;
		}
};
	
	
#endif
