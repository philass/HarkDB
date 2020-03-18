open import "lib/github.com/diku-dk/segmented/segmented"
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
  			  then 1
			  else (if row_ids[i-1] != row_ids[i] 
			  	then 1
				else 0)) idxs
  in vals

let type_func (typ: i32) (v1 : u32)  (v2: u32) : u32 =
	match typ
	case 1 -> (*) v1 v2
	case 2 -> (+) v1 v2
	case 3 -> (u32.max) v1 v2
	case 4 -> (u32.min) v1 v2
	case _ -> (u32.min) v1 v2 -- TODO change to some panic function


--let f  (a: []u32) (b: []u32) = map2 (+) a  b --map (\i -> a[i] + b[i]) s_cols
let merge [n][m] (s_cols_t: [n]i32) (a: [m]u32) (b: [m]u32) : [m]u32 = 
  map (\i -> if i == 0 then a[i]
  		       else (type_func s_cols_t[i-1] a[i] b[i])
      ) (iota m)

	
let groupby [n][m][s][t] (db : [n][m]u32)  (cols: [s]i32)  (t_cols: [t]i32) : [][]u32 =
  let keep_fun columns row = map (\i -> row[i]) columns
  let keep = map (keep_fun cols) db
  let sorted_rows = rsort keep -- ideally pass groupby col here
  let idxs = mk_flags sorted_rows[:, 0]
  let flag = map (== 1) idxs
  let helper = merge t_cols
  in segmented_reduce helper (replicate s 0)  flag sorted_rows

let main db g_col s_cols t_cols = 
  let cols = concat [g_col] s_cols
  in groupby db cols t_cols
