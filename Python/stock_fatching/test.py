import requests
from selenium import webdriver 
from selenium.webdriver.common.by import By
from webdriver_manager.chrome import ChromeDriverManager
from bs4 import BeautifulSoup

class Inventory:
    
    def __init__(self):
        chrome_driver_path = "chromedriver.exe"

        op = webdriver.ChromeOptions()
        op.add_argument('--headless')
        # driver = webdriver.Chrome()

        self.driver = webdriver.Chrome(ChromeDriverManager().install(), options=op)
        
        self.driver.get("https://www.ups.com/track?loc=en_US&tracknum=1Z0244176858248884&requester=WT/trackdetails")
        result = self.driver.find_element(by=By.ID, value="st_App_EstDelLabel").text
        print(result)
        self.driver.quit()


if __name__ == '__main__':
    inventory = Inventory()
