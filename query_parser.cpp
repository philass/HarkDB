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
#include <utility>

#include "table.h"
#include "query_parser.h"


/* 
 * Implementation of QueryRepresentation Constructor
 */

std::pair<int, std::string> getTypeAndColName(std::string colName) {
  std::string str_type = colName.substr(0, 4);
  char ending = colName.at(colName.length());
  std::string middle = colName.substr(4, colName.length() - 1);
  if (ending == ')') {
    if (str_type == "MUL(") {
      return std::pair<int, std::string>(1, middle);
    } else if (str_type == "SUM(") {
      return std::pair<int, std::string>(2, middle);
    } else if (str_type == "MAX(") {
      return std::pair<int, std::string>(3, middle);
    } else if (str_type == "MIN(") {
      return std::pair<int, std::string>(4, middle);
    } else {
      throw std::runtime_error("(1) HarkDB doesn't support the aggregation provided : " + colName);
    }
  }
  // Didn't hit an aggregation function
  return std::pair<int, std::string>(0, colName);
}

int getColIndex(std::string val, std::vector<std::string> headers) {
  int pos = -1;
  for (int i = 0; i < headers.size(); i++) {
    if (headers[i] == val) {
      pos = i;
    }
  }
  return pos;
}
        
 

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
				throw std::invalid_argument("(2) Don't currently support : " + word);
			}
		}
	}

	// Assumes we are only using one table in 
	std::vector<int> selCols;
	if (froms.size() == 1) {
		Table table = tables.at(froms[0]);
		std::vector<std::string> headers = table.getHeaders();
		fromTable = froms[0];
    // Logic in the case of groupby clause
    if (groupbys.size() == 1) {
      // what do i want to do...
      //
      // get the column that is groupby'd
      std::string name_gcol = groupbys[0];
      int pos = -1;
      for (int i = 0; i < headers.size(); i++) {
        if (headers[i] == name_gcol) pos = i;
      } 
      if (pos < 0) {
          throw std::runtime_error("(7) Column : " + name_gcol + " : was not in : " + froms[0]);
      }
      g_col = pos;

      for (std::string val : selects) {
        std::pair<int, std::string> typeAndColPair = getTypeAndColName(val);
        int col_type = std::get<0>(typeAndColPair);

        std::string col_name = std::get<1>(typeAndColPair);
        // Go with the prevailing assumption 
        if (col_type == 0) {
          if (col_name != name_gcol) {
            // we have reached a column that is not aggregated and not the groupby column
            throw std::runtime_error("(8)" + col_name + " needs to be aggregated");
          }
        } else {
          int col_pos = getColIndex(col_name, headers);
          groupbyColumns.push_back(col_pos);
          groupbyColumnsType.push_back(col_type);
        }
      }
    // There is no groupby statement...
    // We simply perform select with no special aggregation
    } else if (groupbys.size() == 0) {
      // There is no groupby column
      g_col = -1;
      for (std::string val : selects) {
        int pos = -1;
        for (int i = 0; i < headers.size(); i++) {
          if (headers[i] == val) {
            pos = i;
          }
        }
        if (pos < 0) {
          throw std::runtime_error("(9) Column : " + val + " : was not in : " + froms[0]);
        } 
        int idx = pos;
        selCols.push_back(idx);
      }
      selectColumns = selCols;
    } else if (groupbys.size() > 1) {
      throw std::runtime_error("(10) Grouping by multiple columns is not currently supported");
    }
	} else if (froms.size() == 0) {
		throw "No table give in FROM clause";
	} else {
		throw "Multi table statements not currently supported";
	}
}
