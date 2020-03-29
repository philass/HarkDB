/*
 * Contains header file for load_csv.cpp
 */

#ifndef TABLE_H
#define TABLE_H

#include <vector>
#include <string>

template <typename T>
std::vector<T> parseLine(std::string line, char delimiter);

class Table
{
private:
	std::vector<std::vector<uint32_t>> data;
	std::vector<std::string> headers;
public:
	Table(std::string fileName, char delimiter=',');

	std::vector<std::vector<uint32_t>> getData() {
		return data;
	}

	std::vector<std::string> getHeaders() {
		return headers;
	}
};

#endif
