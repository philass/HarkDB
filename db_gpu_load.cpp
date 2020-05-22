#include <iostream>
#include <vector>
#include <stdio.h>
#include <stdlib.h>

#include "db_gpu_load.h"

extern "C" {

#include "build/select.h"

}

/*
struct context {
	struct futhark_context_config *cfg;
	struct futhark_context *ctx;
};
*/
void create_context(struct context* cont) {
  cont->cfg = futhark_context_config_new();
  cont->ctx = futhark_context_new(cont->cfg);

}

void free_context(struct context* cont) {
  futhark_context_free(cont->ctx);
  futhark_context_config_free(cont->cfg);
}

//std::vector<uint32> launchAndExecute(std::vector<std::vector<uint32_t>> db, std::vector<uint64_t> select_cols, struct context* cont) {
std::vector<uint32_t> launchAndExecute(uint32_t* db, int* select_cols, int rows, int cols, struct context* cont) {
  struct futhark_context_config *cfg = (cont->cfg);
  struct futhark_context *ctx = (cont->ctx);
  
  struct futhark_u32_2d *db_arr = futhark_new_u32_2d(ctx, db, rows, cols);
  struct futhark_i32_1d *y_arr = futhark_new_i32_1d(ctx, select_cols, sizeof(select_cols)/sizeof(select_cols[0]));
  struct futhark_u32_2d *result;
 
  
  futhark_entry_main(ctx, &result, db_arr, y_arr);
  futhark_context_sync(ctx);
 
  int64_t* shape;
  shape = futhark_shape_u32_2d(ctx, result);
  int64_t num_rows = shape[0];
  int64_t num_cols = shape[1];
  int64_t num_tots = num_rows * num_cols;
  
  uint32_t* data = (uint32_t *) malloc (sizeof(uint32_t) * num_tots);
  futhark_values_u32_2d(ctx, result, data);
  futhark_free_u32_2d(ctx, db_arr);
  futhark_free_i32_1d(ctx, y_arr);
  futhark_free_u32_2d(ctx,result);
//  free_context(&cont);
  std::vector<uint32_t> values(data, data + num_tots);
  free(data);
  return values;
}

std::vector<uint32_t> launchAndExecute(uint32_t* db, int* select_cols, int rows, int cols) {
  struct context cont;
  create_context(&cont); 
  struct futhark_context_config *cfg = cont.cfg;
  struct futhark_context *ctx = cont.ctx;
  
  struct futhark_u32_2d *db_arr = futhark_new_u32_2d(ctx, db, rows, cols);
  struct futhark_i32_1d *y_arr = futhark_new_i32_1d(ctx, select_cols, sizeof(select_cols)/sizeof(select_cols[0]));
  struct futhark_u32_2d *result;
 
  
  futhark_entry_main(ctx, &result, db_arr, y_arr);
  futhark_context_sync(ctx);
 
  int64_t* shape;
  shape = futhark_shape_u32_2d(ctx, result);
  int64_t num_rows = shape[0];
  int64_t num_cols = shape[1];
  int64_t num_tots = num_rows * num_cols;
  
  uint32_t* data = (uint32_t *) malloc (sizeof(uint32_t) * num_tots);
  futhark_values_u32_2d(ctx, result, data);
  for (int i = 0; i < 4; i++) {
	  std::cout << data[i] << std::endl;
  }

  futhark_free_u32_2d(ctx, db_arr);
  futhark_free_i32_1d(ctx, y_arr);
  futhark_free_u32_2d(ctx,result);
  free_context(&cont);
  std::vector<uint32_t> values(data, data + num_tots);
  free(data);
  return values;

}

/*
int main() {
  uint32_t db[] = { 1, 2, 3, 4, 3, 4, 5, 6 };
  int32_t select_cols[] = {0, 2};
  struct context cont;
  create_context(&cont); 

  std::vector<uint32_t> vals = launchAndExecute(db, select_cols, 2, 4, &cont); 
  //std::vector<uint32_t> vals = launchAndExecute(db, select_cols, 2, 4); 
  for (uint32_t v : vals) {
	  std::cout << v << std::endl;
  }
}
*/
