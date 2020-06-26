  
-- File contains functions for handling SQL Queries
--
-- ==
-- input @ dataset_select_where.data


-- select columns of relevance
let sel (cols : []i32) (row : []u32) : []u32 =
  let f = (\i -> row[i])
  in map f cols

-- From table -> db
-- Select [cols] -> [cols]
-- Where exps -> ([cols] -> Bool)

let sel_all (db : [][]u32) (cols : []i32) : [][]u32 =
  -- let rows_to_keep = filter f db
  let result = map (sel cols) db
  in result


let select (db : [][]u32) (cols : []i32) : [][]u32 = sel_all db cols
