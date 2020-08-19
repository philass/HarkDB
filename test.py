from FutharkContext import FutharkContext

fc = FutharkContext()
fc.create_table('game_1', 'data.csv')
#sel = fc.sql("select col1, col3 from game_1")
gpby = fc.sql("select col1,  max(col3) from game_1 group by col1")
#print(sel)
print(gpby)

