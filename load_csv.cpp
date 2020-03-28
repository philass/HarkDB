#include <vector>
#include <string>
#include <sstream>
#include <fstream>
#include <iostream>
#include <algorithm>

std::vector<uint32_t> load_csv(std::string fileName, char delimiter)
{

	std::vector<uint32_t> data;
	std::ifstream input(fileName);
	for (std::string line; getline(input, line) ; ) {
		std::stringstream ss;
		std::replace(line.begin(), line.end(), delimiter, ' ');
		std::cout << line << std::endl;
		ss << line;
		std::string temp;
		uint32_t found;
		while (!ss.eof()) {
			ss >> temp;
			if (std::stringstream(temp) >> found) {
				data.push_back(found);
			}
			temp = "";
		}
	}
	return data;
}

