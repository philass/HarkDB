#include <stdio.h>

#include "dotprod.h"

int main() {
  int x[] = { 1, 2, 3, 4 };
  int y[] = { 2, 3, 4, 1 };

  struct futhark_context_config *cfg = futhark_context_config_new();
  struct futhark_context *ctx = futhark_context_new(cfg);

  struct futhark_i32_1d *x_arr = futhark_new_i32_1d(ctx, x, 4);
  struct futhark_i32_1d *y_arr = futhark_new_i32_1d(ctx, y, 4);

  int res;
  futhark_entry_main(ctx, &res, x_arr, y_arr);
  futhark_context_sync(ctx);

  printf("Result: %d\n", res);

  futhark_free_i32_1d(ctx, x_arr);
  futhark_free_i32_1d(ctx, y_arr);

  futhark_context_free(ctx);
  futhark_context_config_free(cfg);
}
