/*
 * Contains implementation of QueryRepresentation
 */

#include <string>
#include <vector>
#include <iostream>
#include <sstream>
#include <stdexcept>
#include <unordered_map>
#include <algorithm>

#include "table.h"
#include "query_parser.h"


/* 
 * Implementation of QueryRepresentation Constructor
 */
QueryRepresentation::QueryRepresentation(std::string query, std::unordered_map<std::string, Table> tables) {
	
	// Steps split lines up from select -> get everything between
	std::vector<std::string> selects;
	std::vector<std::string> groupbys;
	std::vector<std::string> froms;
	std::replace(query.begin(), query.end(), ',', ' ');
	std::stringstream iss(query);
	std::string word;
	bool select = false;
	bool groupby = false;
	bool from = false;
	
	while (iss >> word) {
		if (word == "select") {
			select = true;
			groupby = false;
			from = false;
		} else if (word == "groupby") {
			select = false;
			groupby = true;
			from = false;
		        	
		} else if (word == "from") {
			select = false;
			groupby = false;
			from = true;
		} else {
			if (select == true) {
				selects.push_back(word);
			} else if (groupby == true) {
				groupbys.push_back(word);
			} else if (from == true) {
				froms.push_back(word);
			} else {
				throw std::invalid_argument("Don't currently support : " + word);
			}
		}
	}

	// Assumes we are only using one table in 
	std::vector<uint64_t> selCols;
	if (froms.size() == 1) {
		Table table = tables.at(froms[0]);
		std::vector<std::string> headers = table.getHeaders();
		for (std::string val : selects) {
			uint64_t pos = headers.size();
			for (int i = 0; i < headers.size(); i++) {
				if (headers[i] == val) {
					pos = i;
				}
			}
			if (pos > headers.size()) {
				throw "Column : " + val + " : was not in : " + froms[0];
			} 
			uint64_t idx = pos;
			selCols.push_back(idx);
		}
		fromTable = froms[0];
		selectColumns = selCols;
	} else if (froms.size() == 0) {
		throw "No table give in FROM clause";
	} else {
		throw "Multi table statements not currently supported";
	}
		
}
