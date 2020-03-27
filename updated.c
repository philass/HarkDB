#include <stdio.h>
#include "build/select_where.h"
/*
struct context {
	struct futhark_context_config *cfg;
	struct futhark_context *ctx;
};

struct context create_context() {
  struct futhark_context_config *cfg = futhark_context_config_new();
  struct futhark_context *ctx = futhark_context_new(cfg);
  struct context cont= {cfg, ctx};
  return cont;
}

void free_context(struct context cont) {
  futhark_context_free(cont.ctx);
  futhark_context_config_free(cont.cfg);
}

*/
int main() {
  uint32_t db[] = { 1, 2, 3, 4, 3, 4, 5, 6 };
  int32_t select_cols[] = {0, 2};

  /** 
  struct context cont = create_context(); 
  struct futhark_context_config *cfg = cont.cfg;
  struct futhark_context *ctx = cont.ctx;
  **/
  struct futhark_context_config *cfg = futhark_context_config_new();
  struct futhark_context *ctx = futhark_context_new(cfg);

  struct futhark_u32_2d *db_arr = futhark_new_u32_2d(ctx, db, 2, 4);
  struct futhark_i32_1d *y_arr = futhark_new_i32_1d(ctx, select_cols, 2);
  struct futhark_u32_2d *result;
  
  futhark_entry_main(ctx, &result, db_arr, y_arr);
  futhark_context_sync(ctx);
  
  uint32_t *data;
  futhark_values_u32_2d(ctx, result, data);
  for (int i = 0; i < 4; i++) {
    printf("Result: %d\n", data[i]);
  }

  futhark_free_u32_2d(ctx, db_arr);
  futhark_free_i32_1d(ctx, y_arr);
  futhark_free_u32_2d(ctx,result);

  /*
   free_context(cont);
   */

  futhark_context_free(ctx);
  futhark_context_config_free(cfg);
}
