#include <iostream>
#include <string>
#include <unordered_map>
#include <sstream>

#include "query_parser.h"
#include "metaCommand.h"
#include "db_gpu_load.h"
#include "table.h"

int main() {
	std::string userInput;
	std::unordered_map<std::string, Table> umap;
	//  uint32_t db[] = { 1, 2, 3, 4, 3, 4, 5, 6 };
	//  int32_t select_cols[] = {0, 2};
	  struct context cont;
	  create_context(&cont); 
	
	do
	{
		std::cout << "harkdb> ";
		std::getline(std::cin, userInput);
		std::istringstream iss(userInput);
		char firstChar;
		iss >> std::skipws >> firstChar;
		if (firstChar == '.') {
			metaCommandParser(userInput, umap);
		} else {
			QueryRepresentation qr(userInput, umap);
			std::string table = qr.getFromTable();
			std::vector<std::vector<uint32_t> > db = umap.at(table).getData();
			std::vector<int> cols = qr.getSelectColumns();
			std::vector<uint32_t> db_v;
			for (std::vector<uint32_t> vec : db) {
				db_v.insert(std::end(db_v), std::begin(vec), std::end(vec));
			}
			std::vector<uint32_t> result = launchAndExecute(db_v.data(), cols.data(), db.size(), db[0].size(), &cont);
			int result_rows = result.size() / cols.size();
			for (int i = 0; i < result_rows; i++) {
				for (int j = 0; j < cols.size(); j++) {
					std::cout << result[i * cols.size() + j] << " ";
				} 
				std::cout << std::endl;
			}
		}
	}
	while (userInput != ".exit");
        //launchAndExecute(db, select_cols, 2, 4, &cont);	
	free_context(&cont);
	return 0;
}


