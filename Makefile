

# Make the opencl python library
fut_lib: db.fut
	futhark pyopencl --library db.fut
