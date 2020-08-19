from FutharkContext import FutharkContext

fc = FutharkContext()
fc.create_table('game_1', 'data.csv')
val = fc.sql("select col1, col3 from game_1")
print(val)

