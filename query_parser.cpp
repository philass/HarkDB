/*
 * Contains implementation of QueryRepresentation
 */

#include <string>
#include <vector>
#include <iostream>
#include <sstream>
#include <stdexcept>

#include "query_parser.h"

/* 
 * Implementation of QueryRepresentation Constructor
 */
QueryRepresentation::QueryRepresentation(std::string query) {
	
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
	std::cout << "printing select values" << std::endl;
	for (std::string s : selects) {
		std::cout << s << std::endl;	
	}
	std::cout << "printing groupby values" << std::endl;
	for (std::string s : groupbys) {
		std::cout << s << std::endl;	
	}
	std::cout << "printing from values" << std::endl;
	for (std::string s : froms) {
		std::cout << s << std::endl;	
	}
	
}

