# ObjectPool
 
先说核心，所谓对象池就是将对象生成之后放入对象池中，当要消除的时候装入池子，要生成的时候再从池子中拿出，用于减少反复生成/销毁时的性能开销，但要注意对象池本身也要占据一定内存，即使不用所有对象都存进去，也还是要占的，其只是为了减少反复生辰时的消耗  

