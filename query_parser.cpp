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
	std::vector<std::string> joins;
	std::vector<std::string> wheres;
	std::vector<std::string> orderbys;
	std::vector<std::string> havings;
	std::replace(query.begin(), query.end(), ',', ' ');
	std::stringstream iss(query);
	std::string word;
	bool select = false;
	bool groupby = false;
	bool from = false;
	bool join = false;
	bool where = false;
	bool orderby = false;
	bool having = false;
	
	while (iss >> word) {
		if (word == "select") {
			select = true;
			groupby = false;
			from = false;
			join = false;
			where = false;
			orderby = false;
			having = false;
		} else if (word == "groupby") {
			select = false;
			groupby = true;
			from = false;
			join = false;
			where = false;
			orderby = false;
			having = false;
		        	
		} else if (word == "from") {
			select = false;
			groupby = false;
			from = true;
			join = false;
			where = false;
			orderby = false;
			having = false;
		} else if (word == "join") {
			select = false;
			groupby = false;
			from = false;
			join = true;
			where = false;
			orderby = false;
			having = false;
		} else if (word == "where") {
			select = false;
			groupby = false;
			from = false;
			join = false;
			where = true;
			orderby = false;
			having = false;
		} else if (word == "orderby") {
			select = false;
			groupby = false;
			from = false;
			join = false;
			where = false;
			orderby = true;
			having = false;
		} else if (word == "having") {
			select = false;
			groupby = false;
			from = false;
			join = false;
			where = false;
			orderby = false;
			having = true;
		} else {
			if (select == true) {
				selects.push_back(word);
			} else if (groupby == true) {
				groupbys.push_back(word);
			} else if (from == true) {
				froms.push_back(word);
			} else if (join == true) {
				joins.push_back(word);
			} else if (where == true) {
				wheres.push_back(word);
			} else if (orderby == true) {
				orderbys.push_back(word);
			} else if (having == true) {
				havings.push_back(word);
			} else {
				throw std::invalid_argument("Don't currently support : " + word);
			}
		}
	}

	// Assumes we are only using one table in 
	std::vector<int> selCols;
	if (froms.size() == 1) {
		Table table = tables.at(froms[0]);
		std::vector<std::string> headers = table.getHeaders();
		for (std::string val : selects) {
			int pos = headers.size();
			for (int i = 0; i < headers.size(); i++) {
				if (headers[i] == val) {
					pos = i;
				}
			}
			if (pos > headers.size()) {
				throw "Column : " + val + " : was not in : " + froms[0];
			} 
			int idx = pos;
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
