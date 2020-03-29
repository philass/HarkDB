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

Table::Table(std::string fileName, char delimiter=',')
{

	std::vector<<std::vector<uint32_t>> tableData;
	std::ifstream input(fileName);
	std::string headerLine;
	std::getline(input, headerLine);
	Table::headers = parseLine(headerLine, delimiter);
	for (std::string line; std::getline(input, line) ; ) {
		std::vector<uint32_t> dataLine = parseLine(line, delimiter);
		tableData.push_back(tableData);
	}
	Table::data = tableData;
}


/** 
 * Parse Line into vector of appropriate types
 */
template <typename T>
std::vector<T> parseLine(std::string, delimiter) {
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
	tableData.push_back(lineData);
	return parseLine;
}



