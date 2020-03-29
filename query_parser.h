/** 
 * Header file for QueryRepresentation.cpp
 */

#include <string>
#include <vector>

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
		QueryRepresentation(std::string query);

		std::vector<uint64_t> getSelectColumns();
		
		std::vector<uint64_t> getGroupbyColumns();

		std::vector<uint64_t> getGroupbyColumnsType();

		std::string getFromTable();
}
	
	


