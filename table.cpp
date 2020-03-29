/*
 * Contains functionality for parsing a csv file
 * and loading it into a vector
 */
#include <vector>
#include <string>
#include <sstream>
#include <fstream>
#include <iostream>
#include <algorithm>

#include "table.h"



Table::Table(std::string fileName, char delimiter)
{

	std::vector<std::vector<uint32_t>> tableData;
	std::ifstream input(fileName);
	std::string headerLine;
	std::getline(input, headerLine);
	Table::headers = parseLine<std::string>(headerLine, delimiter);
	for (std::string line; std::getline(input, line) ; ) {
		std::vector<uint32_t> dataLine = parseLine<uint32_t>(line, delimiter);
		tableData.push_back(dataLine);
	}
	Table::data = tableData;
}

/*
std::vector<std::vector<uint32_t>> getData()
{
	return Table::data;
}

std::vector<std::string> getHeaders()
{
	return Table::headers;
}
*/

/** 
 * Parse Line into vector of appropriate types
 */
template <typename T>
std::vector<T> parseLine(std::string line, char delimiter) {
	std::vector<T> lineData;
	std::stringstream ss;
	std::replace(line.begin(), line.end(), delimiter, ' ');
	ss << line;
	std::string temp;
	T found;
	while (!ss.eof()) {
		ss >> temp;
		if (std::stringstream(temp) >> found) {
			lineData.push_back(found);
		}
		temp = "";
	}
	return lineData;
}


