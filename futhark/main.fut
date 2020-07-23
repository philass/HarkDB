-- Orchestrate the calling of the different SQL clauses from a 
-- single entry point

import "select"
import "groupby"

entry query_sel db s_col = select db s_col

entry query_groupby db g_col s_cols t_cols = groupby_call db g_col s_cols t_cols
