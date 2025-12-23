import requests

response = requests.get("https://jsonplaceholder.typicode.com/todos/1")

#print(response.json().items())

response_with_header = requests.get("https://jsonplaceholder.typicode.com/todos/1", 
                                    headers={"Accept": "application/json"})
print(response_with_header.json())