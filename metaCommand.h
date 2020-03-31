#ifndef METACOMMAND_H
#define METACOMMAND_H

#include <string>
#include <unordered_map>
#include <vector>

#include "table.h"

void helpCommand();
void importTable(std::vector<std::string> words, std::unordered_map<std::string, Table> &tables);
void metaCommandParser(std::string, std::unordered_map<std::string, Table> &tables);

#endif
