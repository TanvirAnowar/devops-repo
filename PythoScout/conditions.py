# Get user input
env = input("Enter Environment: ")

# Logic block with if-else
if env == "aws" or env == "AWS":
    print("AWS service selected")
else:
    print("Invalid environment")