-- Contains GROUP BY CODE


--Groupby Functionality
--let groupby (db : [][]i64) (g_cols: []i32) (s_cols: []i32) : []i64 =

-- Radix sort helper function
let rsort_step [n][m] (xs: [n][m]u32, bitn: i32): [n][m]u32 =
  let xs_val = xs[:, 0]
  let bits1 = map (\x -> (i32.u32 (x >> u32.i32 bitn)) & 1) xs_val
  let bits0 = map (1-) bits1
  let idxs0 = map2 (*) bits0 (scan (+) 0 bits0)
  let idxs1 = scan (+) 0 bits1
  let offs  = reduce (+) 0 bits0
  let idxs1 = map2 (*) bits1 (map (+offs) idxs1)
  let idxs  = map2 (+) idxs0 idxs1
  let idxs  = map (\x->x-1) idxs
  in scatter (copy xs) idxs xs

-- Radix sort algorithm, ascending (for matrices)
let rsort [n][m] (xs: [n][m]u32): [n][m]u32 =
  loop (xs) for i < 32 do rsort_step(xs,i)




--Make flag array (index new rows)
let mk_flags [n] (row_ids: [n]u32): [n]u32 =
  let idxs = iota n
  let vals = map (\i ->   if i == 0
  			  then 0
			  else (if row_ids[i-1] != row_ids[i] 
			  	then 1
				else 0)) idxs
  in scan (+) 0 vals


let f (s_cols: []i32) (a: []u32) (b: []u32) = map (\i -> a[i] + b[i]) s_cols

--
let groupby (db : [][]u32)  (s_cols: []i32) : [][]u32 =
  let sorted_rows = rsort db -- ideally pass groupby col here
  let idxs = mk_flags sorted_rows[:, 0]
  let red_idxs = scan (+) 0 idxs
  in reduce_by_index (copy sorted_rows) (f s_cols) (replicate (length db[0, :]) 0) (map i32.u32 (copy red_idxs)) (copy sorted_rows)
  


