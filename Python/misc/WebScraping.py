from bs4 import BeautifulSoup
import requests
import xlwt
from xlwt import Workbook


html_text = requests.get('https://www.bncollege.com/campus-stores/').text
soup = BeautifulSoup(html_text, 'lxml')
map_results = soup.find_all('section', id='map-results')
# print(map_results)
wb = Workbook()  # create workbook

for map_result in map_results:
    all_divs = map_result.find_all('div', class_='mt-16 first:mt-0')
    for div in all_divs:
        try:
            stateName = div.find('h2').text.strip()
        except:
            stateName = ""
        sheet1 = wb.add_sheet(stateName)  # Create sheet
        # add column
        headerStyle = xlwt.easyxf('font: bold 1')
        sheet1.write(0, 0, 'Store Mame', headerStyle)
        sheet1.write(0, 1, 'School Name', headerStyle)
        sheet1.write(0, 2, 'Address1', headerStyle)
        sheet1.write(0, 3, 'Address2', headerStyle)
        sheet1.write(0, 4, 'Phone No', headerStyle)
        sheet1.write(0, 5, 'Email', headerStyle)

        map_results_items = div.find('div', class_='map__results-items')
        listItems = map_results_items.find_all('div', class_='Map__results-item bg-gray-200 px-4 py-8')

        for i, listItem in enumerate(listItems, start=1):
            try:
                storeName = listItem.find('h3', class_='normal-case text-black text-lg').text.strip()
            except:
                storeName = ""
            try:
                schoolName = listItem.find('p', class_='m-0 text-gray-700 text-sm').text.strip()
            except:
                schoolName = ""
            try:
                address1 = listItem.find('div', class_='text-sm').p.strong.text.strip()
            except:
                address1 = ""
            try:
                address2 = listItem.find('div', class_='address').p.text.strip()
            except:
                address2 = ""
            try:
                phoneNo = listItem.find('div', class_='inline-block md:flex telephone items-center mt-4').a.span.text.strip()
            except:
                phoneNo = ""
            try:
                email = listItem.find('div', class_='inline-block md:flex email items-center mt-4').a.span.text.strip()
            except:
                email = ""
            # Add rows {store name, school name, address1, address2, phone no, email}
            sheet1.write(i, 0, storeName)
            sheet1.write(i, 1, schoolName)
            sheet1.write(i, 2, address1)
            sheet1.write(i, 3, address2)
            sheet1.write(i, 4, phoneNo)
            sheet1.write(i, 5, email)

wb.save('campus-stores.xls')