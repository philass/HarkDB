-- Orchestrate the calling of the different SQL clauses from a 
-- single entry point

import "select.fut"
import "groupby.fut"

entry query db g_col s_cols t_cols =
  if g_col < 0
  then select s_cols
  else groupby_call db g_col s_cols t_cols
