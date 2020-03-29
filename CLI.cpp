#include <iostream>
#include <string>
#include <unordered_map>
#include <sstream>
#include "query_parser.h"

int main() {
	std::string userInput;
	do
	{
		std::cout << "harkdb> ";
		std::getline(std::cin, userInput);
		QueryRepresentation test(userInput);
		std::cout << userInput << std::endl;
	}
	while (userInput != ".exit");
	return 0;
}

