/*
 * Contains header file for load_csv.cpp
 */

#include <vector>
#include <string>

class Table
{
private:
	std::vector<std::vector<uint32_t>> data;
	std::vector<std::string> headers;
public:
	std::vector<uint32_t> Table(std::string fileName, char delimiter='c');
	std::vector<std::vector<uint32_t>> getData();
	std::vector<uint32_t> getHeaders();
};
	
