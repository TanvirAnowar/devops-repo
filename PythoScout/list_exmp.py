'''
name = ["Alice", "Bob", "Charlie"]

# print(f"List of names:{name[2]}")

#for n in range(len(name)):
 #   print(f"*{name[n]}")
 


number_collection = [1, 2, 3, 4, 5,6,7,8,9,10]

for num in range(1,5):
    print(number_collection[num])
    

    

 
# Example Set
example_set = {1, 2, 3, 4, 5, 5, 4, 3, 2, 1 }

check_set = {3, 4, 5, 99,88}

print(example_set.intersection(check_set))

print(example_set.union(check_set))
''' 
# Dictionary Example
dic_exmp = {
    "name": "Alice",
    "age": 30,
    "city": "New York"
}

dic_exmp_2 = {
    "product": "Laptop",
    "price": 1200,
    "brand": "Dell"
}


    
print(dic_exmp_2.get("priceX", "not found")) 
'''
envronment_values =[dic_exmp, dic_exmp_2]

for env in envronment_values:
    print(env.values())
    

for key,value in dic_exmp.items(): # need to convert to items() to get both key and value
    print(f"{key}:{value}")
   # print(f"{}")
 
#removes duplicates by converting to set and back to list
list_data = list( set([ "apple", "banana", "cherry", "date", "elderberry" ,"banana"])) 

#print(list_data)

#print(dir(list_data))

print(list_data.extend.__doc__)

''' 
