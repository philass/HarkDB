#include <string>
#include <vector>
#include <unordered_map>
#include <iostream>

#include "metaCommand.h"
#include "table.h"

void helpCommand() {
    std::cout << ".exit \t Exit this program" << std::endl;
    std::cout << ".help \t Show this message" << std::endl;
    std::cout << ".import FILE TABLE \t import data from FILE into TABLE" << std::endl;
    //print(".timer on \t Turn SQL timer on or off")
}

void importTable(std::vector<std::string> words, std::unordered_map<std::string, Table> &tables) {
	if (words.size() != 3) {
		std::cout <<  "Usage: .import FILE TABLE" << std::endl;
	}
	std::string fileName = words[1];
	std::string tableName = words[2];
	Table table(fileName, ',');
	tables.insert(std::make_pair(tableName, table));
}


void metaCommandParser(std::string line, std::unordered_map<std::string, Table> &tables) {
	std::vector<std::string> words = parseLine<std::string>(line, ',');
	std::string command = words[0];
	if (command == ".help") {
		helpCommand();
	} else if (command == ".import") {
		importTable(words, tables);
  } else if (command == ".exit") {
    // do nothing
	} else {
		std::cout << command << " is not a currently supported command" << std::endl;
	}
}
