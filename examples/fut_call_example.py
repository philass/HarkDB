import numpy as np
from db import db

data = np.zeros((10, 10)).astype(int)

instance = db()
result =  instance.select(data, 3)
print("Getting result")
print(result)

