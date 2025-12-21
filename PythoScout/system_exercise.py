import psutil

def get_system_process_info():
    print(f"current cpu useage: {psutil.cpu_percent()}%")
    
get_system_process_info()