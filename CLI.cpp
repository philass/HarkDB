#include <iostream>
#include <string>
#include <unordered_map>

int main() {
	std::string userInput;
	do
	{
		std::cout << "harkdb> ";
		std::cin >> userInput;
		std::cout << userInput << std::endl;
	}
	while (userInput != ".exit");
	return 0;
}

